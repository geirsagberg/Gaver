using System.Threading.Tasks;

namespace Gaver.Web.Contracts
{
    public interface IClientNotifier
    {
        Task RefreshListAsync(int wishListId, int? excludeUserId = null);
    }
}