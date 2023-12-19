using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using AutoMapper;
using Gaver.Common;
using Gaver.Common.Attributes;
using Gaver.Common.Contracts;
using Gaver.Common.Extensions;
using Gaver.Common.Utils;
using Gaver.Data;
using Gaver.Web.CrossCutting;
using Gaver.Web.Exceptions;
using Gaver.Web.Filters;
using Gaver.Web.MvcUtils;
using Gaver.Web.Options;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scrutor;
using ProblemDetailsFactory = Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory;


namespace Gaver.Web;

public static class ServiceConfig {
    public static void ConfigureServices(this WebApplicationBuilder builder) {
        var services = builder.Services;
        var config = builder.Configuration;
        var environment = builder.Environment;
        services.ScanAssemblies();

        services.AddCustomAuth(config);

        services.AddCustomMvc();
        services.AddCustomSwagger(config);
        services.AddCustomDbContext(config);
        services.AddCustomHealthChecks(config, environment);
        services.AddFeatureManagement();
        services.AddAzureAppConfiguration();

        services.AddSingleton<IMapperService, MapperService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSignalR();
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        ProblemDetailsExtensions.AddProblemDetails(services);
        services.AddValidationProblemDetails();
        services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.AddTransient(typeof(IRequestPreProcessor<>), typeof(AuthenticationPreProcessor<>));
        services.AddScoped(typeof(IRequestPreProcessor<>), typeof(SharedListRequestPreProcessor<>));
        services.AddScoped(typeof(IRequestPreProcessor<>), typeof(MyWishRequestPreProcessor<>));

        ConfigureOptions(services, config);

        if (environment.IsProduction()) services.Configure<HttpsRedirectionOptions>(options => options.HttpsPort = 443);
    }

    private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration) {
        services.AddOptions();

        var missingOptions = new List<string>();

        missingOptions.AddRange(ConfigureOptions<MailOptions>(services, configuration, "mail"));
        missingOptions.AddRange(ConfigureOptions<Auth0Settings>(services, configuration, "auth0"));

        if (missingOptions.Any()) throw new Exception("Missing settings: " + missingOptions.ToJoinedString());
    }

    private static IEnumerable<string> ConfigureOptions<T>(IServiceCollection services, IConfiguration configuration, string key, bool snapshot = false) where T : class, new() {
        var configurationSection = configuration.GetSection(key);
        var options = configurationSection.Get<T>();

        services.Configure<T>(configurationSection);

        if (snapshot) // Enable injection of updated strongly typed options
            services.AddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<T>>().Value);
        else
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<T>>().Value);

        var missing = typeof(T)
            .GetProperties()
            .Where(propertyInfo => propertyInfo.GetValue(options).ToStringOrEmpty().IsNullOrEmpty())
            .Select(propertyInfo => $"{key}:{propertyInfo.Name}");

        return missing;
    }

    public static void AddCustomAuth(this IServiceCollection services, IConfiguration configuration) {
        var authSettings = configuration.GetOrDie<Auth0Settings>("auth0");
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

    private static T GetOrDie<T>(this IConfiguration configuration, string key) {
        var settings = configuration.GetSection(key).Get<T>() ??
            throw new ConfigurationException(key);
        return settings;
    }

    private static Task OnMessageReceived(MessageReceivedContext context) {
        var accessToken = context.Request.Query["access_token"].FirstOrDefault();
        if (context.HttpContext.Request.Path.StartsWithSegments("/hub") && accessToken.IsNotEmpty())
            context.Token = accessToken;

        return Task.CompletedTask;
    }

    private static Task OnTokenValidated(TokenValidatedContext context) {
        if (context.SecurityToken is JsonWebToken jwtSecurityToken)
            context.HttpContext.Items["access_token"] = jwtSecurityToken.EncodedToken;

        return Task.CompletedTask;
    }

    public static void AddCustomMvc(this IServiceCollection services) {
        services.AddControllers(o => {
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(
                    new WhitelistDenyAnonymousAuthorizationRequirement("/serviceworker", "/offline.html"))
                .Build();
            o.Filters.Add(new AuthorizeFilter(policy));
            o.Filters.Add(new CustomExceptionFilterAttribute());
        })
            .AddHybridModelBinder();
    }

    public static void AddCustomSwagger(this IServiceCollection services, IConfiguration configuration) {
        var authSettings = configuration.GetOrDie<Auth0Settings>("auth0");
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
                            { "openid", "Standard openid scope" },
                            { "profile", "Standard openid scope" },
                            { "email", "Standard openid scope" }
                        }
                    }
                }
            });
            config.OperationFilter<SecurityRequirementsOperationFilter>();
        });
    }

    public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration.GetConnectionString("GaverContext");
        if (connectionString.IsNullOrEmpty()) throw new ConfigurationException("ConnectionStrings:GaverContext");

        services.AddDbContext<GaverContext>(options => {
            options.UseNpgsql(connectionString, b => b
                .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
                .SetPostgresVersion(11, 0));
        });
    }

    public static void ScanAssemblies(this IServiceCollection services) {
        services.Scan(scan => {
            scan.FromAssemblyOf<ICommonAssembly>()
                .AddServices();
            scan.FromAssemblyOf<IStartupAssembly>()
                .AddServices()
                .AddMappingProfiles();
        });
    }

    private static IImplementationTypeSelector AddMappingProfiles(this IImplementationTypeSelector selector) {
        return selector.AddClasses(classes => classes.AssignableTo<Profile>()).As<Profile>().WithSingletonLifetime();
    }

    private static IImplementationTypeSelector AddServices(
        this IImplementationTypeSelector implementationTypeSelector) {
        return implementationTypeSelector
            .AddClasses(classes =>
                classes.WithoutAttribute<SingletonServiceAttribute>().Where(c => c.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.WithAttribute<ServiceAttribute>()).AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.WithAttribute<SingletonServiceAttribute>()).AsImplementedInterfaces()
            .WithSingletonLifetime();
    }

    public static IServiceCollection AddValidationProblemDetails(this IServiceCollection services) {
        return services.Configure<ApiBehaviorOptions>(options => {
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
    }

    public static void AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment hostEnvironment) {
        if (hostEnvironment.IsEnvironment("Test")) return;

        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("GaverContext") ?? throw new ConfigurationException("ConnectionStrings:GaverContext"));
    }
}
