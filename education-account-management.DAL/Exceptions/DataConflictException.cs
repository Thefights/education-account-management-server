namespace Exceptions
{
    public class DataConflictException(string? message) : UserFacingException(message, 409)
    {
        public DataConflictException(Type entityType, string propertyName)
            : this($"This {propertyName} of {entityType.Name} has already been exist!") { }
    }
}
