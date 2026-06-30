namespace Infrastructure.Interface
{
    public interface IAiDocumentManagementService
    {
        Task<AiServiceResult> GetHealthAsync();
        Task<AiServiceResult> GetStatsAsync();
        Task<AiServiceResult> GetDocumentsAsync();
        Task<AiServiceResult> DeleteDocumentAsync(string documentId);
        Task<AiServiceResult> UploadDocumentAsync(IFormFile file, bool adminOnly);
    }
}
