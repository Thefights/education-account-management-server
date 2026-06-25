using DTOs.Csv;

namespace Interfaces.SchoolStudents
{
    public interface ISchoolStudentImportService
    {
        Task<BatchImportResultDTO> ImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default);
    }
}