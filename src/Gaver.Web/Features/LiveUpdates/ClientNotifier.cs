using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Gaver.Web.Features.LiveUpdates
{
    public class ClientNotifier
    {
        private readonly IHubContext<ListHub> hub;

        public ClientNotifier(IHubContext<ListHub> hub)
        {
            this.hub = hub;
        }

        public Task RefreshListAsync(int wishListId, int? excludeUserId = null)
        {
            return hub.RefreshDataAsync(wishListId, excludeUserId);
        }
    }
}