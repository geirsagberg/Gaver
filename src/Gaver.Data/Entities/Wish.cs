using System.ComponentModel.DataAnnotations;
using Gaver.Data.Contracts;

namespace Gaver.Data.Entities
{
    public class Wish : IEntityWithId
    {
        public int Id { get; set; }
        public int WishListId { get; set; }
        public int? BoughtByUserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string Url { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public virtual User BoughtByUser { get; set; }
        public virtual WishList WishList { get; set; }
    }
}
