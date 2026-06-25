using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.Schools;
using Filters.Schools;
using Interfaces.Schools;
using Microsoft.AspNetCore.Mvc;
using Services.Base;
using Models;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class SchoolManagementController(ISchoolService service, CsvImportService<School, CreateSchoolDTO> importService)
        : CrudController<CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO, SchoolFilterDTO>(service)
    {
        private readonly ISchoolService _service = service;
        private readonly CsvImportService<School, CreateSchoolDTO> _importService = importService;
        protected override string? EntityName => "School";

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(BatchUpdateSchoolStatusDTO dto, CancellationToken cancellationToken)
        {
            await _service.UpdateSchoolsStatusAsync(dto, cancellationToken);
            return Result.SuccessAction("Schools status updated successfully.");
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(
            [FromForm] IFormFile file,
            CancellationToken cancellationToken)
        {
            var result = await _importService.ImportAsync(file, cancellationToken);
            return Result.SuccessData(result, "School CSV import processed.");
        }
    }
}
