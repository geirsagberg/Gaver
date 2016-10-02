namespace Gaver.Data.Entities
{
    public class Wish : IEntityWithId
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int WishListId { get; set; }

        public virtual WishList WishList { get; set; }
    }
}
