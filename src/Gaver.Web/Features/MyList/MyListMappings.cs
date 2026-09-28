using System.Linq.Expressions;
using Gaver.Data.Entities;
using Gaver.Web.Features.Shared.Models;

namespace Gaver.Web.Features.MyList;

public static class MyListMappings {
    public static Expression<Func<WishList, MyListDto>> MyListDtoProjection { get; } = wishList =>
        new MyListDto {
            Id = wishList.Id,
            Title = wishList.Title,
            Wishes = wishList.Wishes.Select(wish => new WishDto {
                Id = wish.Id,
                Title = wish.Title,
                Url = wish.Url,
                Options = wish.Options.Select(option => new WishOptionDto {
                    Id = option.Id,
                    Title = option.Title,
                    Url = option.Url
                }).ToList()
            }).ToList(),
            WishesOrder = wishList.WishesOrder
        };

    public static WishDto ToWishDto(Wish wish) => new() {
        Id = wish.Id,
        Title = wish.Title,
        Url = wish.Url,
        Options = wish.Options.Select(WishOptionMappings.ToDto).ToList()
    };
}
