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

        [NotDefaultValue]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
