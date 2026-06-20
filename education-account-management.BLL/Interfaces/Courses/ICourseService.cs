using DTOs.Courses;
using Interfaces.Base;

namespace Interfaces.Courses
{
    public interface ICourseService :
        IBaseCrudService<CreateCourseDTO, GetCourseDTO, UpdateCourseDTO>;
}