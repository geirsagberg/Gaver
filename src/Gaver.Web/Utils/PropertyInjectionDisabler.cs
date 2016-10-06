using System;
using System.Collections.Generic;
using LightInject;

namespace Gaver.Web.Utils
{
    internal class PropertyInjectionDisabler : IPropertyDependencySelector
    {
        public IEnumerable<PropertyDependency> Execute(Type type)
        {
            return new PropertyDependency[0];
        }
    }
}