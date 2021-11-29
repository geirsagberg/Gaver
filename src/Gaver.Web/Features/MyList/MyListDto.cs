using System;
using System.Collections.Generic;

namespace Gaver.Web.Features.MyList;

public class MyListDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public IList<WishDto> Wishes { get; set; } = new List<WishDto>();
    public int[] WishesOrder { get; set; } = Array.Empty<int>();
}