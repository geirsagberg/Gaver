using System.Threading.Tasks;
using Gaver.Web.Features.Wishes.Models;
using Gaver.Web.Features.Wishes.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Wishes
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
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
            return mediator.Send(new GetMyListRequest {UserId = UserId});
        }

        [HttpGet("{listId:int}")]
        public Task<SharedListModel> Get(int listId)
        {
            return mediator.Send(new GetSharedListRequest {
                ListId = listId,
                UserId = UserId
            });
        }

        [HttpPost("{listId:int}")]
        public Task<WishModel> Post(int listId, AddWishRequest request)
        {
            request.UserId = UserId;
            request.WishListId = listId;
            return mediator.Send(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetUrl")]
        public Task<WishModel> SetUrl(int listId, int wishId, SetUrlRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return mediator.Send(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetDescription")]
        public Task<WishModel> SetDescription(int listId, int wishId, SetDescriptionRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return mediator.Send(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetBought")]
        public Task<SharedWishModel> SetBought(int listId, int wishId, SetBoughtRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            request.UserId = UserId;
            return mediator.Send(request);
        }

        [HttpDelete("{listId:int}/{wishId:int}")]
        public Task Delete(int listId, int wishId)
        {
            return mediator.Send(new DeleteWishRequest {WishId = wishId, WishListId = listId});
        }

        [HttpPost("{listId:int}/Share")]
        public Task ShareList(int listId, ShareListRequest request)
        {
            request.WishListId = listId;
            request.UserId = UserId;
            return mediator.Send(request);
        }

        [HttpPost("{listId:int}/RegisterToken")]
        public Task RegisterToken(int listId, RegisterTokenRequest request)
        {
            request.WishListId = listId;
            request.UserId = UserId;
            return mediator.Send(request);
        }

        [HttpGet("{listId:int}/Access")]
        public Task<ListAccessStatus> CheckSharedListAccess(int listId)
        {
            return mediator.Send(new CheckSharedListAccessRequest {
                UserId = UserId,
                WishListId = listId
            });
        }
    }
}