using System.Threading.Tasks;
using Gaver.Web.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace Gaver.Web.Features.LiveUpdates
{
    public class ClientNotifier : IClientNotifier
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