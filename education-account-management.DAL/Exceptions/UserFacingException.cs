namespace Exceptions
{
    public class UserFacingException : Exception
    {
        public int StatusCode { get; }

        public UserFacingException(string? message, int statusCode)
            : base(string.IsNullOrWhiteSpace(message) ? "An error occurred." : message)
        {
            StatusCode = statusCode;
        }

        public UserFacingException(string? message, int statusCode, Exception? innerException)
            : base(string.IsNullOrWhiteSpace(message) ? "An error occurred." : message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
