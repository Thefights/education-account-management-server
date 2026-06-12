namespace Interfaces.Base
{
    public interface IFileValidator
    {
        Task<(bool IsValid, string? ErrorMessage)> ValidateAsync(IFormFile file, CancellationToken cancellationToken = default);
    }
}
