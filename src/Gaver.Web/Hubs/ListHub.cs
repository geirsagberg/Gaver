using System.Collections.Generic;
using System.Threading.Tasks;
using Gaver.Data.Entities;
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

        public const string ListGroup = "list";
        private readonly ILogger<ListHub> logger;
        private static readonly Dictionary<string, string> connections = new Dictionary<string, string>();

        public string Ping(string value)
        {
            logger.LogDebug("Ping called: {value)", value);
            return value;
        }

        public SubscriptionStatus Subscribe(string name)
        {
            connections[Context.ConnectionId] = name;
            Groups.Add(Context.ConnectionId, ListGroup);
            var status = GetStatus();
            Clients.OthersInGroup(ListGroup).UpdateUsers(status);
            return status;
        }

        public void Unsubscribe()
        {
            connections.Remove(Context.ConnectionId);
            Groups.Remove(Context.ConnectionId, ListGroup);
            Clients.Group(ListGroup).UpdateUsers(GetStatus());
        }

        private static SubscriptionStatus GetStatus()
        {
            return new SubscriptionStatus
            {
                Count = connections.Count,
                Names = connections.Values
            };
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Unsubscribe();
            return base.OnDisconnected(stopCalled);
        }
    }

    public interface IListHubClient
    {
        void UpdateUsers(SubscriptionStatus status);
        void HeartBeat();
        void Refresh(IEnumerable<Wish> wishes);
    }

    public class SubscriptionStatus
    {
        public int Count { get; set; }
        public IEnumerable<string> Names { get; set; }
    }
}