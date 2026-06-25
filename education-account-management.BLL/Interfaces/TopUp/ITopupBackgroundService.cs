using DTOs.TopUp;

namespace Interfaces.TopUp
{
    public interface ITopupBackgroundService
    {
        Task<List<ExecuteTopupResultDTO>> ExecuteDueTopupsAsync(CancellationToken cancellationToken);
    }
}
