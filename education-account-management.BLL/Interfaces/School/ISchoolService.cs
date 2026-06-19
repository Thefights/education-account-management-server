using DTOs.School;
using Interfaces.Base;

namespace Interfaces.School
{
    public interface ISchoolService :
        IBaseCrudService<CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO>;
}