using AutoMapper;
using Gaver.Data.Entities;
using Gaver.Logic;

namespace Gaver.Web.Features.Users
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, LoginUserModel>()
                .IgnoreMember(u => u.WishListId);
        }
    }
}