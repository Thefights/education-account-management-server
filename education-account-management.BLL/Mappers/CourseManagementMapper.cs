using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class CourseManagementMapper :
        ICrudMapper<Course, CreateCourseManagementDTO, GetCourseManagementDTO, UpdateCourseManagementDTO>
    {
        public partial Course MapFromCreateDTO(CreateCourseManagementDTO createDTO);

        public partial void MapFromUpdateDTO(UpdateCourseManagementDTO updateDTO, Course model);

        [MapProperty(nameof(Course.School) + "." + nameof(School.SchoolName),
            nameof(GetCourseManagementDTO.SchoolName))]
        public partial GetCourseManagementDTO MapToGetDTO(Course model);

        public partial List<GetCourseManagementDTO> MapToGetDTOList(List<Course> models);

        public partial IQueryable<GetCourseManagementDTO> ProjectToGetDTO(IQueryable<Course> query);
    }
}