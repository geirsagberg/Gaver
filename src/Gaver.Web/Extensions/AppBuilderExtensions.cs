using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Logic;
using Gaver.Logic.Extensions;
using Gaver.Web.Features.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Gaver.Web.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder app, Auth0Settings auth0Settings)
        {
            app.Use(async (context, next) =>
            {
                StringValues values;
                if (context.Request.Headers["Authorization"].IsNullOrEmpty()
                    && context.Request.Query.TryGetValue("id_token", out values))
                {
                    var idToken = values.Single();
                    context.Request.Headers.Add("Authorization", $"Bearer {idToken}");
                }

                await next();
            });

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(auth0Settings.ClientSecret));

            return app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Audience = auth0Settings.ClientId,
                Authority = $"https://{auth0Settings.Domain}",
                TokenValidationParameters = {
                    IssuerSigningKey = key
                },
                Events = new JwtBearerEvents
                {
                    OnTokenValidated = context => OnTokenValidatedAsync(context, auth0Settings)
                }
            });
        }

        private static async Task OnTokenValidatedAsync(TokenValidatedContext tokenContext, Auth0Settings settings)
        {
            var identity = tokenContext.Ticket.Principal.Identity as ClaimsIdentity;
            if (identity == null) return;
            var providerId = identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (providerId == null) return;

            var jwtToken = (JwtSecurityToken)tokenContext.SecurityToken;
            var idToken = jwtToken.RawData;
            var userHandler = tokenContext.HttpContext.RequestServices.GetRequiredService<UserHandler>();

            var user = await userHandler.EnsureUserAsync(providerId, idToken);

            identity.AddClaim(new Claim("GaverUserId", user.Id.ToString(), ClaimValueTypes.Integer32));
        }
    }
}