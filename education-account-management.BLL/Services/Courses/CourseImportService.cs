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
        private readonly IGenericRepository<AiAssistantSetting> _settingRepository =
            unitOfWork.Repository<AiAssistantSetting>();

        public override async Task<BatchImportResultDTO> ImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            var fileErrors = ValidateFile(file);
            if (fileErrors.Count != 0)
            {
                CsvImportHelper.ThrowIfImportFailed(fileErrors);
            }

            var rows = ReadRows(file);
            if (rows.Errors.Count != 0)
            {
                CsvImportHelper.ThrowIfImportFailed(rows.Errors);
            }

            if (rows.Items.Count == 0)
            {
                CsvImportHelper.ThrowIfImportFailed(
                    [BatchImportErrorDTO.Create(0, "File", "CSV file must contain at least one data row.")]);
            }

            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
            var taxRate = await GetTaxRateAsync(cancellationToken);
            var reservedCodes = new HashSet<string>(StringComparer.Ordinal);
            var errors = new List<BatchImportErrorDTO>();
            var entities = new List<Course>();

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
                    course.FasApplicationDueDate = course.EnrollmentDeadline;
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

            if (errors.Count != 0)
            {
                CsvImportHelper.ThrowIfImportFailed(errors);
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

        private async Task<decimal> GetTaxRateAsync(CancellationToken cancellationToken)
        {
            return await _settingRepository.Query()
                .OrderBy(setting => setting.Id)
                .Select(setting => setting.TaxRate)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
