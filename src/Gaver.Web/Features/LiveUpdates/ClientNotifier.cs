using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;

namespace Gaver.Web.Features.LiveUpdates
{
    public class ClientNotifier
    {
        private readonly IHubContext<ListHub, IListHubClient> hub;
        public ClientNotifier(IConnectionManager signalRManager)
        {
            hub = signalRManager.GetHubContext<ListHub, IListHubClient>();
        }

        public void RefreshList(int wishListId, int? excludeUserId = null)
        {
            hub.RefreshData(wishListId, excludeUserId);
        }
    }
}