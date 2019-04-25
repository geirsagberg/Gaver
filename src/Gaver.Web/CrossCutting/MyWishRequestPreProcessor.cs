using System.Threading;
using System.Threading.Tasks;
using Gaver.Web.Contracts;
using MediatR.Pipeline;

namespace Gaver.Web.CrossCutting
{
    public class MyWishRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IAccessChecker accessChecker;

        public MyWishRequestPreProcessor(IAccessChecker accessChecker)
        {
            this.accessChecker = accessChecker;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (request is IMyWishRequest myWishRequest) {
                await accessChecker.CheckWishOwner(myWishRequest.WishId, myWishRequest.WishListId, myWishRequest.UserId,
                    cancellationToken);
            }
        }
    }
}
