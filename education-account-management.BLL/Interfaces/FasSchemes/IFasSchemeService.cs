using DTOs.FasSchemes;
using Interfaces.Base;

namespace Interfaces.FasSchemes
{
    public interface IFasSchemeService : IBaseCrudService<CreateFasSchemeDTO, GetFasSchemeDTO, UpdateFasSchemeDTO>
    {
        Task UpdateStatusesAsync(BatchUpdateFasSchemeStatusDTO dto, CancellationToken cancellationToken = default);
        Task<GetFasSchemeDTO> DuplicateAsync(int id, CancellationToken cancellationToken = default);
    }
}
