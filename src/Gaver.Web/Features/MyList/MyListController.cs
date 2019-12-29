using System.Threading.Tasks;
using Gaver.Web.Features.Shared.Models;
using HybridModelBinding;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.MyList
{
    public class MyListController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public MyListController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<MyListDto> GetMyList()
        {
            return mediator.Send(new GetMyListRequest());
        }

        [HttpPost]
        public Task<WishDto> Post(AddWishRequest request)
        {
            return mediator.Send(request);
        }

        [HttpPost("Order")]
        public Task SetWishesOrder(SetWishesOrderRequest request)
        {
            return mediator.Send(request);
        }

        [HttpPost("Share")]
        public Task ShareList(ShareListRequest request)
        {
            return mediator.Send(request);
        }

        [HttpPatch("{wishId:int}")]
        public Task UpdateWish([FromHybrid] UpdateWishRequest request)
        {
            return mediator.Send(request);
        }

        [HttpDelete("{wishId:int}")]
        public Task<DeleteWishResponse> Delete(int wishId)
        {
            return mediator.Send(new DeleteWishRequest {WishId = wishId});
        }

        [HttpPost("{wishId:int}/Option")]
        public Task<WishOptionDto> AddWishOption([FromHybrid] AddWishOptionRequest request) => mediator.Send(request);
    }
}
