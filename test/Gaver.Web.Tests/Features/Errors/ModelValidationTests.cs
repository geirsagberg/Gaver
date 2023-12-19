using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Web.Features.MyList;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests.Features.Errors;

public class ModelValidationTests(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper) : WebTestBase(webAppFactory, testOutputHelper) {
    [Fact]
    public async Task Validation_problems_are_camel_cased() {
        RoleConfig.AnonymousRequest = false;

        var response = await Client.PostAsJsonAsync("/api/MyList", new AddWishRequest());

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<JsonElement>(body);
        var errors = obj.GetProperty("errors");
        errors.ValueKind.Should().Be(JsonValueKind.Object);
        errors.EnumerateObject().Should().Contain(p => p.Name == "title");
    }
}
