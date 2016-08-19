using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using Microsoft.Extensions.Logging;

namespace Gaver.Web
{
    [HubName("listHub")]
    public class ListHub : Hub<IListHubClient>
    {
        public ListHub(ILogger<ListHub> logger)
        {
            this.logger = logger;
        }
        private readonly ILogger<ListHub> logger;
        private static readonly HashSet<string> connections = new HashSet<string>();

        public void Lol()
        {
            logger.LogDebug("Yo");
        }

        public SubscriptionStatus Subscribe()
        {
            connections.Add(Context.ConnectionId);
            return new SubscriptionStatus
            {
                Count = connections.Count
            };
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            connections.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }

    public interface IListHubClient
    {

    }

    public class SubscriptionStatus
    {
        public int Count { get; set; }
    }
}