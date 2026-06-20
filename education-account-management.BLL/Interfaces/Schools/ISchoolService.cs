using DTOs.Schools;
using Interfaces.Base;

namespace Interfaces.Schools
{
    public interface ISchoolService :
        IBaseCrudService<CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO>;
}