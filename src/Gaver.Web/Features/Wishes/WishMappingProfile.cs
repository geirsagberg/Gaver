using AutoMapper;
using Flurl;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Web.Features.Wishes.Models;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.Features.Wishes
{
    public class WishMappingProfile : Profile
    {
        public WishMappingProfile(IHttpContextAccessor httpContextAccessor)
        {
            CreateMap<WishList, SharedListModel>().MapMember(m => m.Owner, wl => wl.User.Name);
            CreateMap<WishList, MyListModel>().IgnoreMember(m => m.Invitations);
            CreateMap<Invitation, InvitationModel>()
                .IgnoreMember(m => m.WishListUrl)
                .AfterMap((i, m) => {
                    var request = httpContextAccessor.HttpContext.Request;
                    m.WishListUrl = Url.Combine(request.Scheme + "://" + request.Host, "list", i.WishListId.ToString());
                });
        }
    }
}