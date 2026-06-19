using Common;
using EntityAnnotations;

namespace Models
{
    public class TopupScheduleRule : AuditEntity
    {
        [NotDefaultValue]
        public int TopupScheduleId { get; set; }
        public TopupSchedule TopupSchedule { get; set; } = null!;

        [NotDefaultValue]
        public int TopupRuleId { get; set; }
        public TopupRule TopupRule { get; set; } = null!;
    }
}
