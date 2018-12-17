using System.Threading.Tasks;
using Gaver.Data;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.Middleware
{
    public class EnsureUserMiddleware
    {
        private readonly RequestDelegate next;
        private readonly GaverContext gaverContext;

        public EnsureUserMiddleware(RequestDelegate next, GaverContext gaverContext)
        {
            this.next = next;
            this.gaverContext = gaverContext;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext?.User.Identity.IsAuthenticated == true) {
                var primaryIdentity = httpContext.User.Identity.Name;
                ;
            }
        }
    }
}
