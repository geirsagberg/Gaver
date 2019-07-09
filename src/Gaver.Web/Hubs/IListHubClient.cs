using System.Threading.Tasks;
using Gaver.Web.Features.Chat;

namespace Gaver.Web.Hubs
{
    public interface IListHubClient
    {
        Task UpdateUsers(SubscriptionStatus status);
        Task Refresh();
        Task MessageAdded(ChatMessageModel message);
    }
}
