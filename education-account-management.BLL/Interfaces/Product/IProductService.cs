using DTOs.Product;
using DTOs.User;

namespace Interfaces.Product
{
    public interface IProductService
    {
        Task<List<GetUserProductDTO>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
        Task<List<GetUserProductDTO>> GetAssignedProductsAsync(CancellationToken cancellationToken = default);
        Task<List<GetUserProductDTO>> GetAvailableProductsAsync(CancellationToken cancellationToken = default);
        Task<List<GetUserProductDTO>> GetUserFavoriteProductsAsync(CancellationToken cancellationToken = default);
        Task<FavoriteProductResponseDTO> ToggleFavoriteAsync(int productId, CancellationToken cancellationToken = default);
    }
}
