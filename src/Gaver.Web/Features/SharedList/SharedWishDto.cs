using System.Collections.Generic;
using Gaver.Web.Features.Shared.Models;

namespace Gaver.Web.Features.SharedList;

public class SharedWishDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public List<WishOptionDto> Options { get; set; } = new();

    public int? BoughtByUserId { get; set; }
}