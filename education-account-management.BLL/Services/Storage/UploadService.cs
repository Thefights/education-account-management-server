using AvepointMosPlatform.BLL;
using DTOs.Base;
using Interfaces.Base;
using Interfaces.Storage;

namespace Services.Storage
{
    public class UploadService(
        IStorageService storageService,
        IFileValidator fileValidator,
        AppConfiguration configuration,
        ILogger<UploadService> logger)
        : IUploadService
    {
        private readonly IStorageService _storageService = storageService;
        private readonly IFileValidator _fileValidator = fileValidator;
        private readonly AppConfiguration _configuration = configuration;
        private readonly ILogger<UploadService> _logger = logger;

        public async Task<UploadResultDTO> UploadAsync(
            IFormFile file,
            string? folder = null,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _fileValidator.ValidateAsync(file, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationFailureException([validationResult.ErrorMessage ?? "Unknown validation error"]);
            }

            await using var fileStream = file.OpenReadStream();
            var fileName = GenerateFileName(file.FileName, folder);
            var contentType = ResolveContentType(file.ContentType);
            var fileSizeBytes = fileStream.CanSeek ? fileStream.Length : file.Length;

            try
            {
                await _storageService.UploadAsync(fileStream, fileName, contentType, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException and not InternalAppException)
            {
                _logger.LogError(ex, "Failed to upload file {FileName}.", file.FileName);
                throw new InternalAppException("File upload failed.", ex);
            }

            return new UploadResultDTO
            {
                FileName = fileName,
                PublicUrl = BuildPublicUrl(fileName),
                FileSizeBytes = fileSizeBytes
            };
        }

        public async Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ValidationFailureException(["File name is required"]);
            }

            var objectKey = NormalizeFileKey(fileName);
            try
            {
                await _storageService.DeleteAsync(objectKey, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed to delete file {FileName}.", objectKey);
            }
        }

        private static string ResolveContentType(string? originalContentType)
        {
            return string.IsNullOrWhiteSpace(originalContentType)
                ? "application/octet-stream"
                : originalContentType;
        }

        private static string GenerateFileName(string originalName, string? folder)
        {
            var id = Guid.NewGuid().ToString("N");
            var extension = Path.GetExtension(originalName).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".bin";
            }

            var finalFileName = $"{id}{extension}";
            var normalizedFolder = NormalizeFolder(folder);

            return string.IsNullOrWhiteSpace(normalizedFolder)
                ? finalFileName
                : $"{normalizedFolder}/{finalFileName}";
        }

        private static string NormalizeFolder(string? folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                return string.Empty;
            }

            var sanitizedSegments = folder
                .Replace('\\', '/')
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Where(segment => segment is not "." and not "..")
                .Select(segment => new string(segment.Where(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == '_').ToArray()))
                .Where(segment => !string.IsNullOrWhiteSpace(segment));

            return string.Join('/', sanitizedSegments);
        }

        private string BuildPublicUrl(string objectKey)
        {
            var publicBaseUrl = _configuration.R2Config.PublicBaseUrl.TrimEnd('/');
            var normalizedObjectKey = objectKey.TrimStart('/');

            return string.IsNullOrWhiteSpace(publicBaseUrl)
                ? normalizedObjectKey
                : $"{publicBaseUrl}/{normalizedObjectKey}";
        }

        private static string NormalizeFileKey(string fileName)
        {
            return Uri.TryCreate(fileName, UriKind.Absolute, out var uri) && uri.IsAbsoluteUri
                ? uri.AbsolutePath.TrimStart('/')
                : fileName.Trim().TrimStart('/');
        }
    }
}
