using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;

namespace Gaver.Web.Features.WishList
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class WishController : Controller
    {
        private readonly GaverContext gaverContext;
        private readonly IHubContext<ListHub, IListHubClient> hub;
        private readonly IMediator mediator;
        private readonly IMapperService mapperService;

        public WishController(GaverContext gaverContext, IMediator mediator, IConnectionManager signalRManager, IMapperService mapperService)
        {
            this.gaverContext = gaverContext;
            this.mediator = mediator;
            this.mapperService = mapperService;
            hub = signalRManager.GetHubContext<ListHub, IListHubClient>();
        }

        [HttpGet]
        public MyListModel Get()
        {
            return mediator.Send(new GetMyListRequest { UserName = User.Identity.Name });
        }

        [HttpGet("/listId:int")]
        public SharedListModel Get(int listId)
        {
            return mediator.Send(new GetSharedListRequest { ListId = listId });
        }

        [HttpPost]
        public WishModel Post(AddWishRequest request)
        {
            var authenticatedRequest = mapperService.Map<AuthenticatedAddWishRequest>(request);
            authenticatedRequest.UserName = User.Identity.Name;
            return mediator.Send(authenticatedRequest);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            mediator.Send(new DeleteWishRequest { WishId = id });
        }

        [HttpPost("Share")]
        public async Task ShareList(ShareListRequest request)
        {
            await mediator.SendAsync(request);
        }

    }
}
