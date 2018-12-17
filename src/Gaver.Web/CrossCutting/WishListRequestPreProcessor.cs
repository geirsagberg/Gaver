using System.Threading;
using System.Threading.Tasks;
using Gaver.Web.Contracts;
using MediatR.Pipeline;

namespace Gaver.Web.CrossCutting
{
    public class WishListRequestPreProcessor : IRequestPreProcessor<IWishListRequest>
    {
        private readonly IAccessChecker accessChecker;

        public WishListRequestPreProcessor(IAccessChecker accessChecker)
        {
            this.accessChecker = accessChecker;
        }

        //public Task ProcessRequestAsync(IWishListRequest request)
        //{
        //    accessChecker.CheckWishListInvitations(request.WishListId, request.User);
        //    accessChecker.CheckNotOwner(request.WishListId, request.User);
        //    return Task.CompletedTask;
        //}

        public Task Process(IWishListRequest request, CancellationToken cancellationToken)
        {
            accessChecker.CheckWishListInvitations(request.WishListId, request.UserId);
            accessChecker.CheckNotOwner(request.WishListId, request.UserId);
            return Task.CompletedTask;
        }
    }

    //public interface IRequestPreProcessor<in TRequest>
    //{
    //    Task ProcessRequestAsync(TRequest request);
    //}

    //public class PipelineActionFilter : IAsyncActionFilter
    //{
    //    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //    {
    //        foreach (var value in context.ActionArguments.Values) {
    //            var processorType = typeof(IRequestPreProcessor<>).MakeGenericType(value.GetType());
    //            var processor = context.HttpContext.RequestServices.GetService(processorType);
    //            if (processor != null) {
    //                var task = (Task) processorType.GetMethod(nameof(IRequestPreProcessor<object>.ProcessRequestAsync))
    //                    .Invoke(processor, new[] {value});
    //                await task;
    //            }
    //        }

    //        await next();
    //    }
    //}
}
