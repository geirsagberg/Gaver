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
        private readonly GaverContext gaverContext;
        private readonly IHubContext<ListHub, IListHubClient> hub;
        private readonly IMediator mediator;
        private readonly IMapperService mapperService;

        public WishListController(GaverContext gaverContext, IMediator mediator, IConnectionManager signalRManager,
            IMapperService mapperService)
        {
            this.gaverContext = gaverContext;
            this.mediator = mediator;
            this.mapperService = mapperService;
            hub = signalRManager.GetHubContext<ListHub, IListHubClient>();
        }

        [HttpGet]
        public MyListModel Get()
        {
            return mediator.Send(new GetMyListRequest {UserName = User.Identity.Name});
        }

        [HttpGet("{listId:int}")]
        public SharedListModel Get(int listId)
        {
            return mediator.Send(new GetSharedListRequest {ListId = listId});
        }

        [HttpPost("{listId:int}")]
        public WishModel Post(int listId, AddWishRequest request)
        {
            var authenticatedRequest = mapperService.Map<AuthenticatedAddWishRequest>(request);
            authenticatedRequest.UserName = User.Identity.Name;
            authenticatedRequest.WishListId = listId;
            return mediator.Send(authenticatedRequest);
        }

        [HttpDelete("{listId:int}/{wishId:int}")]
        public void Delete(int listId, int wishId)
        {
            mediator.Send(new DeleteWishRequest {WishId = wishId, WishListId = listId});
        }

        [HttpPost("Share")]
        public async Task ShareList(ShareListRequest request)
        {
            await mediator.SendAsync(request);
        }
    }
}