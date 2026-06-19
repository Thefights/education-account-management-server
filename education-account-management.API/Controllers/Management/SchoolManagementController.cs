using Authorization;
using Controllers.Base;
using DTOs.School;
using Filters.School;
using Interfaces.School;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class SchoolManagementController(ISchoolService service)
        : CrudController<CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO, SchoolFilterDTO>(service)
    {
        protected override string? EntityName => "School";
    }
}
