namespace DTOs.TopUp
{
    public sealed class GetSystemTopupDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal? TopupAmount { get; set; }
        public string? Status { get; set; }
        public TopupConditionGroupDTO? RootConditionGroup { get; set; }
    }

    public sealed class CreateSystemTopupDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal? TopupAmount { get; set; }
        public TopupConditionGroupRequestDTO RootConditionGroup { get; set; } = new();
    }

    public sealed class UpdateSystemTopupDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal? TopupAmount { get; set; }

        [EnumDefined]
        public SystemTopupStatus Status { get; set; }

        public TopupConditionGroupRequestDTO RootConditionGroup { get; set; } = new();
    }

    public sealed class BatchUpdateSystemTopupStatusDTO
    {
        [MessageMinLength(1)]
        public List<int> Ids { get; set; } = [];

        [EnumDefined]
        public SystemTopupStatus Status { get; set; }

        [MessageRequired, MessageMinLength(10), MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }

}
