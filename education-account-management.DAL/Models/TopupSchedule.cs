using Common;
using EntityAnnotations;
using EntityAnnotations.OnDeleteAttributes;
using Enums;
using System;
using System.Collections.Generic;

namespace Models
{
    public class TopupSchedule : AuditEntity
    {
        [MessageRequired, MessageMaxLength(150)]
        public string ScheduleName { get; set; } = string.Empty;

        [EnumDefined]
        public TopupScheduleType ScheduleType { get; set; } = TopupScheduleType.OneTime;

        public DateTime? OneTimeExecutionDate { get; set; }

        [MessageRange(1, 31)]
        public int? ExecuteAtDay { get; set; }

        [MessageRange(1, 12)]
        public int? ExecuteAtMonth { get; set; }

        public TimeSpan SpecificExecutionTime { get; set; } = TimeSpan.Zero;

        public bool IsActive { get; set; } = true;

        public DateTime? LastExecutedAt { get; set; }

        public DateTime NextExecutionAt { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<TopupScheduleRule> ScheduleRules { get; set; } = [];
    }
}
