using DTOs.Csv;

namespace Exceptions
{
    public class CsvImportFailedException(BatchImportResultDTO result)
        : UserFacingException("CSV import failed. Please check the failed records.", 400)
    {
        public BatchImportResultDTO Result { get; } = result;
    }
}
