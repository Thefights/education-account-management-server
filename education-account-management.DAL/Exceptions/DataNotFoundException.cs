namespace Exceptions
{
    public class DataNotFoundException(string? message) : UserFacingException(message, 404)
    {
        public DataNotFoundException(Type entityType, int id)
            : this($"{entityType.Name} ({id}) was not found!") { }

        public DataNotFoundException(Type entityType, string id)
            : this($"{entityType.Name} ({id}) was not found!") { }

        public DataNotFoundException(string entityName, int id)
            : this($"{entityName} ({id}) was not found!") { }
    }
}
