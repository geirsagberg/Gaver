using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Home
{
    public class HomeController : Controller
    {
	public IActionResult Index()
	{
	    return View();
	}

	public IActionResult Error()
	{
	    return View();
	}
    }
}
