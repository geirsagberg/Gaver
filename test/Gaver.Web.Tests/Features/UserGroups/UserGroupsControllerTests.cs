using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Web.Features.UserGroups;
using Gaver.Web.MvcUtils;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests.Features.UserGroups
{
    public class UserGroupsControllerTests : WebTestBase
    {
        public UserGroupsControllerTests(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper)
            : base(webAppFactory, testOutputHelper)
        {
        }

        [Fact]
        public async Task Can_create_and_read_userGroups()
        {
            RoleConfig.AddClaim(GaverClaimTypes.GaverUserId, "1");
            var request = new CreateUserGroupRequest {
                Name = "Familien"
            };

            var result = await Client.PostAsJsonAsync("/api/userGroups", request);

            result.IsSuccessStatusCode.Should().BeTrue();
            var userGroup = await result.Content.ReadAsAsync<UserGroupDto>();
            userGroup.Name.Should().Be("Familien");
            userGroup.Id.Should().BeGreaterThan(0);

            result = await Client.GetAsync("/api/userGroups");

            result.IsSuccessStatusCode.Should().BeTrue();
            var userGroups = await result.Content.ReadAsAsync<UserGroupsDto>();
            userGroups.UserGroups.Single().Should().BeEquivalentTo(userGroup);

        }
    }
}
