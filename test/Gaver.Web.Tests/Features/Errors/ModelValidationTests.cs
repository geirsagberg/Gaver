using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Web.Features.MyList;
using Gaver.Web.Features.Wishes.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Gaver.Web.Tests.Features.Errors
{
    public class ModelValidationTests : WebTestBase
    {
        public ModelValidationTests(CustomWebApplicationFactory webAppFactory) : base(webAppFactory)
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
