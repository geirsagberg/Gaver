using System.Threading;
using System.Threading.Tasks;
using Gaver.Web.Contracts;
using MediatR.Pipeline;

namespace Gaver.Web.CrossCutting
{
    public class MyListRequestPreProcessor : IRequestPreProcessor<IMyListRequest>
    {
        private readonly IAccessChecker accessChecker;

        public MyListRequestPreProcessor(IAccessChecker accessChecker)
        {
            this.accessChecker = accessChecker;
        }

        public async Task Process(IMyListRequest request, CancellationToken cancellationToken)
        {
            await accessChecker.CheckOwner(request.WishListId, request.UserId, cancellationToken);
        }
    }
}
