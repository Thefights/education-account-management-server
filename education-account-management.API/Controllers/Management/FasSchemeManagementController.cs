using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.FasSchemes;
using Filters.FasSchemes;
using Interfaces.FasSchemes;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class FasSchemeManagementController(IFasSchemeService service)
        : GetController<GetFasSchemeDTO, FasSchemeFilterDTO>(service)
    {
        private readonly IFasSchemeService _fasSchemeService = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFasSchemeDTO createDTO)
        {
            var result = await _fasSchemeService.CreateAsync(createDTO);
            return Result.SuccessData(result, "FasScheme created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFasSchemeDTO updateDTO)
        {
            var result = await _fasSchemeService.UpdateAsync(id, updateDTO);
            return Result.SuccessData(result, "FasScheme updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _fasSchemeService.DeleteAsync(id);
            return Result.SuccessAction("FasScheme deleted successfully");
        }

        [HttpDelete("selected")]
        public async Task<IActionResult> DeleteSelectedIds([FromQuery] List<int> ids)
        {
            await _fasSchemeService.DeleteSelectedIdsAsync(ids);
            return Result.SuccessAction($"{ids.Count} selected FasSchemes deleted successfully");
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(
            [FromBody] BatchUpdateFasSchemeStatusDTO dto,
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
