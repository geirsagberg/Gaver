using Gaver.Web.Contracts;
using MediatR.Pipeline;

namespace Gaver.Web.CrossCutting;

public class MyWishRequestPreProcessor<TRequest>(IAccessChecker accessChecker) : IRequestPreProcessor<TRequest> where TRequest : notnull {
    private readonly IAccessChecker accessChecker = accessChecker;

    public async Task Process(TRequest request, CancellationToken cancellationToken) {
        if (request is IMyWishRequest myWishRequest) {
            await accessChecker.CheckWishOwner(myWishRequest.WishId, myWishRequest.UserId,
                cancellationToken);
        }
    }
}
