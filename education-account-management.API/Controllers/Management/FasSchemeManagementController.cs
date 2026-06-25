using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.FasSchemes;
using Filters.FasSchemes;
using Interfaces.FasSchemes;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class FasSchemeManagementController(IFasSchemeService service)
        : CrudController<CreateFasSchemeDTO, GetFasSchemeDTO, UpdateFasSchemeDTO, FasSchemeFilterDTO>(service)
    {
        private readonly IFasSchemeService _fasSchemeService = service;

        protected override string? EntityName => "FasScheme";

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(
            BatchUpdateFasSchemeStatusDTO dto,
            CancellationToken cancellationToken)
        {
            await _fasSchemeService.UpdateStatusesAsync(dto, cancellationToken);
            return Result.SuccessAction("FAS scheme statuses updated successfully.");
        }

        [HttpPost("{id}/duplicate")]
        public async Task<IActionResult> Duplicate(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _fasSchemeService.DuplicateAsync(id, cancellationToken);
            return Result.SuccessData(result, "FAS scheme duplicated successfully.");
        }
    }
}
