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

        public string? PayloadJson { get; set; }

        public DateTime OccurredAt { get; set; }

        public string? ActorUserRole { get; set; }
        public string? Nric { get; set; }
    }

    public class ExportAuditLogRequestDTO
    {
        public AuditLogFilterDTO Filter { get; set; } = new();

        public List<string> Fields { get; set; } = [];
    }
}