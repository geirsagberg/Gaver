using System.IO;
using System.Linq;
using Gaver.Web;
using Newtonsoft.Json;

namespace SharpTypeGen
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var types = typeof(Startup).Assembly.ExportedTypes.Where(t => !t.IsAbstract && !t.IsInterface &&
                (t.Name.EndsWith("Dto") || t.Name.EndsWith("Response") || t.Name.EndsWith("Request")));
            const string destinationPath = "../../../../Gaver.Web/ClientApp/types";
            Directory.CreateDirectory(destinationPath);
            using var textWriter = File.CreateText(destinationPath + "/data.d.ts");
            new TypeWriter()
                .FilterProperties(p => !p.GetCustomAttributes(true).Any(a => a is JsonIgnoreAttribute))
                .Write(types, textWriter);
        }
    }
}
