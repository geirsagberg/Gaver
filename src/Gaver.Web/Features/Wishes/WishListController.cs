using System.Threading.Tasks;
using Gaver.Web.Features.Wishes.Models;
using Gaver.Web.Features.Wishes.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Wishes
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class WishListController : GaverControllerBase
    {
        private readonly WishReader wishReader;
        private readonly WishMailer wishMailer;
        private readonly WishCommander wishCommander;

        public WishListController(WishReader wishReader, WishMailer wishMailer, WishCommander wishCommander)
        {
            this.wishReader = wishReader;
            this.wishMailer = wishMailer;
            this.wishCommander = wishCommander;
        }

        [HttpGet]
        public MyListModel Get()
        {
            return wishReader.Handle(new GetMyListRequest {UserId = UserId});
        }

        [HttpGet("{listId:int}")]
        public SharedListModel Get(int listId)
        {
            return wishReader.Handle(new GetSharedListRequest {
                ListId = listId,
                UserId = UserId
            });
        }

        [HttpPost("{listId:int}")]
        public WishModel Post(int listId, AddWishRequest request)
        {
            request.UserId = UserId;
            request.WishListId = listId;
            return wishCommander.Handle(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetUrl")]
        public WishModel SetUrl(int listId, int wishId, SetUrlRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return wishCommander.Handle(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetDescription")]
        public WishModel SetDescription(int listId, int wishId, SetDescriptionRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            return wishCommander.Handle(request);
        }

        [HttpPut("{listId:int}/{wishId:int}/SetBought")]
        public SharedWishModel SetBought(int listId, int wishId, SetBoughtRequest request)
        {
            request.WishListId = listId;
            request.WishId = wishId;
            request.UserId = UserId;
            return wishCommander.Handle(request);
        }

        [HttpDelete("{listId:int}/{wishId:int}")]
        public void Delete(int listId, int wishId)
        {
            wishCommander.Handle(new DeleteWishRequest {WishId = wishId, WishListId = listId});
        }

        [HttpPost("{listId:int}/Share")]
        public async Task ShareList(int listId, ShareListRequest request)
        {
            request.WishListId = listId;
            request.UserId = UserId;
            await wishMailer.HandleAsync(request);
        }

        [HttpPost("{listId:int}/RegisterToken")]
        public void RegisterToken(int listId, RegisterTokenRequest request)
        {
            request.WishListId = listId;
            request.UserId = UserId;
            wishCommander.Handle(request);
        }

        [HttpGet("{listId:int}/Access")]
        public Task<ListAccessStatus> CheckSharedListAccess(int listId)
        {
            return wishReader.HandleAsync(new CheckSharedListAccessRequest {
                UserId = UserId,
                WishListId = listId
            });
        }
    }
}