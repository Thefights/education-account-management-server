namespace Interfaces.FasApplications
{
    public interface IFasBackgroundService
    {
        Task<int> SweepExpiredApplicationsAsync(CancellationToken cancellationToken = default);
    }
}
