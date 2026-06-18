namespace Models
{
    public class AdminProfile : AuditEntity
    {
        [MessageMaxLength(50), Unique]
        public string StaffCode { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(320), EmailValidator, Unique]
        public string Email { get; set; } = string.Empty;

        [MessageMaxLength(20), PhoneNumberValidator]
        public string? PhoneNumber { get; set; }

        [NotDefaultValue, Unique]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int? SchoolId { get; set; }
        public School? School { get; set; }
    }
}