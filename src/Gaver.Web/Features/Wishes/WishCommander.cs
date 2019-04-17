using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Features.LiveUpdates;
using Gaver.Web.Features.Wishes.Models;
using Gaver.Web.Features.Wishes.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gaver.Web.Features.Wishes
{
    public class WishCommander :
        IRequestHandler<AddWishRequest, WishModel>,
        IRequestHandler<SetUrlRequest, WishModel>,
        IRequestHandler<SetBoughtRequest, SharedWishModel>,
        IRequestHandler<SetDescriptionRequest, WishModel>,
        IRequestHandler<SetTitleRequest, WishModel>,
        IRequestHandler<DeleteWishRequest, DeleteWishResponse>
    {
        private readonly ClientNotifier clientNotifier;
        private readonly GaverContext context;
        private readonly ILogger<WishCommander> logger;
        private readonly IMapperService mapper;

        public WishCommander(GaverContext context, IMapperService mapper, ClientNotifier clientNotifier,
            ILogger<WishCommander> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.clientNotifier = clientNotifier;
            this.logger = logger;
        }

        public async Task<WishModel> Handle(AddWishRequest message, CancellationToken token)
        {
            var wishList = context.WishLists.Single(wl => wl.UserId == message.UserId);
            var wish = new Wish {
                Title = message.Title,
                WishList = wishList
            };
            context.Add(wish);
            await context.SaveChangesAsync(token);
            if (wishList.WishesOrder != null) {
                wishList.WishesOrder = wishList.WishesOrder.Concat(new[] {wish.Id}).ToArray();
            } else {
                var wishesOrder = await context.Wishes.Where(w => w.WishList == wishList).Select(w => w.Id)
                    .ToArrayAsync(token);
                wishList.WishesOrder = wishesOrder;
            }

            await context.SaveChangesAsync(token);
            await clientNotifier.RefreshListAsync(wishList.Id);
            return mapper.Map<WishModel>(wish);
        }

        public async Task<DeleteWishResponse> Handle(DeleteWishRequest message, CancellationToken token)
        {
            var wish = await context.Set<Wish>().Include(w => w.WishList)
                .SingleAsync(w => w.Id == message.WishId, token);

            context.Remove(wish);
            await context.SaveChangesAsync(token);
            wish.WishList.WishesOrder = wish.WishList.WishesOrder != null
                ? wish.WishList.WishesOrder.Except(new[] {message.WishId}).ToArray()
                : wish.WishList.Wishes.Select(w => w.Id).ToArray();
            await context.SaveChangesAsync(token);
            await clientNotifier.RefreshListAsync(message.WishListId);
            return new DeleteWishResponse {
                WishesOrder = wish.WishList.WishesOrder
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

        public async Task<WishModel> Handle(SetDescriptionRequest message, CancellationToken cancellationToken)
        {
            var wish = GetWish(message.WishId, message.WishListId);
            wish.Description = message.Description;
            await context.SaveChangesAsync(cancellationToken);

            await clientNotifier.RefreshListAsync(message.WishListId, null);
            return mapper.Map<WishModel>(wish);
        }

        public async Task<WishModel> Handle(SetTitleRequest request, CancellationToken cancellationToken)
        {
            var wish = GetWish(request.WishId, request.WishListId);
            wish.Title = request.Title;
            await context.SaveChangesAsync(cancellationToken);
            await clientNotifier.RefreshListAsync(request.WishListId);
            return mapper.Map<WishModel>(wish);
        }

        public async Task<WishModel> Handle(SetUrlRequest message, CancellationToken token)
        {
            var wish = GetWish(message.WishId, message.WishListId);

            var urlString = message.Url;

            if (urlString.IsNullOrEmpty()) {
                wish.Url = null;
            } else {
                if (!urlString.StartsWith("http"))
                    urlString = $"http://{urlString}";

                if (!Uri.TryCreate(urlString, UriKind.Absolute, out var uri))
                    throw new FriendlyException("Ugyldig lenke");

                wish.Url = uri.ToString();
            }

            await context.SaveChangesAsync(token);
            await clientNotifier.RefreshListAsync(message.WishListId, null);
            return mapper.Map<WishModel>(wish);
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
