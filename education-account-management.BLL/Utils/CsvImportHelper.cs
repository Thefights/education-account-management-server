using Common;
using CsvHelper;
using CsvHelper.Configuration;
using DTOs.Csv;
using Exceptions;
using System.Globalization;

namespace Utils
{
    public static class CsvImportHelper
    {
        public static List<BatchImportErrorDTO> ValidateFile(IFormFile file)
        {
            var errors = new List<BatchImportErrorDTO>();

            if (file == null)
            {
                errors.Add(BatchImportErrorDTO.Create(0, "File", "CSV file is required."));
                return errors;
            }

            if (file.Length == 0)
            {
                errors.Add(BatchImportErrorDTO.Create(0, "File", "CSV file cannot be empty."));
            }

            var extension = Path.GetExtension(file.FileName);
            if (!string.Equals(extension, ".csv", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add(BatchImportErrorDTO.Create(0, "File", "Only .csv files are supported."));
            }

            return errors;
        }

        public static CsvReadResult<TRow> ReadRows<TRow>(IFormFile file)
            where TRow : class
        {
            var result = new CsvReadResult<TRow>();

            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CreateConfiguration());

                if (!csv.Read())
                {
                    result.Errors.Add(BatchImportErrorDTO.Create(0, "File", "CSV file must contain a header row."));
                    return result;
                }

                csv.ReadHeader();

                while (csv.Read())
                {
                    result.Total++;
                    var rowNumber = csv.Context.Parser?.Row ?? 0;

                    try
                    {
                        var row = csv.GetRecord<TRow>();
                        result.Items.Add(new CsvRowItem<TRow>(rowNumber, row));
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        result.Errors.Add(BatchImportErrorDTO.Create(rowNumber, string.Empty, ex.Message));
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                result.Errors.Add(BatchImportErrorDTO.Create(0, "File", ex.Message));
            }

            return result;
        }

        public static CsvConfiguration CreateConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,
                PrepareHeaderForMatch = args => args.Header.Trim().ToLowerInvariant(),
                HeaderValidated = null,
                MissingFieldFound = null
            };
        }

        public static BatchImportResultDTO BuildFailureResult(
            int total,
            List<BatchImportErrorDTO> errors)
        {
            return new BatchImportResultDTO
            {
                Total = total,
                Succeeded = 0,
                Failed = errors
                    .Where(error => error.RowNumber > 0)
                    .Select(error => error.RowNumber)
                    .Distinct()
                    .Count(),
                Errors = errors
            };
        }

        public static void AddEntityValidationErrors<TModel>(
            List<BatchImportErrorDTO> errors,
            TModel entity,
            int rowNumber)
            where TModel : Entity
        {
            try
            {
                entity.TryValidate();
            }
            catch (ValidationFailureException ex)
            {
                foreach (var fieldError in ex.FieldErrors)
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, fieldError.Key, fieldError.Value));
                }

                foreach (var globalError in ex.GlobalErrors)
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, string.Empty, globalError));
                }
            }
        }

        public static string? RequireText(
            string? value,
            string field,
            int rowNumber,
            List<BatchImportErrorDTO> errors)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, field, $"{field} is required."));
                return null;
            }

            return value.Trim();
        }

        public static List<int> ParseRequiredIds(
            string? value,
            string field,
            int rowNumber,
            List<BatchImportErrorDTO> errors)
        {
            var ids = new List<int>();
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, field, $"{field} is required."));
                return ids;
            }

            foreach (var item in value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!int.TryParse(item, out var id) || id <= 0)
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, field, $"{field} must contain positive integer IDs separated by ';'."));
                    continue;
                }

                ids.Add(id);
            }

            if (ids.Count == 0)
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, field, $"{field} must contain at least one ID."));
            }

            return ids.Distinct().ToList();
        }

        public static TEnum? ParseRequiredEnum<TEnum>(
            string? value,
            string field,
            int rowNumber,
            List<BatchImportErrorDTO> errors)
            where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, field, $"{field} is required."));
                return null;
            }

            if (Enum.TryParse<TEnum>(value.Trim(), ignoreCase: true, out var parsed)
                && Enum.IsDefined(parsed))
            {
                return parsed;
            }

            errors.Add(BatchImportErrorDTO.Create(rowNumber, field, $"{field} is invalid."));
            return null;
        }

        public static void AddDuplicateErrors<TItem>(
            IEnumerable<TItem> items,
            Func<TItem, string> keySelector,
            Func<TItem, int> rowNumberSelector,
            string field,
            string message,
            List<BatchImportErrorDTO> errors,
            StringComparer? comparer = null)
        {
            var duplicateItems = items
                .GroupBy(keySelector, comparer ?? StringComparer.Ordinal)
                .Where(group => group.Count() > 1)
                .SelectMany(group => group);

            foreach (var item in duplicateItems)
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumberSelector(item), field, message));
            }
        }

        public static void ValidateImageUrl(
            List<BatchImportErrorDTO> errors,
            string imageUrl,
            int rowNumber,
            string field)
        {
            if (!Uri.TryCreate(imageUrl.Trim(), UriKind.Absolute, out var uri))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, field, $"{field} must be an absolute URL."));
                return;
            }

            var extension = Path.GetExtension(uri.LocalPath);
            if (!FileExtensionMap.ImageExtensions.Contains(extension))
            {
                var allowedExtensions = string.Join(", ", FileExtensionMap.ImageExtensions.Order());
                errors.Add(BatchImportErrorDTO.Create(
                    rowNumber,
                    field,
                    $"{field} must point to a supported image file. Allowed: {allowedExtensions}."));
            }
        }
    }

    public class CsvReadResult<TRow>
        where TRow : class
    {
        public int Total { get; set; }

        public List<CsvRowItem<TRow>> Items { get; } = [];

        public List<BatchImportErrorDTO> Errors { get; } = [];
    }

    public sealed record CsvRowItem<TRow>(int RowNumber, TRow Row)
        where TRow : class;
}
