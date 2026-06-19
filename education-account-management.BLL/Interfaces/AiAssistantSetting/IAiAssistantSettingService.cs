using DTOs.AiAssistantSetting;

namespace Interfaces.AiAssistantSetting
{
    public interface IAiAssistantSettingService
    {
        Task<GetAiAssistantSettingDTO> GetAsync(CancellationToken cancellationToken = default);

        Task<GetAiAssistantSettingDTO> UpdateAsync(
            UpdateAiAssistantSettingDTO updateDTO,
            CancellationToken cancellationToken = default);
    }
}