using System.Collections.Generic;
using Gaver.Web.Features.Users;

namespace Gaver.Web.Hubs
{
    public class SubscriptionStatus
    {
        public IEnumerable<UserDto> CurrentUsers { get; set; } = new UserDto[0];
    }
}
