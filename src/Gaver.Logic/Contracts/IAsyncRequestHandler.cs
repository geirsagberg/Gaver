using System.Threading.Tasks;

namespace Gaver.Logic.Contracts
{
    public interface IAsyncRequestHandler<in TRequest, TReturn>
    {
        Task<TReturn> HandleAsync(TRequest request);
    }

    public interface IAsyncRequestHandler<in TRequest>
    {
        Task HandleAsync(TRequest request);
    }
}