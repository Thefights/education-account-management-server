using Filters.Audit;

namespace DTOs.Audit
{
    public class GetAuditLogDTO
    {
        public int Id { get; set; }

        public int? ActorUserId { get; set; }

        public string? Category { get; set; }

        public string? Action { get; set; }

        public string? IpAddress { get; set; }

        public DateTime OccurredAt { get; set; }

        public string? ActorUserRole { get; set; }
        public string? Nric { get; set; }
    }

    public class ExportAuditLogRequestDTO
    {
        public AuditLogFilterDTO Filter { get; set; } = new();

        public List<string> Fields { get; set; } = [];
    }

    public class GetManagementActionLogDTO
    {
        public int Id { get; set; }

        public Guid BatchId { get; set; }

        public string EntityType { get; set; } = string.Empty;

        public int EntityId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string? PreviousStatus { get; set; }

        public string? NewStatus { get; set; }

        public string Reason { get; set; } = string.Empty;

        public int? ActorUserId { get; set; }

        public string? ActorUserRole { get; set; }

        public string? ActorFullName { get; set; }

        public string? ActorEmail { get; set; }

        public DateTime OccurredAt { get; set; }

        public string IpAddress { get; set; } = string.Empty;
    }

    public class ExportManagementActionLogRequestDTO
    {
        public ManagementActionLogFilterDTO Filter { get; set; } = new();

        public List<string> Fields { get; set; } = [];
    }
}
