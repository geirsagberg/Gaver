using System.ComponentModel.DataAnnotations;
using Gaver.Data.Contracts;

namespace Gaver.Data.Entities;

public class WishOption : IEntityWithId
{
    public int WishId { get; set; }
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = "";

    [MaxLength(255)]
    public string? Url { get; set; }

    public Wish? Wish { get; set; }
}