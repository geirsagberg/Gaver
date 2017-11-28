namespace Gaver.Web.Features.Wishes.Models
{
    public class WishModel
    {
        public int Id { get; set; }
        public int WishListId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

    }
}