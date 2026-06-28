using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ApplicationSettingSeedBuilder : ISeedBuilder
    {
        public int Priority => 180;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationSetting>().HasData(
                new ApplicationSetting
                {
                    Id = 1,
                    IsAiFeatureEnabled = true,
                    TaxRate = 0.09m,
                    InstallmentDueDay = 5,
                    CreatedAt = SeedDataConstants.CreatedAt
                });

            return modelBuilder;
        }
    }
}
