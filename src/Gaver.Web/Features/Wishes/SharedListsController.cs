using System.Threading.Tasks;
using Gaver.Web.Features.Wishes.Requests;
using Gaver.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Wishes
{
    public class SharedListsController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public SharedListsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<SharedListsModel> GetSharedLists()
        {
            return mediator.Send(new GetSharedListsRequest());
        }

        [HttpGet("{listId:int}")]
        public Task<SharedListModel> Get(int listId)
        {
            return mediator.Send(new GetSharedListRequest {
                WishListId = listId
            });
        }

        [HttpPut("{listId:int}/{wishId:int}/Bought")]
        public Task<SharedWishModel> SetBought(int listId, int wishId, SetBoughtRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return mediator.Send(request);
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
