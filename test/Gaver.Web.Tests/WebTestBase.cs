using System.Net.Http;
using Xunit;

namespace Gaver.Web.Tests
{
    public abstract class WebTestBase : IClassFixture<CustomWebApplicationFactory>
    {
        protected WebTestBase(CustomWebApplicationFactory webAppFactory)
        {
            Client = webAppFactory.CreateClient();
        }

        protected HttpClient Client { get; }
    }
}
