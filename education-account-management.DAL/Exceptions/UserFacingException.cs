namespace Exceptions
{
    public class UserFacingException(
        string? message,
        int statusCode,
        Exception? innerException = null)
        : Exception(string.IsNullOrWhiteSpace(message) ? "An error occurred." : message, innerException)
    {
        public int StatusCode { get; } = statusCode;
    }
}
