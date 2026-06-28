using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class TopupSystemApplicationSeedBuilder : ISeedBuilder
    {
        public int Priority => 220;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<TopupSystemApplication>().HasData(
                new TopupSystemApplication { Id = 1, SystemTopupId = 1, EducationAccountId = 1, TopupExecutionTargetId = 1 },
                new TopupSystemApplication { Id = 2, SystemTopupId = 2, EducationAccountId = 2, TopupExecutionTargetId = 2 },
                new TopupSystemApplication { Id = 3, SystemTopupId = 3, EducationAccountId = 3, TopupExecutionTargetId = 3 });

            return modelBuilder;
        }
    }
}
