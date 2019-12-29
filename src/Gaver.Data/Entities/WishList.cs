using System.Collections.Generic;
using Gaver.Data.Contracts;

namespace Gaver.Data.Entities
{
    public class WishList : IEntityWithId
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int UserId { get; set; }

        public int[] WishesOrder { get; set; } = new int[0];

        public User? User { get; set; }
        public ICollection<Wish> Wishes { get; set; } = new HashSet<Wish>();
        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();
        public ICollection<InvitationToken> InvitationTokens { get; set; } = new HashSet<InvitationToken>();
    }
}
