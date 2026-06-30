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
                    targets.Add(new EducationAccountSweepTarget
                    {
                        Id = id++,
                        SweepReportId = reportId,
                        CitizenId = citizenId,
                        Nric = SingaporeNricUtil.Generate(citizenId),
                        Action = SweepAction.Create,
                        Status = SweepTargetStatus.Success,
                        Reason = "Account created successfully."
                    });
                }

                targets.Add(new EducationAccountSweepTarget
                {
                    Id = id++,
                    SweepReportId = reportId,
                    CitizenId = 2,
                    Nric = SingaporeNricUtil.Generate(2),
                    Action = SweepAction.Close,
                    Status = SweepTargetStatus.Success,
                    Reason = "Account closure completed successfully."
                });

                targets.Add(new EducationAccountSweepTarget
                {
                    Id = id++,
                    SweepReportId = reportId,
                    CitizenId = 3,
                    Nric = SingaporeNricUtil.Generate(3),
                    Action = SweepAction.Extend,
                    Status = SweepTargetStatus.Success,
                    Reason = "Account extension completed successfully."
                });

                targets.Add(new EducationAccountSweepTarget
                {
                    Id = id++,
                    SweepReportId = reportId,
                    CitizenId = 4,
                    Nric = SingaporeNricUtil.Generate(4),
                    Action = SweepAction.Create,
                    Status = SweepTargetStatus.Failed,
                    Reason = "Seeded failed target for report filtering."
                });
            }

            modelBuilder.Entity<EducationAccountSweepTarget>().HasData(targets);

            return modelBuilder;
        }
    }
}
