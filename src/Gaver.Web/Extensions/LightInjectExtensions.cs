using System;
using System.Reflection;
using LightInject;

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
    }
}