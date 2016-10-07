using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using Newtonsoft.Json;

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

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public string UserName { get; set; }
    }

    public class GetSharedListRequest : IRequest<SharedListModel>
    {
        public int ListId { get; set; }
    }

    public class AddWishRequest : IRequest<WishModel>
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; }

        [JsonIgnore]
        public string UserName { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }
    }

    public class DeleteWishRequest : IRequest
    {
        public int WishId { get; set; }
        public int WishListId { get; set; }
    }
}