using System.Linq;
using AutoMapper;
using Gaver.Common.Extensions;
using Gaver.Data.Entities;
using Gaver.Web.Models;

namespace Gaver.Web.Features.Wishes
{
    public class WishMappingProfile : Profile
    {
        public WishMappingProfile()
        {
            CreateMap<WishList, SharedListModel>()
                .MapMember(m => m.OwnerUserId, wl => wl.UserId)
                .MapMember(m => m.Users,
                    wl => wl.Wishes.Select(w => w.BoughtByUser).Where(u => u != null).Concat(new[] {wl.User}));
            CreateMap<WishList, MyListModel>();
            CreateMap<Invitation, InvitationModel>();
            CreateMap<Wish, WishModel>();
            CreateMap<Wish, SharedWishModel>();
        }
    }
}
