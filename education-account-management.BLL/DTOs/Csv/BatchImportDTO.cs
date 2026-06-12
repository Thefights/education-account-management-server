namespace DTOs.Csv
{
    public class BatchImportResultDTO
    {
        public int Total { get; set; }

        public int Succeeded { get; set; }

        public int Failed { get; set; }

        public List<BatchImportErrorDTO> Errors { get; set; } = [];
    }

    public class BatchImportErrorDTO
    {
        public int RowNumber { get; set; }

        public string Field { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public static BatchImportErrorDTO Create(int rowNumber, string field, string message)
        {
            return new BatchImportErrorDTO
            {
                RowNumber = rowNumber,
                Field = field,
                Message = string.IsNullOrWhiteSpace(message) ? "Invalid" : message
            };
        }
    }
}
