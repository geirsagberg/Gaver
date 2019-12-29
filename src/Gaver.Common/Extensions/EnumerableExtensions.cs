using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Gaver.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static T SingleOrThrow<T>(this IEnumerable<T> enumerable, Exception exception)
        {
            var result = enumerable.SingleOrDefault();
            if (result == null) throw exception;
            return result;
        }

        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
            => enumerable == null || !enumerable.Any();

        public static string ToJoinedString<T>(this IEnumerable<T> enumerable, string separator = ", ")
        {
            return string.Join(separator, enumerable);
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? enumerable) => enumerable ?? new T[0];

        public static bool In<T>(this T value, [NotNullWhen(true)] IEnumerable<T>? enumerable) =>
            enumerable?.Contains(value) == true;

        public static bool NotIn<T>(this T value, [NotNullWhen(false)] IEnumerable<T>? enumerable) =>
            enumerable?.Contains(value) != true;

        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) =>
            !enumerable.Any(predicate);
    }
}
