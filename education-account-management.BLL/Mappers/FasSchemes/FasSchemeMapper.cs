using DTOs.FasSchemes;
using Riok.Mapperly.Abstractions;

namespace Mappers.FasSchemes
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class FasSchemeMapper : ICrudMapper<FasScheme, CreateFasSchemeDTO, GetFasSchemeDTO, UpdateFasSchemeDTO>
    {
        public partial GetFasSchemeDTO MapToGetDTO(FasScheme model);
        public partial List<GetFasSchemeDTO> MapToGetDTOList(List<FasScheme> models);
        public partial IQueryable<GetFasSchemeDTO> ProjectToGetDTO(IQueryable<FasScheme> query);
        [MapperIgnoreTarget(nameof(FasScheme.Tiers))]
        [MapperIgnoreTarget(nameof(FasScheme.RequiredDocuments))]
        [MapperIgnoreTarget(nameof(FasScheme.SchemeCourses))]
        public partial FasScheme MapFromCreateDTO(CreateFasSchemeDTO createDTO);

        [MapperIgnoreTarget(nameof(FasScheme.Tiers))]
        [MapperIgnoreTarget(nameof(FasScheme.RequiredDocuments))]
        [MapperIgnoreTarget(nameof(FasScheme.SchemeCourses))]
        public partial void MapFromUpdateDTO(UpdateFasSchemeDTO updateDTO, FasScheme model);

        [MapProperty(nameof(FasSchemeCourse.CourseId), nameof(FasSchemeCourseDTO.CourseId))]
        [MapProperty($"{nameof(FasSchemeCourse.Course)}.{nameof(Course.CourseCode)}", nameof(FasSchemeCourseDTO.CourseCode))]
        [MapProperty($"{nameof(FasSchemeCourse.Course)}.{nameof(Course.CourseName)}", nameof(FasSchemeCourseDTO.CourseName))]
        public partial FasSchemeCourseDTO MapCourseToDTO(FasSchemeCourse model);
    }
}
