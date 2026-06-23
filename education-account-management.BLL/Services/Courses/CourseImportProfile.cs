using DTOs.Courses;
using DTOs.Csv;
using Interfaces.Csv;

namespace Services.Courses
{
    public class CourseImportProfile(CourseMapper mapper)
        : ICsvImportProfile<Course, CreateCourseDTO>
    {
        private readonly CourseMapper _mapper = mapper;

        public string EntityName => nameof(Course);

        public Course MapToEntity(CreateCourseDTO row)
        {
            return _mapper.MapFromCreateDTO(row);
        }

        public Task<List<BatchImportErrorDTO>> ValidateRowAsync(
            CreateCourseDTO row,
            int rowNumber,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<List<BatchImportErrorDTO>>([]);
        }
    }
}