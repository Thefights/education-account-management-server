using DTOs.Courses;
using Filters.Courses;
using Results;

namespace Interfaces.Courses
{
    public interface IStudentCourseService
    {
        Task<PaginationResult<GetCourseDTO>> GetMyCoursesPaginatedAsync(
            StudentCourseFilterDTO filter, 
            CancellationToken cancellationToken = default);

        Task<StudentCourseDetailDTO> GetMyCourseDetailAsync(
            int courseId,
            CancellationToken cancellationToken = default);
    }
}
