using System.Collections.Generic;

namespace Gaver.Web.Features.Wishes
{
    public class SharedListModel
    {
        public int Id { get; set; }
        public IList<SharedWishModel> Wishes { get; set; } = new List<SharedWishModel>();
        public string Owner { get; set; }
    }
}