using System;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Web.Contracts;
using MediatR.Pipeline;

namespace Gaver.Web.CrossCutting
{
    public class SharedListRequestPreProcessor : IRequestPreProcessor<ISharedListRequest>
    {
        private readonly IAccessChecker accessChecker;

        public SharedListRequestPreProcessor(IAccessChecker accessChecker)
        {
            this.accessChecker = accessChecker;
        }

        public async Task Process(ISharedListRequest request, CancellationToken cancellationToken)
        {
            await accessChecker.CheckWishListInvitations(request.WishListId, request.UserId, cancellationToken);
            await accessChecker.CheckNotOwner(request.WishListId, request.UserId, cancellationToken);
        }
    }
}
