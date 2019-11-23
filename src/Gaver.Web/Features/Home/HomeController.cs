using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace Gaver.Web.Features.Home
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("/features")]
        public Dictionary<Feature, bool> Features([FromServices] IFeatureManager featureManager)
        {
            return Enum.GetValues(typeof(Feature)).Cast<Feature>()
                .ToDictionary(f => f, f => featureManager.IsEnabled(f.ToString()));
        }
    }
}
