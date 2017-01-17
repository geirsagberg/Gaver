using System;

namespace Gaver.Data.Entities
{
    public class InvitationToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset Created { get; set; }
        public int WishListId { get; set; }
        public DateTimeOffset? Accepted { get; set; }
    }
}