namespace Gaver.Web.Contracts
{
    public interface IAccessChecker
    {
        void CheckWishListInvitations(int wishListId, int userId);
        void CheckNotOwner(int wishListId, int userId);
    }
}