using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class EducationAccountSweepReportSeedBuilder : ISeedBuilder
{
    public int Priority => 105;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<EducationAccountSweepReport>().HasData(
            new EducationAccountSweepReport
            {
                Id = 1,
                BatchDate = new DateOnly(2026, 6, 19),
                StartedAt = createdAt.AddHours(1),
                CompletedAt = createdAt.AddHours(1).AddMinutes(4),
                AccountsCreatedCount = 1,
                AccountsClosedCount = 1,
                AccountsExtendedCount = 1
            },
            new EducationAccountSweepReport
            {
                Id = 2,
                BatchDate = new DateOnly(2026, 6, 20),
                StartedAt = createdAt.AddDays(1).AddHours(1),
                CompletedAt = createdAt.AddDays(1).AddHours(1).AddMinutes(3),
                AccountsCreatedCount = 1,
                AccountsClosedCount = 0,
                AccountsExtendedCount = 1
            });

        return modelBuilder;
    }
}
