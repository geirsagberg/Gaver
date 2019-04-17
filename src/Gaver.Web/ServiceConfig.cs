using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Gaver.Common;
using Gaver.Data;
using Gaver.Web.Exceptions;
using Gaver.Web.Filters;
using Gaver.Web.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Gaver.Web
{
    public static class ServiceConfig
    {
        //private static async Task OnTokenValidated(TokenValidatedContext tokenContext)
        //{
        //    var identity = tokenContext.Principal.Identity as ClaimsIdentity;
        //    var providerId = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //    if (providerId == null) {
        //        return;
        //    }

        //    var userHandler = tokenContext.HttpContext.RequestServices.GetRequiredService<UserHandler>();

        //    var userId = await userHandler.GetUserIdOrNullAsync(providerId);
        //    if (userId != null) {
        //        identity.AddClaim(new Claim("GaverUserId", userId.Value.ToString(), ClaimValueTypes.Integer32));
        //    }
        //}

        public static void AddCustomAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("auth0").Get<Auth0Settings>();
            services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.Audience = authSettings.Audience;
                    options.Authority = $"https://{authSettings.Domain}";
                    options.TokenValidationParameters = new TokenValidationParameters {
                        IssuerSigningKey = authSettings.SigningKey
                    };
                    options.Events = new JwtBearerEvents {
                        OnTokenValidated = OnTokenValidated
                    };
                });
            services.AddAuthorization();
        }

        private static Task OnTokenValidated(TokenValidatedContext context)
        {
            if (context.SecurityToken is JwtSecurityToken jwtSecurityToken) {
                context.HttpContext.Items["access_token"] = jwtSecurityToken.RawData;
            }

            return Task.CompletedTask;
        }

        public static void AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc(o => {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                o.Filters.Add(new AuthorizeFilter(policy));
                o.Filters.Add(new CustomExceptionFilterAttribute());
            }).AddRazorOptions(o => {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
                o.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                o.ViewLocationFormats.Add("/Features/{0}.cshtml");
            });
        }

        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info {
                    Title = "My API",
                    Version = "v1"
                });
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
        }

        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("GaverContext");
            if (connectionString.IsNullOrEmpty()) {
                throw new ConfigurationException("ConnectionStrings:GaverContext");
            }

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<GaverContext>(options => {
                    options.ConfigureWarnings(b => b.Throw(RelationalEventId.QueryClientEvaluationWarning));
                    options
                        .UseNpgsql(connectionString, b => b
                            .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
                });
        }

        public static void ScanAssemblies(this IServiceCollection services)
        {
            services.Scan(scan => {
                scan.FromAssemblyOf<ICommonAssembly>().AddClasses().AsImplementedInterfaces().WithTransientLifetime();
                scan.FromEntryAssembly().AddClasses().AsImplementedInterfaces().WithTransientLifetime();
                scan.FromEntryAssembly().AddClasses().AsSelf().WithTransientLifetime();

                scan.FromEntryAssembly().AddClasses(classes => classes.AssignableTo<Profile>()).As<Profile>()
                    .WithSingletonLifetime();
            });
        }
    }
}
