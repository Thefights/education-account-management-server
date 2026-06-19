using DTOs.SchoolManagement;
using Interfaces.Base;

namespace Interfaces.SchoolManagement
{
    public interface ISchoolManagementService :
        IBaseCrudService<CreateSchoolManagementDTO, GetSchoolManagementDTO, UpdateSchoolManagementDTO>;
}
