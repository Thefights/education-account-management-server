using Interfaces.Base;
using Results;

namespace Services.Base
{
    public class BaseGetService<TModel, TGetDTO>(
        IUnitOfWork unitOfWork,
        IReadMapper<TModel, TGetDTO> mapper,
        string[]? includes = null)
        : IBaseGetService<TGetDTO>
        where TModel : BaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected readonly string[]? _includes = includes;
        protected readonly IReadMapper<TModel, TGetDTO> _readMapper = mapper;
        protected readonly IGenericRepository<TModel> _repository = unitOfWork.Repository<TModel>();

        public virtual async Task<PaginationResult<TGetDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);

            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);
            var (total, items) = await _repository.GetProjectedPaginatedAsync(
                _readMapper.ProjectToGetDTO,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                _includes,
                cancellationToken);

            return new PaginationResult<TGetDTO>(total, pageSize, items);
        }

        public virtual async Task<List<TGetDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _repository.GetProjectedAsync(
                _readMapper.ProjectToGetDTO,
                includes: _includes,
                cancellationToken: cancellationToken);
        }

        public virtual async Task<List<TGetDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            return await _repository.GetProjectedAsync(
                _readMapper.ProjectToGetDTO,
                entity => ids.Contains(entity.Id),
                _includes,
                cancellationToken);
        }

        public virtual async Task<TGetDTO> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _repository.FirstOrDefaultProjectedAsync(
                    _readMapper.ProjectToGetDTO,
                    entity => entity.Id == id,
                    _includes,
                    cancellationToken)
                ?? throw new DataNotFoundException(typeof(TModel), id);
        }
    }
}
