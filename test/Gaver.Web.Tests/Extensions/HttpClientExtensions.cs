using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Gaver.Web.Tests.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value,
            CancellationToken cancellationToken = default) => client.PatchAsync(requestUri,
            new ObjectContent<T>(value, new JsonMediaTypeFormatter(), new MediaTypeHeaderValue("application/json")),
            cancellationToken);
    }
}
