using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Gaver.Data;
using Gaver.Logic;
using Gaver.Logic.Contracts;
using Gaver.Logic.Extensions;
using Gaver.Logic.Services;
using Gaver.Web.Exceptions;
using Gaver.Web.Features.Users;
using Gaver.Web.Hubs;
using JetBrains.Annotations;
using Joonasw.AspNetCore.SecurityHeaders;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using WebApiContrib.Core;
using WebApiContrib.Core.Filters;

[assembly: AspMvcViewLocationFormat(@"~\Features\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\Features\Shared\{0}.cshtml")]

namespace Gaver.Web
{
    public class Startup
    {
        private readonly List<string> missingOptions = new List<string>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        private static void UseRootNodeModules(IHostingEnvironment hostingEnvironment)
        {
            var nodeDir = Path.Combine(hostingEnvironment.ContentRootPath, "../../node_modules");
            Environment.SetEnvironmentVariable("NODE_PATH", nodeDir);
        }

        private static async Task OnTokenValidated(TokenValidatedContext tokenContext)
        {
            var identity = tokenContext.Principal.Identity as ClaimsIdentity;
            var providerId = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (providerId == null) {
                return;
            }

            var userHandler = tokenContext.HttpContext.RequestServices.GetRequiredService<UserHandler>();

            var userId = await userHandler.GetUserIdOrNullAsync(providerId);
            if (userId != null) {
                identity.AddClaim(new Claim("GaverUserId", userId.Value.ToString(), ClaimValueTypes.Integer32));
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureOptions(services);

            var authSettings = Configuration.GetSection("auth0").Get<Auth0Settings>();
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.Audience = authSettings.ClientId;
                options.Authority = $"https://{authSettings.Domain}";
                options.TokenValidationParameters = new TokenValidationParameters {
                    IssuerSigningKey = authSettings.SigningKey
                };
                // TODO Handle other events, e.g. OnAuthenticationFailed and OnChallenge
                options.Events = new JwtBearerEvents {
                    OnTokenValidated = OnTokenValidated
                };
            });
            services.AddAuthorization();

            services.AddMvc(o => {
                o.Filters.Add(new CustomExceptionFilterAttribute());
                o.Filters.Add(new ValidationAttribute());
                o.UseFromBodyBinding();
            }).AddRazorOptions(o => {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
                o.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                o.ViewLocationFormats.Add("/Features/{0}.cshtml");
            });
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info {
                    Title = "My API",
                    Version = "v1"
                });
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
            var connectionString = Configuration.GetConnectionString("GaverContext");
            if (connectionString.IsNullOrEmpty()) {
                throw new ConfigurationException("ConnectionStrings:GaverContext");
            }
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<GaverContext>(options => {
                    options.ConfigureWarnings(b => b.Throw(RelationalEventId.QueryClientEvaluationWarning));
                    options
                        .UseNpgsql(connectionString, b => b
                            .MigrationsAssembly(GetType().GetTypeInfo().Assembly.FullName));
                });

            services.AddSingleton<IMapperService, MapperService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR();
            services.AddMediatR();

            services.Scan(scan => {
                scan.FromAssemblyOf<ILogicAssembly>().AddClasses().AsImplementedInterfaces().WithTransientLifetime();
                scan.FromAssemblyOf<ILogicAssembly>().AddClasses(classes => classes.AssignableTo<Profile>())
                    .As<Profile>().WithSingletonLifetime();
                scan.FromEntryAssembly().AddClasses().AsImplementedInterfaces().WithTransientLifetime();
                scan.FromEntryAssembly().AddClasses().AsSelf().WithTransientLifetime();

                scan.FromEntryAssembly().AddClasses(classes => classes.AssignableTo<Profile>()).As<Profile>()
                    .WithSingletonLifetime();
            });
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.AddOptions();

            ConfigureOptions<MailOptions>(services, "mail");
            ConfigureOptions<Auth0Settings>(services, "auth0");

            if (missingOptions.Any()) {
                throw new Exception("Missing settings: " + missingOptions.ToJoinedString());
            }
        }

        private void ConfigureOptions<T>(IServiceCollection services, string key) where T : class, new()
        {
            var configurationSection = Configuration.GetSection(key);
            var options = configurationSection.Get<T>();

            var missing = typeof(T)
                .GetProperties()
                .Where(propertyInfo => propertyInfo.GetValue(options).ToStringOrEmpty().IsNullOrEmpty())
                .Select(propertyInfo => $"{key}:{propertyInfo.Name}");

            missingOptions.AddRange(missing);


            services.Configure<T>(configurationSection);

            // Enable injection of updated strongly typed options
            services.AddScoped(provider => provider.GetService<IOptionsSnapshot<T>>().Value);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env,
            Auth0Settings auth0Settings)
        {
            if (env.IsDevelopment()) {
                SetupForDevelopment(app, loggerFactory, env);
            } else {
                SetupForProduction(app, loggerFactory);
            }
            app.UseJwtAuthentication(auth0Settings);

            app.UseFileServer();

            app.UseHttpException();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseSignalR(routes => routes.MapHub<ListHub>("listHub"));

            SetupRoutes(app);
        }

        private static void SetupRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes => {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("API 404", "api/{*anything}", new {
                    controller = "Error",
                    action = "NotFound"
                });
                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new {
                        controller = "Home",
                        action = "Index"
                    });
            });
        }

        [Conditional("RELEASE")]
        private static void SetupForProduction(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
            app.UseHsts();
        }

        private static void SetupForDevelopment(IApplicationBuilder app, ILoggerFactory loggerFactory,
            IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            UseRootNodeModules(env);

            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                HotModuleReplacement = true,
                ReactHotModuleReplacement = true,
                HotModuleReplacementClientOptions = new Dictionary<string, string> {
                    {"reload", "true"}
                }
            });
        }
    }
}