namespace Filters
{
    public class AuditLogFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(AuditLog.Id),
                ["actorUserId"] = nameof(AuditLog.ActorUserId),
                ["actorUserIdText"] = nameof(AuditLog.ActorUserIdText),
                ["actorFullName"] = nameof(AuditLog.ActorFullName),
                ["category"] = nameof(AuditLog.Category),
                ["action"] = nameof(AuditLog.Action),
                ["object"] = nameof(AuditLog.Object),
                ["ipAddress"] = nameof(AuditLog.IpAddress),
                ["createdAt"] = nameof(AuditLog.CreatedAt)
            };

        public override string Sort { get; set; } = "createdAt desc";

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(AuditLog.Category))]
        public List<AuditLogCategory>? Categories { get; set; }

        [FilterField(FilterOperationEnum.In, nameof(AuditLog.Action))]
        public List<AuditLogAction>? Actions { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(AuditLog.ActorUserIdText))]
        public string? ActorUserIdText { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(AuditLog.ActorFullName))]
        public string? ActorFullName { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(AuditLog.Object))]
        public string? Object { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(AuditLog.IpAddress))]
        public string? IpAddress { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(AuditLog.CreatedAt))]
        public DateTime? CreatedFrom { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(AuditLog.CreatedAt))]
        public DateTime? CreatedTo { get; set; }
    }
}
