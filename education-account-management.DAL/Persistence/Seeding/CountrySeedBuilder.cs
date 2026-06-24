using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class CountrySeedBuilder : ISeedBuilder
    {
        public int Priority => 15;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Code = "SG", Name = "Singapore", IsActive = true, CreatedAt = createdAt },
                new Country { Id = 2, Code = "MY", Name = "Malaysia", IsActive = true, CreatedAt = createdAt },
                new Country { Id = 3, Code = "ID", Name = "Indonesia", IsActive = true, CreatedAt = createdAt },
                new Country { Id = 4, Code = "VN", Name = "Vietnam", IsActive = true, CreatedAt = createdAt },
                new Country { Id = 5, Code = "TH", Name = "Thailand", IsActive = true, CreatedAt = createdAt },
                new Country { Id = 6, Code = "PH", Name = "Philippines", IsActive = true, CreatedAt = createdAt },
                new Country { Id = 7, Code = "CN", Name = "China", IsActive = true, CreatedAt = createdAt },
                new Country { Id = 8, Code = "IN", Name = "India", IsActive = true, CreatedAt = createdAt },
                new Country { Id = 9, Code = "AU", Name = "Australia", IsActive = true, CreatedAt = createdAt },
                new Country { Id = 10, Code = "GB", Name = "United Kingdom", IsActive = true, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
