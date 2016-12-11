using System.Linq;
using AutoMapper.QueryableExtensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Extensions;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using Gaver.Web.Features.Wishes.Models;
using Gaver.Web.Features.Wishes.Requests;

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
            var model = GetModel(message);
            if (model == null)
            {
                context.Add(new WishList
                {
                    UserId = message.UserId
                });
                context.SaveChanges();
            }
            return GetModel(message);
        }

        private MyListModel GetModel(GetMyListRequest message)
        {
            return context.Set<WishList>()
                .Where(wl => wl.UserId == message.UserId)
                .ProjectTo<MyListModel>(mapper.MapperConfiguration)
                .SingleOrDefault();
        }

        public SharedListModel Handle(GetSharedListRequest message)
        {
            if (context.Set<WishList>().Any(wl => wl.Id == message.ListId && wl.UserId == message.UserId))
            {
                throw new FriendlyException(EventIds.OwnerAccessingSharedList, "Du kan ikke se din egen liste");
            }

            var sharedListModel = context.Set<WishList>()
                .Where(wl => wl.Id == message.ListId)
                .ProjectTo<SharedListModel>(mapper.MapperConfiguration)
                .SingleOrThrow(new FriendlyException(EventIds.SharedListMissing, "Listen finnes ikke"));
            return sharedListModel;
        }
    }
}