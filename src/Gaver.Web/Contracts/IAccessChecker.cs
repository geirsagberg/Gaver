using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Gaver.Web.Contracts
{
    public interface IAccessChecker
    {
        [AssertionMethod]
        Task CheckWishListInvitations(int wishListId, int UserId);

        [AssertionMethod]
        Task CheckNotOwner(int wishListId, int UserId);
    }
}
