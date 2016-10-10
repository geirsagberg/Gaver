using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Wishes
{
    public class WishReader :
        IRequestHandler<GetMyListRequest, MyListModel>,
        IRequestHandler<GetSharedListRequest, SharedListModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public WishReader(GaverContext context, IMapperService mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public MyListModel Handle(GetMyListRequest message)
        {
            var wishList = context.Set<WishList>().Include(wl => wl.Wishes).Single(wl => wl.User.Name == message.UserName);
            var wishModels = mapper.Map<IList<WishModel>>(wishList.Wishes);
            return new MyListModel
            {
                Id = wishList.Id,
                Title = wishList.Title,
                Wishes = wishModels
            };
        }

        public SharedListModel Handle(GetSharedListRequest message)
        {
            return context.Set<WishList>()
                .Where(wl => wl.Id == message.ListId)
                .ProjectTo<SharedListModel>(mapper.MapperConfiguration)
                .Single();
        }

    }
}