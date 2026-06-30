using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationAccountSeedBuilder : ISeedBuilder
    {
        public int Priority => 20;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var accounts = new List<EducationAccount>();

            for (int i = 1; i <= 50; i++)
            {
                decimal totalCredit = 1000m;
                if (i == 3) totalCredit += 105m;
                if (i == 7) totalCredit += 145m;

                decimal totalDebit = 0m;
                if (i == 1)
                {
                    totalDebit = SeedScenarioConstants.SterlingCourseIds
                        .Take(3)
                        .Sum(SeedScenarioConstants.GetNetAmount);

                    totalDebit += Math.Round(SeedScenarioConstants.GetNetAmount(5) / 6m, 2);
                }

                decimal availableBalance = totalCredit - totalDebit;

                accounts.Add(new EducationAccount
                {
                    Id = i,
                    AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, i),
                    EducationCreditBalance = availableBalance,
                    Status = EducationAccountStatus.Active,
                    CitizenId = i,
                    CreatedAt = createdAt,
                    OpenedAt = createdAt
                });
            }

            var totalSweepAccounts = SeedScenarioConstants.SweepAccountsPerDay * SeedScenarioConstants.SweepDayCount;
            for (int index = 0; index < totalSweepAccounts; index++)
            {
                var citizenId = SeedScenarioConstants.SweepCitizenStartId + index;
                var batchDate = SeedScenarioConstants.SweepStartDate.AddDays(index / SeedScenarioConstants.SweepAccountsPerDay);
                var openedAt = batchDate.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.FromHours(1)), DateTimeKind.Utc);

                accounts.Add(new EducationAccount
                {
                    Id = citizenId,
                    AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, citizenId),
                    EducationCreditBalance = 0m,
                    Status = EducationAccountStatus.Active,
                    CitizenId = citizenId,
                    CreatedAt = openedAt,
                    OpenedAt = openedAt
                });
            }

            modelBuilder.Entity<EducationAccount>().HasData(accounts);
            return modelBuilder;
        }
    }
}
