using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Gaver.Web.Contracts;

public interface IAccessChecker
{
    [AssertionMethod]
    Task CheckWishListAccess(int wishListId, int userId, CancellationToken cancellationToken = default);

    [AssertionMethod]
    Task CheckNotOwner(int wishListId, int userId, CancellationToken cancellationToken = default);

    [AssertionMethod]
    Task CheckOwner(int wishListId, int userId, CancellationToken cancellationToken = default);

    [AssertionMethod]
    Task CheckWishOwner(int wishId, int userId, CancellationToken cancellationToken = default);
}