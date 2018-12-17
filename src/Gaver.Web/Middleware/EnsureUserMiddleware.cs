using System.Threading.Tasks;
using Gaver.Data;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.Middleware
{
    //public class EnsureUserMiddleware
    //{
    //    private readonly RequestDelegate next;

    //    public EnsureUserMiddleware(RequestDelegate next)
    //    {
    //        this.next = next;
    //    }

    //    public async Task Invoke(HttpContext httpContext, GaverContext gaverContext)
    //    {
    //        if (httpContext?.User.Identity.IsAuthenticated == true) {
    //            var primaryIdentity = httpContext.User.Identity.Name;
    //            ;
    //        }

    //        await next(httpContext);
    //    }
    //}
}
