using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Gaver.Common.Extensions;
using Gaver.Web;
using Gaver.Web.Attributes;

namespace SharpTypeGen;

internal class Program
{
    private static void Main()
    {
        var types = typeof(AppConfig).Assembly.ExportedTypes.Where(t => !t.IsAbstract && !t.IsInterface &&
            (t.Name.EndsWith("Dto") || t.Name.EndsWith("Response") || t.Name.EndsWith("Request") ||
                t.HasAttribute<GenerateTypeScriptAttribute>()));
        const string destinationPath = "../../../../../frontend/src/types";
        Directory.CreateDirectory(destinationPath);
        using var textWriter = File.CreateText(destinationPath + "/data.d.ts");
        new TypeWriter()
            .FilterProperties(p => !p.GetCustomAttributes(true).Any(a => a is JsonIgnoreAttribute))
            .Write(types, textWriter);
    }
}
