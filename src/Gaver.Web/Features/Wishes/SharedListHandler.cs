using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Common.Extensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Data.Exceptions;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Wishes.Requests;
using Gaver.Web.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gaver.Web.Features.Wishes
{
    public class SharedListHandler :
        IRequestHandler<SetBoughtRequest, SharedWishModel>,
        IRequestHandler<GetSharedListRequest, SharedListModel>,
        IRequestHandler<GetSharedListsRequest, SharedListsModel>,
        IRequestHandler<CheckSharedListAccessRequest, ListAccessStatus>
    {
        private readonly IClientNotifier clientNotifier;
        private readonly GaverContext context;
        private readonly ILogger<SharedListHandler> logger;
        private readonly IMapperService mapper;

        public SharedListHandler(GaverContext context, IMapperService mapper, IClientNotifier clientNotifier,
            ILogger<SharedListHandler> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.clientNotifier = clientNotifier;
            this.logger = logger;
        }

        public async Task<ListAccessStatus> Handle(CheckSharedListAccessRequest request,
            CancellationToken token = default)
        {
            var wishListOwnerId = await context.WishLists.Where(wl => wl.Id == request.WishListId)
                .Select(wl => wl.UserId)
                .SingleOrDefaultAsync(token);
            if (wishListOwnerId == 0)
                throw new EntityNotFoundException<WishList>(request.WishListId);

            if (wishListOwnerId == request.UserId)
                return ListAccessStatus.Owner;

            if (await context.Invitations.AnyAsync(
                wl => wl.WishListId == request.WishListId && wl.UserId == request.UserId, token))
                return ListAccessStatus.Invited;

            return ListAccessStatus.NotInvited;
        }

        public Task<SharedListModel> Handle(GetSharedListRequest message, CancellationToken cancellationToken = default)
        {
            var sharedListModel = context.Set<WishList>()
                .Where(wl => wl.Id == message.WishListId)
                .ProjectTo<SharedListModel>(mapper.MapperConfiguration)
                .SingleOrThrow(new FriendlyException("Listen finnes ikke"));
            return Task.FromResult(sharedListModel);
        }

        public async Task<SharedListsModel> Handle(GetSharedListsRequest request, CancellationToken cancellationToken)
        {
            var invitations = await context.Set<Invitation>()
                .Where(i => i.UserId == request.UserId)
                .ProjectTo<InvitationModel>(mapper.MapperConfiguration)
                .ToListAsync(cancellationToken);

            return new SharedListsModel {
                Invitations = invitations
            };
        }

        public async Task<SharedWishModel> Handle(SetBoughtRequest message, CancellationToken cancellationToken)
        {
            var wish = GetWish(message.WishId, message.WishListId);
            var userId = message.UserId;
            if (wish.BoughtByUserId != null && wish.BoughtByUserId != userId)
                throw new FriendlyException("Wish has already been bought by someone else");

            if (message.IsBought) wish.BoughtByUserId = userId;
            else wish.BoughtByUserId = null;
            await context.SaveChangesAsync(cancellationToken);

            await clientNotifier.RefreshListAsync(message.WishListId, userId);
            return mapper.Map<SharedWishModel>(wish);
        }

        private Wish GetWish(int wishId, int wishListId)
        {
            var wish = context.GetOrDie<Wish>(wishId);
            if (wish.WishListId != wishListId)
                throw new FriendlyException($"Wish {wishId} does not belong to list {wishListId}");
            return wish;
        }
    }
}
