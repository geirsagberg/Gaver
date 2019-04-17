namespace Gaver.Web.Contracts
{
    public interface IMyWishRequest : IMyListRequest
    {
        int WishId { get; set; }
    }
}
