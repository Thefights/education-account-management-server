using DTOs.Base;
using Interfaces.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.HttpResults;
using Controllers.Base;

namespace Controllers.Storage
{
    [Authorize] // Require authentication to upload
    public class StorageManagementController(IUploadService uploadService) : BaseController
    {
        private readonly IUploadService _uploadService = uploadService;

        public record UploadDto(IFormFile File);

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadDto request, [FromQuery] string? folder = null, CancellationToken cancellationToken = default)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
            {
                return Result.FailError<object?>(null, "File is missing or empty.", 400);
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".pdf", ".docx" };
            if (!allowedExtensions.Contains(fileExtension))
            {
                return Result.FailError<object?>(null, "Chỉ cho phép tải lên file định dạng PDF hoặc DOCX.", 400);
            }

            var result = await _uploadService.UploadAsync(file, folder, cancellationToken);
            return Result.SuccessData(result);
        }
    }
}
