using System.Collections.Generic;
using Gaver.Web.Features.Users;

namespace Gaver.Web.Features.SharedList
{
    public class SharedListDto
    {
        public int Id { get; set; }
        public IList<SharedWishDto> Wishes { get; set; } = new List<SharedWishDto>();
        public IList<UserDto> Users { get; set; } = new List<UserDto>();
        public int OwnerUserId { get; set; }
        public int[] WishesOrder { get; set; } = new int[0];
        public bool CanSeeMyList { get; set; }
    }
}
