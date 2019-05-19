using System.Threading.Tasks;
using Gaver.Web.Features.Wishes.Requests;
using Gaver.Web.Models;
using HybridModelBinding;
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

        [HttpGet("{wishListId:int}")]
        public Task<SharedListModel> Get(int wishListId)
        {
            return mediator.Send(new GetSharedListRequest {
                WishListId = wishListId
            });
        }

        [HttpPut("{wishListId:int}/{wishId:int}/Bought")]
        public Task<SharedWishModel> SetBought([FromHybrid] SetBoughtRequest request)
        {
            return mediator.Send(request);
        }

        [HttpGet("{wishListId:int}/Access")]
        public Task<ListAccessStatus> CheckSharedListAccess(int wishListId)
        {
            return mediator.Send(new CheckSharedListAccessRequest {
                WishListId = wishListId
            });
        }
    }
}
