namespace Interfaces.Storage
{
    public interface IStorageService
    {
        Task UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);

        Task DeleteAsync(string fileName, CancellationToken cancellationToken = default);
    }
}
