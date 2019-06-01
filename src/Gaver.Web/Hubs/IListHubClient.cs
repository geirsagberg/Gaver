using System.Threading.Tasks;

namespace Gaver.Web.Hubs
{
    public interface IListHubClient
    {
        Task UpdateUsers(SubscriptionStatus status);
        Task Refresh();
    }
}
