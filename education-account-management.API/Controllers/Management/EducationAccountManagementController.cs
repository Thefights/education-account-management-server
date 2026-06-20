using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.EducationAccounts;
using Filters.EducationAccounts;
using Interfaces.EducationAccounts;
using Models;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class EducationAccountManagementController(
        IEducationAccountService educationAccountService,
        IEducationAccountImportService importService)
        : CrudController<CreateEducationAccountDTO, GetEducationAccountDTO,
            UpdateEducationAccountDTO, EducationAccountFilterDTO>(educationAccountService)
    {
        private readonly IEducationAccountImportService _importService = importService;

        protected override string EntityName => nameof(EducationAccount);

        [HttpPost("import")]
        public async Task<IActionResult> Import(
            [FromForm] IFormFile file,
            CancellationToken cancellationToken)
        {
            var result = await _importService.ImportAsync(file, cancellationToken);
            return Result.SuccessData(result, "Education account CSV import processed.");
        }
    }
}
