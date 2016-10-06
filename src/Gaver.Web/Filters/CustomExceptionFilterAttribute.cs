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
        public override void OnException(ExceptionContext context)
        {
            var loggerFactory = (ILoggerFactory) context.HttpContext.RequestServices.GetService(typeof(ILoggerFactory));
            var logger = loggerFactory.CreateLogger("API Error");
            logger.LogError(EventIds.ApiError, context.Exception, "Error in " + context.HttpContext.Request.Path);

            if (context.HttpContext.Request.Path.ToUriComponent().ToLowerInvariant().Contains("/api/"))
            {
                context.Result = new ObjectResult(new {
                    context.Exception.Message
                });
                context.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}