using AutoMapper;
using Gaver.Common.Extensions;
using Gaver.Data.Entities;


namespace Gaver.Web.Features.Users
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, CurrentUserDto>()
                .MapMember(m => m.WishListId, u => u.WishList!.Id);

            CreateMap<User, UserDto>();
        }
    }
}
