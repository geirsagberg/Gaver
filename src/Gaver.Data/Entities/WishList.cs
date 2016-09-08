using System.Collections.Generic;

namespace Gaver.Data.Entities
{
    public class WishList : IEntityWithId
    {
        public int Id { get; set; }
        public string Title { get; set; }

	public virtual ISet<Wish> Wishes { get; set; } = new HashSet<Wish>();
    }
}
