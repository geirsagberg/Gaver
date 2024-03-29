using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Web.Contracts;
using Gaver.Web.Extensions;
using Gaver.Web.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Gaver.Web.Hubs;

[Authorize]
public class ListHub(ILogger<ListHub> logger, GaverContext gaverContext, IMapperService mapper,
    IAccessChecker accessChecker) : Hub<IListHubClient> {
    private static readonly HashSet<UserListConnection> UserListConnections = new();
    private readonly IAccessChecker accessChecker = accessChecker;
    private readonly GaverContext gaverContext = gaverContext;
    private readonly ILogger<ListHub> logger = logger;
    private readonly IMapperService mapper = mapper;

    public static string[] GetConnectionIdsForUser(int userId)
        => UserListConnections.Where(ulc => ulc.UserId == userId).Select(ulc => ulc.ConnectionId).ToArray();

    public static string GetGroup(int listId) => $"List-{listId}";

    public async Task<SubscriptionStatus> Subscribe(int listId) {
        var principal = Context.User ?? throw new FriendlyException("No user logged in");
        var userId = principal.GetUserId();
        await accessChecker.CheckWishListAccess(listId, userId);
        var connectionId = Context.ConnectionId;
        UserListConnections.Add(new UserListConnection {
            UserId = userId,
            ListId = listId,
            ConnectionId = connectionId
        });
        var group = GetGroup(listId);
        await Groups.AddToGroupAsync(connectionId, group);
        var status = GetStatus(listId);
        await Clients.Group(group).UpdateUsers(status);
        return status;
    }

    public override Task OnConnectedAsync() => Task.CompletedTask;

    public Task Unsubscribe(int listId) {
        UserListConnections.RemoveWhere(c => c.ConnectionId == Context.ConnectionId);
        var status = GetStatus(listId);
        var groupName = GetGroup(listId);
        return Clients.Group(groupName).UpdateUsers(status);
    }

    public Task UnsubscribeAll() {
        var listIds = UserListConnections
            .Where(c => c.ConnectionId == Context.ConnectionId)
            .Select(c => c.ListId).ToList();
        UserListConnections.RemoveWhere(c => c.ConnectionId == Context.ConnectionId);
        var tasks = listIds.Select(listId => {
            var status = GetStatus(listId);
            var groupName = GetGroup(listId);
            return Clients.Group(groupName).UpdateUsers(status);
        });
        return Task.WhenAll(tasks);
    }

    private SubscriptionStatus GetStatus(int listId) {
        var connections = UserListConnections.Where(c => c.ListId == listId).ToList();
        var userIds = connections.Select(c => c.UserId).ToList();
        var users = gaverContext.Users.Where(u => userIds.Contains(u.Id));
        var userModels = mapper.Map<UserDto[]>(users);
        return new SubscriptionStatus {
            CurrentUsers = userModels
        };
    }

    public override async Task OnDisconnectedAsync(Exception? ex) {
        logger.LogDebug("Connection {ConnectionId} disconnected", Context.ConnectionId);
        await UnsubscribeAll();
        await base.OnDisconnectedAsync(ex);
    }
}
