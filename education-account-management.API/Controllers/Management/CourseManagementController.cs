using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.Courses;
using Filters.Courses;
using Interfaces.Courses;
using Models;
using Services.Base;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class CourseManagementController(ICourseService service, CsvImportService<Course, CreateCourseDTO> importService)
        : CrudController<CreateCourseDTO, GetCourseDTO, UpdateCourseDTO, CourseFilterDTO>(service)
    {
        private readonly CsvImportService<Course, CreateCourseDTO> _importService = importService;
        protected override string? EntityName => "Course";

        [HttpPost("import")]
        public async Task<IActionResult> Import(
            [FromForm] IFormFile file,
            CancellationToken cancellationToken)
        {
            var result = await _importService.ImportAsync(file, cancellationToken);
            return Result.SuccessData(result, "Course CSV import processed.");
        }
    }
}
