namespace DTOs.TopUp
{
    // ── Schedule response ───────────────────────────────────────────────────

    public class GetTopupScheduleDTO
    {
        public int Id { get; set; }

        public int TopupRuleId { get; set; }

        public string? Frequency { get; set; }

        public string? Status { get; set; }

        public DateTime? OneTimeExecutionAt { get; set; }

        public int? ExecuteAtDay { get; set; }

        public int? ExecuteAtMonth { get; set; }

        public TimeOnly ExecutionTime { get; set; }

        public DateTime? NextExecutionAt { get; set; }

        public GetTopupRuleDTO? TopupRule { get; set; }
    }

    // ── Schedule requests ───────────────────────────────────────────────────

    public class CreateTopupScheduleDTO
    {
        public int TopupRuleId { get; set; }

        [EnumDefined]
        public TopupScheduleType Frequency { get; set; }

        public DateTime? OneTimeExecutionAt { get; set; }

        public int? ExecuteAtDay { get; set; }

        public int? ExecuteAtMonth { get; set; }

        public TimeOnly ExecutionTime { get; set; }
    }

    public class UpdateTopupScheduleDTO
    {
        public int TopupRuleId { get; set; }

        [EnumDefined]
        public TopupScheduleType Frequency { get; set; }

        public DateTime? OneTimeExecutionAt { get; set; }

        public int? ExecuteAtDay { get; set; }

        public int? ExecuteAtMonth { get; set; }

        public TimeOnly ExecutionTime { get; set; }

        [EnumDefined]
        public TopupScheduleStatus Status { get; set; }
    }

    public class BatchUpdateTopupScheduleStatusDTO
    {
        [MessageMinLength(1)]
        public List<int> Ids { get; set; } = [];

        [EnumDefined]
        public TopupScheduleStatus Status { get; set; }
    }
}