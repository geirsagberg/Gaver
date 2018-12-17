using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Gaver.Web.Tests.Features.Home
{
    public class HomeControllerTests : WebTestBase
    {
        public HomeControllerTests(CustomWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
        }

        [Fact]
        public async Task Can_call_Index()
        {
            var result = await Client.GetAsync("/");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
