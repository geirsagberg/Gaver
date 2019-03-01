using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Gaver.Web.Contracts
{
    public interface IAccessChecker
    {
        [AssertionMethod]
        Task CheckWishListInvitations(int wishListId, int UserId, CancellationToken cancellationToken = default);

        [AssertionMethod]
        Task CheckNotOwner(int wishListId, int UserId, CancellationToken cancellationToken = default);
    }
}
