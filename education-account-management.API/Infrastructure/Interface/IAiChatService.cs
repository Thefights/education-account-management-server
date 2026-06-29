using DTOs.AiChat;
using DTOs.Base;

namespace Infrastructure.Interface
{
    public interface IAiChatService
    {
        Task<bool> GetAiStatusAsync();
        Task<AiServiceResult> SendChatMessageAsync(AiChatRequestDTO request);
    }
}
