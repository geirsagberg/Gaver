using Gaver.Web.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace Gaver.Web.Features.Home;

public class FeaturesController : GaverControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<FeatureFlags> Features([FromServices] IFeatureManager featureManager)
    {
        return new FeatureFlags {
            UserGroups = await featureManager.IsEnabledAsync(nameof(FeatureFlags.UserGroups)),
            WishOptions = await featureManager.IsEnabledAsync(nameof(FeatureFlags.WishOptions))
        };
    }
}
