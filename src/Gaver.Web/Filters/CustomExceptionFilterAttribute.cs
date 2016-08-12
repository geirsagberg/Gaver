using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gaver.Web
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute {
        public override void OnException(ExceptionContext context) {
        if (context.HttpContext.Request.Path.ToUriComponent().Contains("/api/")) {
                context.Result = new JsonResult(context.Exception.Message);
            }
        }
    }
}