using DTOs.Auth;
using Interfaces.Auth;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.Admin)]
    public class AuthAccountManagementController(IAuthAccountManagementService authAccountManagementService)
        : CrudController<CreateAuthAccountDTO, GetAuthAccountDTO, UpdateAuthAccountDTO, AuthAccountFilterDTO>(authAccountManagementService)
    {
        private readonly IAuthAccountManagementService _authAccountManagementService = authAccountManagementService;

        protected override string? EntityName => "AuthAccount";

        [HttpPost("batch-import")]
        public async Task<IActionResult> BatchImport(
            BatchImportAuthAccountRequestDTO requestDTO,
            CancellationToken cancellationToken)
        {
            var result = await _authAccountManagementService.BatchImportAsync(
                requestDTO.File,
                requestDTO.SendEmail,
                cancellationToken);
            return Result.SuccessData(result, "AuthAccount batch import completed");
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateAuthAccountStatus(
            UpdateAuthAccountsStatusDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _authAccountManagementService.UpdateAuthAccountStatusAsync(updateDTO, cancellationToken);
            return Result.SuccessData(result, $"{result.Count} selected AuthAccounts status updated successfully");
        }

        [HttpPut("unlock")]
        public async Task<IActionResult> UnlockAuthAccounts(
            UnlockAuthAccountsDTO unlockDTO,
            CancellationToken cancellationToken)
        {
            var result = await _authAccountManagementService.UnlockAuthAccountsAsync(unlockDTO, cancellationToken);
            return Result.SuccessData(result, $"{result.Count} selected AuthAccounts unlocked successfully");
        }
    }
}