using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class AiAssistantSettingSeedBuilder : ISeedBuilder
{
    public int Priority => 180;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AiAssistantSetting>().HasData(
            new AiAssistantSetting
            {
                Id = 1,
                IsEnabled = true,
                CreatedAt = SeedDataConstants.CreatedAt
            });

        return modelBuilder;
    }
}
