using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Web.Contracts;
using Gaver.Web.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.CrossCutting
{
    public class AuthenticationPreProcessor<TRequest> : MediatR.Pipeline.IRequestPreProcessor<TRequest>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationPreProcessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (request is IAuthenticatedRequest authenticatedRequest) {
                var user = httpContextAccessor.HttpContext.User;
                if (!user.Identity.IsAuthenticated)
                    throw new HttpException(HttpStatusCode.Unauthorized);
                //authenticatedRequest.User = user;
            }

            return Task.CompletedTask;
        }
    }
}
