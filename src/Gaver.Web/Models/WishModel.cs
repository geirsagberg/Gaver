using System.Collections.Generic;

namespace Gaver.Web.Models
{
    public class WishModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public List<WishOptionModel> Options { get; set; } = new List<WishOptionModel>();
    }
}
