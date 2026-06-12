using Controllers.Base;
using Interfaces.Product;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.AdminOrTenantUser)]
    public class ProductController(IProductService productService) : BaseController
    {
        private readonly IProductService _productService = productService;

        [HttpGet("all")]
        public async Task<IActionResult> GetActiveProducts(CancellationToken cancellationToken)
        {
            var result = await _productService.GetActiveProductsAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("my-services")]
        public async Task<IActionResult> GetAssignedProducts(CancellationToken cancellationToken)
        {
            var result = await _productService.GetAssignedProductsAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("app-store")]
        public async Task<IActionResult> GetAvailableProducts(CancellationToken cancellationToken)
        {
            var result = await _productService.GetAvailableProductsAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetUserFavoriteProducts(CancellationToken cancellationToken)
        {
            var result = await _productService.GetUserFavoriteProductsAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPost("{productId}/favorite")]
        public async Task<IActionResult> ToggleFavorite(int productId, CancellationToken cancellationToken)
        {
            var result = await _productService.ToggleFavoriteAsync(productId, cancellationToken);
            return Result.SuccessData(result, result.IsFavorited ? "Product favorited successfully" : "Product favorite removed successfully");
        }
    }
}
