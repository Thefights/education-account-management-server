using DTOs.Product;
namespace Interfaces.Product
{
    public interface IProductService
    {
        Task<List<GetUserProductDTO>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    }
}
