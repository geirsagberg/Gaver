using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Logic;
using Gaver.Logic.Contracts;
using Gaver.Logic.Extensions;
using Gaver.Logic.Services;
using Gaver.Web.Exceptions;
using Gaver.Web.Extensions;
using Gaver.Web.Features.Users;
using Gaver.Web.Utils;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using WebApiContrib.Core;
using WebApiContrib.Core.Filters;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

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
            if (providerId == null) return;

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
                    OnTokenValidated = OnTokenValidated,
                    OnAuthenticationFailed = OnAuthenticationFailed
                };
            });
            services.AddAuthorization();

            services.AddMvc(o => {
                o.Filters.Add(new CustomExceptionFilterAttribute());
                o.Filters.Add(new ValidationAttribute());
                o.UseFromBodyBinding();
            });
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
            var connectionString = Configuration.GetConnectionString("GaverContext");
            if (connectionString.IsNullOrEmpty())
                throw new ConfigurationException("ConnectionStrings:GaverContext");
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<GaverContext>(options => options
                    .UseNpgsql(connectionString, b => b
                        .MigrationsAssembly(GetType().GetTypeInfo().Assembly.FullName)), ServiceLifetime.Transient);

            services.AddSingleton<IMapperService, MapperService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // services.AddSignalR();
            services.AddMediatR();

            services.Scan(scan => {
                scan.FromAssemblyOf<ILogicAssembly>().AddClasses().AsImplementedInterfaces().WithTransientLifetime();
                scan.FromEntryAssembly().AddClasses().AsImplementedInterfaces().WithTransientLifetime();
                scan.FromEntryAssembly().AddClasses().AsSelf().WithTransientLifetime();
            });
        }

        private Task OnAuthenticationFailed(AuthenticationFailedContext arg)
        {
            throw new NotImplementedException();
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.AddOptions();

            ConfigureOptions<MailOptions>(services, "mail");
            ConfigureOptions<Auth0Settings>(services, "auth0");

            if (missingOptions.Any())
                throw new Exception("Missing settings: " + missingOptions.ToJoinedString());
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
                SetupForProduction(loggerFactory);
            }
            // app.UseJwtAuthentication(auth0Settings);

            app.UseFileServer();
            // app.UseSignalR(routes => routes.MapHub<ListHub>("listHub"));

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpException();
            SetupRoutes(app);
        }

        private static void SetupRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes => {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("API 404", "api/{*anything}", new { controller = "Error", action = "NotFound" });
                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new { controller = "Home", action = "Index" });
            });
        }

        private static void SetupForProduction(ILoggerFactory loggerFactory)
        {
        }

        private static void SetupForDevelopment(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            UseRootNodeModules(env);

            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                HotModuleReplacement = true,
                ReactHotModuleReplacement = true
            });
        }
    }
}