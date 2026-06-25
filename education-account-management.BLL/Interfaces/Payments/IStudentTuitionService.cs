using DTOs.Courses;
using Filters.Courses;
using Results;
using System.Threading;
using System.Threading.Tasks;

namespace Interfaces.Payments
{
    public interface IStudentTuitionService
    {
        Task<StudentTuitionSummaryDTO> GetTuitionSummaryAsync(
            CancellationToken cancellationToken = default);

        Task<PaginationResult<StudentTuitionChargeDTO>> GetTuitionChargesPaginatedAsync(
            StudentTuitionFilterDTO filter, 
            CancellationToken cancellationToken = default);
    }
}
