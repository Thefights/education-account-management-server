namespace Models
{
    public class Role : AuditEntity
    {
        [MessageRequired, MessageMaxLength(50), Unique]
        public string Name { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
