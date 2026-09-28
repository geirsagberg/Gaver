using System.Linq.Expressions;
using Gaver.Data.Entities;
using Gaver.Web.Features.Shared.Models;
using Gaver.Web.Features.Users;

namespace Gaver.Web.Features.SharedList;

public static class SharedListMappings {
    public static Expression<Func<WishList, SharedListDto>> SharedListDtoProjection { get; } = wishList =>
        new SharedListDto {
            Id = wishList.Id,
            Wishes = wishList.Wishes.Select(wish => new SharedWishDto {
                Id = wish.Id,
                Title = wish.Title,
                Url = wish.Url,
                Options = wish.Options.Select(option => new WishOptionDto {
                    Id = option.Id,
                    Title = option.Title,
                    Url = option.Url
                }).ToList(),
                BoughtByUserId = wish.BoughtByUserId
            }).ToList(),
            Users = wishList.Wishes
                .Where(wish => wish.BoughtByUser != null)
                .Select(wish => new UserDto {
                    Id = wish.BoughtByUser!.Id,
                    WishListId = wish.BoughtByUser.WishList!.Id,
                    Name = wish.BoughtByUser.Name,
                    PictureUrl = wish.BoughtByUser.PictureUrl
                }).ToList(),
            OwnerUserId = wishList.UserId,
            WishesOrder = wishList.WishesOrder
        };

    public static SharedWishDto ToSharedWishDto(Wish wish) => new() {
        Id = wish.Id,
        Title = wish.Title,
        Url = wish.Url,
        Options = wish.Options.Select(WishOptionMappings.ToDto).ToList(),
        BoughtByUserId = wish.BoughtByUserId
    };
}
