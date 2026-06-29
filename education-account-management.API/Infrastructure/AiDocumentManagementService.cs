using Infrastructure.Interface;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using DTOs.Base;

namespace Infrastructure
{
    public class AiDocumentManagementService : IAiDocumentManagementService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AiDocumentManagementService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
            var response = await client.DeleteAsync($"/ai/documents/{documentId}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
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

            using var content = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream();
            
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
            
            content.Add(fileContent, "file", file.FileName);
            content.Add(new StringContent(adminOnly.ToString()), "admin_only");

            var client = _httpClientFactory.CreateClient("AiClient");
            var response = await client.PostAsync("/ai/documents/upload", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return AiServiceResult.Success(responseContent, (int)response.StatusCode);
            }
            return AiServiceResult.Failure("Failed to upload the document.", (int)response.StatusCode);
        }
    }
}
