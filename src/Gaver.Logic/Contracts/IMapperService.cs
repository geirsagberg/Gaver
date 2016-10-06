using AutoMapper;

namespace Gaver.Logic.Contracts
{
    public interface IMapperService
    {
        TTo Map<TTo>(object source);
        IConfigurationProvider MapperConfiguration { get; }
        void ValidateMappings();
    }
}