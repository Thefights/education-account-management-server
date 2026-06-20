using DTOs.Schools;
using Interfaces.Schools;
using Services.Base;


namespace Services.Schools
{
    public class SchoolService(
        IUnitOfWork unitOfWork,
        SchoolMapper mapper)
        : BaseService<School, CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO>(
            unitOfWork,
            mapper),
            ISchoolService;
}
