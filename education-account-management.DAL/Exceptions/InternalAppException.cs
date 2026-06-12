namespace Exceptions
{
    public class InternalAppException : Exception
    {
        public InternalAppException(string? message)
            : base(message)
        {
        }

        public InternalAppException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
