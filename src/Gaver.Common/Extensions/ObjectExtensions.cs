namespace Gaver.Common.Extensions;

public static class ObjectExtensions
{
    public static string ToStringOrEmpty(this object? obj) => obj?.ToString() ?? "";
}
