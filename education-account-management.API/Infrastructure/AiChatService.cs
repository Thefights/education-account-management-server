using Infrastructure.Interface;
using DTOs.AiChat;
using Interfaces.AiAssistantSettings;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using DTOs.Base;

namespace Infrastructure
{
    public class AiChatService : IAiChatService
    {
        private readonly IAiAssistantSettingService _aiSettingService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrentUserService _currentUserService;

        public AiChatService(
            IAiAssistantSettingService aiSettingService,
            IHttpClientFactory httpClientFactory,
            ICurrentUserService currentUserService)
        {
            _aiSettingService = aiSettingService;
            _httpClientFactory = httpClientFactory;
            _currentUserService = currentUserService;
        }

        public async Task<bool> GetAiStatusAsync()
        {
            try 
            {
                var setting = await _aiSettingService.GetAsync();
                return setting?.IsEnabled ?? false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<AiServiceResult> SendChatMessageAsync(AiChatRequestDTO request)
        {
            var setting = await _aiSettingService.GetAsync();
            if (setting == null || !setting.IsEnabled)
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
