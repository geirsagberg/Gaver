using System.Collections.Generic;

namespace Gaver.Web.Features.Wishes
{
    public class SharedListModel {
        public int Id { get; set; }
        public IList<WishModel> Wishes { get; set; }
        public string Owner { get; set; }
    }
}