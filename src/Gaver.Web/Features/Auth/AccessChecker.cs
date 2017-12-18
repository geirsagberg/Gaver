using System.Linq;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Constants;
using Gaver.Web.Contracts;
using JetBrains.Annotations;

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
        public void CheckWishListInvitations(int wishListId, int userId)
        {
            if (!context.Invitations.Any(i => i.WishListId == wishListId && i.UserId == userId))
                throw new FriendlyException(EventIds.MissingInvitation, "Du har ikke blitt invitert til å se denne listen");
        }

        [AssertionMethod]
        public void CheckNotOwner(int wishListId, int userId)
        {
            if (context.Set<WishList>().Any(wl => wl.Id == wishListId && wl.UserId == userId))
                throw new FriendlyException(EventIds.OwnerAccessingSharedList, "Du kan ikke se din egen liste");
        }
    }
}