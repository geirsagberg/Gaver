using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace Gaver.Web.Features.Home
{
    public class FeaturesController : GaverControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<Dictionary<Feature, bool>> Features([FromServices] IFeatureManager featureManager)
        {
            var features = Enum.GetValues(typeof(Feature)).Cast<Feature>().ToList();

            var isEnabled = await Task.WhenAll(features.Select(f => featureManager.IsEnabledAsync(f.ToString())));

            return features.Zip(isEnabled).ToDictionary(i => i.First, i => i.Second);
        }
    }
}
