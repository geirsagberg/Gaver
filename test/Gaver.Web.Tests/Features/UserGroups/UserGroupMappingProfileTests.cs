using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Gaver.Common.Utils;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.UserGroups;
using LightInject;
using Xunit;

namespace Gaver.Web.Tests.Features.UserGroups;

public class UserGroupMappingProfileTests : TestBase<MapperService> {
    public UserGroupMappingProfileTests() {
        Container.Register<IEnumerable<Profile>>(factory => [
            factory.Create<UserGroupMappingProfile>()
        ]);
    }

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

        var model = TestSubject.Map<UserGroupDto>(userGroup);

        model.Should().BeEquivalentTo(new UserGroupDto {
            Id = 1,
            Name = "Familien",
            UserIds = { 3 },
            CreatedByUserId = 2
        });
    }
}
