using System.Threading.Tasks;
using Gaver.Logic.Contracts;
using MediatR;

namespace Gaver.Web.CrossCutting
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IWishListRequest
    {
        private readonly IAccessChecker accessChecker;

        public AuthorizationBehavior(IAccessChecker accessChecker)
        {
            this.accessChecker = accessChecker;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            accessChecker.CheckWishListInvitations(request.WishListId, request.UserId);
            accessChecker.CheckNotOwner(request.WishListId, request.UserId);
            return next();
        }
    }
}