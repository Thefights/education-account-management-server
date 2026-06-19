using DTOs.SchoolManagement;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class SchoolManagementMapper :
        ICrudMapper<School, CreateSchoolManagementDTO, GetSchoolManagementDTO, UpdateSchoolManagementDTO>
    {
        public partial School MapFromCreateDTO(CreateSchoolManagementDTO createDTO);

        public partial void MapFromUpdateDTO(UpdateSchoolManagementDTO updateDTO, School model);

        public partial GetSchoolManagementDTO MapToGetDTO(School model);

        public partial List<GetSchoolManagementDTO> MapToGetDTOList(List<School> models);

        public partial IQueryable<GetSchoolManagementDTO> ProjectToGetDTO(IQueryable<School> query);
    }
}
