using DTOs.AiChat;

namespace Infrastructure.Interface
{
    public interface IAiChatService
    {
        Task<bool> GetAiStatusAsync();
        Task<AiServiceResult> SendChatMessageAsync(AiChatRequestDTO request);
        Task<AiServiceResult> SendDynamicFasChatAsync(DynamicFasChatRequestDTO request);
        Task<AiServiceResult> ResetDynamicFasSessionAsync(DynamicFasResetSessionRequestDTO request);
    }
}
