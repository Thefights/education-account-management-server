using Infrastructure.Interface;
using System.Net.Http.Headers;
using System.Text.Json;
using Interfaces.Storage;
using education_account_management.BLL;

namespace Infrastructure
{
    public class AiDocumentManagementService : IAiDocumentManagementService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStorageService _storageService;
        private readonly AppConfiguration _appConfig;

        public AiDocumentManagementService(
            IHttpClientFactory httpClientFactory,
            IStorageService storageService,
            AppConfiguration appConfig)
        {
            _httpClientFactory = httpClientFactory;
            _storageService = storageService;
            _appConfig = appConfig;
        }

        public async Task<AiServiceResult> GetHealthAsync()
        {
            var client = _httpClientFactory.CreateClient("AiClient");
            var response = await client.GetAsync("/health");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return AiServiceResult.Success(responseContent, (int)response.StatusCode);
            }
            return AiServiceResult.Failure("Failed to connect to the AI server.", (int)response.StatusCode);
        }

        public async Task<AiServiceResult> GetStatsAsync()
        {
            var client = _httpClientFactory.CreateClient("AiClient");
            var response = await client.GetAsync("/ai/documents/stats");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return AiServiceResult.Success(responseContent, (int)response.StatusCode);
            }
            return AiServiceResult.Failure("Failed to get statistics.", (int)response.StatusCode);
        }

        public async Task<AiServiceResult> GetDocumentsAsync()
        {
            var client = _httpClientFactory.CreateClient("AiClient");
            var response = await client.GetAsync("/ai/documents");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return AiServiceResult.Success(responseContent, (int)response.StatusCode);
            }
            return AiServiceResult.Failure("Failed to get the document list.", (int)response.StatusCode);
        }

        public async Task<AiServiceResult> DeleteDocumentAsync(string documentId)
        {
            var client = _httpClientFactory.CreateClient("AiClient");
            
            // Try to find the document first to get its doc_id for cloud deletion
            string? docIdForDeletion = null;
            var listResponse = await client.GetAsync("/ai/documents");
            if (listResponse.IsSuccessStatusCode)
            {
                var listContent = await listResponse.Content.ReadAsStringAsync();
                try
                {
                    var docs = JsonSerializer.Deserialize<List<JsonElement>>(listContent);
                    var targetDoc = docs?.FirstOrDefault(d => d.GetProperty("id").ToString() == documentId);
                    if (targetDoc != null)
                    {
                        docIdForDeletion = targetDoc.Value.GetProperty("doc_id").GetString();
                    }
                }
                catch { }
            }

            var response = await client.DeleteAsync($"/ai/documents/{documentId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                
                // Delete file from cloud storage
                if (!string.IsNullOrEmpty(docIdForDeletion))
                {
                    try
                    {
                        await _storageService.DeleteAsync($"AiDocuments/{docIdForDeletion}.pdf");
                    }
                    catch { /* Ignore cloud deletion errors to not fail the API */ }
                }

                return AiServiceResult.Success(responseContent, (int)response.StatusCode);
            }
            return AiServiceResult.Failure("Failed to delete the document.", (int)response.StatusCode);
        }

        public async Task<AiServiceResult> UploadDocumentAsync(IFormFile file, bool adminOnly)
        {
            if (file == null || file.Length == 0)
            {
                return AiServiceResult.Failure("File is empty.", StatusCodes.Status400BadRequest);
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(memoryStream);
            if (MediaTypeHeaderValue.TryParse(file.ContentType, out var parsedContentType))
            {
                fileContent.Headers.ContentType = parsedContentType;
            }
            else
            {
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }

            content.Add(fileContent, "file", file.FileName);
            content.Add(new StringContent(adminOnly.ToString()), "admin_only");

            var client = _httpClientFactory.CreateClient("AiClient");
            var response = await client.PostAsync("/ai/documents/upload", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                
                try
                {
                    var resultJson = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    if (resultJson.TryGetProperty("doc_id", out var docIdProp))
                    {
                        var docId = docIdProp.GetString();
                        if (!string.IsNullOrEmpty(docId) && docId != "duplicate")
                        {
                            memoryStream.Position = 0;
                            var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/pdf" : file.ContentType;
                            await _storageService.UploadAsync(memoryStream, $"AiDocuments/{docId}.pdf", contentType);
                        }
                    }
                }
                catch { }

                return AiServiceResult.Success(responseContent, (int)response.StatusCode);
            }
            return AiServiceResult.Failure("Failed to upload the document.", (int)response.StatusCode);
        }

        public async Task<(Stream FileStream, string ContentType, string FileName)?> DownloadDocumentAsync(string docId, string fileName)
        {
            var publicBaseUrl = _appConfig.R2Config.PublicBaseUrl.TrimEnd('/');
            var publicUrl = $"{publicBaseUrl}/AiDocuments/{docId}.pdf";

            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(publicUrl);
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return (stream, "application/pdf", fileName);
                }
            }
            catch
            {
                // Fallback to null on error
            }

            return null;
        }
    }
}
