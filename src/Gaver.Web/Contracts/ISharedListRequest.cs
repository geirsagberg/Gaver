namespace Gaver.Web.Contracts
{
    public interface ISharedListRequest : IAuthenticatedRequest
    {
        int WishListId { get; }
    }
}
