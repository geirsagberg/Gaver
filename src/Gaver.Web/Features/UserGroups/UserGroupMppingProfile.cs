using AutoMapper;
using Gaver.Common.Extensions;
using Gaver.Data.Entities;

namespace Gaver.Web.Features.UserGroups;

public class UserGroupMppingProfile : Profile
{
    public UserGroupMppingProfile()
    {
        CreateMap<UserGroup, UserGroupDto>()
            .MapMember(m => m.UserIds, m => m.UserGroupConnections.Select(c => c.UserId));
    }
}
