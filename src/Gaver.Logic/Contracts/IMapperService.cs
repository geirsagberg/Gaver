namespace Gaver.Logic.Contracts
{
    public interface IMapperService
    {
        TTo Map<TFrom, TTo>(TFrom source);
    }
}