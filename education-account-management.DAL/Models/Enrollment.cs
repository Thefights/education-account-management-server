namespace Models
{
    public class Enrollment : AuditEntity
    {
        [NotDefaultValue]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        [NotDefaultValue]
        public int SchoolStudentId { get; set; }
        public SchoolStudent SchoolStudent { get; set; } = null!;

        [MessageRequired, MessageMaxLength(150)]
        public string SchoolNameSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string CourseNameSnapshot { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? CourseDescriptionSnapshot { get; set; }

        [MessageRequired, MessageMaxLength(20), SingaporeNric]
        public string CitizenNricSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string CitizenFullNameSnapshot { get; set; } = string.Empty;

        [MessageMaxLength(320), EmailValidator]
        public string? CitizenEmailSnapshot { get; set; }

        [MessageMaxLength(20), PhoneNumberValidator]
        public string? CitizenPhoneNumberSnapshot { get; set; }

        [MessageRequired, MessageMaxLength(30)]
        public string AccountNumberSnapshot { get; set; } = string.Empty;

        [OnDelete(OnDeleteBehavior.Restrict)]
        public Charge? Charge { get; set; }
    }
}
