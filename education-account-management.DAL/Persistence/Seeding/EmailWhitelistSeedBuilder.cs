using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class EmailWhitelistSeedBuilder : ISeedBuilder
    {
        public int Priority => 70;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailWhitelist>().HasData(
                new EmailWhitelist { Id = 1, Value = "avepoint.com", CreationDate = SeedConstants.CreatedAt },
                new EmailWhitelist { Id = 2, Value = "example.com", CreationDate = SeedConstants.CreatedAt },
                new EmailWhitelist { Id = 3, Value = "admin@example.com", CreationDate = SeedConstants.CreatedAt },
                new EmailWhitelist { Id = 4, Value = "manager@example.com", CreationDate = SeedConstants.CreatedAt },
                new EmailWhitelist { Id = 5, Value = "support@example.com", CreationDate = SeedConstants.CreatedAt },
                new EmailWhitelist { Id = 6, Value = "partner.example.com", CreationDate = SeedConstants.CreatedAt },
                new EmailWhitelist { Id = 7, Value = "security@example.com", CreationDate = SeedConstants.CreatedAt },
                new EmailWhitelist { Id = 8, Value = "ops@example.com", CreationDate = SeedConstants.CreatedAt },
                new EmailWhitelist { Id = 9, Value = "contoso.com", CreationDate = SeedConstants.CreatedAt },
                new EmailWhitelist { Id = 10, Value = "phuckhang1088@gmail.com", CreationDate = SeedConstants.CreatedAt });

            return modelBuilder;
        }
    }
}
