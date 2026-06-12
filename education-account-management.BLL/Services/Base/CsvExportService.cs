using Interfaces.Csv;
using System.Globalization;
using System.Text;

namespace Services.Base
{
    public class CsvExportService : ICsvExportService
    {
        public byte[] Export<T>(
            IEnumerable<T> records,
            IEnumerable<string> fieldKeys,
            IReadOnlyDictionary<string, CsvExportColumn<T>> columns)
        {
            var selectedKeys = fieldKeys
                .Where(field => !string.IsNullOrWhiteSpace(field))
                .Select(field => field.Trim())
                .ToList();
            if (selectedKeys.Count == 0)
            {
                throw new InvalidDataException("At least one export field is required.");
            }

            var selectedColumns = selectedKeys
                .Select(field => columns.TryGetValue(field, out var column)
                    ? column
                    : throw new InvalidDataException($"Export field '{field}' is not supported."))
                .ToList();

            var builder = new StringBuilder();
            builder.AppendLine(string.Join(",", selectedColumns.Select(column => Escape(column.Header))));

            foreach (var record in records)
            {
                var values = selectedColumns.Select(column => Escape(FormatValue(column.ValueSelector(record))));
                builder.AppendLine(string.Join(",", values));
            }

            return new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(builder.ToString());
        }

        private static string FormatValue(object? value)
        {
            return value switch
            {
                null => string.Empty,
                DateTime dateTime => dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
                _ => value.ToString() ?? string.Empty
            };
        }

        private static string Escape(string value)
        {
            var escaped = value.Replace("\"", "\"\"", StringComparison.Ordinal);
            return escaped.Contains(',')
                || escaped.Contains('"')
                || escaped.Contains('\r')
                || escaped.Contains('\n')
                ? $"\"{escaped}\""
                : escaped;
        }
    }
}
