using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gaver.Web.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Gaver.Web.MvcUtils
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly IMediator mediator;

        public ClaimsTransformer(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var providerId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (providerId == null) {
                return principal;
            }

            if (principal.HasClaim(c => c.Type == GaverClaimTypes.GaverUserId)) {
                return principal;
            }

            var transformed = new ClaimsPrincipal();
            transformed.AddIdentities(principal.Identities);

            var user = await mediator.Send(new GetOrCreateUserRequest(providerId));
            var gaverIdentity = new ClaimsIdentity(new[] {
                new Claim(GaverClaimTypes.GaverUserId, user.Id.ToString(), ClaimValueTypes.Integer32)
            });
            transformed.AddIdentity(gaverIdentity);
            return transformed;
        }
    }
}
