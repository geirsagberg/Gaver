using System.Threading.Tasks;
using Gaver.Web.Features.SharedList.Requests;
using HybridModelBinding;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.SharedList
{
    public class SharedListsController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public SharedListsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<SharedListsDto> GetSharedLists()
        {
            return mediator.Send(new GetSharedListsRequest());
        }

        [HttpGet("{wishListId:int}")]
        public Task<SharedListDto> Get(int wishListId)
        {
            return mediator.Send(new GetSharedListRequest {
                WishListId = wishListId
            });
        }

        [HttpPut("{wishListId:int}/{wishId:int}/Bought")]
        public Task<SharedWishDto> SetBought([FromHybrid] SetBoughtRequest request)
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
