using System.Collections.Generic;
using Gaver.Data.Contracts;

namespace Gaver.Data.Entities
{
    public class WishList : IEntityWithId
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ISet<Wish> Wishes { get; set; } = new HashSet<Wish>();
        public virtual ISet<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();
        public virtual ISet<Invitation> Invitations { get; set; } = new HashSet<Invitation>();
        public virtual ISet<InvitationToken> InvitationTokens { get; set; } = new HashSet<InvitationToken>();
    }
}
