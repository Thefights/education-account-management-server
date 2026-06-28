using Authorization;
using Common.HttpResults;
using Controllers.Base;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class SchoolStudentManagementController(
        ISchoolStudentService service,
        ISchoolStudentImportService importService)
        : CrudController<CreateSchoolStudentDTO, GetSchoolStudentDTO, UpdateSchoolStudentDTO, SchoolStudentFilterDTO>(service)
    {
        private readonly ISchoolStudentImportService _importService = importService;

        protected override string? EntityName => "School Student";

        [HttpPost("import")]
        public async Task<IActionResult> Import(
            [FromForm] IFormFile file,
            CancellationToken cancellationToken)
        {
            var result = await _importService.ImportAsync(file, cancellationToken);
            return Result.SuccessData(result, "School student CSV import processed.");
        }
    }
}
