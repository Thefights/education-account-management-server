using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class SystemTopupSeedBuilder : ISeedBuilder
{
    public int Priority => 90;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var random = new Random(12345);
        var topups = Enumerable.Range(21, 30).Select(id => new SystemTopup
        {
            Id = id,
            Name = $"System Top-up {id:000}",
            TopupAmount = random.Next(1, 100) * 10m,
            Status = random.NextDouble() > 0.1 ? SystemTopupStatus.Active : SystemTopupStatus.Inactive,
            CreatedAt = SeedDataConstants.CreatedAt
        });

        modelBuilder.Entity<SystemTopup>().HasData(topups);
        return modelBuilder;
    }
}
