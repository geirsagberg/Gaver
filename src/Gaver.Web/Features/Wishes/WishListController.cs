using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Logic.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;

namespace Gaver.Web.Features.Wishes
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class WishListController : Controller
    {
        private readonly GetMyListHandler _getMyListHandler;
        private readonly GetSharedListHandler _getSharedListHandler;
        private readonly IMediator mediator;

        public WishListController(GetMyListHandler getMyListHandler, GetSharedListHandler getSharedListHandler)
        {
            _getMyListHandler = getMyListHandler;
            _getSharedListHandler = getSharedListHandler;
//            hub = signalRManager.GetHubContext<ListHub, IListHubClient>();
        }

        [HttpGet]
        public MyListModel Get()
        {
            return _getMyListHandler.Handle(new GetMyListRequest {UserName = User.Identity.Name});
        }

        [HttpGet("{listId:int}")]
        public SharedListModel Get(int listId)
        {
            return _getSharedListHandler.Handle(new GetSharedListRequest {ListId = listId});
        }

        [HttpPost("{listId:int}")]
        public WishModel Post(int listId, AddWishRequest request)
        {
            request.UserName = User.Identity.Name;
            request.WishListId = listId;
            return mediator.Send(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetUrl")]
        public WishModel SetUrl(int listId, int wishId, SetUrlRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return mediator.Send(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetBought")]
        public WishModel SetBought(int listId, int wishId, SetBoughtRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            request.UserName = User.Identity.Name;
            return mediator.Send(request);
        }

        [HttpDelete("{listId:int}/{wishId:int}")]
        public void Delete(int listId, int wishId)
        {
            mediator.Send(new DeleteWishRequest {WishId = wishId, WishListId = listId});
        }

        [HttpPost("{listId:int}/Share")]
        public async Task ShareList(int listId, ShareListRequest request)
        {
            request.WishListId = listId;
            request.UserName = User.Identity.Name;
            await mediator.SendAsync(request);
        }
    }
}