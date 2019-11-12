using AutoMapper;
using Gaver.Data.Entities;

namespace Gaver.Web.Features.Chat
{
    public class ChatMappingProfile : Profile
    {
        public ChatMappingProfile()
        {
            CreateMap<ChatMessage, ChatMessageDto>();
        }
    }
}