using Common;
using DTOs.Csv;
using Interfaces.Csv;
using Repositories.Interfaces;
using Utils;

namespace Services.Base
{
    public class CsvImportService<TEntity, TRow>
        where TEntity : BaseEntity
        where TRow : class
    {
        private readonly ICsvImportProfile<TEntity, TRow>? _profile;

        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IGenericRepository<TEntity> Repository;

        public CsvImportService(
            IUnitOfWork unitOfWork,
            ICsvImportProfile<TEntity, TRow> profile)
            : this(unitOfWork)
        {
            _profile = profile;
        }

        protected CsvImportService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Repository = unitOfWork.Repository<TEntity>();
        }

        public virtual async Task<BatchImportResultDTO> ImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            if (_profile == null)
            {
                throw new InvalidOperationException($"A CSV import profile is required for {typeof(TEntity).Name} imports.");
            }

            var fileErrors = ValidateFile(file);
            if (fileErrors.Count != 0)
            {
                return CsvImportHelper.BuildFailureResult(0, fileErrors);
            }

            var rows = ReadRows(file);
            if (rows.Errors.Count != 0)
            {
                return CsvImportHelper.BuildFailureResult(rows.Total, rows.Errors);
            }

            if (rows.Items.Count == 0)
            {
                return CsvImportHelper.BuildFailureResult(0, [BatchImportErrorDTO.Create(0, "File", "CSV file must contain at least one data row.")]);
            }

            var errors = new List<BatchImportErrorDTO>();
            var entities = new List<TEntity>();

            foreach (var item in rows.Items)
            {
                errors.AddRange(await _profile.ValidateRowAsync(item.Row, item.RowNumber, cancellationToken));

                try
                {
                    var entity = _profile.MapToEntity(item.Row);
                    CsvImportHelper.AddEntityValidationErrors(errors, entity, item.RowNumber);
                    entities.Add(entity);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    errors.Add(BatchImportErrorDTO.Create(item.RowNumber, string.Empty, ex.Message));
                }
            }

            if (errors.Count != 0)
            {
                return CsvImportHelper.BuildFailureResult(rows.Total, errors);
            }

            await Repository.AddRangeAsync(entities, cancellationToken);
            await UnitOfWork.SaveChangeAsync(cancellationToken);

            return new BatchImportResultDTO
            {
                Total = rows.Total,
                Succeeded = rows.Total,
                Failed = 0,
                Errors = []
            };
        }

        protected static List<BatchImportErrorDTO> ValidateFile(IFormFile file)
        {
            return CsvImportHelper.ValidateFile(file);
        }

        protected static CsvReadResult<TRow> ReadRows(IFormFile file)
        {
            return CsvImportHelper.ReadRows<TRow>(file);
        }
    }
}
