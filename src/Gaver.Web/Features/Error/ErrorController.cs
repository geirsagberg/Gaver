using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Error
{
    public class ErrorController : Controller
    {
        public new ActionResult NotFound() => base.NotFound();
    }
}
