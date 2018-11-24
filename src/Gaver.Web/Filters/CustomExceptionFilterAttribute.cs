using Gaver.Common.Exceptions;
using Gaver.Web.Constants;
using Gaver.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Gaver.Web.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is HttpException)
                return;
            var loggerFactory = (ILoggerFactory) context.HttpContext.RequestServices.GetService(typeof(ILoggerFactory));
            var logger = loggerFactory.CreateLogger("API Error");
            logger.LogError(EventIds.ApiError, context.Exception, "Error in " + context.HttpContext.Request.Path);

            if (context.HttpContext.Request.Path.ToUriComponent().ToLowerInvariant().Contains("/api/")) {
                var result = context.Exception is FriendlyException friendlyException
                    ? (object) new {
                        friendlyException.Message,
                        friendlyException.EventId
                    }
                    : new {
                        context.Exception.Message
                    };
                context.Result = new ObjectResult(result);
                context.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}