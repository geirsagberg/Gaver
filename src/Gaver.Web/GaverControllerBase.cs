using Gaver.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web
{
    public abstract class GaverControllerBase : Controller
    {
        protected int UserId => User.GetUserId();
    }
}