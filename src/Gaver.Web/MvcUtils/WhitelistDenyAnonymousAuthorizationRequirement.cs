using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gaver.Web.MvcUtils;

internal class WhitelistDenyAnonymousAuthorizationRequirement : DenyAnonymousAuthorizationRequirement
{
    private readonly string[] whitelistedPaths;

    public WhitelistDenyAnonymousAuthorizationRequirement(params string[] whitelistedPaths)
    {
        this.whitelistedPaths = whitelistedPaths;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        DenyAnonymousAuthorizationRequirement requirement)
    {
        if (context.Resource is AuthorizationFilterContext authorizationFilterContext) {
            var path = authorizationFilterContext.HttpContext.Request.Path;
            var isPathWhitelisted = whitelistedPaths
                .Any(x => x.Equals(path, StringComparison.OrdinalIgnoreCase));
            if (isPathWhitelisted) {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }

        return base.HandleRequirementAsync(context, requirement);
    }
}