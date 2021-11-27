using System.Linq;
using AutoMapper;
using Gaver.Common.Extensions;
using Gaver.Data.Entities;
using Gaver.Web.Features.MyList;
using Gaver.Web.Features.Shared.Models;
using Gaver.Web.Features.UserGroups;

namespace Gaver.Web.Features.SharedList
{
    public class WishMappingProfile : Profile
    {
        public WishMappingProfile()
        {
            CreateMap<WishList, SharedListDto>()
                .MapMember(m => m.OwnerUserId, wl => wl.UserId)
                .MapMember(m => m.Users,
                    wl => wl.Wishes.Select(w => w.BoughtByUser).Where(u => u != null))
                .IgnoreMember(m => m.CanSeeMyList);
            CreateMap<WishList, MyListDto>();
            CreateMap<Wish, WishDto>();
            CreateMap<Wish, SharedWishDto>();
            CreateMap<WishOption, WishOptionDto>();
            CreateMap<UserGroup, UserGroupDto>()
                .MapMember(m => m.UserIds, m => m.Users.Select(c => c.Id));
        }
    }
}
