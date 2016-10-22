namespace Gaver.Logic.Contracts
{
    public interface IRequestHandler <in TRequest, out TReturn>
    {
        TReturn Handle(TRequest request);
    }

    public interface IRequestHandler<in TRequest>
    {
        void Handle(TRequest request);
    }
}