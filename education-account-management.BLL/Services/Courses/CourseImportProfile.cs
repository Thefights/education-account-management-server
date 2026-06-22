using DTOs.Courses;
using DTOs.Csv;
using Interfaces.Csv;

namespace Services.Courses
{
    public class CourseImportProfile(
        CourseMapper mapper,
        IUnitOfWork unitOfWork) : ICsvImportProfile<Course, CreateCourseDTO>
    {
        private readonly CourseMapper _mapper = mapper;
        private readonly IGenericRepository<Course> _courseRepository = unitOfWork.Repository<Course>();
        private readonly IGenericRepository<School> _schoolRepository = unitOfWork.Repository<School>();

        public string EntityName => nameof(Course);

        public Course MapToEntity(CreateCourseDTO row)
        {
            return _mapper.MapFromCreateDTO(row);
        }

        public async Task<List<BatchImportErrorDTO>> ValidateRowAsync(CreateCourseDTO row, int rowNumber, CancellationToken cancellationToken = default)
        {
            var errors = new List<BatchImportErrorDTO>();

            if (row.SchoolId <= 0)
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.SchoolId), "School ID is required and must be valid."));
            }
            else
            {
                var schoolExists = await _schoolRepository.AnyAsync(s => s.Id == row.SchoolId, cancellationToken);
                if (!schoolExists)
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.SchoolId), $"School with ID {row.SchoolId} does not exist."));
                }
            }

            if (string.IsNullOrWhiteSpace(row.CourseName))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.CourseName), "Course name is required."));
            }

            if (row.CourseFeeAmount < 0)
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.CourseFeeAmount), "Course fee cannot be negative."));
            }

            if (row.MiscFeeAmount < 0)
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.MiscFeeAmount), "Misc fee cannot be negative."));
            }

            if (row.GstAmount < 0)
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.GstAmount), "GST amount cannot be negative."));
            }

            return errors;
        }
    }
}
