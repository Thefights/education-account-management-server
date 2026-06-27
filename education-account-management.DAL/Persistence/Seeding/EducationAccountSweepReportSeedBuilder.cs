using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationAccountSweepReportSeedBuilder : ISeedBuilder
    {
        public int Priority => 240;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<EducationAccountSweepReport>().HasData(
                new EducationAccountSweepReport { Id = 1, BatchDate = new DateOnly(2026, 6, 1), StartedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(1), AccountsCreatedCount = 1, AccountsClosedCount = 1, AccountsExtendedCount = 1 },
                new EducationAccountSweepReport { Id = 2, BatchDate = new DateOnly(2026, 6, 2), StartedAt = new DateTime(2026, 6, 2, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2026, 6, 2, 0, 0, 0, DateTimeKind.Utc).AddHours(1), AccountsCreatedCount = 0, AccountsClosedCount = 1, AccountsExtendedCount = 0 });

            return modelBuilder;
        }
    }
}
