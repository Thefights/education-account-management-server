namespace Exceptions
{
    public class InternalAppException(string? message, Exception? innerException = null)
        : Exception(message, innerException);
}
