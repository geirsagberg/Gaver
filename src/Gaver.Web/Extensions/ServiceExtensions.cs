using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Gaver.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var descriptors = from type in assembly.ExportedTypes
                let typeInfo = type.GetTypeInfo()
                where !typeInfo.IsAbstract && !typeInfo.IsInterface
                from contract in type.GetInterfaces()
                select new ServiceDescriptor(contract, type, lifetime);

            foreach (var descriptor in descriptors) {
                services.Add(descriptor);
            }
            return services;
        }

        public static IServiceCollection AddAssembly(this IServiceCollection services, string assemblyName)
            => services.AddAssembly(Assembly.Load(new AssemblyName(assemblyName)));
    }
}