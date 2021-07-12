using System.IO;
using System.Linq;
using Gaver.Common.Extensions;
using Gaver.Web;
using Gaver.Web.Attributes;
using System.Text.Json.Serialization;

namespace SharpTypeGen
{
    internal class Program
    {
        private static void Main()
        {
            var types = typeof(Startup).Assembly.ExportedTypes.Where(t => !t.IsAbstract && !t.IsInterface &&
                (t.Name.EndsWith("Dto") || t.Name.EndsWith("Response") || t.Name.EndsWith("Request") ||
                    t.HasAttribute<GenerateTypeScriptAttribute>()));
            const string destinationPath = "../../../../Gaver.Web/ClientApp/types";
            Directory.CreateDirectory(destinationPath);
            using var textWriter = File.CreateText(destinationPath + "/data.d.ts");
            new TypeWriter()
                .FilterProperties(p => !p.GetCustomAttributes(true).Any(a => a is JsonIgnoreAttribute))
                .Write(types, textWriter);
        }
    }
}
