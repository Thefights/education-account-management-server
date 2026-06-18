using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.AiAssistant;
using Interfaces.AiAssistant;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.Admin)]
    public class AiAssistantSettingController(IAiAssistantSettingService service) : BaseController
    {
        private readonly IAiAssistantSettingService _service = service;

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _service.GetAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            [FromBody] UpdateAiAssistantSettingDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(updateDTO, cancellationToken);
            return Result.SuccessData(result, "AI Assistant setting updated successfully");
        }
    }
}
