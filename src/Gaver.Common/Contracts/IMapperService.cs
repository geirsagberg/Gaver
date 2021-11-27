using System.Collections.Generic;
using AutoMapper;

namespace Gaver.Common.Contracts
{
    public interface IMapperService
    {
        IConfigurationProvider MapperConfiguration { get; }
        IEnumerable<Profile> Profiles { get; }
        TTo Map<TTo>(object source);
        void ValidateMappings();
    }
}
