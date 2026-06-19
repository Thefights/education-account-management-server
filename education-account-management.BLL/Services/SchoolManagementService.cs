using Interfaces;
using Services.Base;

namespace Services
{
    public class SchoolManagementService(
        IUnitOfWork unitOfWork,
        SchoolManagementMapper mapper)
        : BaseService<School, CreateSchoolManagementDTO, GetSchoolManagementDTO, UpdateSchoolManagementDTO>(
            unitOfWork,
            mapper),
            ISchoolManagementService;
}