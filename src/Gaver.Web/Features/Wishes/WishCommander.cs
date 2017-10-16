using System;
using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using Gaver.Logic.Exceptions;
using Gaver.Web.Features.LiveUpdates;
using Gaver.Web.Features.Wishes.Models;
using Gaver.Web.Features.Wishes.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gaver.Web.Features.Wishes
{
    public class WishCommander :
        IRequestHandler<AddWishRequest, WishModel>,
        IRequestHandler<SetUrlRequest, WishModel>,
        IRequestHandler<SetBoughtRequest, SharedWishModel>,
        IRequestHandler<SetDescriptionRequest, WishModel>,
        IRequestHandler<DeleteWishRequest>,
        IRequestHandler<RegisterTokenRequest>
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

        public WishModel Handle(AddWishRequest message)
        {
            var wishListId = context.WishLists.Single(wl => wl.UserId == message.UserId).Id;
            var wish = new Wish {
                Title = message.Title,
                WishListId = wishListId
            };
            context.Add(wish);
            context.SaveChanges();
            clientNotifier.RefreshListAsync(wishListId);
            return mapper.Map<WishModel>(wish);
        }

        public void Handle(DeleteWishRequest message)
        {
            context.Delete<Wish>(message.WishId);
            context.SaveChanges();
            clientNotifier.RefreshListAsync(message.WishListId);
        }

        public void Handle(RegisterTokenRequest request)
        {
            if (context.Invitations.Any(i => i.WishListId == request.WishListId && i.UserId == request.UserId)) {
                logger.LogInformation("User {UserId} already has access to {WishListId}", request.UserId,
                    request.WishListId);
                return;
            }
            var token = context.Find<InvitationToken>(request.Token);
            if (token == null)
                throw new FriendlyException(EventIds.UnknownToken, "Unknown token");
            if (token.Accepted != null)
                throw new FriendlyException(EventIds.TokenAlreadyAccepted, "Token already accepted");

            var invitation = new Invitation {
                UserId = request.UserId,
                WishListId = request.WishListId
            };
            context.Add(invitation);
            token.Accepted = DateTimeOffset.Now;
            context.SaveChanges();
        }

        public SharedWishModel Handle(SetBoughtRequest message)
        {
            var wish = GetWish(message.WishId, message.WishListId);
            var userId = message.UserId;
            if (wish.BoughtByUserId != null && wish.BoughtByUserId != userId)
                throw new FriendlyException(EventIds.AlreadyBought, "Wish has already been bought by someone else");

            if (message.IsBought) wish.BoughtByUserId = userId;
            else wish.BoughtByUserId = null;
            context.SaveChanges();

            clientNotifier.RefreshListAsync(message.WishListId, message.UserId);
            return mapper.Map<SharedWishModel>(wish);
        }

        public WishModel Handle(SetDescriptionRequest message)
        {
            var wish = GetWish(message.WishId, message.WishListId);
            wish.Description = message.Description;
            context.SaveChanges();

            clientNotifier.RefreshListAsync(message.WishListId, null);
            return mapper.Map<WishModel>(wish);
        }

        public WishModel Handle(SetUrlRequest message)
        {
            var wish = GetWish(message.WishId, message.WishListId);

            var urlString = message.Url;

            if (urlString.IsNullOrEmpty()) {
                wish.Url = null;
            }
            else {
                if (!urlString.StartsWith("http"))
                    urlString = $"http://{urlString}";

                if (!Uri.TryCreate(urlString, UriKind.Absolute, out var uri))
                    throw new FriendlyException(EventIds.InvalidUrl, "Ugyldig lenke");

                wish.Url = uri.ToString();
            }

            context.SaveChanges();
            clientNotifier.RefreshListAsync(message.WishListId, null);
            return mapper.Map<WishModel>(wish);
        }

        private Wish GetWish(int wishId, int wishListId)
        {
            var wish = context.GetOrDie<Wish>(wishId);
            if (wish.WishListId != wishListId)
                throw new FriendlyException(EventIds.WrongList,
                    $"Wish {wishId} does not belong to list {wishListId}");
            return wish;
        }
    }
}