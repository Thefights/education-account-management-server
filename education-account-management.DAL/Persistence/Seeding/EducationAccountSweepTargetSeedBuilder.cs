using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationAccountSweepTargetSeedBuilder : ISeedBuilder
    {
        public int Priority => 250;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<EducationAccountSweepTarget>().HasData(
                new EducationAccountSweepTarget { Id = 1, SweepReportId = 1, Nric = SingaporeNricUtil.Generate(1), Action = SweepAction.Create, Status = SweepTargetStatus.Pending, Reason = "Seed pending create." },
                new EducationAccountSweepTarget { Id = 2, SweepReportId = 1, Nric = SingaporeNricUtil.Generate(2), Action = SweepAction.Close, Status = SweepTargetStatus.Success, Reason = "Seed close success." },
                new EducationAccountSweepTarget { Id = 3, SweepReportId = 1, Nric = SingaporeNricUtil.Generate(3), Action = SweepAction.Extend, Status = SweepTargetStatus.Failed, Reason = "Seed extend failed." },
                new EducationAccountSweepTarget { Id = 4, SweepReportId = 2, Nric = SingaporeNricUtil.Generate(4), Action = SweepAction.Close, Status = SweepTargetStatus.Success, Reason = "Seed close success." });

            return modelBuilder;
        }
    }
}
