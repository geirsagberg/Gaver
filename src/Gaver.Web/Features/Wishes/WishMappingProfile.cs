using AutoMapper;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Web.Features.Wishes.Models;

namespace Gaver.Web.Features.Wishes
{
    public class WishMappingProfile : Profile
    {
        public WishMappingProfile()
        {
            CreateMap<WishList, SharedListModel>().MapMember(m => m.Owner, wl => wl.User.Name);
        }
    }
}