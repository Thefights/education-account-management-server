using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class EmailWhitelistSettingSeedBuilder : ISeedBuilder
    {
        public int Priority => 60;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailWhitelistSetting>().HasData(
                new EmailWhitelistSetting { Id = 1, IsEnabled = false, CreationDate = SeedConstants.CreatedAt });

            return modelBuilder;
        }
    }
}
