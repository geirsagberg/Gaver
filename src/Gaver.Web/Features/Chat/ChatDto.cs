using System.Collections.Generic;

namespace Gaver.Web.Features.Chat;

public class ChatDto
{
    public IList<ChatMessageDto> Messages { get; set; } = new List<ChatMessageDto>();
}