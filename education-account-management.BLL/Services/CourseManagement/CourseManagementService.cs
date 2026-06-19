using DTOs.CourseManagement;
using Interfaces.CourseManagement;
using Services.Base;

namespace Services.CourseManagement
{
    public class CourseManagementService(
        IUnitOfWork unitOfWork,
        CourseManagementMapper mapper)
        : BaseService<Course, CreateCourseManagementDTO, GetCourseManagementDTO, UpdateCourseManagementDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(Course.School)]),
            ICourseManagementService;
}
