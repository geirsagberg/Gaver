using AutoMapper;
using Gaver.Common.Extensions;
using Gaver.Data.Entities;


namespace Gaver.Web.Features.Users
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, LoginUserModel>()
                .IgnoreMember<User, LoginUserModel>(u => u.WishListId);
        }
    }
}