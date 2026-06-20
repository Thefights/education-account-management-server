using Authorization;
using Controllers.Base;
using DTOs.Courses;
using Filters.Courses;
using Interfaces.Courses;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class CourseManagementController(ICourseService service)
        : CrudController<CreateCourseDTO, GetCourseDTO, UpdateCourseDTO, CourseFilterDTO>(service)
    {
        protected override string? EntityName => "Course";
    }
}
