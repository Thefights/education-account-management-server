using Common.HttpResults;
using Controllers.Base;
using Enums;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Management
{
    [Authorize(Roles = nameof(UserRole.SystemAdmin))]
    public class AiDocumentManagementController(IAiDocumentManagementService aiDocumentManagementService) 
        : BaseController
    {
        private readonly IAiDocumentManagementService _aiDocumentManagementService = aiDocumentManagementService;

        [HttpGet("health")]
        public async Task<IActionResult> GetHealth()
        {
            var result = await _aiDocumentManagementService.GetHealthAsync();
            if (result.IsSuccess) return Content(result.Content!, "application/json");
            return Result.FailError<object?>(null, result.ErrorMessage ?? "Error", result.StatusCode);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _aiDocumentManagementService.GetStatsAsync();
            if (result.IsSuccess) return Content(result.Content!, "application/json");
            return Result.FailError<object?>(null, result.ErrorMessage ?? "Error", result.StatusCode);
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var result = await _aiDocumentManagementService.GetDocumentsAsync();
            if (result.IsSuccess)
            {
                var data = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(result.Content!);
                return Result.SuccessData(data);
            }
            return Result.FailError<object?>(null, result.ErrorMessage ?? "Error", result.StatusCode);
        }

        [HttpDelete("{documentId}")]
        public async Task<IActionResult> DeleteDocument(string documentId)
        {
            var result = await _aiDocumentManagementService.DeleteDocumentAsync(documentId);
            if (result.IsSuccess)
            {
                var data = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(result.Content!);
                return Result.SuccessData(data);
            }
            return Result.FailError<object?>(null, result.ErrorMessage ?? "Error", result.StatusCode);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(IFormFile file, [FromForm] bool admin_only = false)
        {
            var result = await _aiDocumentManagementService.UploadDocumentAsync(file, admin_only);
            if (result.IsSuccess)
            {
                var data = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(result.Content!);
                return Result.SuccessData(data);
            }
            return Result.FailError<object?>(null, result.ErrorMessage ?? "Error", result.StatusCode);
        }

        [HttpGet("{docId}/download")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadDocument(string docId, [FromQuery] string fileName)
        {
            var result = await _aiDocumentManagementService.DownloadDocumentAsync(docId, fileName);
            if (result.HasValue)
            {
                var (stream, contentType, name) = result.Value;
                return File(stream, contentType, name);
            }
            return Result.FailError<object?>(null, "Document not found or file missing on server.", StatusCodes.Status404NotFound);
        }
    }
}
