using System.Linq;
using System.Reflection;

namespace Gaver.Logic
{
    public static class TypeExtensions
    {
        public static bool Implements<T>(this TypeInfo typeInfo)
        {
            return typeInfo.ImplementedInterfaces.Contains(typeof(T));
        }
    }
}