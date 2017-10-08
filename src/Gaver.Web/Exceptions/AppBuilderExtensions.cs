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
            return app.Use(async (context, next) => {
                AddAuthorizationHeaderFromQueryIfNecessary(context);
                await next();
            });
        }

        private static void AddAuthorizationHeaderFromQueryIfNecessary(HttpContext context)
        {
            StringValues values;
            if (context.Request.Headers["Authorization"].IsNullOrEmpty()
                && context.Request.Query.TryGetValue("id_token", out values)) {
                var idToken = values.Single();
                context.Request.Headers.Add("Authorization", $"Bearer {idToken}");
            }
        }

        public static IApplicationBuilder UseHttpException(this IApplicationBuilder application)
        {
            return application.UseMiddleware<HttpExceptionMiddleware>();
        }
    }
}