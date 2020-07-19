using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gaver.Common.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace Gaver.Web.Features.Home
{
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            mediator.Send(new WakeDatabaseRequest()).Forget();
            return View();
        }

        [AllowAnonymous]
        [HttpGet("/features")]
        public async Task<Dictionary<Feature, bool>> Features([FromServices] IFeatureManager featureManager)
        {
            var features = Enum.GetValues(typeof(Feature)).Cast<Feature>().ToList();

            var isEnabled = await Task.WhenAll(features.Select(f => featureManager.IsEnabledAsync(f.ToString())));

            return features.Zip(isEnabled).ToDictionary(i => i.First, i => i.Second);
        }
    }
}
