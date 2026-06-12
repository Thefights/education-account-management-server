using Results;

namespace Interfaces.Base
{
    public interface IBaseGetService<TGetDTO>
    {
        Task<PaginationResult<TGetDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default);

        Task<List<TGetDTO>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<List<TGetDTO>> GetAllByIdsAsync(List<int> ids, CancellationToken cancellationToken = default);

        Task<TGetDTO> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
