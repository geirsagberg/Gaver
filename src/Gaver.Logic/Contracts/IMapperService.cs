namespace Gaver.Logic
{
    public interface IMapperService {
        TTo Map<TFrom, TTo>(TFrom source);
    }
}