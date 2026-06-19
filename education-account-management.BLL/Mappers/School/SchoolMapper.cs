using DTOs.School;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class SchoolMapper :
        ICrudMapper<School, CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO>
    {
        public partial School MapFromCreateDTO(CreateSchoolDTO createDTO);

        public partial void MapFromUpdateDTO(UpdateSchoolDTO updateDTO, School model);

        public partial GetSchoolDTO MapToGetDTO(School model);

        public partial List<GetSchoolDTO> MapToGetDTOList(List<School> models);

        public partial IQueryable<GetSchoolDTO> ProjectToGetDTO(IQueryable<School> query);
    }
}