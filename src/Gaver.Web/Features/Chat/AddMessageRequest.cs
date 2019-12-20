using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Chat
{
    public class AddMessageRequest : IRequest<ChatMessageDto>, ISharedListRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Text { get; set; } = "";

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
