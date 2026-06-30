using DTOs.Courses;
using DTOs.Csv;
using Interfaces.Csv;
using Services.Base;
using Services.Courses.Utils;

namespace Services.Courses
{
    public class CourseImportService(
        IUnitOfWork unitOfWork,
        ICsvImportProfile<Course, CreateCourseDTO> profile,
        SchoolScopeResolver schoolScopeResolver,
        TimeProvider timeProvider)
        : CsvImportService<Course, CreateCourseDTO>(unitOfWork, profile)
    {
        private readonly ICsvImportProfile<Course, CreateCourseDTO> _profile = profile;
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly TimeProvider _timeProvider = timeProvider;
        private readonly IGenericRepository<ApplicationSetting> _settingRepository =
            unitOfWork.Repository<ApplicationSetting>();

        public override async Task<BatchImportResultDTO> ImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            var fileErrors = ValidateFile(file);
            if (fileErrors.Count != 0)
            {
                CsvImportHelper.ThrowIfImportFailed(0, fileErrors);
            }

            var rows = ReadRows(file);
            if (rows.Errors.Count != 0)
            {
                CsvImportHelper.ThrowIfImportFailed(rows.Total, rows.Errors);
            }

            if (rows.Items.Count == 0)
            {
                CsvImportHelper.ThrowIfImportFailed(
                    0,
                    [BatchImportErrorDTO.Create(0, "File", "CSV file must contain at least one data row.")]);
            }

            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
            var taxRate = await GetTaxRateAsync(cancellationToken);
            var reservedCodes = new HashSet<string>(StringComparer.Ordinal);
            var errors = new List<BatchImportErrorDTO>();
            var entities = new List<Course>();
            var courseNames = new List<(int RowNumber, string CourseName)>();

            foreach (var item in rows.Items)
            {
                errors.AddRange(await _profile.ValidateRowAsync(
                    item.Row,
                    item.RowNumber,
                    cancellationToken));

                try
                {
                    var course = _profile.MapToEntity(item.Row);
                    course.SchoolId = schoolId;
                    course.Status = CourseStatus.Draft;
                    course.GstAmount = CourseFeeCalculator.CalculateTaxAmount(
                        course.CourseFeeAmount,
                        course.MiscFeeAmount,
                        taxRate);
                    CourseDateTimeHelper.NormalizeToUtc(course);
                    course.CourseCode = await CourseCodeGenerator.GenerateUniqueAsync(
                        Repository,
                        schoolId,
                        utcNow,
                        reservedCodes,
                        cancellationToken);
                    CsvImportHelper.AddEntityValidationErrors(errors, course, item.RowNumber);
                    entities.Add(course);
                    if (!string.IsNullOrWhiteSpace(course.CourseName))
                    {
                        courseNames.Add((item.RowNumber, course.CourseName.Trim()));
                    }
                }
                catch (ValidationFailureException ex)
                {
                    AddValidationErrors(errors, ex, item.RowNumber);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    errors.Add(BatchImportErrorDTO.Create(item.RowNumber, string.Empty, ex.Message));
                }
            }

            await AddCourseNameValidationErrorsAsync(errors, courseNames, cancellationToken);

            if (errors.Count != 0)
            {
                CsvImportHelper.ThrowIfImportFailed(rows.Total, errors);
            }

            await UnitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    await Repository.AddRangeAsync(entities, token);
                    await UnitOfWork.SaveChangeAsync(token);
                },
                cancellationToken);

            return new BatchImportResultDTO
            {
                Total = rows.Total,
                Succeeded = rows.Total,
                Failed = 0,
                Errors = []
            };
        }

        private static void AddValidationErrors(
            List<BatchImportErrorDTO> errors,
            ValidationFailureException exception,
            int rowNumber)
        {
            errors.AddRange(exception.FieldErrors.Select(error =>
                BatchImportErrorDTO.Create(rowNumber, error.Key, error.Value)));
            errors.AddRange(exception.GlobalErrors.Select(error =>
                BatchImportErrorDTO.Create(rowNumber, string.Empty, error)));
        }

        private async Task AddCourseNameValidationErrorsAsync(
            List<BatchImportErrorDTO> errors,
            List<(int RowNumber, string CourseName)> courseNames,
            CancellationToken cancellationToken)
        {
            if (courseNames.Count == 0) return;

            var duplicateImportNames = courseNames
                .GroupBy(item => item.CourseName, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var item in courseNames.Where(item => duplicateImportNames.Contains(item.CourseName)))
            {
                errors.Add(BatchImportErrorDTO.Create(
                    item.RowNumber,
                    nameof(Course.CourseName),
                    "Course name is duplicated in the import file."));
            }

            var distinctNames = courseNames
                .Select(item => item.CourseName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingNames = await Repository.Query()
                .Where(course => distinctNames.Contains(course.CourseName))
                .Select(course => course.CourseName)
                .ToListAsync(cancellationToken);
            var existingNameSet = existingNames.ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var item in courseNames.Where(item => existingNameSet.Contains(item.CourseName)))
            {
                errors.Add(BatchImportErrorDTO.Create(
                    item.RowNumber,
                    nameof(Course.CourseName),
                    "Course name already exists."));
            }
        }

        private async Task<decimal> GetTaxRateAsync(CancellationToken cancellationToken)
        {
            return await _settingRepository.Query()
                .OrderBy(setting => setting.Id)
                .Select(setting => setting.TaxRate)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
