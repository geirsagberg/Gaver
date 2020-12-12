//MIT License
//
//Copyright (c) 2019 Miron Jakubowski
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SharpTypeGen
{
    public sealed class TypeWriter
    {
        private static readonly Dictionary<Type, string> Types = new() {
            { typeof(Guid), "string" },
            { typeof(string), "string" },
            { typeof(char), "string" },
            { typeof(bool), "boolean" },
            { typeof(int), "number" },
            { typeof(uint), "number" },
            { typeof(long), "number" },
            { typeof(ulong), "number" },
            { typeof(float), "number" },
            { typeof(double), "number" },
            { typeof(short), "number" },
            { typeof(ushort), "number" },
            { typeof(byte), "number" },
            { typeof(sbyte), "number" },
            { typeof(decimal), "number" },
            { typeof(DateTime), "string" },
            { typeof(DateTimeOffset), "string" }
        };

        private readonly List<string> duplicatesGuard;

        private readonly List<Func<PropertyInfo, bool>> filters = new();

        public TypeWriter()
        {
            duplicatesGuard = new List<string>();
        }

        public TypeWriter FilterProperties(Func<PropertyInfo, bool> predicate)
        {
            filters.Add(predicate);
            return this;
        }

        public void Write(IEnumerable<Type> types, TextWriter textWriter)
        {
            foreach (var type in types) Write(type, textWriter);
        }

        public void Write(Type type, TextWriter textWriter)
        {
            var nestedTypes = new List<Type>();

            var classType = GetClassType(type);
            if (classType?.FullName == null) return;

            if (duplicatesGuard.Contains(classType.FullName)) return;

            duplicatesGuard.Add(classType.FullName);

            var properties = classType.GetProperties();

            if (filters.Any()) properties = properties.Where(p => filters.All(f => f(p))).ToArray();

            if (properties.Length == 0) return;

            textWriter.Write($"export interface {classType.Name} {{ ");

            foreach (var property in properties) {
                var propName = char.ToLowerInvariant(property.Name[0]) + property.Name[1..];

                var (symbol, nullable) = GetTypeSymbol(property.PropertyType);
                textWriter.Write($"{propName}{(nullable ? "?" : "")}: {symbol}; ");

                var nestedType = GetClassType(property.PropertyType);
                if (nestedType != null && !nestedTypes.Contains(nestedType)) nestedTypes.Add(nestedType);
            }

            textWriter.Write("}\n");

            Write(nestedTypes, textWriter);
        }

        private static bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

        private static Type? GetClassType(Type? type)
        {
            while (true) {
                if (type == null) return null;

                if (type.IsArray) {
                    type = type.GetElementType();
                    continue;
                }

                if ((!typeof(IEnumerable).IsAssignableFrom(type) || type.GenericTypeArguments.Length != 1) &&
                    (type.BaseType != typeof(Task) || !type.IsGenericType))
                    return IsUserClass(type) ? type : null;

                type = type.GenericTypeArguments[0];
            }
        }

        private static (string symbol, bool nullable) GetTypeSymbol(Type type)
        {
            if (Types.ContainsKey(type))
                return (Types[type], false);

            if (type.IsArray)
                return ($"{GetTypeSymbol(type.GetElementType()!).symbol}[]", false);

            if (typeof(IEnumerable).IsAssignableFrom(type) && type.GenericTypeArguments.Length == 1)
                return ($"{GetTypeSymbol(type.GenericTypeArguments[0]).symbol}[]", false);

            if (IsNullable(type))
                return (GetTypeSymbol(Nullable.GetUnderlyingType(type)!).symbol, true);

            return IsUserClass(type) ? (type.Name, false) : ("any", true);
        }

        private static bool IsUserClass(Type type)
        {
            if (type == typeof(string) || type == typeof(object)) return false;

            return !type.IsPrimitive && !type.IsGenericType && type.IsClass;
        }
    }
}
