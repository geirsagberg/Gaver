using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Gaver.Web.Hubs
{
    public static class ListHubExtensions
    {
        public static Task RefreshDataAsync(this IHubContext<ListHub> hub, int listId, int? excludeUserId = null) =>
            excludeUserId != null
                ? hub.Clients
                    .GroupExcept(ListHub.GetGroup(listId), ListHub.GetConnectionIdsForUser(excludeUserId.Value))
                    .SendAsync("refresh")
                : hub.Clients.Group(ListHub.GetGroup(listId)).SendAsync("refresh");
    }
}