using DTOs.Base;

namespace Interfaces.Storage
{
    public interface IUploadService
    {
        Task<UploadResultDTO> UploadAsync(IFormFile file, string? folder = null, CancellationToken cancellationToken = default);

        Task DeleteAsync(string fileName, CancellationToken cancellationToken = default);
    }
}
