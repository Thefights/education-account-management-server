namespace Interfaces.Csv
{
    public interface ICsvExportService
    {
        byte[] Export<T>(
            IEnumerable<T> records,
            IEnumerable<string> fieldKeys,
            IReadOnlyDictionary<string, CsvExportColumn<T>> columns);
    }

    public sealed record CsvExportColumn<T>(
        string Header,
        Func<T, object?> ValueSelector);
}
