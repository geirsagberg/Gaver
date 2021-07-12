using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GaverControllerBase : Controller
    {
    }
}
