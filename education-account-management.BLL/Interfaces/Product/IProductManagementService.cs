using DTOs.Csv;
using DTOs.Product;
using Interfaces.Base;

namespace Interfaces.Product
{
    public interface IProductManagementService : IBaseCrudService<CreateProductDTO, GetProductDTO, UpdateProductDTO>
    {
        Task<BatchImportResultDTO> BatchImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default);

        Task<List<GetProductDTO>> UpdateProductsStatusAsync(
            UpdateProductsStatusDTO updateDTO,
            CancellationToken cancellationToken = default);
    }
}
