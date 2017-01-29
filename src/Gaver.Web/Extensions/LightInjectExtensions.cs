using System;
using System.Reflection;
using LightInject;
using Serilog.Events;

namespace Gaver.Web
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
            switch (logLevel) {
                case LogLevel.Info:
                    return LogEventLevel.Information;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                default:
                    throw new ArgumentException("Unknown loglevel");
            }
        }
    }
}