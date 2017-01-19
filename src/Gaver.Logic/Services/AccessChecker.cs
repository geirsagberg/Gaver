using System.Linq;
using Gaver.Data;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using Gaver.Logic.Exceptions;
using JetBrains.Annotations;

namespace Gaver.Logic.Services
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
    }
}