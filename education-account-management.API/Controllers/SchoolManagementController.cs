using Authorization;
using Controllers.Base;
using DTOs.SchoolManagement;
using Filters;
using Interfaces.SchoolManagement;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.Admin)]
    public class SchoolManagementController(ISchoolManagementService service)
        : CrudController<CreateSchoolManagementDTO, GetSchoolManagementDTO, UpdateSchoolManagementDTO, SchoolManagementFilterDTO>(service)
    {
        protected override string? EntityName => "School";
    }
}
