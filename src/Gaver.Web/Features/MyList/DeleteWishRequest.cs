using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.MyList
{
    public class DeleteWishRequest : IRequest<DeleteWishResponse>, IMyWishRequest
    {
        public int WishId { get; set; }
        public int UserId { get; set; }
    }
}
