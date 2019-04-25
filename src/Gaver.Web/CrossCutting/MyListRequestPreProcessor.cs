using System.Threading;
using System.Threading.Tasks;
using Gaver.Web.Contracts;
using MediatR.Pipeline;

namespace Gaver.Web.CrossCutting
{
    public class MyListRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IAccessChecker accessChecker;

        public MyListRequestPreProcessor(IAccessChecker accessChecker)
        {
            this.accessChecker = accessChecker;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (request is IMyListRequest myListRequest) {
                await accessChecker.CheckOwner(myListRequest.WishListId, myListRequest.UserId, cancellationToken);
            }
        }
    }
}
