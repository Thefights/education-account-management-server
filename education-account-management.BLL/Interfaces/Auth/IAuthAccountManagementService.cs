using DTOs.Auth;
using DTOs.Csv;
using Interfaces.Base;

namespace Interfaces.Auth
{
    public interface IAuthAccountManagementService : IBaseCrudService<CreateAuthAccountDTO, GetAuthAccountDTO, UpdateAuthAccountDTO>
    {
        Task<BatchImportResultDTO> BatchImportAsync(
            IFormFile file,
            bool sendEmail = true,
            CancellationToken cancellationToken = default);

        Task<List<GetAuthAccountDTO>> UpdateAuthAccountStatusAsync(
            UpdateAuthAccountsStatusDTO updateDTO,
            CancellationToken cancellationToken = default);

        Task<List<GetAuthAccountDTO>> UnlockAuthAccountsAsync(
            UnlockAuthAccountsDTO unlockDTO,
            CancellationToken cancellationToken = default);
    }
}
