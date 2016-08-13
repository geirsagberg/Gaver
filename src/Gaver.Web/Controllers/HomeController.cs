using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;

namespace Gaver.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext hub;

        public HomeController(IConnectionManager signalRManager)
        {
            hub = signalRManager.GetHubContext<ListHub>();
        }

        public IActionResult Index()
        {
            hub.Clients.All.hello("World!");
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
