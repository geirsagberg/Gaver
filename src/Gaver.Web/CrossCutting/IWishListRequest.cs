namespace Gaver.Web.CrossCutting
{
    public interface IWishListRequest
    {
        int WishListId { get; }
        int UserId { get; }
    }
}