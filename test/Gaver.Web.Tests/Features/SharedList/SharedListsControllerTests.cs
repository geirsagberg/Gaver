using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests.Features.SharedList;

public class SharedListsControllerTests : WebTestBase
{
    public SharedListsControllerTests(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper)
        : base(webAppFactory, testOutputHelper)
    {
    }

    [Fact]
    public async Task Cannot_read_own_sharedList()
    {
        var user = new User {
            Name = "Alice",
            PrimaryIdentityId = "1"
        };
        GaverContext.Add(user);
        GaverContext.SaveChanges();
        SetAuthenticatedUser(user);

        var result = await Client.GetAsync($"/api/SharedLists/{user.WishList!.Id}");

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
