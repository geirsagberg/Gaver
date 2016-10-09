namespace Gaver.Data.Entities
{
    public class Wish : IEntityWithId
    {
        public int Id { get; set; }
        public int WishListId { get; set; }
        public int? BoughtByUserId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public virtual User BoughtByUser { get; set; }
        public virtual WishList WishList { get; set; }
    }
}
