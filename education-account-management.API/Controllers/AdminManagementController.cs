using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.Admin;
using Filters;
using Interfaces;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class AdminManagementController(IAdminManagementService service)
        : GetController<GetAdminManagementDTO, AdminManagementFilterDTO>(service)
    {
        private readonly IAdminManagementService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdminManagementDTO createDTO, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(createDTO, cancellationToken);
            return Result.SuccessData(result, "Admin account created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,
            UpdateAdminManagementDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(id, updateDTO, cancellationToken);
            return Result.SuccessData(result, "Admin account updated successfully");
        }
    }
}