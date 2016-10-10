using System;
using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;

namespace Gaver.Web.Features.Wishes
{
    public class WishCommander :
        IRequestHandler<AddWishRequest, WishModel>,
        IRequestHandler<SetUrlRequest, WishModel>,
        IRequestHandler<SetBoughtRequest, WishModel>,
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
            var wishListId = context.Users.Where(u => u.Name == message.UserName).Select(u => u.WishLists.Single().Id).Single();
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
            var wish = context.GetOrDie<Wish>(message.WishId);
            if (wish.WishListId != message.WishListId)
                throw new FriendlyException(EventIds.WrongList, $"Wish {message.WishId} does not belong to list {message.WishListId}");

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

        public WishModel Handle(SetBoughtRequest message)
        {
            var wish = context.GetOrDie<Wish>(message.WishId);
            if (wish.WishListId != message.WishListId)
                throw new FriendlyException(EventIds.WrongList, $"Wish {message.WishId} does not belong to list {message.WishListId}");

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
            return mapper.Map<WishModel>(wish);
        }


        public void Handle(DeleteWishRequest message)
        {
            context.Delete<Wish>(message.WishId);
            context.SaveChanges();
        }
    }
}