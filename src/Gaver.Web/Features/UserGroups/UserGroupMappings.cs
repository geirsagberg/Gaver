using System.Linq.Expressions;
using Gaver.Data.Entities;

namespace Gaver.Web.Features.UserGroups;

public static class UserGroupMappings {
    public static Expression<Func<UserGroup, UserGroupDto>> UserGroupDtoProjection { get; } = group =>
        new UserGroupDto {
            Id = group.Id,
            Name = group.Name,
            UserIds = group.UserGroupConnections.Select(connection => connection.UserId).ToList(),
            CreatedByUserId = group.CreatedByUserId
        };

    private static Func<UserGroup, UserGroupDto> MapUserGroup { get; } = UserGroupDtoProjection.Compile();

    public static UserGroupDto ToDto(UserGroup group) => MapUserGroup(group);
}
