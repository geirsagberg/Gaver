using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Web.Features.MyList;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests.Features.Errors
{
    public class ModelValidationTests : WebTestBase
    {
        public ModelValidationTests(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper) :
            base(webAppFactory, testOutputHelper)
        {
        }

        [Fact]
        public async Task Validation_problems_are_camel_cased()
        {
            RoleConfig.AnonymousRequest = false;

            var response = await Client.PostAsJsonAsync("/api/MyList", new AddWishRequest());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var body = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<JObject>(body);
            var errors = (JObject) obj.Property("errors").Value;
            errors.Should().ContainKey("title");
        }
    }
}
