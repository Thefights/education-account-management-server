using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.Schools;
using Filters.Schools;
using Interfaces.Schools;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class SchoolManagementController(ISchoolService service)
        : CrudController<CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO, SchoolFilterDTO>(service)
    {
        private readonly ISchoolService _service = service;
        protected override string? EntityName => "School";

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(BatchUpdateSchoolStatusDTO dto, CancellationToken cancellationToken)
        {
            await _service.UpdateSchoolsStatusAsync(dto, cancellationToken);
            return Result.SuccessAction("Schools status updated successfully.");
        }
    }
}
