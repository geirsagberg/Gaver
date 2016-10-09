using System;
using System.ComponentModel.DataAnnotations;

namespace Gaver.Data.Entities
{
    public class ChatMessage : IEntityWithId
    {
        public int Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public int UserId { get; set; }
        public int WishListId { get; set; }

        [MaxLength(255)]
        [Required]
        public string Text { get; set; }

        public virtual User User { get; set; }
        public virtual WishList WishList { get; set; }
    }
}