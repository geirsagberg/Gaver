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

        private const string ListGroup = "list";
        private readonly ILogger<ListHub> logger;
        private static readonly HashSet<string> connections = new HashSet<string>();

        public void Lol()
        {
            logger.LogDebug("Yo");
        }

        public SubscriptionStatus Subscribe()
        {
            connections.Add(Context.ConnectionId);
            Groups.Add(Context.ConnectionId, ListGroup);
            var status = new SubscriptionStatus
            {
                Count = connections.Count
            };
            Clients.OthersInGroup(ListGroup).UpdateUsers(status);
            return status;
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            connections.Remove(Context.ConnectionId);
            Groups.Remove(Context.ConnectionId, ListGroup);
            Clients.Group(ListGroup).UpdateUsers(new SubscriptionStatus{Count = connections.Count});
            return base.OnDisconnected(stopCalled);
        }
    }

    public interface IListHubClient
    {
        void UpdateUsers(SubscriptionStatus status);
    }

    public class SubscriptionStatus
    {
        public int Count { get; set; }
    }
}