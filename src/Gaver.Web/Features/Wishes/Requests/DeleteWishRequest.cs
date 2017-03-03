using MediatR;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class DeleteWishRequest : IRequest
    {
        public int WishId { get; set; }
        public int WishListId { get; set; }
    }
}