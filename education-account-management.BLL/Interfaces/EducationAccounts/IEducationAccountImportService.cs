using DTOs.Csv;

namespace Interfaces.EducationAccounts
{
    public interface IEducationAccountImportService
    {
        Task<BatchImportResultDTO> ImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default);
    }
}
