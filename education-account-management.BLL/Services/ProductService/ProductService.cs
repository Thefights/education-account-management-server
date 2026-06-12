using DTOs.Product;
using Interfaces.Product;

namespace Services.ProductService
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository = unitOfWork.Repository<Product>();

        public async Task<List<GetUserProductDTO>> GetActiveProductsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _productRepository
                .Query()
                .Where(product => product.Status == ProductStatus.Active)
                .Select(product => new GetUserProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Status = product.Status.ToString()
                })
                .ToListAsync(cancellationToken);
        }
    }
}
