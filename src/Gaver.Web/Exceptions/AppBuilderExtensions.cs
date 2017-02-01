using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gaver.Logic;
using Gaver.Logic.Extensions;
using Gaver.Web.Features.Users;
using Gaver.Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Gaver.Web.Exceptions
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder app, Auth0Settings auth0Settings)
        {
            var options = CreateJwtBearerOptions(auth0Settings);
            return app.Use(async (context, next) => {
                AddAuthorizationHeaderFromQueryIfNecessary(context);
                await next();
            }).UseJwtBearerAuthentication(options);
        }

        private static JwtBearerOptions CreateJwtBearerOptions(Auth0Settings auth0Settings) => new JwtBearerOptions {
            Audience = auth0Settings.ClientId,
            Authority = $"https://{auth0Settings.Domain}",
            TokenValidationParameters = {
                IssuerSigningKey = auth0Settings.SigningKey
            },
            // TODO Handle other events, e.g. OnAuthenticationFailed and OnChallenge
            Events = new JwtBearerEvents {
                OnTokenValidated = OnTokenValidated
            }
        };

        private static void AddAuthorizationHeaderFromQueryIfNecessary(HttpContext context)
        {
            StringValues values;
            if (context.Request.Headers["Authorization"].IsNullOrEmpty()
                && context.Request.Query.TryGetValue("id_token", out values)) {
                var idToken = values.Single();
                context.Request.Headers.Add("Authorization", $"Bearer {idToken}");
            }
        }

        private static async Task OnTokenValidated(TokenValidatedContext tokenContext)
        {
            var identity = tokenContext.Ticket.Principal.Identity as ClaimsIdentity;
            var providerId = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (providerId == null) return;

            var userHandler = tokenContext.HttpContext.RequestServices.GetRequiredService<UserHandler>();

            var userId = await userHandler.GetUserIdOrNullAsync(providerId);
            if (userId != null) {
                identity.AddClaim(new Claim("GaverUserId", userId.Value.ToString(), ClaimValueTypes.Integer32));
            }
        }

        public static IApplicationBuilder UseHttpException(this IApplicationBuilder application)
        {
            return application.UseMiddleware<HttpExceptionMiddleware>();
        }
    }
}