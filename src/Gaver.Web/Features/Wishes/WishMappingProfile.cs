using AutoMapper;
using Gaver.Common.Extensions;
using Gaver.Data.Entities;
using Gaver.Web.Features.Wishes.Models;

namespace Gaver.Web.Features.Wishes
{
    public class WishMappingProfile : Profile
    {
        public WishMappingProfile()
        {
            CreateMap<WishList, SharedListModel>().MapMember(m => m.Owner, wl => wl.User.Name);
            CreateMap<WishList, MyListModel>().IgnoreMember(m => m.Invitations);
            CreateMap<Invitation, InvitationModel>();
            CreateMap<Wish, WishModel>();
            CreateMap<Wish, SharedWishModel>();
        }
    }
}
