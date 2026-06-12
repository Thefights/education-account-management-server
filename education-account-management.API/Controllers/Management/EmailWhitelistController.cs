using Controllers.Base;
using DTOs.Email;
using Interfaces.Email;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.Admin)]
    public class EmailWhitelistController(IEmailWhitelistService emailWhitelistService) : BaseController
    {
        private readonly IEmailWhitelistService _emailWhitelistService = emailWhitelistService;

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _emailWhitelistService.GetAllAsync();
            return Result.SuccessData(result);
        }

        [HttpPut]
        public async Task<IActionResult> Save(SaveEmailWhitelistDTO saveDTO, CancellationToken cancellationToken)
        {
            var result = await _emailWhitelistService.SaveAsync(saveDTO, cancellationToken);
            return Result.SuccessData(result, "Email whitelist saved successfully");
        }
    }
}
