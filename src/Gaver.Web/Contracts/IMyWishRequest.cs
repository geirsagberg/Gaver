namespace Gaver.Web.Contracts;

public interface IMyWishRequest : IAuthenticatedRequest
{
    int WishId { get; }
}