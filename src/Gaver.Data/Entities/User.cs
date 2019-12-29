using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gaver.Data.Contracts;

namespace Gaver.Data.Entities
{
    public class User : IEntityWithId
    {
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string PrimaryIdentityId { get; set; } = "";

        [MaxLength(40)]
        [Required]
        public string Name { get; set; } = "";

        [MaxLength(255)]
        [Required]
        public string Email { get; set; } = "";

        [MaxLength(255)]
        public string? PictureUrl { get; set; }

        public WishList? WishList { get; set; }

        public ICollection<Wish> BoughtWishes { get; set; } = new HashSet<Wish>();
        public ICollection<UserGroupConnection> UserGroupConnections { get; set; } = new HashSet<UserGroupConnection>();
        public ICollection<UserFriendConnection> Friends { get; set; } = new HashSet<UserFriendConnection>();
    }
}
