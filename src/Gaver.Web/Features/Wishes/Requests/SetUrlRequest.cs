using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class SetUrlRequest
    {
        public string Url { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int WishId { get; set; }
    }
}