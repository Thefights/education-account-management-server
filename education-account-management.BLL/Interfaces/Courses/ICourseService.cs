using DTOs.Courses;
using Interfaces.Base;

namespace Interfaces.Courses
{
    public interface ICourseService : IBaseGetService<GetCourseDTO>
    {
        Task<GetCourseDTO> CreateAsync(
            CreateCourseDTO createDTO,
            CancellationToken cancellationToken = default);

        Task<GetCourseDTO> UpdateAsync(
            int id,
            UpdateCourseDTO updateDTO,
            CancellationToken cancellationToken = default);

        Task<List<GetCourseDTO>> PublishAsync(
            PublishCourseDTO publishDTO,
            CancellationToken cancellationToken = default);

        Task<GetCourseDTO> DuplicateAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<GetCourseDTO> AssignFasSchemesAsync(
            int id,
            AssignCourseFasSchemesDTO assignDTO,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            int id,
            byte[] rowVersion,
            CancellationToken cancellationToken = default);

        Task DeleteSelectedAsync(
            DeleteSelectedCoursesDTO deleteDTO,
            CancellationToken cancellationToken = default);
    }
}
