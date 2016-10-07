using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gaver.Web.Features.Wishes
{
    public class GetMyListRequest : IRequest<MyListModel>
    {
        public string UserName { get; set; }
    }

    public class ShareListRequest : IAsyncRequest
    {
        [Required]
        [MinLength(1)]
        public string[] Emails { get; set; }

        public int WishListId { get; set; }
    }

    public class GetSharedListRequest : IRequest<SharedListModel>
    {
        public int ListId { get; set; }
    }

    public class AddWishRequest
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; }
    }

    public class AuthenticatedAddWishRequest : AddWishRequest, IRequest<WishModel>
    {
        public string UserName { get; set; }
        public int WishListId { get; set; }
    }

    public class DeleteWishRequest : IRequest
    {
        public int WishId { get; set; }
        public int WishListId { get; set; }
    }
}