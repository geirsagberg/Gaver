using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Extensions;
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
            if (identity != null)
            {
                var providerIdClaim = identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (providerIdClaim != null)
                {
                    var providerId = providerIdClaim.Value;
                    var gaverContext = tokenContext.HttpContext.RequestServices.GetRequiredService<GaverContext>();
                    var user = gaverContext.Users.SingleOrDefault(u => u.PrimaryIdentityId == providerId);
                    if (user == null)
                    {
                        var jwtToken = (JwtSecurityToken)tokenContext.SecurityToken;
                        dynamic userInfo = await $"https://{settings.Domain}/tokeninfo".PostJsonAsync(new
                        {
                            id_token = jwtToken.RawData
                        }).ReceiveJson();
                        string name = userInfo.name;
                        string email = userInfo.email;
                        user = new User
                        {
                            PrimaryIdentityId = providerId,
                            Name = name,
                            Email = email
                        };
                        gaverContext.Users.Add(user);
                        await gaverContext.SaveChangesAsync();
                    }
                    identity.AddClaim(new Claim("GaverUserId", user.Id.ToString(), ClaimValueTypes.Integer32));
                }
            }
        }
    }
}