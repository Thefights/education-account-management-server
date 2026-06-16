using education_account_management.BLL;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer;
using Repositories.Interfaces;

namespace Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddApplicationDbContext<TContext>(
          this IServiceCollection services,
          AppConfiguration configuration)
          where TContext : ApplicationDbContext
        {
            services.AddDbContext<TContext>(options =>
            options.UseSqlServer(configuration.DatabaseConfig.ConnectionString));

            services.AddScoped<IUnitOfWork>(provider =>
                new UnitOfWork(
                    provider.GetRequiredService<TContext>(),
                    provider.GetRequiredService<ILogger<UnitOfWork>>()));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
