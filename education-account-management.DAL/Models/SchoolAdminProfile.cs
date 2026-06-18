namespace Models
{
    public class SchoolAdminProfile : AuditEntity
    {
        [NotDefaultValue, Unique]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [NotDefaultValue]
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;

        [MessageRequired, MessageMaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(320), EmailValidator, Unique]
        public string Email { get; set; } = string.Empty;

        [MessageMaxLength(20), PhoneNumberValidator]
        public string? PhoneNumber { get; set; }
    }
}
