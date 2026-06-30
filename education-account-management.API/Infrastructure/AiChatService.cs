using DTOs.AiChat;
using Infrastructure.Interface;
using Interfaces.ApplicationSettings;
using System.Text;
using System.Text.Json;

namespace Infrastructure
{
    public class AiChatService : IAiChatService
    {
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrentUserService _currentUserService;

        public AiChatService(
            IApplicationSettingService applicationSettingService,
            IHttpClientFactory httpClientFactory,
            ICurrentUserService currentUserService)
        {
            _applicationSettingService = applicationSettingService;
            _httpClientFactory = httpClientFactory;
            _currentUserService = currentUserService;
        }

        public async Task<bool> GetAiStatusAsync()
        {
            try
            {
                var setting = await _applicationSettingService.GetAsync();
                return setting?.IsAiFeatureEnabled ?? false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<AiServiceResult> SendChatMessageAsync(AiChatRequestDTO request)
        {
            var setting = await _applicationSettingService.GetAsync();
            if (setting == null || !setting.IsAiFeatureEnabled)
            {
                return AiServiceResult.Failure("The AI Assistant feature is currently disabled by Admin.", StatusCodes.Status403Forbidden);
            }

            var forwardRequest = new AiChatForwardRequestDTO
            {
                Message = request.Message,
                History = request.History,
                UserId = _currentUserService.UserId.ToString(),
                Role = _currentUserService.Role.ToString().ToLower()
            };

            var client = _httpClientFactory.CreateClient("AiClient");
            var jsonContent = JsonSerializer.Serialize(forwardRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/ai/faq/chat", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return AiServiceResult.Success(responseContent, (int)response.StatusCode);
            }

            return AiServiceResult.Failure("Failed to connect to the AI server.", (int)response.StatusCode);
        }
    }
}
