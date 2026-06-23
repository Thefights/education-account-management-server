using DTOs.SchoolStudents;
using Interfaces.Base;

namespace Interfaces.SchoolStudents
{
    public interface ISchoolStudentService :
        IBaseCrudService<CreateSchoolStudentDTO, GetSchoolStudentDTO, UpdateSchoolStudentDTO>
    {
    }
}