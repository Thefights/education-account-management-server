using DTOs.TopUp;

namespace Interfaces.TopUp
{
    public interface ITopupService
    {
        Task<ExecuteTopupResultDTO> ExecuteManualTopupAsync(ManualTopupRequestDTO request, CancellationToken cancellationToken);
    }
}
