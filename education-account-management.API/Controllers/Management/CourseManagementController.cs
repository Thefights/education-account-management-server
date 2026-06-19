using Authorization;
using Controllers.Base;
using DTOs.Course;
using Filters;
using Interfaces.Course;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class CourseManagementController(ICourseService service)
        : CrudController<CreateCourseDTO, GetCourseDTO, UpdateCourseDTO, CourseFilterDTO>(service)
    {
        protected override string? EntityName => "Course";
    }
}
