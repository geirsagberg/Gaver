using System;
using Gaver.Logic.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Chat
{
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : GaverControllerBase
    {
        private readonly AddMessageHandler addMessageHandler;
        private readonly GetMessagesHandler getMessagesHandler;
        private readonly IAccessChecker accessChecker;

        public ChatController(AddMessageHandler addMessageHandler, GetMessagesHandler getMessagesHandler, IAccessChecker accessChecker)
        {
            this.addMessageHandler = addMessageHandler;
            this.getMessagesHandler = getMessagesHandler;
            this.accessChecker = accessChecker;
        }

        [HttpPost("{listId:int}")]
        public ChatMessageModel AddMessage(int listId, AddMessageRequest request)
        {
            accessChecker.CheckWishListInvitations(listId, UserId);
            request.WishListId = listId;
            request.UserId = UserId;
            return addMessageHandler.Handle(request);
        }

        [HttpGet("{listId:int}")]
        public ChatModel GetMessages(int listId)
        {
            return getMessagesHandler.Handle(new GetMessagesRequest{WishListId = listId});
        }
    }
}