using System.Threading.Tasks;
using Gaver.Web.Features.Chat;

namespace Gaver.Web.Contracts
{
    public interface IClientNotifier
    {
        Task RefreshListAsync(int wishListId, int? excludeUserId = null);
        Task MessageAdded(int wishListId, ChatMessageModel chatMessage);
    }
}