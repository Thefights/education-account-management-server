using DTOs.School;
using Interfaces.School;
using Services.Base;

namespace Services.School
{
    public class SchoolService(
        IUnitOfWork unitOfWork,
        SchoolMapper mapper)
        : BaseService<School, CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO>(
            unitOfWork,
            mapper),
            ISchoolService;
}
