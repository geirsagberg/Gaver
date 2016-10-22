using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Web.Extensions;
using Gaver.Web.Features.Wishes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using Microsoft.Extensions.Logging;

namespace Gaver.Web
{
    [HubName("listHub")]
    public class ListHub : Hub<IListHubClient>
    {
        public ListHub(ILogger<ListHub> logger, GaverContext gaverContext)
        {
            this.logger = logger;
            _gaverContext = gaverContext;
        }

        private readonly ILogger<ListHub> logger;
        private readonly GaverContext _gaverContext;

        private static readonly HashSet<UserListConnection> userListConnections
            = new HashSet<UserListConnection>();

        public static string[] GetConnectionIdsForUser(int userId)
            => userListConnections.Where(ulc => ulc.UserId == userId).Select(ulc => ulc.ConnectionId).ToArray();

        public string Ping(string value)
        {
            logger.LogDebug("Ping called: {value)", value);
            return value;
        }

        public static string GetGroup(int listId) => $"List-{listId}";

        [Authorize]
        public SubscriptionStatus Subscribe(int listId)
        {
            var principal = (ClaimsPrincipal) Context.User;
            var userId = principal.GetUserId();
            var connectionId = Context.ConnectionId;
            userListConnections.Add(new UserListConnection
            {
                UserId = userId,
                ListId = listId,
                ConnectionId = connectionId
            });
            var group = GetGroup(listId);
            Groups.Add(Context.ConnectionId, group);
            var status = GetStatus(listId);
            Clients.OthersInGroup(group).UpdateUsers(status);
            return status;
        }

        [Authorize]
        public void Unsubscribe()
        {
            var listIds = userListConnections
                .Where(c => c.ConnectionId == Context.ConnectionId)
                .Select(c => c.ListId);
            userListConnections.RemoveWhere(c => c.ConnectionId == Context.ConnectionId);
            foreach (var listId in listIds)
            {
                Clients.Group(GetGroup(listId)).UpdateUsers(GetStatus(listId));
            }
        }

        private SubscriptionStatus GetStatus(int listId)
        {
            var connections = userListConnections.Where(c => c.ListId == listId).ToList();
            var userIds = connections.Select(c => c.UserId).ToList();
            var userNamesById = _gaverContext.Users.Where(u => userIds.Contains(u.Id)).ToDictionary(u => u.Id, u => u.Name);
            return new SubscriptionStatus
            {
                Count = connections.Count,
                Names = userNamesById.Values.Distinct()
            };
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Unsubscribe();
            return base.OnDisconnected(stopCalled);
        }
    }

    public static class ListHubExtensions
    {
        public static void RefreshData(this IHubContext<ListHub, IListHubClient> hub, int listId, SharedListModel model)
            => hub.Clients.Group(ListHub.GetGroup(listId)).Refresh(model);
    }

    public class UserListConnection
    {
        public int UserId { get; set; }
        public int ListId { get; set; }
        public string ConnectionId { get; set; }
    }

    public interface IListHubClient
    {
        void UpdateUsers(SubscriptionStatus status);
        void HeartBeat();
        void Refresh(SharedListModel model);
    }

    public class SubscriptionStatus
    {
        public int Count { get; set; }
        public IEnumerable<string> Names { get; set; }
    }
}