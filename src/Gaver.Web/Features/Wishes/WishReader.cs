using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Data.Exceptions;
using Gaver.Logic;
using Gaver.Logic.Extensions;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using Gaver.Logic.Exceptions;
using Gaver.Web.Features.Wishes.Models;
using Gaver.Web.Features.Wishes.Requests;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Wishes
{
    public class WishReader :
        IRequestHandler<GetMyListRequest, MyListModel>,
        IRequestHandler<GetSharedListRequest, SharedListModel>,
        IAsyncRequestHandler<CheckSharedListAccessRequest, ListAccessStatus>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapper;
        private readonly IAccessChecker accessChecker;

        public WishReader(GaverContext context, IMapperService mapper, IAccessChecker accessChecker)
        {
            this.context = context;
            this.mapper = mapper;
            this.accessChecker = accessChecker;
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
            accessChecker.CheckWishListInvitations(message.ListId, message.UserId);

            var sharedListModel = context.Set<WishList>()
                .Where(wl => wl.Id == message.ListId)
                .ProjectTo<SharedListModel>(mapper.MapperConfiguration)
                .SingleOrThrow(new FriendlyException(EventIds.SharedListMissing, "Listen finnes ikke"));
            return sharedListModel;
        }

        public async Task<ListAccessStatus> HandleAsync(CheckSharedListAccessRequest request)
        {
            var wishListOwnerId = await context.WishLists.Where(wl => wl.Id == request.WishListId)
                .Select(wl => wl.UserId)
                .SingleOrDefaultAsync();
            if (wishListOwnerId == 0)
                throw new EntityNotFoundException<WishList>(request.WishListId);

            if (wishListOwnerId == request.UserId)
                return ListAccessStatus.Owner;

            if (await context.Invitations.AnyAsync(wl => wl.WishListId == request.WishListId && wl.UserId == request.UserId))
                return ListAccessStatus.Invited;

            return ListAccessStatus.NotInvited;
        }
    }
}