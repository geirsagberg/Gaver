using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class SetBoughtRequest
    {
        public bool IsBought { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }
        [JsonIgnore]
        public int WishId { get; set; }
        [JsonIgnore]
        public string UserName { get; set; }
    }
}