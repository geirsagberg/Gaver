using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Contracts;
using Gaver.Web.Extensions;
using Gaver.Web.Features.Wishes.Requests;
using Gaver.Web.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.MyList
{
    public class MyListHandler : IRequestHandler<UpdateWishRequest>,
        IRequestHandler<GetMyListRequest, MyListModel>,
        IRequestHandler<SetWishesOrderRequest>,
        IRequestHandler<AddWishRequest, WishModel>,
        IRequestHandler<DeleteWishRequest, DeleteWishResponse>
    {
        private readonly IClientNotifier clientNotifier;
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public MyListHandler(GaverContext context, IClientNotifier clientNotifier, IMapperService mapper)
        {
            this.context = context;
            this.clientNotifier = clientNotifier;
            this.mapper = mapper;
        }

        public async Task<WishModel> Handle(AddWishRequest message, CancellationToken token)
        {
            var wishList = context.WishLists.Single(wl => wl.UserId == message.UserId);
            var wish = new Wish {
                Title = message.Title,
                Url = message.Url,
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

            var wishListId = await context.GetUserWishListId(message.UserId);

            context.Remove(wish);
            await context.SaveChangesAsync(token);
            wish.WishList.WishesOrder = wish.WishList.WishesOrder != null
                ? wish.WishList.WishesOrder.Except(new[] {message.WishId}).ToArray()
                : wish.WishList.Wishes.Select(w => w.Id).ToArray();
            await context.SaveChangesAsync(token);
            await clientNotifier.RefreshListAsync(wishListId);
            return new DeleteWishResponse {
                WishesOrder = wish.WishList.WishesOrder
            };
        }

        public Task<MyListModel> Handle(GetMyListRequest message, CancellationToken token = default)
        {
            var model = context.Set<WishList>()
                .Where(wl => wl.UserId == message.UserId)
                .ProjectTo<MyListModel>(mapper.MapperConfiguration)
                .Single();

            if (model.WishesOrder?.Length != model.Wishes.Count) {
                model.WishesOrder = model.Wishes.Select(w => w.Id).ToArray();
            }

            return Task.FromResult(model);
        }

        public async Task<Unit> Handle(SetWishesOrderRequest request, CancellationToken cancellationToken)
        {
            var wishList = await context.Set<WishList>()
                .SingleAsync(wl => wl.UserId == request.UserId, cancellationToken);
            var wishIds = await context.Set<Wish>().Where(w => w.WishListId == wishList.Id).Select(w => w.Id)
                .ToListAsync(cancellationToken);
            if (wishIds.Intersect(request.WishesOrder).Count() != wishIds.Count) {
                throw new FriendlyException("Ugyldig rekkefølge");
            }

            wishList.WishesOrder = request.WishesOrder;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateWishRequest request, CancellationToken cancellationToken)
        {
            var wish = await context.GetOrDieAsync<Wish>(request.WishId);

            if (request.Title != null) {
                wish.Title = request.Title;
            }

            if (request.Url != null) {
                wish.Url = request.Url;
            }

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
