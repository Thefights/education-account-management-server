namespace Exceptions
{
    public class ForbiddenException : UserFacingException
    {
        public ForbiddenException() : base("Access denied", 403)
        {
        }

        public ForbiddenException(string message) : base(message, 403)
        {
        }
    }
}
