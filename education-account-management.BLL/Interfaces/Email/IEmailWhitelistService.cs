using DTOs.Email;

namespace Interfaces.Email
{
    public interface IEmailWhitelistService
    {
        Task<List<GetEmailWhitelistDTO>> GetAllAsync();
        Task<List<GetEmailWhitelistDTO>> SaveAsync(SaveEmailWhitelistDTO saveDTO, CancellationToken cancellationToken = default);
    }
}
