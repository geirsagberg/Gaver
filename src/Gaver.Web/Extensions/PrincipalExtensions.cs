using System.Linq;
using System.Security.Claims;

namespace Gaver.Web.Extensions
{
    public static class PrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var idClaim = principal.Claims.SingleOrDefault(c => c.Type == "GaverUserId");
            int userId;
            int.TryParse(idClaim?.Value, out userId);
            return userId;
        }
    }
}