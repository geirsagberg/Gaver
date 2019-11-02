using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gaver.Common.Attributes;
using Gaver.Common.Contracts;

namespace Gaver.Common.Utils
{
    [SingletonService]
    public class MapperService : IMapperService
    {
        private readonly IMapper mapper;

        public MapperService(IEnumerable<Profile> profiles)
        {
            Profiles = profiles.ToList();
            MapperConfiguration = new MapperConfiguration(config => {
                foreach (var profile in Profiles) {
                    config.AddProfile(profile);
                }
            });
            mapper = MapperConfiguration.CreateMapper();
        }

        public IEnumerable<Profile> Profiles { get; }
        public IConfigurationProvider MapperConfiguration { get; }

        public TTo Map<TTo>(object source)
        {
            return mapper.Map<TTo>(source);
        }

        public void ValidateMappings()
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
