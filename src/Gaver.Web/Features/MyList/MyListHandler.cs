using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Flurl;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Common.Extensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Contracts;
using Gaver.Web.Extensions;
using Gaver.Web.Features.Mail;
using Gaver.Web.Features.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.MyList
{
    public class MyListHandler : IRequestHandler<UpdateWishRequest>,
        IRequestHandler<GetMyListRequest, MyListDto>,
        IRequestHandler<SetWishesOrderRequest>,
        IRequestHandler<AddWishRequest, WishDto>,
        IRequestHandler<DeleteWishRequest, DeleteWishResponse>,
        IRequestHandler<ResetListRequest>
    {
        private readonly IClientNotifier clientNotifier;
        private readonly GaverContext context;
        private readonly IMapperService mapper;
        private readonly IMailSender mailSender;
        private readonly IHostUrlAccessor hostUrlAccessor;

        public MyListHandler(GaverContext context, IClientNotifier clientNotifier, IMapperService mapper, IMailSender mailSender, IHostUrlAccessor hostUrlAccessor)
        {
            this.context = context;
            this.clientNotifier = clientNotifier;
            this.mapper = mapper;
            this.mailSender = mailSender;
            this.hostUrlAccessor = hostUrlAccessor;
        }

        public async Task<WishDto> Handle(AddWishRequest message, CancellationToken cancellationToken)
        {
            var wishList = context.WishLists.Single(wl => wl.UserId == message.UserId);
            var wish = new Wish {
                Title = message.Title,
                Url = message.Url,
                WishList = wishList
            };
            context.Add(wish);
            await context.SaveChangesAsync(cancellationToken);
            wishList.WishesOrder = wishList.WishesOrder.IsNullOrEmpty()
                ? await context.Wishes.Where(w => w.WishList == wishList).Select(w => w.Id)
                    .ToArrayAsync(cancellationToken)
                : wishList.WishesOrder.Concat(new[] { wish.Id }).ToArray();

            await context.SaveChangesAsync(cancellationToken);
            await clientNotifier.RefreshListAsync(wishList.Id);
            return mapper.Map<WishDto>(wish);
        }

        public async Task<DeleteWishResponse> Handle(DeleteWishRequest message, CancellationToken cancellationToken)
        {
            var wish = await context.Set<Wish>().Include(w => w.WishList)
                .SingleAsync(w => w.Id == message.WishId, cancellationToken);

            var wishListId = await context.GetUserWishListId(message.UserId);

            context.Remove(wish);
            await context.SaveChangesAsync(cancellationToken);
            wish.WishList!.WishesOrder = wish.WishList.WishesOrder.IsNullOrEmpty()
                ? wish.WishList.Wishes.Select(w => w.Id).ToArray()
                : wish.WishList.WishesOrder.Except(new[] { message.WishId }).ToArray();
            await context.SaveChangesAsync(cancellationToken);
            await clientNotifier.RefreshListAsync(wishListId);
            return new DeleteWishResponse {
                WishesOrder = wish.WishList.WishesOrder
            };
        }

        public async Task<MyListDto> Handle(GetMyListRequest message, CancellationToken cancellationToken = default)
        {
            var model = await context.Set<WishList>()
                .Where(wl => wl.UserId == message.UserId)
                .ProjectTo<MyListDto>(mapper.MapperConfiguration)
                .SingleAsync(cancellationToken);

            if (model.WishesOrder.Length != model.Wishes.Count) {
                model.WishesOrder = model.Wishes.Select(w => w.Id).ToArray();
            }

            return model;
        }

        public async Task<Unit> Handle(SetWishesOrderRequest request, CancellationToken cancellationToken)
        {
            var wishList = await GetUserWishListWithWishes(request, cancellationToken);
            var wishIds = await context.Set<Wish>().Where(w => w.WishListId == wishList.Id).Select(w => w.Id)
                .ToListAsync(cancellationToken);
            if (wishIds.Intersect(request.WishesOrder).Count() != wishIds.Count) {
                throw new FriendlyException("Ugyldig rekkefølge");
            }

            wishList.WishesOrder = request.WishesOrder;
            await context.SaveChangesAsync(cancellationToken);
            await clientNotifier.RefreshListAsync(wishList.Id);
            return Unit.Value;
        }

        private async Task<WishList> GetUserWishListWithWishes(IAuthenticatedRequest request, CancellationToken cancellationToken) =>
            await context.Set<WishList>()
                .Include(w => w.Wishes)
                .SingleAsync(wl => wl.UserId == request.UserId, cancellationToken);

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
            await clientNotifier.RefreshListAsync(wish.WishListId);
            return Unit.Value;
        }

        public async Task<Unit> Handle(ResetListRequest request, CancellationToken cancellationToken)
        {
            var wishList = await GetUserWishListWithWishes(request, cancellationToken);
            var user = await GetUserWithFriends(request, cancellationToken);
            wishList.Wishes = wishList.Wishes.Where(w => request.KeepWishes.Contains(w.Id)).ToHashSet();
            wishList.WishesOrder = wishList.WishesOrder.IsNullOrEmpty()
                ? wishList.Wishes.Select(w => w.Id).ToArray()
                : wishList.WishesOrder.Intersect(request.KeepWishes).ToArray();
            await context.SaveChangesAsync(cancellationToken);
            await clientNotifier.RefreshListAsync(wishList.Id);

            var mail = new MailModel {
                To = user.Friends.Select(f => f.Email).ToList(),
                Content = $@"<h1>{user.Name} har oppdatert ønskelisten sin!</h1>
<p>Hvis du har tidligere krysset av for at du har kjøpt noe, må du krysse av på nytt :)</p>
<p><a href='{Url.Combine(hostUrlAccessor.GetHostUrl(), "sharedlists", wishList.Id.ToString())}'>Klikk her for å åpne listen.</a></p>
",
                Subject = $"{user.Name} har oppdatert ønskelisten sin!"
            };
            await mailSender.SendAsync(mail, cancellationToken);

            return Unit.Value;
        }

        private async Task<User> GetUserWithFriends(ResetListRequest request, CancellationToken cancellationToken)
        {
            return await context.Users.Include(u => u.Friends).SingleAsync(u => u.Id == request.UserId, cancellationToken);
        }
    }
}
