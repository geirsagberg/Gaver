using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Web.CrossCutting;
using Gaver.Web.Exceptions;
using Gaver.Web.Features.Users;
using Gaver.Web.Filters;
using Gaver.Web.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        public static void AddCustomAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("auth0").Get<Auth0Settings>();
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
        }

        public static void AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc(o => {
                o.Filters.Add(new CustomExceptionFilterAttribute());
                o.Filters.Add<PipelineActionFilter>();
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
                            .MigrationsAssembly(Assembly.GetCallingAssembly().FullName));
                });
        }
    }
}