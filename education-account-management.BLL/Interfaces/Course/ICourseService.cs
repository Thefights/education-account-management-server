using DTOs.Course;
using Interfaces.Base;

namespace Interfaces.Course
{
    public interface ICourseService :
        IBaseCrudService<CreateCourseDTO, GetCourseDTO, UpdateCourseDTO>;
}