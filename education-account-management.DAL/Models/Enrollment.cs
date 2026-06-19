using EntityAnnotations.DateAttributes;

namespace Models
{
    public class Enrollment : AuditEntity
    {
        [NotDefaultValue]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        [NotDefaultValue]
        public int EducationAccountId { get; set; }
        public EducationAccount EducationAccount { get; set; } = null!;

        [MessageRequired, MessageMaxLength(150)]
        public string SchoolNameSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string CourseNameSnapshot { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? CourseDescriptionSnapshot { get; set; }

        [MessageRequired, MessageMaxLength(9)]
        public string CitizenNricSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string CitizenFullNameSnapshot { get; set; } = string.Empty;

        [MessageMaxLength(320), EmailValidator]
        public string? CitizenEmailSnapshot { get; set; }

        [MessageMaxLength(20), PhoneNumberValidator]
        public string? CitizenPhoneNumberSnapshot { get; set; }

        [MessageRequired, MessageMaxLength(20)]
        public string AccountNumberSnapshot { get; set; } = string.Empty;

        public DateTime EnrolledAt { get; set; }

        [DateValidator(NotBefore = nameof(EnrolledAt))]
        public DateTime? CompletedAt { get; set; }

        [DateValidator(NotBefore = nameof(EnrolledAt))]
        public DateTime? WithdrawnAt { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public Charge? Charge { get; set; }
    }
}