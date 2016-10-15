using System;
using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using Gaver.Web.Features.Wishes.Requests;

namespace Gaver.Web.Features.Wishes
{
    public class WishCommander :
        IRequestHandler<AddWishRequest, WishModel>,
        IRequestHandler<SetUrlRequest, WishModel>,
        IRequestHandler<SetBoughtRequest, SharedWishModel>,
        IRequestHandler<SetDescriptionRequest, WishModel>,
        IRequestHandler<DeleteWishRequest>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public WishCommander(GaverContext context, IMapperService mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public WishModel Handle(AddWishRequest message)
        {
            var wishListId = context.WishLists.Single(wl => wl.UserId == message.UserId).Id;
            var wish = new Wish
            {
                Title = message.Title,
                WishListId = wishListId
            };
            context.Add(wish);
            context.SaveChanges();
            return mapper.Map<WishModel>(wish);
        }

        public WishModel Handle(SetUrlRequest message)
        {
            var wish = GetWish(message.WishId, message.WishListId);

            var urlString = message.Url;

            if (urlString.IsNullOrEmpty())
            {
                wish.Url = null;
            }
            else
            {
                if (!urlString.StartsWith("http"))
                    urlString = $"http://{urlString}";

                Uri uri;
                if (!Uri.TryCreate(urlString, UriKind.Absolute, out uri))
                    throw new FriendlyException(EventIds.InvalidUrl, "Ugyldig lenke");

                wish.Url = uri.ToString();
            }

            context.SaveChanges();
            return mapper.Map<WishModel>(wish);
        }

        public WishModel Handle(SetDescriptionRequest message)
        {
            var wish = GetWish(message.WishId, message.WishListId);
            wish.Description = message.Description;
            context.SaveChanges();
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

        public SharedWishModel Handle(SetBoughtRequest message)
        {
            var wish = GetWish(message.WishId, message.WishListId);

            var userId = context.Set<User>().Single(u => u.Name == message.UserName).Id;

            if (wish.BoughtByUserId != null && wish.BoughtByUserId != userId)
                throw new FriendlyException(EventIds.AlreadyBought, "Wish has already been bought by someone else");

            if (message.IsBought)
            {
                wish.BoughtByUserId = userId;
            }
            else
            {
                wish.BoughtByUserId = null;
            }
            context.SaveChanges();
            return mapper.Map<SharedWishModel>(wish);
        }


        public void Handle(DeleteWishRequest message)
        {
            context.Delete<Wish>(message.WishId);
            context.SaveChanges();
        }
    }
}