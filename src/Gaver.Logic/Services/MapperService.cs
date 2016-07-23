using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Gaver.Logic
{
    public class MapperService : IMapperService
    {
        private readonly IMapper mapper;
        public MapperService()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMissingTypeMaps = true;
                config.CreateMap<Mail, SendGridMail>()
                    .MapMember(m => m.From, m => new SendGridAddress
                    {
                        Email = m.From
                    })
                    .MapMember(m => m.Personalizations, m => new SendGridPersonalization
                    {
                        To = m.To.Select(to => new SendGridAddress
                        {
                            Email = to
                        }).ToList()
                    });
            });
            mapper = mapperConfig.CreateMapper();
        }

        public TTo Map<TFrom, TTo>(TFrom source)
        {
            return mapper.Map<TFrom, TTo>(source);
        }

        public void ValidateMappings() => mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}