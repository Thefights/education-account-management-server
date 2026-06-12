using DTOs.Csv;
using Interfaces.Csv;
using Interfaces.Product;
using Models;
using Services.Csv;
using Services.ProductService;

namespace Extensions.DependencyInjection;

public static class ProductServiceExtensions
{
    public static IServiceCollection AddProductServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductManagementService, ProductManagementService>();

        services.AddScoped<ICsvImportProfile<Product, ImportProductCsvRowDTO>, ProductCsvImportProfile>();

        return services;
    }
}