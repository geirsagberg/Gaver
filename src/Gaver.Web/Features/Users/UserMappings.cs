using System.Linq.Expressions;
using Gaver.Data.Entities;

namespace Gaver.Web.Features.Users;

public static class UserMappings {
    public static Expression<Func<User, UserDto>> UserDtoProjection { get; } = user => new UserDto {
        Id = user.Id,
        WishListId = user.WishList!.Id,
        Name = user.Name,
        PictureUrl = user.PictureUrl
    };

    public static Expression<Func<User, CurrentUserDto>> CurrentUserDtoProjection { get; } =
        user => new CurrentUserDto {
            Id = user.Id,
            WishListId = user.WishList!.Id,
            Name = user.Name,
            PictureUrl = user.PictureUrl
        };
}
