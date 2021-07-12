using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Gaver.Common;
using Gaver.Common.Attributes;
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
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scrutor;

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
            if (context.HttpContext.Request.Path.StartsWithSegments("/hub") && accessToken.IsNotEmpty())
                context.Token = accessToken;

            return Task.CompletedTask;
        }

        private static Task OnTokenValidated(TokenValidatedContext context)
        {
            if (context.SecurityToken is JwtSecurityToken jwtSecurityToken)
                context.HttpContext.Items["access_token"] = jwtSecurityToken.RawData;

            return Task.CompletedTask;
        }

        public static void AddCustomMvc(this IServiceCollection services)
        {
            services.AddControllers(o => {
                    var policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(
                            new WhitelistDenyAnonymousAuthorizationRequirement("/serviceworker", "/offline.html"))
                        .Build();
                    o.Filters.Add(new AuthorizeFilter(policy));
                    o.Filters.Add(new CustomExceptionFilterAttribute());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddHybridModelBinder();
        }

        public static void AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("auth0").Get<Auth0Settings>();
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "My API",
                    Version = "v1"
                });
                config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows {
                        Implicit = new OpenApiOAuthFlow {
                            TokenUrl = new Uri($"https://{authSettings.Domain}/token"),
                            AuthorizationUrl = new Uri($"https://{authSettings.Domain}/authorize"),
                            Scopes = new Dictionary<string, string> {
                                {"openid", "Standard openid scope"},
                                {"profile", "Standard openid scope"},
                                {"email", "Standard openid scope"}
                            }
                        }
                    }
                });
                config.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("GaverContext");
            if (connectionString.IsNullOrEmpty()) throw new ConfigurationException("ConnectionStrings:GaverContext");

            services.AddDbContext<GaverContext>(options => {
                options.UseNpgsql(connectionString, b => b
                    .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
                    .SetPostgresVersion(11, 0));
            });
        }

        public static void ScanAssemblies(this IServiceCollection services)
        {
            services.Scan(scan => {
                scan.FromAssemblyOf<ICommonAssembly>()
                    .AddServices();
                scan.FromAssemblyOf<Startup>()
                    .AddServices()
                    .AddMappingProfiles();
            });
        }

        private static IImplementationTypeSelector AddMappingProfiles(this IImplementationTypeSelector selector)
            => selector.AddClasses(classes => classes.AssignableTo<Profile>()).As<Profile>().WithSingletonLifetime();

        private static IImplementationTypeSelector AddServices(
            this IImplementationTypeSelector implementationTypeSelector) => implementationTypeSelector
            .AddClasses(classes =>
                classes.WithoutAttribute<SingletonServiceAttribute>().Where(c => c.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.WithAttribute<ServiceAttribute>()).AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.WithAttribute<SingletonServiceAttribute>()).AsImplementedInterfaces()
            .WithSingletonLifetime();

        public static IServiceCollection AddValidationProblemDetails(this IServiceCollection services) =>
            services.Configure<ApiBehaviorOptions>(options => {
                // options.InvalidModelStateResponseFactory = context => {
                //     var problemDetails = new ValidationProblemDetails(context.ModelState) {
                //         Instance = context.HttpContext.Request.Path,
                //         Status = StatusCodes.Status400BadRequest,
                //         Type = "https://httpstatuses.com/400",
                //         Detail = "Please refer to the errors property for additional details."
                //     };
                //     return new BadRequestObjectResult(problemDetails) {
                //         ContentTypes = {"application/problem+json", "application/problem+xml"}
                //     };
                // };
            });

        public static void AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsEnvironment("Test")) return;

            services.AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("GaverContext"));
        }
    }
}
