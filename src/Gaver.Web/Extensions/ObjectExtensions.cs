using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace Gaver.Web.Extensions
{
    public static class ObjectExtensions
    {
        public static HtmlString ToJsonHtml(this object obj) => new HtmlString(JsonConvert.SerializeObject(obj));
    }
}
