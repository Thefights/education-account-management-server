namespace DTOs.TopUp;

public sealed class GetScheduleTopUpDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? TopupAmount { get; set; }
    public string? Frequency { get; set; }
    public string? Status { get; set; }
    public DateTime? OneTimeExecutionAt { get; set; }
    public int? ExecuteAtDay { get; set; }
    public int? ExecuteAtMonth { get; set; }
    public TimeOnly ExecutionTime { get; set; }
    public DateTime? NextExecutionAt { get; set; }
    public TopupConditionGroupDTO? RootConditionGroup { get; set; }
}

public sealed class CreateScheduleTopUpDTO
{
    public string Name { get; set; } = string.Empty;
    public decimal? TopupAmount { get; set; }

    [EnumDefined]
    public ScheduleTopUpFrequency Frequency { get; set; }

    public DateTime? OneTimeExecutionAt { get; set; }
    public int? ExecuteAtDay { get; set; }
    public int? ExecuteAtMonth { get; set; }
    public TimeOnly ExecutionTime { get; set; }
    public TopupConditionGroupRequestDTO RootConditionGroup { get; set; } = new();
}

public sealed class UpdateScheduleTopUpDTO
{
    public string Name { get; set; } = string.Empty;
    public decimal? TopupAmount { get; set; }

    [EnumDefined]
    public ScheduleTopUpFrequency Frequency { get; set; }

    public DateTime? OneTimeExecutionAt { get; set; }
    public int? ExecuteAtDay { get; set; }
    public int? ExecuteAtMonth { get; set; }
    public TimeOnly ExecutionTime { get; set; }

    [EnumDefined]
    public ScheduleTopUpStatus Status { get; set; }

    public TopupConditionGroupRequestDTO RootConditionGroup { get; set; } = new();
}

public sealed class BatchUpdateScheduleTopUpStatusDTO
{
    [MessageMinLength(1)]
    public List<int> Ids { get; set; } = [];

    [EnumDefined]
    public ScheduleTopUpStatus Status { get; set; }
}
