using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class ProductSeedBuilder : ISeedBuilder
    {
        public int Priority => 40;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "MOS Portal", Description = "Customer access portal", Status = ProductStatus.Active, CreationDate = SeedConstants.CreatedAt },
                new Product { Id = 2, Name = "MOS Admin", Description = "Administrative management console", Status = ProductStatus.Active, CreationDate = SeedConstants.CreatedAt },
                new Product { Id = 3, Name = "MOS Analytics", Description = "Analytics and reporting workspace", Status = ProductStatus.Active, CreationDate = SeedConstants.CreatedAt },
                new Product { Id = 4, Name = "MOS Billing", Description = "Billing and subscription management", Status = ProductStatus.Active, CreationDate = SeedConstants.CreatedAt },
                new Product { Id = 5, Name = "MOS Support", Description = "Support ticket management", Status = ProductStatus.Active, CreationDate = SeedConstants.CreatedAt },
                new Product { Id = 6, Name = "MOS Identity", Description = "Identity and access management", Status = ProductStatus.Active, CreationDate = SeedConstants.CreatedAt },
                new Product { Id = 7, Name = "MOS Docs", Description = "Document management workspace", Status = ProductStatus.Active, CreationDate = SeedConstants.CreatedAt },
                new Product { Id = 8, Name = "MOS Workflow", Description = "Workflow automation module", Status = ProductStatus.Active, CreationDate = SeedConstants.CreatedAt },
                new Product { Id = 9, Name = "MOS Reports", Description = "Scheduled reporting module", Status = ProductStatus.Inactive, CreationDate = SeedConstants.CreatedAt },
                new Product { Id = 10, Name = "MOS Audit", Description = "Audit trail review module", Status = ProductStatus.Active, CreationDate = SeedConstants.CreatedAt });

            return modelBuilder;
        }
    }
}
