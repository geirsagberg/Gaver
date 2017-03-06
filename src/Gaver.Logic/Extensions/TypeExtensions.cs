using System;
using System.Linq;
using System.Reflection;

namespace Gaver.Logic
{
    public static class TypeExtensions
    {
        public static bool Implements<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public static bool Implements<T>(this TypeInfo typeInfo)
        {
            return typeInfo.ImplementedInterfaces.Contains(typeof(T));
        }

        public static bool Implements(this Type type, Type interfaceType)
        {
            var interfaceTypeInfo = interfaceType.GetTypeInfo();
            if (!interfaceTypeInfo.IsInterface)
                throw new ArgumentException("Type must be an interface");

            var interfaces = type.GetInterfaces();

            if (interfaceTypeInfo.IsGenericType) {
                return interfaces.Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
            }
            else {
                return interfaces.Any(i => i == interfaceType);
            }
        }
    }
}