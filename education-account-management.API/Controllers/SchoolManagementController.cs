using Authorization;
using Controllers.Base;
using DTOs;
using Filters;
using Interfaces;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class SchoolManagementController(ISchoolManagementService service)
        : CrudController<CreateSchoolManagementDTO, GetSchoolManagementDTO, UpdateSchoolManagementDTO, SchoolManagementFilterDTO>(service)
    {
        protected override string? EntityName => "School";
    }
}