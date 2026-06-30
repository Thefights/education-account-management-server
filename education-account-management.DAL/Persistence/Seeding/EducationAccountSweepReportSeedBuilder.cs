using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationAccountSweepReportSeedBuilder : ISeedBuilder
    {
        public int Priority => 240;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var reports = new List<EducationAccountSweepReport>();

            for (var dayIndex = 0; dayIndex < SeedScenarioConstants.SweepDayCount; dayIndex++)
            {
                var batchDate = SeedScenarioConstants.SweepStartDate.AddDays(dayIndex);
                var startedAt = batchDate.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(5)), DateTimeKind.Utc);

                reports.Add(new EducationAccountSweepReport
                {
                    Id = dayIndex + 1,
                    BatchDate = batchDate,
                    StartedAt = startedAt,
                    CompletedAt = startedAt.AddMinutes(55),
                    AccountsCreatedCount = SeedScenarioConstants.SweepAccountsPerDay,
                    AccountsClosedCount = 1,
                    AccountsExtendedCount = 1
                });
            }

            modelBuilder.Entity<EducationAccountSweepReport>().HasData(reports);

            return modelBuilder;
        }
    }
}
