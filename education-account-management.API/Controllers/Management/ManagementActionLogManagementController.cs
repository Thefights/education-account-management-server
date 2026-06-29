using Authorization;
using Controllers.Base;
using DTOs.Audit;
using Filters.Audit;
using Interfaces.Audit;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class ManagementActionLogManagementController(IManagementActionLogService managementActionLogService)
        : GetController<GetManagementActionLogDTO, ManagementActionLogFilterDTO>(managementActionLogService)
    {
        private readonly IManagementActionLogService _managementActionLogService = managementActionLogService;

        [HttpPost("export")]
        public async Task<IActionResult> Export(
            [FromBody] ExportManagementActionLogRequestDTO request,
            CancellationToken cancellationToken)
        {
            var content = await _managementActionLogService.ExportCsvAsync(request, cancellationToken);
            var fileName = $"management-action-logs-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

            return File(content, "text/csv; charset=utf-8", fileName);
        }
    }
}
