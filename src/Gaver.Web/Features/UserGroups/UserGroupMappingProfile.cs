using AutoMapper;
using Gaver.Common.Extensions;
using Gaver.Data.Entities;

namespace Gaver.Web.Features.UserGroups;

public class UserGroupMappingProfile : Profile
{
    public UserGroupMappingProfile()
    {
        CreateMap<UserGroup, UserGroupDto>()
            .MapMember(m => m.UserIds, m => m.UserGroupConnections.Select(c => c.UserId));
    }
}
