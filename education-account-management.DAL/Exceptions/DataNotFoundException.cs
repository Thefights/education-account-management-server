namespace Exceptions
{
    public class DataNotFoundException : UserFacingException
    {
        public DataNotFoundException(Type entityType, int id)
            : base($"{entityType.Name} ({id}) was not found!", 404) { }

        public DataNotFoundException(Type entityType, string id)
            : base($"{entityType.Name} ({id}) was not found!", 404) { }

        public DataNotFoundException(string entityName, int id)
            : base($"{entityName} ({id}) was not found!", 404) { }

        public DataNotFoundException(string? message) : base(message, 404) { }
    }
}
