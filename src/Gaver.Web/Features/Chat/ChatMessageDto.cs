using System;
using Gaver.Web.Features.Users;

namespace Gaver.Web.Features.Chat;

public class ChatMessageDto
{
    public int Id { get; set; }
    public string? Text { get; set; } = "";
    public DateTimeOffset Created { get; set; }
    public ChatUserDto? User { get; set; }
}

public class ChatUserDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PictureUrl { get; set; }
}
