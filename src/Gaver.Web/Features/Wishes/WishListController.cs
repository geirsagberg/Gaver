using System.Threading.Tasks;
using Gaver.Web.Extensions;
using Gaver.Web.Features.Wishes.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Wishes
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class WishListController : GaverControllerBase
    {
        private readonly WishReader _wishReader;
        private readonly WishMailer _wishMailer;
        private readonly WishCommander _wishCommander;

        public WishListController(WishReader wishReader, WishMailer wishMailer, WishCommander wishCommander)
        {
            _wishReader = wishReader;
            _wishMailer = wishMailer;
            _wishCommander = wishCommander;
        }

        [HttpGet]
        public MyListModel Get()
        {
            return _wishReader.Handle(new GetMyListRequest {UserId = UserId});
        }

        [HttpGet("{listId:int}")]
        public SharedListModel Get(int listId)
        {
            return _wishReader.Handle(new GetSharedListRequest {ListId = listId});
        }

        [HttpPost("{listId:int}")]
        public WishModel Post(int listId, AddWishRequest request)
        {
            request.UserId = UserId;
            request.WishListId = listId;
            return _wishCommander.Handle(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetUrl")]
        public WishModel SetUrl(int listId, int wishId, SetUrlRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return _wishCommander.Handle(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetDescription")]
        public WishModel SetDescription(int listId, int wishId, SetDescriptionRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return _wishCommander.Handle(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetBought")]
        public SharedWishModel SetBought(int listId, int wishId, SetBoughtRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            request.UserName = User.Identity.Name;
            return _wishCommander.Handle(request);
        }

        [HttpDelete("{listId:int}/{wishId:int}")]
        public void Delete(int listId, int wishId)
        {
            _wishCommander.Handle(new DeleteWishRequest {WishId = wishId, WishListId = listId});
        }

        [HttpPost("{listId:int}/Share")]
        public async Task ShareList(int listId, ShareListRequest request)
        {
            request.WishListId = listId;
            request.UserName = User.Identity.Name;
            await _wishMailer.HandleAsync(request);
        }
    }
}