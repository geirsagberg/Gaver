using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Gaver.Web.CrossCutting;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Chat
{
    public class AddMessageRequest : IRequest<ChatMessageModel>, IWishListRequest
    {
        [Required]
        [MinLength(1)]
        public string Text { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }
        
        [JsonIgnore]
        public int UserId { get; set; }
    }
}