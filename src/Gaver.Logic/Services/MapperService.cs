using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gaver.Logic.Contracts;

namespace Gaver.Logic.Services
{
    public class MapperService : IMapperService
    {
        private readonly IMapper mapper;
        public IConfigurationProvider MapperConfiguration { get; }

        public MapperService(IEnumerable<Profile> profiles)
        {
            MapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMissingTypeMaps = true;
                config.CreateMap<Mail, SendGridMail>()
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
                foreach (var profile in profiles)
                {
                    config.AddProfile(profile);
                }
            });
            mapper = MapperConfiguration.CreateMapper();
        }

        public TTo Map<TTo>(object source)
        {
            return mapper.Map<TTo>(source);
        }

        public void ValidateMappings() => mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}