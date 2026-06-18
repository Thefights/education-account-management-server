using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.AdminManagement;
using Filters;
using Interfaces.AdminManagement;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.Admin)]
    public class AdminManagementController(IAdminManagementService service)
        : GetController<GetAdminManagementDTO, AdminManagementFilterDTO>(service)
    {
        private readonly IAdminManagementService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAdminManagementDTO createDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(createDTO, cancellationToken);
            return Result.SuccessData(result, "Admin account created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateAdminManagementDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(id, updateDTO, cancellationToken);
            return Result.SuccessData(result, "Admin account updated successfully");
        }
    }
}
