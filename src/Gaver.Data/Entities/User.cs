using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gaver.Data.Entities
{
    public class User : IEntityWithId
    {
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string PrimaryIdentityId { get; set; }

        [MaxLength(40)]
        [Required]
        public string Name { get; set; }

        [MaxLength(255)]
        [Required]
        public string Email { get; set; }

        public virtual ISet<WishList> WishLists { get; set; } = new HashSet<WishList>();
        public virtual ISet<Wish> BoughtWishes { get; set; } = new HashSet<Wish>();
    }
}