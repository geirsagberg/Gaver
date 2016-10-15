using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{

    public class AddWishRequest
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }
    }
}