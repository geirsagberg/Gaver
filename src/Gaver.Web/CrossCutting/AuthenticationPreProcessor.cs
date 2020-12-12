using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Exceptions;
using Gaver.Web.Contracts;
using Gaver.Web.Exceptions;
using Gaver.Web.MvcUtils;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.CrossCutting
{
    public class AuthenticationPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationPreProcessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (request is IAuthenticatedRequest authenticatedRequest) {
                var user = httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                    throw new HttpException(HttpStatusCode.Unauthorized);
                var userId = user.Claims.SingleOrDefault(c => c.Type == GaverClaimTypes.GaverUserId)?.Value;
                if (userId == null)
                    throw new FriendlyException("Ugyldig bruker");

                authenticatedRequest.UserId = int.Parse(userId);
            }
        }
    }
}
