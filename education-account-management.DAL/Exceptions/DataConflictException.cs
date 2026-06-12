namespace Exceptions
{
    public class DataConflictException : UserFacingException
    {
        public DataConflictException(Type entityType, string propertyName)
            : base($"This {propertyName} of {entityType.Name} has already been exist!", 409) { }

        public DataConflictException(string? message) : base(message, 409) { }
    }
}
