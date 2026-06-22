using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class TopupSystemApplicationSeedBuilder : ISeedBuilder
{
    public int Priority => 152;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TopupSystemApplication>().HasData(
            new TopupSystemApplication
            {
                Id = 1,
                TopupRuleId = 21,
                EducationAccountId = 3,
                TopupExecutionTargetId = 3
            });

        return modelBuilder;
    }
}
