using System.Threading;
using System.Threading.Tasks;
using Gaver.Web.Contracts;
using MediatR.Pipeline;

namespace Gaver.Web.CrossCutting
{
    public class MyWishRequestPreProcessor : IRequestPreProcessor<IMyWishRequest>
    {
        private readonly IAccessChecker accessChecker;

        public MyWishRequestPreProcessor(IAccessChecker accessChecker)
        {
            this.accessChecker = accessChecker;
        }

        public async Task Process(IMyWishRequest request, CancellationToken cancellationToken)
        {
            await accessChecker.CheckWishOwner(request.WishId, request.WishListId, request.UserId, cancellationToken);
        }
    }
}