using System;
using Gaver.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace Gaver.Web.Extensions;

public static class LoggerExtensions
{
    public static void LogErrorAndThrow(this ILogger logger, Exception e, string message)
    {
        logger.LogError(e, message);
        throw new FriendlyException(message);
    }
}