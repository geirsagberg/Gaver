using System.Collections.Generic;

namespace Gaver.Web.Models
{
    public class SharedListsModel
    {
        public IList<InvitationModel> Invitations { get; set; } = new List<InvitationModel>();
    }
}
