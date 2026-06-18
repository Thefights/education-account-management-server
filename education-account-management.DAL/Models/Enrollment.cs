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

        public DateTime EnrolledAt { get; set; }

        [DateValidator(NotBefore = nameof(EnrolledAt))]
        public DateTime? CompletedAt { get; set; }

        [DateValidator(NotBefore = nameof(EnrolledAt))]
        public DateTime? WithdrawnAt { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public Charge? Charge { get; set; }
    }
}
