using System.Linq;
using AutoMapper;
using Gaver.Common.Extensions;
using Gaver.Data.Entities;


namespace Gaver.Web.Features.Users
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, CurrentUserModel>()
                .MapMember(m => m.WishListId, u => u.WishLists.First().Id);

            CreateMap<User, UserModel>();
        }
    }
}
