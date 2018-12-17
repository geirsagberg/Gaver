using System.Linq;
using System.Security.Claims;
using Gaver.Common.Exceptions;
using Gaver.Web.Constants;

namespace Gaver.Web.Extensions
{
    public static class PrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var idClaim = principal.Claims.SingleOrDefault(c => c.Type == "GaverUserId");
            if (!int.TryParse(idClaim?.Value, out var userId)) {
                throw new FriendlyException(EventIds.UserNotRegistered, "Bruker-ID mangler. Vennligst last siden på nytt.");
            }
            return userId;
        }
    }
}
