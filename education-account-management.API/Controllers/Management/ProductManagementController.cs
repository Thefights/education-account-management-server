using Controllers.Base;
using DTOs.Csv;
using DTOs.Product;
using Interfaces.Product;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.Admin)]
    public class ProductManagementController(IProductManagementService productManagementService)
        : CrudController<CreateProductDTO, GetProductDTO, UpdateProductDTO, ProductFilterDTO>(productManagementService)
    {
        private readonly IProductManagementService _productManagementService = productManagementService;

        protected override string? EntityName => "Product";

        [HttpPost("batch-import")]
        public async Task<IActionResult> BatchImport(
           BatchImportProductRequestDTO requestDTO,
            CancellationToken cancellationToken)
        {
            var result = await _productManagementService.BatchImportAsync(requestDTO.File, cancellationToken);
            return Result.SuccessData(result, "Product batch import completed");
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateProductsStatus(
            UpdateProductsStatusDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _productManagementService.UpdateProductsStatusAsync(updateDTO, cancellationToken);
            return Result.SuccessData(result, $"{result.Count} selected Products status updated successfully");
        }
    }
}
