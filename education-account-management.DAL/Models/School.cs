namespace Models
{
    public class School : AuditEntity
    {
        [EnumDefined]
        public SchoolStatus Status { get; set; } = SchoolStatus.Active;

        [MessageRequired, MessageMaxLength(150), Unique]
        public string SchoolName { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(20), PhoneNumberValidator, Unique]
        public string PhoneNumber { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(320), EmailValidator, Unique]
        public string Email { get; set; } = string.Empty;

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<AdminProfile> AdminProfiles { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<Course> Courses { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<SchoolStudent> SchoolStudents { get; set; } = [];
    }
}
