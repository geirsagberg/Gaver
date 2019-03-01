using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Constants;
using Gaver.Web.Contracts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Auth
{
    public class AccessChecker : IAccessChecker
    {
        private readonly GaverContext context;

        public AccessChecker(GaverContext context)
        {
            this.context = context;
        }

        [AssertionMethod]
        public async Task CheckWishListInvitations(int wishListId, int userId, CancellationToken cancellationToken)
        {
            if (!await context.Invitations.AnyAsync(i => i.WishListId == wishListId && i.UserId == userId, cancellationToken))
                throw new FriendlyException(EventIds.MissingInvitation,
                    "Du har ikke blitt invitert til å se denne listen");
        }

        [AssertionMethod]
        public async Task CheckNotOwner(int wishListId, int userId, CancellationToken cancellationToken)
        {
            if (await context.Set<WishList>().AnyAsync(wl => wl.Id == wishListId && wl.UserId == userId))
                throw new FriendlyException(EventIds.OwnerAccessingSharedList, "Du kan ikke se din egen liste");
        }
    }
}
