using DTOs.Csv;
using DTOs.SchoolStudents;
using Interfaces.SchoolStudents;
using Services.Base;

namespace Services.SchoolStudents
{
    public sealed class SchoolStudentImportService(
        IUnitOfWork unitOfWork,
        SchoolScopeResolver schoolScopeResolver)
        : CsvImportService<SchoolStudent, CreateSchoolStudentDTO>(unitOfWork),
          ISchoolStudentImportService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly IGenericRepository<Citizen> _citizenRepository = unitOfWork.Repository<Citizen>();
        private readonly IGenericRepository<EducationAccount> _educationAccountRepository = unitOfWork.Repository<EducationAccount>();
        private readonly IGenericRepository<SchoolStudent> _repository = unitOfWork.Repository<SchoolStudent>();

        public override async Task<BatchImportResultDTO> ImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var fileErrors = ValidateFile(file);
            if (fileErrors.Count != 0)
                CsvImportHelper.ThrowIfImportFailed(fileErrors);

            var rows = ReadRows(file);
            if (rows.Errors.Count != 0)
                CsvImportHelper.ThrowIfImportFailed(rows.Errors);
            if (rows.Items.Count == 0)
                CsvImportHelper.ThrowIfImportFailed(
                    [BatchImportErrorDTO.Create(0, "File", "CSV file must contain at least one data row.")]);

            var rowsWithNumbers = rows.Items
                .Select(item => (item.RowNumber, item.Row))
                .ToList();

            return await BatchImportAsync(rowsWithNumbers, schoolId, cancellationToken);
        }

        private async Task<BatchImportResultDTO> BatchImportAsync(
            IReadOnlyList<(int RowNumber, CreateSchoolStudentDTO Row)> rows,
            int schoolId,
            CancellationToken cancellationToken = default)
        {
            var nrics = rows.Select(r => r.Row.Nric?.Trim().ToUpperInvariant()).Where(n => !string.IsNullOrEmpty(n)).Distinct().ToList();

            var citizens = await _citizenRepository.Query()
                .Include(c => c.EducationAccount)
                .Where(c => nrics.Contains(c.Nric))
                .ToDictionaryAsync(c => c.Nric, cancellationToken);

            var educationAccountIds = citizens.Values
                .Where(c => c.EducationAccount != null)
                .Select(c => c.EducationAccount!.Id)
                .ToList();

            var existingSchoolStudents = await _repository.Query()
                .Where(ss => educationAccountIds.Contains(ss.EducationAccountId))
                .ToDictionaryAsync(ss => ss.EducationAccountId, cancellationToken);

            var errors = new List<BatchImportErrorDTO>();
            var validSchoolStudents = new List<SchoolStudent>();
            var seenNrics = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var (rowNumber, row) in rows)
            {
                if (string.IsNullOrWhiteSpace(row.Nric))
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Nric), "NRIC is required."));
                    continue;
                }

                var nric = row.Nric.Trim().ToUpperInvariant();

                if (!citizens.TryGetValue(nric, out var citizen))
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Nric), "Student with this NRIC not found."));
                    continue;
                }

                if (citizen.EducationAccount == null)
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Nric), "This student does not have an Education Account."));
                    continue;
                }

                if (!seenNrics.Add(nric))
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Nric), "Duplicate NRIC in import file."));
                    continue;
                }

                if (existingSchoolStudents.TryGetValue(citizen.EducationAccount.Id, out var existingSchoolStudent))
                {
                    if (existingSchoolStudent.SchoolId == schoolId)
                    {
                        errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Nric), "This student is already in your school."));
                    }
                    else
                    {
                        errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Nric), "This student belongs to another school."));
                    }
                    continue;
                }

                var schoolStudent = new SchoolStudent
                {
                    SchoolId = schoolId,
                    EducationAccountId = citizen.EducationAccount.Id,
                };

                schoolStudent.TryValidate();
                validSchoolStudents.Add(schoolStudent);
            }

            if (errors.Count != 0)
            {
                CsvImportHelper.ThrowIfImportFailed(errors);
            }

            if (validSchoolStudents.Count > 0)
            {
                await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
                {
                    await _repository.AddRangeAsync(validSchoolStudents, token);
                    await _unitOfWork.SaveChangeAsync(token);
                }, cancellationToken);
            }

            return new BatchImportResultDTO
            {
                Total = rows.Count,
                Succeeded = validSchoolStudents.Count,
                Failed = rows.Count - validSchoolStudents.Count,
                Errors = errors
            };
        }
    }
}
