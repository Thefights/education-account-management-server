using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.Courses;
using Exceptions;
using Filters.Courses;
using Interfaces.Courses;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class CourseManagementController(
        ICourseService service,
        CourseImportService importService,
        IEnrollmentService enrollmentService)
        : GetController<GetCourseDTO, CourseFilterDTO>(service)
    {
        private readonly ICourseService _service = service;
        private readonly CourseImportService _importService = importService;
        private readonly IEnrollmentService _enrollmentService = enrollmentService;

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCourseDTO createDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(createDTO, cancellationToken);
            return Result.SuccessData(result, "Course created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateCourseDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(id, updateDTO, cancellationToken);
            return Result.SuccessData(result, "Course updated successfully");
        }

        [HttpPost("publish")]
        public async Task<IActionResult> Publish(
            PublishCourseDTO publishDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.PublishAsync(publishDTO, cancellationToken);
            return Result.SuccessData(result, $"{result.Count} Course(s) published successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var rowVersion = ParseIfMatchHeader(Request.Headers.IfMatch.ToString());
            await _service.DeleteAsync(id, rowVersion, cancellationToken);
            return Result.SuccessAction("Course deleted successfully");
        }

        [HttpDelete("selected")]
        public async Task<IActionResult> DeleteSelected(
            DeleteSelectedCoursesDTO deleteDTO,
            CancellationToken cancellationToken)
        {
            await _service.DeleteSelectedAsync(deleteDTO, cancellationToken);
            return Result.SuccessAction($"{deleteDTO.Items.Count} selected Courses deleted successfully");
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(
            [FromForm] IFormFile file,
            CancellationToken cancellationToken)
        {
            var result = await _importService.ImportAsync(file, cancellationToken);
            return Result.SuccessData(result, "Course CSV import processed.");
        }

        [HttpGet("{id}/eligible-students")]
        public async Task<IActionResult> GetEligibleStudents(
            int id,
            SchoolStudentFilterDTO filterDTO,
            CancellationToken cancellationToken)
        {
            var result = await _enrollmentService.GetEligibleStudentsAsync(
                id,
                filterDTO,
                cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPost("{id}/enrollments")]
        public async Task<IActionResult> AssignStudents(
            int id,
            AssignCourseStudentsDTO assignDTO,
            CancellationToken cancellationToken)
        {
            var result = await _enrollmentService.AssignAsync(
                new AssignEnrollmentsDTO
                {
                    CourseId = id,
                    SchoolStudentIds = assignDTO.SchoolStudentIds
                },
                cancellationToken);
            return Result.SuccessData(result, $"{result.Count} student(s) assigned successfully");
        }

        private static byte[] ParseIfMatchHeader(string ifMatch)
        {
            if (string.IsNullOrWhiteSpace(ifMatch))
            {
                throw new ValidationFailureException(
                    nameof(Course.RowVersion),
                    "If-Match header is required.");
            }

            var value = ifMatch.Trim();
            if (value.StartsWith("W/", StringComparison.OrdinalIgnoreCase))
            {
                value = value[2..].Trim();
            }

            value = value.Trim('"');
            try
            {
                return Convert.FromBase64String(value);
            }
            catch (FormatException)
            {
                throw new ValidationFailureException(
                    nameof(Course.RowVersion),
                    "If-Match header must contain a valid Base64 row version.");
            }
        }
    }
}