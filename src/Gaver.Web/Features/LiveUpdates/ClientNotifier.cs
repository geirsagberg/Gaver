using Gaver.Common.Attributes;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Chat;
using Gaver.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Gaver.Web.Features.LiveUpdates;

[Service]
public class ClientNotifier(IHubContext<ListHub, IListHubClient> hub) : IClientNotifier {
    private readonly IHubContext<ListHub, IListHubClient> hub = hub;

    public Task RefreshListAsync(int wishListId, int? excludeUserId = null) {
        return hub.RefreshDataAsync(wishListId, excludeUserId);
    }

    public Task MessageAdded(int wishListId, ChatMessageDto chatMessage) {
        return chatMessage.User == null ? Task.CompletedTask : hub.Clients.GroupExcept(ListHub.GetGroup(wishListId),
                ListHub.GetConnectionIdsForUser(chatMessage.User.Id))
            .MessageAdded(chatMessage);
    }
}
