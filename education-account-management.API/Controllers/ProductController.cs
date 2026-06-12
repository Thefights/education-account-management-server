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
    }
}
