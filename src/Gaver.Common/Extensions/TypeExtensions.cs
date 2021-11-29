using System;
using System.Linq;
using System.Reflection;

namespace Gaver.Common.Extensions;

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

        return interfaceTypeInfo.IsGenericType
            ? interfaces.Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
            : interfaces.Any(i => i == interfaceType);
    }

    public static bool HasAttribute<T>(this Type type) => type.GetCustomAttribute(typeof(T)) != null;
}