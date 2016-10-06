using System.Collections.Generic;

namespace Gaver.Web.Features.Wishes
{
    public class MyListModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<WishModel> Wishes { get; set; }
    }
}