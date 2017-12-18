using System;
using Gaver.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace Gaver.Web.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogErrorAndThrow(this ILogger logger, int eventId, Exception e, string message)
        {
            logger.LogError(eventId, e, message);
            throw new FriendlyException(eventId, message);
        }
    }
}