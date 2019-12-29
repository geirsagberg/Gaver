namespace Gaver.Web.Features.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public int WishListId { get; set; }
        public string? Name { get; set; }
        public string? PictureUrl { get; set; }
    }
}
