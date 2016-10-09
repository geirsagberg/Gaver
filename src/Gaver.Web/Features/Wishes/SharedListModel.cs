using System.Collections.Generic;
using Gaver.Web.Features.Chatting;

namespace Gaver.Web.Features.Wishes
{
    public class SharedListModel
    {
        public int Id { get; set; }
        public IList<WishModel> Wishes { get; set; } = new List<WishModel>();
        public string Owner { get; set; }
    }
}