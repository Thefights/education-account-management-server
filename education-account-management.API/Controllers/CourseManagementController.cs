using Authorization;
using Controllers.Base;
using DTOs.CourseManagement;
using Filters;
using Interfaces.CourseManagement;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.Admin)]
    public class CourseManagementController(ICourseManagementService service)
        : CrudController<CreateCourseManagementDTO, GetCourseManagementDTO, UpdateCourseManagementDTO, CourseManagementFilterDTO>(service)
    {
        protected override string? EntityName => "Course";
    }
}
