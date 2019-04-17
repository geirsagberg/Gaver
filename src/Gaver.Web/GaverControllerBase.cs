using Gaver.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GaverControllerBase : Controller
    {
        //protected int UserId => User.GetUserId();
    }
}
