using System.Collections.Generic;
using Gaver.Web.Features.Invitations;

namespace Gaver.Web.Features.SharedList
{
    public class SharedListsDto
    {
        public IList<InvitationDto> Invitations { get; set; } = new List<InvitationDto>();
    }
}
