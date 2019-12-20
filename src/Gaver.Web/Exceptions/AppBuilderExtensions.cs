using Gaver.Web.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Gaver.Web.Exceptions
{
    public static class AppBuilderExtensions
    {
        //public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder app, Auth0Settings auth0Settings)
        //{
        //    return app.Use(async (context, next) => {
        //        AddAuthorizationHeaderFromQueryIfNecessary(context);
        //        await next();
        //    });
        //}

        //private static void AddAuthorizationHeaderFromQueryIfNecessary(HttpContext context)
        //{
        //    StringValues values;
        //    if (context.Request.Headers["Authorization"].IsNullOrEmpty()
        //        && context.Request.Query.TryGetValue("id_token", out values)) {
        //        var idToken = values.Single();
        //        context.Request.Headers.Add("Authorization", $"Bearer {idToken}");
        //    }
        //}

        public static IApplicationBuilder UseHttpException(this IApplicationBuilder application)
        {
            return application.UseMiddleware<HttpExceptionMiddleware>();
        }
    }
}
