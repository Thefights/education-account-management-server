using DTOs.Enrollments;
using DTOs.SchoolStudents;
using Filters.SchoolStudents;
using Interfaces.Base;
using Results;

namespace Interfaces.Enrollments
{
    public interface IEnrollmentService : IBaseGetService<GetEnrollmentDTO>
    {
        Task<List<GetEnrollmentDTO>> AssignAsync(
            AssignEnrollmentsDTO assignDTO,
            CancellationToken cancellationToken = default);

        Task<PaginationResult<GetSchoolStudentDTO>> GetEligibleStudentsAsync(
            int courseId,
            SchoolStudentFilterDTO filterDTO,
            CancellationToken cancellationToken = default);

        Task RemoveSelectedAsync(
            RemoveSelectedEnrollmentsDTO removeDTO,
            CancellationToken cancellationToken = default);

        Task WithdrawAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
