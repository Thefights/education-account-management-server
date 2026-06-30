using Common.HttpResults;
using Controllers.Base;
using DTOs.AiChat;
using Infrastructure.Interface;

namespace Controllers
{
    [Authorize]
    public class AiChatController(IAiChatService aiChatService) : BaseController
    {
        private readonly IAiChatService _aiChatService = aiChatService;

        [HttpGet("status")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatus()
        {
            var isEnabled = await _aiChatService.GetAiStatusAsync();
            return Result.SuccessData(new { isEnabled = isEnabled });
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromForm] AiChatRequestDTO request)
        {
            var result = await _aiChatService.SendChatMessageAsync(request);

            if (result.IsSuccess)
            {
                return Content(result.Content!, "application/json");
            }

            return Result.FailError<object?>(null, result.ErrorMessage ?? "Error", result.StatusCode);
        }
    }
}
