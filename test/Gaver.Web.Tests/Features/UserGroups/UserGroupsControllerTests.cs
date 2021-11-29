using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Features.UserGroups;
using Gaver.Web.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests.Features.UserGroups;

public class UserGroupsControllerTests : WebTestBase
{
    private readonly User me;
    private readonly User someoneElse;

    public UserGroupsControllerTests(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper)
        : base(webAppFactory, testOutputHelper)
    {
        SetAuthenticatedUser(1);
        me = new User {
            Id = 1,
            Name = "Me",
            PrimaryIdentityId = "1"
        };
        someoneElse = new User {
            Id = 2,
            Name = "Someone else",
            PrimaryIdentityId = "2"
        };
        GaverContext.AddRange(me, someoneElse);
        GaverContext.SaveChanges();
    }

    private async Task<UserGroupDto> AddUserGroup()
    {
        var createUserGroupRequest = new {
            Name = "Familien"
        };
        var createUserGroupResult = await Client.PostAsJsonAsync("/api/userGroups", createUserGroupRequest);
        var userGroupDto = await createUserGroupResult.Content.ReadAsAsync<UserGroupDto>();
        return userGroupDto;
    }

    [Fact]
    public async Task Can_create_and_read_userGroups()
    {
        var request = new {
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

    [Fact]
    public async Task Can_update_userGroup_name_and_members_independently()
    {
        var userGroupDto = await AddUserGroup();
        var updateUserGroupRequest = new {
            Name = "Venner"
        };
        var updateResult =
            await Client.PatchAsJsonAsync($"/api/userGroups/{userGroupDto.Id}", updateUserGroupRequest);
        updateResult.IsSuccessStatusCode.Should().BeTrue();


        var userGroup = await GaverContext.GetOrDieAsync<UserGroup>(userGroupDto.Id);
        userGroup.Name.Should().Be("Venner");

        await Client.PatchAsJsonAsync($"/api/userGroups/{userGroupDto.Id}", new { UserIds = new[] { 1, 2 } });

        GaverContext.Reset();
        userGroup = await GaverContext.UserGroups.Include(u => u.UserGroupConnections)
            .SingleAsync(u => u.Id == userGroupDto.Id);
        userGroup.Name.Should().Be("Venner");
        userGroup.UserGroupConnections.Select(c => c.UserId).Should().BeEquivalentTo(new[] { 1, 2 });
    }

    [Fact]
    public async Task Cannot_remove_oneself_from_group()
    {
        var userGroupDto = await AddUserGroup();

        var result = await Client.PatchAsJsonAsync($"/api/userGroups/{userGroupDto.Id}", new { UserIds = new[] { 2 } });

        result.IsSuccessStatusCode.Should().BeFalse();
    }

    [Fact]
    public async Task Cannot_edit_group_where_not_member()
    {
        var userGroup = new UserGroup {
            Name = "Familien",
            CreatedByUser = someoneElse,
            Users = {
                someoneElse
            }
        };
        GaverContext.Add(userGroup);
        GaverContext.SaveChanges();

        var result = await Client.PatchAsJsonAsync($"/api/userGroups/{userGroup.Id}", new { Name = "Venner" });

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
