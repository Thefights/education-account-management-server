using Interfaces.Base;

namespace Interfaces
{
    public interface ISchoolManagementService :
        IBaseCrudService<CreateSchoolManagementDTO, GetSchoolManagementDTO, UpdateSchoolManagementDTO>;
}