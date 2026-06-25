using DTOs.FasApplications;

namespace Interfaces.FasApplications
{
    public interface IAccountHolderFasApplicationService
    {
        Task<string> SubmitApplicationAsync(SubmitFasApplicationDTO dto, CancellationToken cancellationToken = default);
    }
}
