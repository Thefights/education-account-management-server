using Enums;
using Filters.Base;
using Models;

namespace Filters.Audit
{
    public class AuditLogFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(AuditLog.Id),
                ["actorUserId"] = nameof(AuditLog.ActorUserId),
                ["actorUserRole"] = $"{nameof(AuditLog.ActorUser)}.{nameof(User.Role)}",
                ["category"] = nameof(AuditLog.Category),
                ["action"] = nameof(AuditLog.Action),
                ["nric"] = nameof(AuditLog.Nric),
                ["ipAddress"] = nameof(AuditLog.IpAddress),
                ["occurredAt"] = nameof(AuditLog.OccurredAt)
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(AuditLog.Category))]
        public List<AuditLogCategory>? Categories { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(AuditLog.Action))]
        [SearchField(nameof(AuditLog.Action))]
        public string? Action { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(AuditLog.IpAddress))]
        public string? IpAddress { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(AuditLog.OccurredAt))]
        public DateTime? OccurredFrom { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(AuditLog.OccurredAt))]
        public DateTime? OccurredTo { get; set; }
    }
}
