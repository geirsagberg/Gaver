using System.Collections.Generic;

namespace Gaver.Web.Features.Wishes.Models
{
    public class MyListModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<WishModel> Wishes { get; set; } = new List<WishModel>();
        public IList<InvitationModel> Invitations { get; set; } = new List<InvitationModel>();
        public int[] WishesOrder { get; set; } = new int[0];
    }
}
