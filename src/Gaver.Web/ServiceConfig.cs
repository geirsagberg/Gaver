using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Gaver.Common;
using Gaver.Data;
using Gaver.Web.Exceptions;
using Gaver.Web.Filters;
using Gaver.Web.MvcUtils;
using Gaver.Web.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Gaver.Web
{
    public static class ServiceConfig
    {
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
                        OnTokenValidated = OnTokenValidated,
                        OnMessageReceived = OnMessageReceived
                    };
                });
            services.AddAuthorization();
        }

        private static Task OnMessageReceived(MessageReceivedContext context)
        {
            var accessToken = context.Request.Query["access_token"].FirstOrDefault();
            if (context.HttpContext.Request.Path.StartsWithSegments("/listHub") && accessToken.IsNotEmpty()) {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
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
                        .AddRequirements(new WhitelistDenyAnonymousAuthorizationRequirement("/serviceworker", "/offline.html"))
                        .Build();
                    o.Filters.Add(new AuthorizeFilter(policy));
                    o.Filters.Add(new CustomExceptionFilterAttribute());
                }).AddRazorOptions(o => {
                    o.ViewLocationFormats.Clear();
                    o.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
                    o.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                    o.ViewLocationFormats.Add("/Features/{0}.cshtml");
                }).AddJsonOptions(o => o.UseCamelCasing(true))
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddHybridModelBinder()
                ;
        }

        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new Info {
                    Title = "My API",
                    Version = "v1"
                });
                config.AddSecurityDefinition("Bearer", new ApiKeyScheme {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                config.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
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

        public static IServiceCollection AddValidationProblemDetails(this IServiceCollection services) =>
            services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = context => {
                    var problemDetails = new ValidationProblemDetails(context.ModelState) {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://httpstatuses.com/400",
                        Detail = "Please refer to the errors property for additional details."
                    };
                    return new BadRequestObjectResult(problemDetails) {
                        ContentTypes = {"application/problem+json", "application/problem+xml"}
                    };
                };
            });
    }
}
