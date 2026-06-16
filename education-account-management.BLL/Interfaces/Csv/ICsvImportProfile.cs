using Common;
using DTOs.Csv;

namespace Interfaces.Csv
{
    public interface ICsvImportProfile<TEntity, TRow>
        where TEntity : BaseEntity
        where TRow : class
    {
        string EntityName { get; }

        TEntity MapToEntity(TRow row);

        Task<List<BatchImportErrorDTO>> ValidateRowAsync(
            TRow row,
            int rowNumber,
            CancellationToken cancellationToken = default);
    }
}
