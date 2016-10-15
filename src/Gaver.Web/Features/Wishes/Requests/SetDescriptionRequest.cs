using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class SetDescriptionRequest
    {
        public string Description { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int WishId { get; set; }
    }
}