using DTOs.CourseManagement;
using Interfaces.Base;

namespace Interfaces.CourseManagement
{
    public interface ICourseManagementService :
        IBaseCrudService<CreateCourseManagementDTO, GetCourseManagementDTO, UpdateCourseManagementDTO>;
}
