using System.Threading.Tasks;
using Gaver.Web.Features.Wishes.Models;
using Gaver.Web.Features.Wishes.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Wishes
{
    public class WishListController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public WishListController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<MyListModel> Get()
        {
            return mediator.Send(new GetMyListRequest());
        }

        [HttpGet("{listId:int}")]
        public Task<SharedListModel> Get(int listId)
        {
            return mediator.Send(new GetSharedListRequest {
                WishListId = listId,
            });
        }

        [HttpPost]
        public Task<WishModel> Post(AddWishRequest request)
        {
            return mediator.Send(request);
        }

        [HttpPost("Share")]
        public Task ShareList(ShareListRequest request)
        {
            return mediator.Send(request);
        }

        [HttpPost("Order")]
        public Task SetWishesOrder(SetWishesOrderRequest request)
        {
            return mediator.Send(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/Title")]
        public Task<WishModel> SetTitle(int listId, int wishId, SetTitleRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return mediator.Send(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/Url")]
        public Task<WishModel> SetUrl(int listId, int wishId, SetUrlRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return mediator.Send(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/Description")]
        public Task<WishModel> SetDescription(int listId, int wishId, SetDescriptionRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return mediator.Send(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/Bought")]
        public Task<SharedWishModel> SetBought(int listId, int wishId, SetBoughtRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return mediator.Send(request);
        }

        [HttpDelete("{listId:int}/{wishId:int}")]
        public Task<DeleteWishResponse> Delete(int listId, int wishId)
        {
            return mediator.Send(new DeleteWishRequest { WishId = wishId, WishListId = listId });
        }

        [HttpGet("{listId:int}/Access")]
        public Task<ListAccessStatus> CheckSharedListAccess(int listId)
        {
            return mediator.Send(new CheckSharedListAccessRequest {
                WishListId = listId
            });
        }
    }
}
