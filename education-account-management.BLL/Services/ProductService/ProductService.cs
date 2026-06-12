using DTOs.Product;
using DTOs.User;
using Interfaces.Audit;
using Interfaces.Product;

namespace Services.ProductService
{
    public class ProductService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IAuditLogWriter auditLogWriter)
        : IProductService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IGenericRepository<Product> _productRepository = unitOfWork.Repository<Product>();
        private readonly IGenericRepository<UserFavoriteProduct> _favoriteRepository = unitOfWork.Repository<UserFavoriteProduct>();
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetUserProductDTO>> GetActiveProductsAsync(
            CancellationToken cancellationToken = default)
        {
            return await GetAssignedProductsAsync(cancellationToken);
        }

        public async Task<List<GetUserProductDTO>> GetAssignedProductsAsync(
            CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;
            var isAdmin = _currentUserService.Role == RoleEnum.Admin;

            return await _productRepository
                .Query()
                .Where(product => product.Status == ProductStatus.Active
                    && (isAdmin || product.UserProductAssignments.Any(assignment => assignment.UserId == userId)))
                .Select(product => new GetUserProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Status = product.Status.ToString(),
                    IsFavorited = product.UserFavoriteProducts.Any(favorite => favorite.UserId == userId)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetUserProductDTO>> GetAvailableProductsAsync(
            CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;
            var isAdmin = _currentUserService.Role == RoleEnum.Admin;

            return await _productRepository
                .Query()
                .Where(product => product.Status == ProductStatus.Active
                    && !isAdmin
                    && !product.UserProductAssignments.Any(assignment => assignment.UserId == userId))
                .Select(product => new GetUserProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Status = product.Status.ToString(),
                    IsFavorited = product.UserFavoriteProducts.Any(favorite => favorite.UserId == userId)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetUserProductDTO>> GetUserFavoriteProductsAsync(
    CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;

            return await _productRepository
                .Query()
                .Where(product => product.Status == ProductStatus.Active
                    && product.UserFavoriteProducts.Any(favorite => favorite.UserId == userId))
                .Select(product => new GetUserProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Status = product.Status.ToString(),
                    IsFavorited = true
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<FavoriteProductResponseDTO> ToggleFavoriteAsync(int productId,
            CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;

            var product = await _productRepository
                .Query()
                .FirstOrDefaultAsync(
                    product => product.Id == productId
                        && product.Status == ProductStatus.Active,
                    cancellationToken);
            if (product == null)
            {
                throw new DataNotFoundException(typeof(Product), productId);
            }

            var existingFavorite = await _favoriteRepository
                .Query(tracking: true)
                .FirstOrDefaultAsync(
                    favorite => favorite.UserId == userId && favorite.ProductId == productId,
                    cancellationToken);

            var isFavorited = existingFavorite == null;
            if (existingFavorite == null)
            {
                await _favoriteRepository.AddAsync(
                    new UserFavoriteProduct
                    {
                        UserId = userId,
                        ProductId = productId
                    },
                    cancellationToken);
            }
            else
            {
                _favoriteRepository.Remove(existingFavorite);
            }

            await _auditLogWriter.LogAsync(
                AuditLogCategory.FavoriteProduct,
                isFavorited ? AuditLogAction.AddFavoriteProduct : AuditLogAction.RemoveFavoriteProduct,
                $"Product:{product.Id}:{product.Name}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return new FavoriteProductResponseDTO
            {
                ProductId = productId,
                IsFavorited = isFavorited
            };
        }
    }
}
