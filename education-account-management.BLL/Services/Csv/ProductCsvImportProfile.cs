using DTOs.Csv;
using Interfaces.Csv;

namespace Services.Csv
{
    public class ProductCsvImportProfile : ICsvImportProfile<Product, ImportProductCsvRowDTO>
    {
        public string EntityName => nameof(Product);

        public Product MapToEntity(ImportProductCsvRowDTO row)
        {
            return new Product
            {
                Name = row.Name?.Trim() ?? string.Empty,
                Description = row.Description?.Trim() ?? string.Empty,
                ImageUrl = row.ImageUrl?.Trim(),
            };
        }

        public Task<List<BatchImportErrorDTO>> ValidateRowAsync(
            ImportProductCsvRowDTO row,
            int rowNumber,
            CancellationToken cancellationToken = default)
        {
            var errors = new List<BatchImportErrorDTO>();

            if (string.IsNullOrWhiteSpace(row.ImageUrl))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.ImageUrl), "ImageUrl is required."));
            }
            else
            {
                CsvImportHelper.ValidateImageUrl(errors, row.ImageUrl, rowNumber, nameof(ImportProductCsvRowDTO.ImageUrl));
            }

            return Task.FromResult(errors);
        }
    }
}
