using System.Security.Claims;
using Gaver.Data.Entities;

namespace Gaver.Web.Tests;

public static class UserExtensions {
    public static ClaimsPrincipal GetClaimsPrincipal(this User user) {
        return new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, user.PrimaryIdentityId),
        ]));
    }
}
