using Gaver.Web.Contracts;

namespace Gaver.Web.CrossCutting
{
    public interface IWishListRequest : IAuthenticatedRequest
    {
        int WishListId { get; }
    }
}
