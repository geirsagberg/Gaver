using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gaver.Web
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new JsonResult(context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                context.HttpContext.Response.StatusCode = 400;
            }
        }
    }
}