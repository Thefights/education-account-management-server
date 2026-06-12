using DTOs.Email;

namespace Interfaces.Email
{
    public interface IMfaSettingService
    {
        Task<GetMfaSettingDTO> GetAsync(CancellationToken cancellationToken = default);

        Task<GetMfaSettingDTO> UpdateAsync(UpdateMfaSettingDTO updateDTO, CancellationToken cancellationToken = default);
    }
}
