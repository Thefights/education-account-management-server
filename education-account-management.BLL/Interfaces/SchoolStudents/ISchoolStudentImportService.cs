using DTOs.Csv;
using Microsoft.AspNetCore.Http;

namespace Interfaces.SchoolStudents;

public interface ISchoolStudentImportService
{
    Task<BatchImportResultDTO> ImportAsync(
        IFormFile file,
        CancellationToken cancellationToken = default);
}
