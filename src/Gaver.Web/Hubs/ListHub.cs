using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;

namespace Gaver.Web
{
    [HubName("listHub")]
    public class ListHub : Hub<IListHubClient>
    {
        private static readonly HashSet<string> connections = new HashSet<string>();

        public SubscriptionStatus Subscribe()
        {
            connections.Add(Context.ConnectionId);
            return new SubscriptionStatus
            {
                Count = connections.Count
            };
        }

        public override Task OnDisconnected(bool stopCalled) {
            connections.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }

    public interface IListHubClient {

    }

    public class SubscriptionStatus
    {
        public int Count { get; set; }
    }
}