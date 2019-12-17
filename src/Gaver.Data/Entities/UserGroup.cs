using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gaver.Data.Contracts;

namespace Gaver.Data.Entities
{
    public class UserGroup : IEntityWithId
    {
        public int Id { get; set; }
        public int CreatedByUserId { get; set; }

        [MaxLength(40)]
        [Required]
        public string Name { get; set; } = "";

        public ICollection<UserGroupConnection> UserGroupConnections { get; set; } = new HashSet<UserGroupConnection>();
    }
}
