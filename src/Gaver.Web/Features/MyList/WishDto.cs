using System.Collections.Generic;
using Gaver.Web.Features.Shared.Models;

namespace Gaver.Web.Features.MyList;

public class WishDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public List<WishOptionDto> Options { get; set; } = new();
}
