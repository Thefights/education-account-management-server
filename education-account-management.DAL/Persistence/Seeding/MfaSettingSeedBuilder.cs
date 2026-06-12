using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class MfaSettingSeedBuilder : ISeedBuilder
    {
        public int Priority => 50;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MfaSetting>().HasData(
                new MfaSetting
                {
                    Id = 1,
                    IsEnabled = false,
                    EmailEnabled = false,
                    SmsEnabled = false,
                    CreationDate = SeedConstants.CreatedAt
                });

            return modelBuilder;
        }
    }
}
