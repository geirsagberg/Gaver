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
        public virtual ICollection<Wish> Wishes { get; set; } = new HashSet<Wish>();
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();
        public virtual ICollection<Invitation> Invitations { get; set; } = new HashSet<Invitation>();
        public virtual ICollection<InvitationToken> InvitationTokens { get; set; } = new HashSet<InvitationToken>();
    }
}
