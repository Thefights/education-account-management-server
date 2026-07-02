using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationAccountSweepTargetSeedBuilder : ISeedBuilder
    {
        public int Priority => 250;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var targets = new List<EducationAccountSweepTarget>();
            var id = 1;

            for (var dayIndex = 0; dayIndex < SeedScenarioConstants.SweepDayCount; dayIndex++)
            {
                var reportId = dayIndex + 1;
                var startCitizenId = SeedScenarioConstants.SweepCitizenStartId
                    + (dayIndex * SeedScenarioConstants.SweepAccountsPerDay);

                for (var accountIndex = 0; accountIndex < SeedScenarioConstants.SweepAccountsPerDay; accountIndex++)
                {
                    var citizenId = startCitizenId + accountIndex;
                    var action = accountIndex < SeedScenarioConstants.SweepCreateCountPerDay
                        ? SweepAction.Create
                        : accountIndex < SeedScenarioConstants.SweepCreateCountPerDay
                            + SeedScenarioConstants.SweepExtendCountPerDay
                            ? SweepAction.Extend
                            : SweepAction.Close;

                    targets.Add(new EducationAccountSweepTarget
                    {
                        Id = id++,
                        SweepReportId = reportId,
                        CitizenId = citizenId,
                        Nric = SingaporeNricUtil.Generate(citizenId),
                        Action = action,
                        Status = SweepTargetStatus.Success,
                        Reason = action switch
                        {
                            SweepAction.Create => "Account created successfully.",
                            SweepAction.Extend => "Account extension completed successfully.",
                            _ => "Account closure completed successfully."
                        }
                    });
                }
            }

            modelBuilder.Entity<EducationAccountSweepTarget>().HasData(targets);

            return modelBuilder;
        }
    }
}
