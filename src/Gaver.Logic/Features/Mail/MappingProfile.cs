using System.Linq;
using AutoMapper;

namespace Gaver.Logic.Features.Mail
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MailModel, SendGridMail>()
                    .MapMember(m => m.From, m => new SendGridAddress
                    {
                        Email = m.From,
                        Name = "Gaver"
                    })
                    .MapMember(m => m.Content, m => new[] {
                        new SendGridContent {
                            Value = m.Content,
                            Type = "text/html"
                        }
                    })
                    .MapMember(m => m.Personalizations, m => new[] {
                        new SendGridPersonalization {
                            To = m.To.Select(to => new SendGridAddress {
                                Email = to
                            }).ToList()
                        }
                    });
        }
    }
}
