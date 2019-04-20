using System.Collections.Generic;
using Gaver.Web.Features.Users;

namespace Gaver.Web.Features.Wishes.Models
{
    public class SharedListModel
    {
        public int Id { get; set; }
        public IList<SharedWishModel> Wishes { get; set; } = new List<SharedWishModel>();
        public IList<UserModel> Users { get; set; } = new List<UserModel>();
        public int OwnerUserId { get; set; }
    }
}
