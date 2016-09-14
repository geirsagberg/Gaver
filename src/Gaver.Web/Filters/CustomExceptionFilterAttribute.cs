using System;
using System.Diagnostics;
using Gaver.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Gaver.Web
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        // private readonly ILogger logger;

        public CustomExceptionFilterAttribute()
        {
            // this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            // logger.LogError(EventIds.ApiError, context.Exception, "Error in " + context.HttpContext.Request.Path);

            // Console.Error.WriteLine(context.Exception);

            if (context.HttpContext.Request.Path.ToUriComponent().Contains("/api/"))
            {
                context.Result = new ObjectResult(new {
                    context.Exception.Message
                });
                context.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}