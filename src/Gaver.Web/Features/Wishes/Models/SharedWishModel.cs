namespace Gaver.Web.Features.Wishes.Models
{
    public class SharedWishModel : WishModel
    {
        public UserModel BoughtByUser { get; set; }
    }
}