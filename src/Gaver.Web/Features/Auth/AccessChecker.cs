using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Attributes;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Contracts;
using Gaver.Web.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Auth;

[Service]
public class AccessChecker : IAccessChecker
{
    private readonly GaverContext context;

    public AccessChecker(GaverContext context)
    {
        this.context = context;
    }

    public async Task CheckWishListAccess(int wishListId, int userId,
        CancellationToken cancellationToken = default)
    {
        // Can see wishlist if friend of owner, or member of any of the same groups as the owner
        if (!await context.WishLists.AnyAsync(wl => wl.Id == wishListId
                && (wl.User!.Friends.Any(f => f.Id == userId) || wl.User!.Groups.Any(c =>
                    c.Users.Any(c2 => c2.Id == userId))), cancellationToken))
            throw new HttpException(HttpStatusCode.Forbidden, "Du har ikke tilgang til å se denne listen");
    }

    public async Task CheckNotOwner(int wishListId, int userId, CancellationToken cancellationToken = default)
    {
        if (await context.Set<WishList>()
                .AnyAsync(wl => wl.Id == wishListId && wl.UserId == userId, cancellationToken))
            throw new HttpException(HttpStatusCode.Forbidden, "Du kan ikke se din egen liste");
    }

    public async Task CheckOwner(int wishListId, int userId, CancellationToken cancellationToken = default)
    {
        if (!await context.WishLists.AnyAsync(wl => wl.Id == wishListId && wl.UserId == userId, cancellationToken)
           )
            throw new HttpException(HttpStatusCode.Forbidden, "Denne listen finnes ikke eller tilhører noen andre");
    }

    public async Task CheckWishOwner(int wishId, int userId,
        CancellationToken cancellationToken = default)
    {
        if (!await context.Wishes.AnyAsync(w =>
                w.Id == wishId && w.WishList!.UserId == userId, cancellationToken))
            throw new HttpException(HttpStatusCode.Forbidden,
                "Dette ønsket finnes ikke, eller tilhører en annen liste");
    }
}