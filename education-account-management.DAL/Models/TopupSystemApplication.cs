namespace Models
{
    public class TopupSystemApplication : BaseEntity
    {
        [NotDefaultValue]
        public int SystemTopupId { get; set; }
        [OnDelete(OnDeleteBehavior.NoAction)]
        public SystemTopup SystemTopup { get; set; } = null!;

        [NotDefaultValue]
        public int EducationAccountId { get; set; }
        [OnDelete(OnDeleteBehavior.NoAction)]
        public EducationAccount EducationAccount { get; set; } = null!;

        [NotDefaultValue]
        public int TopupExecutionTargetId { get; set; }
        [OnDelete(OnDeleteBehavior.NoAction)]
        public TopupExecutionTarget TopupExecutionTarget { get; set; } = null!;
    }
}