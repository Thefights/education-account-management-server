using DTOs.Email;

namespace Interfaces.Email
{
    public interface IEmailWhitelistSettingService
    {
        Task<GetEmailWhitelistSettingDTO> GetAsync(CancellationToken cancellationToken = default);

        Task<GetEmailWhitelistSettingDTO> UpdateAsync(UpdateEmailWhitelistSettingDTO updateDTO, CancellationToken cancellationToken = default);
    }
}
