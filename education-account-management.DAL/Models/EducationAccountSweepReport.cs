namespace Models
{
    public class EducationAccountSweepReport : BaseEntity
    {
        [Unique]
        public DateOnly BatchDate { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime CompletedAt { get; set; }

        public int AccountsCreatedCount { get; set; }

        public int AccountsClosedCount { get; set; }

        public int AccountsExtendedCount { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<EducationAccountSweepTarget> Targets { get; set; } = [];
    }
}