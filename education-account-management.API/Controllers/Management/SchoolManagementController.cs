using Authorization;
using Controllers.Base;
using DTOs.Schools;
using Filters.Schools;
using Interfaces.Schools;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class SchoolManagementController(ISchoolService service)
        : CrudController<CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO, SchoolFilterDTO>(service)
    {
        protected override string? EntityName => "School";
    }
}
