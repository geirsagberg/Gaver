using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gaver.Data.Entities
{
    public class User : IEntityWithId
    {
	public int Id { get; set; }

	[MaxLength(40)]
	[Required]
	public string Name { get; set; }

	public virtual ISet<WishList> WishLists { get; set; } = new HashSet<WishList>();
    }
}