namespace Gaver.Data.Exceptions
{
    public class EntityNotFoundException<T> : DataException
    {
        public EntityNotFoundException(object id) : base($"Cannot find {typeof(T).Name} with ID {id}") { }
    }
}