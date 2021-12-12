using Gaver.Web.Contracts;
using MediatR.Pipeline;

namespace Gaver.Web.CrossCutting;

public class SharedListRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly IAccessChecker accessChecker;

    public SharedListRequestPreProcessor(IAccessChecker accessChecker)
    {
        this.accessChecker = accessChecker;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (request is ISharedListRequest sharedListRequest) {
            await accessChecker.CheckWishListAccess(sharedListRequest.WishListId, sharedListRequest.UserId,
                cancellationToken);
            await accessChecker.CheckNotOwner(sharedListRequest.WishListId, sharedListRequest.UserId,
                cancellationToken);
        }
    }
}