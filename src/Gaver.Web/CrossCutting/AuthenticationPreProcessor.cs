using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Exceptions;
using Gaver.Web.Constants;
using Gaver.Web.Contracts;
using Gaver.Web.Exceptions;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.CrossCutting
{
    public class AuthenticationPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
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
                var userId = user.Claims.SingleOrDefault(c => c.Type == GaverClaimTypes.GaverUserId)?.Value;
                if (userId == null)
                    throw new FriendlyException(EventIds.UnknownUserId, "Ugyldig bruker");

                authenticatedRequest.UserId = int.Parse(userId);
            }

            return Task.CompletedTask;
        }
    }
}
