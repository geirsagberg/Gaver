namespace Gaver.Data;

public static class ObjectExtensions
{
    public static string ToStringOrEmpty(this object? obj) => obj?.ToString() ?? "";
}
