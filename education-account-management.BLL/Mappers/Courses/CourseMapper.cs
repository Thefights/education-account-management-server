using DTOs.Courses;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class CourseMapper :
        ICrudMapper<Course, CreateCourseDTO, GetCourseDTO, UpdateCourseDTO>
    {
        public partial Course MapFromCreateDTO(CreateCourseDTO createDTO);

        [MapperIgnoreTarget(nameof(Course.RowVersion))]
        public partial void MapFromUpdateDTO(UpdateCourseDTO updateDTO, Course model);

        [MapProperty(nameof(Course.School) + "." + nameof(School.SchoolName),
            nameof(GetCourseDTO.SchoolName))]
        [MapProperty(nameof(Course.Enrollments) + "." + nameof(Course.Enrollments.Count),
            nameof(GetCourseDTO.EnrollmentCount))]
        public partial GetCourseDTO MapToGetDTO(Course model);

        public partial List<GetCourseDTO> MapToGetDTOList(List<Course> models);

        public partial IQueryable<GetCourseDTO> ProjectToGetDTO(IQueryable<Course> query);
    }
}
