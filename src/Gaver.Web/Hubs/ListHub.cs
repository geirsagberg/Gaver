using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Logic.Contracts;
using Gaver.Web.Extensions;
using Gaver.Web.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using Microsoft.Extensions.Logging;

namespace Gaver.Web
{
    [HubName("listHub")]
    public class ListHub : Hub<IListHubClient>
    {
        public ListHub(ILogger<ListHub> logger, GaverContext gaverContext, IMapperService mapper)
        {
            this.logger = logger;
            _gaverContext = gaverContext;
            _mapper = mapper;
        }

        private readonly ILogger<ListHub> logger;
        private readonly GaverContext _gaverContext;
        private readonly IMapperService _mapper;

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
        public void UnsubscribeList(int listId)
        {
            userListConnections.RemoveWhere(c => c.ConnectionId == Context.ConnectionId);
            Clients.Group(GetGroup(listId)).UpdateUsers(GetStatus(listId));
        }

        [Authorize]
        public void UnsubscribeAll()
        {
            var listIds = userListConnections
                .Where(c => c.ConnectionId == Context.ConnectionId)
                .Select(c => c.ListId).ToList();
            userListConnections.RemoveWhere(c => c.ConnectionId == Context.ConnectionId);
            foreach (var listId in listIds)
            {
                Clients.OthersInGroup(GetGroup(listId)).UpdateUsers(GetStatus(listId));
            }
        }

        private SubscriptionStatus GetStatus(int listId)
        {
            var connections = userListConnections.Where(c => c.ListId == listId).ToList();
            var userIds = connections.Select(c => c.UserId).ToList();
            var users = _gaverContext.Users.Where(u => userIds.Contains(u.Id));
            var userModels = _mapper.Map<UserModel[]>(users);
            return new SubscriptionStatus
            {
                CurrentUsers = userModels
            };
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            logger.LogDebug("Connection {ConnectionId} disconnected", Context.ConnectionId);
            UnsubscribeAll();
            return base.OnDisconnected(stopCalled);
        }
    }

    public static class ListHubExtensions
    {
        public static void RefreshData(this IHubContext<ListHub, IListHubClient> hub, int listId, int? excludeUserId = null)
        {
            var excludeConnectionIds = excludeUserId.HasValue
                ? ListHub.GetConnectionIdsForUser(excludeUserId.Value)
                : new string[0];
            hub.Clients.Group(ListHub.GetGroup(listId), excludeConnectionIds).Refresh();
        }
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
        void Refresh();
    }

    public class SubscriptionStatus
    {
        public IEnumerable<UserModel> CurrentUsers { get; set; }
    }
}