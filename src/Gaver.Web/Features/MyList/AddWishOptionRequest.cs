using Gaver.Web.Contracts;
using Gaver.Web.Models;
using MediatR;

namespace Gaver.Web.Features.MyList
{
    public class AddWishOptionRequest : IRequest<WishOptionModel>, IMyWishRequest
    {
        public int UserId { get; set; }
        public int WishId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
