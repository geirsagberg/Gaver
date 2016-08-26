namespace Gaver.Data
{
    public class DataException : System.Exception
    {
        public DataException() { }
        public DataException( string message ) : base( message ) { }
        public DataException( string message, System.Exception inner ) : base( message, inner ) { }
    }
}