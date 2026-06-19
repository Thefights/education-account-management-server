using DTOs.SchoolManagement;
using Interfaces.SchoolManagement;
using Services.Base;

namespace Services.SchoolManagement
{
    public class SchoolManagementService(
        IUnitOfWork unitOfWork,
        SchoolManagementMapper mapper)
        : BaseService<School, CreateSchoolManagementDTO, GetSchoolManagementDTO, UpdateSchoolManagementDTO>(
            unitOfWork,
            mapper),
            ISchoolManagementService;
}
