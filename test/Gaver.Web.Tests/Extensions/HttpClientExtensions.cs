using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Gaver.Web.Tests.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value,
            CancellationToken cancellationToken = default) =>
            client.PatchAsync(requestUri, JsonContent.Create(value), cancellationToken);
    }
}
