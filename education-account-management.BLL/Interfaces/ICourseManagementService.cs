using Interfaces.Base;

namespace Interfaces
{
    public interface ICourseManagementService :
        IBaseCrudService<CreateCourseManagementDTO, GetCourseManagementDTO, UpdateCourseManagementDTO>;
}