using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Web.Contracts;
using Gaver.Web.Extensions;
using Gaver.Web.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Gaver.Web.Hubs
{
    [Authorize]
    public class ListHub : Hub<IListHubClient>
    {
        private static readonly HashSet<UserListConnection> userListConnections
            = new HashSet<UserListConnection>();

        private readonly IAccessChecker accessChecker;
        private readonly GaverContext gaverContext;

        private readonly ILogger<ListHub> logger;
        private readonly IMapperService mapper;

        public ListHub(ILogger<ListHub> logger, GaverContext gaverContext, IMapperService mapper,
            IAccessChecker accessChecker)
        {
            this.logger = logger;
            this.gaverContext = gaverContext;
            this.mapper = mapper;
            this.accessChecker = accessChecker;
        }

        public static string[] GetConnectionIdsForUser(int userId)
            => userListConnections.Where(ulc => ulc.UserId == userId).Select(ulc => ulc.ConnectionId).ToArray();

        public static string GetGroup(int listId) => $"List-{listId}";

        public async Task<SubscriptionStatus> Subscribe(int listId)
        {
            var principal = Context.User;
            var userId = principal.GetUserId();
            accessChecker.CheckWishListInvitations(listId, userId);
            var connectionId = Context.ConnectionId;
            userListConnections.Add(new UserListConnection {
                UserId = userId,
                ListId = listId,
                ConnectionId = connectionId
            });
            var group = GetGroup(listId);
            await Groups.AddAsync(connectionId, group);
            var status = GetStatus(listId);
            await Clients.Group(group).UpdateUsers(status);
            return status;
        }

        public override Task OnConnectedAsync() => Task.CompletedTask;

        public Task Unsubscribe(int listId)
        {
            userListConnections.RemoveWhere(c => c.ConnectionId == Context.ConnectionId);
            var status = GetStatus(listId);
            var groupName = GetGroup(listId);
            return Clients.Group(groupName).UpdateUsers(status);
        }

        public Task UnsubscribeAll()
        {
            var listIds = userListConnections
                .Where(c => c.ConnectionId == Context.ConnectionId)
                .Select(c => c.ListId).ToList();
            userListConnections.RemoveWhere(c => c.ConnectionId == Context.ConnectionId);
            var tasks = listIds.Select(listId => {
                var status = GetStatus(listId);
                var groupName = GetGroup(listId);
                return Clients.Group(groupName).UpdateUsers(status);
            });
            return Task.WhenAll(tasks);
        }

        private SubscriptionStatus GetStatus(int listId)
        {
            var connections = userListConnections.Where(c => c.ListId == listId).ToList();
            var userIds = connections.Select(c => c.UserId).ToList();
            var users = gaverContext.Users.Where(u => userIds.Contains(u.Id));
            var userModels = mapper.Map<UserModel[]>(users);
            return new SubscriptionStatus {
                CurrentUsers = userModels
            };
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            logger.LogDebug("Connection {ConnectionId} disconnected", Context.ConnectionId);
            await UnsubscribeAll();
            await base.OnDisconnectedAsync(ex);
        }
    }

    public static class ListHubExtensions
    {
        public static Task RefreshDataAsync(this IHubContext<ListHub> hub, int listId, int? excludeUserId = null) =>
            hub.Clients.Group(ListHub.GetGroup(listId)).InvokeAsync("refresh");
    }

    public class UserListConnection
    {
        public int UserId { get; set; }
        public int ListId { get; set; }
        public string ConnectionId { get; set; }
    }

    public interface IListHubClient
    {
        Task UpdateUsers(SubscriptionStatus status);
        Task HeartBeat();
        Task Refresh();
    }

    public class SubscriptionStatus
    {
        public IEnumerable<UserModel> CurrentUsers { get; set; }
    }
}