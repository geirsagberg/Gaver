namespace Gaver.Web.Contracts
{
  public interface IMyListRequest : IAuthenticatedRequest
  {
    int WishListId { get; set; }
  }
}
