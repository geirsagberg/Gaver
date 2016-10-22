using System;
using System.Collections.Generic;
using LightInject;
using Newtonsoft.Json.Serialization;

namespace Gaver.Web.Utils
{
    internal class PropertyInjectionDisabler : IPropertyDependencySelector
    {
        public IEnumerable<PropertyDependency> Execute(Type type)
        {
            Console.WriteLine($"Requesting dependencies for type {type.Name}");
            return new PropertyDependency[0];
        }
    }
}