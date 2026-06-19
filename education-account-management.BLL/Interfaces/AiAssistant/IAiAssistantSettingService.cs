using DTOs.AiAssistant;

namespace Interfaces.AiAssistant
{
    public interface IAiAssistantSettingService
    {
        Task<GetAiAssistantSettingDTO> GetAsync(CancellationToken cancellationToken = default);

        Task<GetAiAssistantSettingDTO> UpdateAsync(
            UpdateAiAssistantSettingDTO updateDTO,
            CancellationToken cancellationToken = default);
    }
}
