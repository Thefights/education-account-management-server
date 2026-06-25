using DTOs.FasSchemes;
using Filters.FasSchemes;

namespace Interfaces.FasSchemes
{
    public interface IAccountHolderFasSchemeService
    {
        Task<FasSchemeAvailableResponseDTO> GetAvailableSchemesAsync(FasSchemeFilterDTO filter, CancellationToken cancellationToken = default);
    }
}
