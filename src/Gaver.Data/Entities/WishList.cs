using System.Collections.Generic;

namespace Gaver.Data.Entities
{
    public class WishList
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ISet<Wish> Wishes { get; set; }
    }
}
