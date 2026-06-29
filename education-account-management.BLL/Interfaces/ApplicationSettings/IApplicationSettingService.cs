using DTOs.ApplicationSettings;

namespace Interfaces.ApplicationSettings
{
    public interface IApplicationSettingService
    {
        Task<GetApplicationSettingDTO> GetAsync(CancellationToken cancellationToken = default);

        Task<GetApplicationSettingDTO> UpdateAsync(
            UpdateApplicationSettingDTO updateDTO,
            CancellationToken cancellationToken = default);
    }
}