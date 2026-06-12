using DTOs.Csv;
using DTOs.Product;
using Interfaces.Audit;
using Interfaces.Product;
using Interfaces.Storage;
using Services.Base;

namespace Services.ProductService
{
    public class ProductManagementService(
        IUnitOfWork unitOfWork,
        ProductMapper productMapper,
        IUploadService uploadService,
        IAuditLogWriter auditLogWriter,
        CsvImportService<Product, ImportProductCsvRowDTO> csvImportService)
        : BaseService<Product, CreateProductDTO, GetProductDTO, UpdateProductDTO>(
            unitOfWork,
            productMapper,
            uploadService),
            IProductManagementService
    {
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly CsvImportService<Product, ImportProductCsvRowDTO> _csvImportService = csvImportService;

        public override async Task<GetProductDTO> CreateAsync(
            CreateProductDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            var result = await base.CreateAsync(createDTO, cancellationToken);
            await _auditLogWriter.LogAsync(
                AuditLogCategory.Product,
                AuditLogAction.CreateProduct,
                $"Product:{result.Id}:{result.Name}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return result;
        }

        public override async Task<GetProductDTO> UpdateAsync(
            int id,
            UpdateProductDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            var result = await base.UpdateAsync(id, updateDTO, cancellationToken);
            await _auditLogWriter.LogAsync(
                AuditLogCategory.Product,
                AuditLogAction.UpdateProduct,
                $"Product:{result.Id}:{result.Name}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return result;
        }

        public override async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(id, cancellationToken);
            await _auditLogWriter.LogAsync(
                AuditLogCategory.Product,
                AuditLogAction.DeleteProduct,
                $"Product:{id}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public override async Task DeleteSelectedIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            await base.DeleteSelectedIdsAsync(ids, cancellationToken);
            await _auditLogWriter.LogAsync(
                AuditLogCategory.Product,
                AuditLogAction.DeleteProducts,
                $"Products:{ids.Count}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task<BatchImportResultDTO> BatchImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            var result = await _csvImportService.ImportAsync(file, cancellationToken);
            if (result.Succeeded > 0)
            {
                await _auditLogWriter.LogAsync(
                    AuditLogCategory.Product,
                    AuditLogAction.ImportProducts,
                    $"Products:{result.Succeeded}",
                    cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }

            return result;
        }

        public async Task<List<GetProductDTO>> UpdateProductsStatusAsync(
            UpdateProductsStatusDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            updateDTO.TryValidate();

            var productIds = updateDTO.ProductIds.Distinct().ToList();
            await EnsureProductsExistAsync(productIds, cancellationToken);

            var products = await _repository.GetTrackedByIdsAsync(productIds, cancellationToken: cancellationToken);

            foreach (var product in products)
            {
                product.Status = updateDTO.Status;
            }

            if (products.Count == 1)
            {
                _repository.Update(products.Single());
            }
            else
            {
                _repository.UpdateRange(products);
            }

            await _auditLogWriter.LogAsync(
                AuditLogCategory.Product,
                AuditLogAction.UpdateProductStatus,
                $"Products:{products.Count}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return await GetAllByIdsAsync(productIds, cancellationToken);
        }

        private async Task EnsureProductsExistAsync(
            List<int> productIds,
            CancellationToken cancellationToken)
        {
            if (productIds.Count == 0)
            {
                throw new InvalidDataException("At least one product is required.");
            }

            var existingProductCount = await _repository.CountAsync(
                product => productIds.Contains(product.Id),
                cancellationToken);
            if (existingProductCount != productIds.Count)
            {
                throw new DataNotFoundException("One or more products were not found.");
            }
        }
    }
}
