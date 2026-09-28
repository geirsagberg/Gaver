using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.Web.Features.UserGroups;
using Xunit;

namespace Gaver.Web.Tests.Features.UserGroups;

public class UserGroupMappingsTests {
    [Fact]
    public void UserGroup_is_mapped_correctly() {
        var userGroup = new UserGroup {
            Id = 1,
            Name = "Familien",
            CreatedByUserId = 2,
            UserGroupConnections = {
                new UserGroupConnection {
                    UserId = 3
                }
            }
        };

        var model = UserGroupMappings.ToDto(userGroup);

        model.Should().BeEquivalentTo(new UserGroupDto {
            Id = 1,
            Name = "Familien",
            UserIds = { 3 },
            CreatedByUserId = 2
        });
    }
}
