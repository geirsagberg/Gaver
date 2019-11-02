using System;
using System.Reflection;
using LightInject;
using Serilog.Events;

namespace Gaver.Web.Extensions
{
    public static class LightInjectExtensions
    {
        public static IServiceContainer RegisterAssembly<T>(this IServiceContainer container)
        {
            container.RegisterAssembly(typeof(T).GetTypeInfo().Assembly);
            return container;
        }

        public static IServiceContainer RegisterAssembly<T>(this IServiceContainer container, Func<Type, Type, bool> shouldRegister)
        {
            container.RegisterAssembly(typeof(T).GetTypeInfo().Assembly, shouldRegister);
            return container;
        }

        public static LogEventLevel ToSerilogEventLevel(this LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Info => LogEventLevel.Information,
                LogLevel.Warning => LogEventLevel.Warning,
                _ => throw new ArgumentException("Unknown loglevel"),
            };
        }
    }
}
