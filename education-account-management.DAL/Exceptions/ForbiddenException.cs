namespace Exceptions
{
    public class ForbiddenException(string message = "Access denied") : UserFacingException(message, 403);
}
