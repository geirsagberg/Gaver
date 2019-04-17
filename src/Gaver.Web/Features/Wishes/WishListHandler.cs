using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Features.Wishes.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Wishes
{
    public class WishListHandler : IRequestHandler<SetWishesOrderRequest>
    {
        private readonly GaverContext context;

        public WishListHandler(GaverContext context)
        {
            this.context = context;
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
    }
}
