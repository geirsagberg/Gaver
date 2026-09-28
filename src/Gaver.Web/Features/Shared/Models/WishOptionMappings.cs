using Gaver.Data.Entities;

namespace Gaver.Web.Features.Shared.Models;

public static class WishOptionMappings {
    public static WishOptionDto ToDto(WishOption option) => new() {
        Id = option.Id,
        Title = option.Title,
        Url = option.Url
    };
}
