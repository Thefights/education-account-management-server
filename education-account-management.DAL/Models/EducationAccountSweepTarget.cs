using Enums;

namespace Models
{
    public class EducationAccountSweepTarget : BaseEntity
    {
        [NotDefaultValue]
        public int SweepReportId { get; set; }
        public EducationAccountSweepReport SweepReport { get; set; } = null!;

        [MessageRequired, MessageMaxLength(9), SingaporeNric]
        public string Nric { get; set; } = string.Empty;

        [EnumDefined]
        public SweepAction Action { get; set; }

        [EnumDefined]
        public SweepTargetStatus Status { get; set; }

        [MessageMaxLength(1000)]
        public string? Reason { get; set; }
    }
}
