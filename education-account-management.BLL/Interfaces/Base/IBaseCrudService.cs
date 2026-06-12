namespace Interfaces.Base
{
    public interface IBaseCrudService<TCreateDTO, TGetDTO, TUpdateDTO> : IBaseGetService<TGetDTO>
    {
        Task<TGetDTO> CreateAsync(TCreateDTO createDTO, CancellationToken cancellationToken = default);
        Task<TGetDTO> UpdateAsync(int id, TUpdateDTO updateDTO, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteSelectedIdsAsync(List<int> ids, CancellationToken cancellationToken = default);
    }
}
