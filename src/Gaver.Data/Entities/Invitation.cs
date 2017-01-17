namespace Gaver.Data.Entities
{
    public class Invitation
    {
        public int WishListId { get; set; }
        public int UserId { get; set; }

        public virtual WishList WishList { get; set; }
        public virtual User User { get; set; }
    }
}