namespace Filters.Audit
{
    public class ManagementActionLogFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(ManagementActionLog.Id),
                ["batchId"] = nameof(ManagementActionLog.BatchId),
                ["entityType"] = nameof(ManagementActionLog.EntityType),
                ["entityId"] = nameof(ManagementActionLog.EntityId),
                ["action"] = nameof(ManagementActionLog.Action),
                ["actorUserId"] = nameof(ManagementActionLog.ActorUserId),
                ["actorUserRole"] = $"{nameof(ManagementActionLog.ActorUser)}.{nameof(User.Role)}",
                ["ipAddress"] = nameof(ManagementActionLog.IpAddress),
                ["occurredAt"] = nameof(ManagementActionLog.OccurredAt)
            };

        public override string Sort { get; set; } = "occurredAt desc";

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(ManagementActionLog.EntityType))]
        public List<ManagementActionEntityType>? EntityTypes { get; set; }

        [FilterField(FilterOperationEnum.In, nameof(ManagementActionLog.Action))]
        public List<ManagementAction>? Actions { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(ManagementActionLog.EntityId))]
        public int? EntityId { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(ManagementActionLog.BatchId))]
        public Guid? BatchId { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(ManagementActionLog.ActorUserId))]
        public int? ActorUserId { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(ManagementActionLog.IpAddress))]
        public string? IpAddress { get; set; }

        [SearchField(nameof(ManagementActionLog.Reason))]
        public string? Reason { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(ManagementActionLog.OccurredAt))]
        public DateTime? OccurredFrom { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(ManagementActionLog.OccurredAt))]
        public DateTime? OccurredTo { get; set; }
    }
}
