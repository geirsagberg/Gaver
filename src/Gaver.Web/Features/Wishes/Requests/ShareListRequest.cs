using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class ShareListRequest
    {
        [Required]
        [MinLength(1)]
        public string[] Emails { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public string UserName { get; set; }
    }
}