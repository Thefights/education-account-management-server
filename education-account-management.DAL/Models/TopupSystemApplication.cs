using System;

namespace Models
{
    public class TopupSystemApplication : BaseEntity
    {
        [NotDefaultValue]
        public int TopupRuleId { get; set; }
        [OnDelete(OnDeleteBehavior.NoAction)]
        public TopupRule TopupRule { get; set; } = null!;

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
