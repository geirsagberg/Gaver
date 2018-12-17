using System.Linq;
using System.Net;
using System.Security.Claims;
using Gaver.Web.Exceptions;

namespace Gaver.Web.Extensions
{
    public static class UserExtensions
    {
        //public static string GetPrimaryIdentityId(this int UserId)
        //{
        //    var primaryIdentityId = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //    if (primaryIdentityId == null) {
        //        throw new HttpException(HttpStatusCode.Unauthorized);
        //    }

        //    return primaryIdentityId;
        //}
    }
}
