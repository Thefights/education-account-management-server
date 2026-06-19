using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.Admin;
using Filters.Admin;
using Interfaces.Admin;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class AdminManagementController(IAdminService service)
        : GetController<GetAdminDTO, AdminFilterDTO>(service)
    {
        private readonly IAdminService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdminDTO createDTO, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(createDTO, cancellationToken);
            return Result.SuccessData(result, "Admin account created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,
            UpdateAdminDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(id, updateDTO, cancellationToken);
            return Result.SuccessData(result, "Admin account updated successfully");
        }
    }
}