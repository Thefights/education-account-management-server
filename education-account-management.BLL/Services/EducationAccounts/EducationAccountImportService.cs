using DTOs.Csv;
using DTOs.EducationAccounts;
using Interfaces.Audit;
using Interfaces.EducationAccounts;
using Repositories.Interfaces;
using Services.Base;
using Services.EducationAccounts.Utils;
using System.Security.Cryptography;
using Validators;

namespace Services.EducationAccounts
{
    public sealed class EducationAccountImportService(
        IUnitOfWork unitOfWork,
        IAuditLogWriter auditLogWriter)
        : CsvImportService<EducationAccount, CreateEducationAccountDTO>(unitOfWork),
          IEducationAccountImportService
    {
        private readonly IGenericRepository<Citizen> _citizenRepository = unitOfWork.Repository<Citizen>();
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGenericRepository<EducationAccount> _repository = unitOfWork.Repository<EducationAccount>();

        public override async Task<BatchImportResultDTO> ImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            var fileErrors = ValidateFile(file);
            if (fileErrors.Count != 0)
                return CsvImportHelper.BuildFailureResult(0, fileErrors);

            var rows = ReadRows(file);
            if (rows.Errors.Count != 0)
                return CsvImportHelper.BuildFailureResult(rows.Total, rows.Errors);
            if (rows.Items.Count == 0)
                return CsvImportHelper.BuildFailureResult(0,
                    [BatchImportErrorDTO.Create(0, "File", "CSV file must contain at least one data row.")]);

            var rowsWithNumbers = rows.Items
                .Select(item => (item.RowNumber, item.Row))
                .ToList();

            return await BatchImportAsync(rowsWithNumbers, cancellationToken);
        }

        private async Task<BatchImportResultDTO> BatchImportAsync(
            IReadOnlyList<(int RowNumber, CreateEducationAccountDTO Row)> rows,
            CancellationToken cancellationToken = default)
        {
            var nrics = rows.Select(r => r.Row.Nric).Distinct().ToList();

            var citizens = await _citizenRepository.Query()
                .Include(c => c.EducationAccount)
                .Where(c => nrics.Contains(c.Nric))
                .ToDictionaryAsync(c => c.Nric, cancellationToken);

            var errors = new List<BatchImportErrorDTO>();
            var validRows = new List<(int RowNumber, CreateEducationAccountDTO Row, Citizen Citizen)>();
            var seenNrics = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var todaySgt = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(8));

            foreach (var (rowNumber, row) in rows)
            {
                row.Nric = row.Nric.Trim().ToUpperInvariant();

                if (!citizens.TryGetValue(row.Nric, out var citizen))
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Nric), "Citizen not found in registry."));
                    continue;
                }

                var validationError = EducationAccountValidationHelper.ValidateCitizenEligibility(citizen, todaySgt);
                if (validationError != null)
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Nric), validationError));
                    continue;
                }

                if (!seenNrics.Add(row.Nric))
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Nric), "Duplicate NRIC in import file."));
                    continue;
                }

                validRows.Add((rowNumber, row, citizen));
            }

            if (validRows.Count == 0)
            {
                return new BatchImportResultDTO
                {
                    Total = rows.Count,
                    Succeeded = 0,
                    Failed = rows.Count,
                    Errors = errors
                };
            }

            var accounts = validRows.Select(entry =>
            {
                var account = new EducationAccount
                {
                    AccountNumber = EducationAccountHelper.GenerateNextAccountNumber(),
                    CitizenId = entry.Citizen.Id
                };
                account.TryValidate();
                return (entry.RowNumber, entry.Row.Nric, Account: account);
            }).ToList();

            var rowErrors = new List<BatchImportErrorDTO>();
            var validAccounts = new List<EducationAccount>();
            foreach (var (rowNumber, nric, account) in accounts)
            {
                try
                {
                    account.TryValidate();
                    validAccounts.Add(account);
                }
                catch (ValidationFailureException ex)
                {
                    rowErrors.AddRange(ex.FieldErrors.Select(e =>
                        BatchImportErrorDTO.Create(rowNumber, e.Key, e.Value)));
                    rowErrors.AddRange(ex.GlobalErrors.Select(e =>
                        BatchImportErrorDTO.Create(rowNumber, string.Empty, e)));
                }
            }

            errors.AddRange(rowErrors);

            if (validAccounts.Count > 0)
            {
                await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
                {
                    await _repository.AddRangeAsync(validAccounts, token);

                    foreach (var entry in validRows.Where(r => validAccounts.Any(a => a.CitizenId == r.Citizen.Id)))
                    {
                        await _auditLogWriter.LogAsync(
                            AuditLogCategory.AccountCreation,
                            "CreateEducationAccount",
                            entry.Citizen.Nric,
                            token);
                    }
                }, cancellationToken);
            }

            return new BatchImportResultDTO
            {
                Total = rows.Count,
                Succeeded = validAccounts.Count,
                Failed = rows.Count - validAccounts.Count,
                Errors = errors
            };
        }
    }
}
