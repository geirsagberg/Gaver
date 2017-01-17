using System;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class RegisterTokenRequest
    {
        public int WishListId { get; set; }
        public int UserId { get; set; }
        public Guid Token { get; set; }
    }
}