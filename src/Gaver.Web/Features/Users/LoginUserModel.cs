namespace Gaver.Web.Features.Users
{
    public class LoginUserModel : UserModel
    {
        /// <summary>
        /// Used to check if the user is trying to see his own list.
        /// Maybe find a better way to avoid this extra attribute, e.g. redirect?
        /// </summary>
        public int WishListId { get; set; }
    }
}