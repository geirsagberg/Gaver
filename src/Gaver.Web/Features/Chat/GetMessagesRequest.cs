using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;

namespace Gaver.Web.Features.Chat;

public class GetMessagesRequest : IRequest<ChatDto>, ISharedListRequest {
    [JsonIgnore]
    [HybridBindProperty(Source.Route)]
    public int WishListId { get; init; }

    [JsonIgnore] public int UserId { get; set; }
}
