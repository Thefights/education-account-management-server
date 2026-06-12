using Filters.Base;
using Interfaces.Base;

namespace Controllers.Base
{
    public abstract class GetController<TGetDTO>(IBaseGetService<TGetDTO> _service)
        : GetController<TGetDTO, FilterDTO>(_service)
        where TGetDTO : class;

    public abstract class GetController<TGetDTO, TFilter>(
        IBaseGetService<TGetDTO> _service)
        : BaseController
        where TGetDTO : class
        where TFilter : FilterDTO, new()
    {
        [HttpGet]
        public virtual async Task<IActionResult> GetAllPaginated(
            TFilter filterDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetAllPaginatedAsync(filterDTO, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("all")]
        public virtual async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _service.GetAllAsync(cancellationToken);
            return Result.SuccessData(result);
        }
    }
}
