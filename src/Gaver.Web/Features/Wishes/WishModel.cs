namespace Gaver.Web.Features.Wishes
{
    public class WishModel
    {
        public int Id { get; set; }
        public int WishListId { get; set; }
        public WishUserModel BoughtByUser { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}