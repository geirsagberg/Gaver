using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Gaver.Web.Extensions
{
    public static class ObjectExtensions
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static HtmlString ToJsonHtml(this object obj) =>
            new(JsonConvert.SerializeObject(obj, JsonSerializerSettings));
    }
}
