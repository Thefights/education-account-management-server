using Authorization;
using Controllers.Base;
using DTOs.Audit;
using Filters;
using Interfaces.Audit;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class AuditLogController(IAuditLogService auditLogService)
        : GetController<GetAuditLogDTO, AuditLogFilterDTO>(auditLogService)
    {
        private readonly IAuditLogService _auditLogService = auditLogService;

        [HttpPost("export")]
        public async Task<IActionResult> Export(
            [FromBody] ExportAuditLogRequestDTO request,
            CancellationToken cancellationToken)
        {
            var content = await _auditLogService.ExportCsvAsync(request, cancellationToken);
            var fileName = $"audit-logs-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

            return File(content, "text/csv; charset=utf-8", fileName);
        }
    }
}
