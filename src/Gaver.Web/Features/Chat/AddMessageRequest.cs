using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.Chat;

public class AddMessageRequest : IRequest<ChatMessageDto>, ISharedListRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public string Text { get; init; } = "";

    [JsonIgnore]
    [HybridBindProperty(Source.Route)]
    public int WishListId { get; set; }

    [JsonIgnore]
    public int UserId { get; set; }
}