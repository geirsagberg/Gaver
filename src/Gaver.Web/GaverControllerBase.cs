using Gaver.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web
{
    public abstract class GaverControllerBase : Controller
    {
        public int UserId => User.GetUserId();
    }
}