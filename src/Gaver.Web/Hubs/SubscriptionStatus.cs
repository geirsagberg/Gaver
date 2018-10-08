using System.Collections.Generic;
using Gaver.Web.Features.Users;

namespace Gaver.Web.Hubs
{
    public class SubscriptionStatus
    {
        public IEnumerable<UserModel> CurrentUsers { get; set; }
    }
}