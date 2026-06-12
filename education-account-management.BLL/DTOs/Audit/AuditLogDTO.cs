using Filters;

namespace DTOs.Audit
{
    public class GetAuditLogDTO
    {
        public int Id { get; set; }

        public int? ActorUserId { get; set; }

        public string? ActorFullName { get; set; }

        public string? ActorUserIdText { get; set; }

        public string? Category { get; set; }

        public string? Action { get; set; }

        public string? Object { get; set; }

        public string? IpAddress { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class ExportAuditLogRequestDTO
    {
        public AuditLogFilterDTO Filter { get; set; } = new();

        public List<string> Fields { get; set; } = [];
    }
}
