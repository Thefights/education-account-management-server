using Enums;
using Models;
using Persistence.Seeding.Constants;
using Utils;

namespace Persistence.Seeding
{
    public sealed class EducationAccountSweepTargetSeedBuilder : ISeedBuilder
    {
        public int Priority => 106;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EducationAccountSweepTarget>().HasData(
                new EducationAccountSweepTarget { Id = 1, SweepReportId = 1, Nric = SingaporeNricUtil.Generate(1), Action = SweepAction.Create, Status = SweepTargetStatus.Success, Reason = "Eligible citizen account created." },
                new EducationAccountSweepTarget { Id = 2, SweepReportId = 1, Nric = SingaporeNricUtil.Generate(2), Action = SweepAction.Close, Status = SweepTargetStatus.Success, Reason = "Account reached the closing date." },
                new EducationAccountSweepTarget { Id = 3, SweepReportId = 1, Nric = SingaporeNricUtil.Generate(3), Action = SweepAction.Extend, Status = SweepTargetStatus.Success, Reason = "Active enrollment requires an extension." },
                new EducationAccountSweepTarget { Id = 4, SweepReportId = 1, Nric = SingaporeNricUtil.Generate(4), Action = SweepAction.Create, Status = SweepTargetStatus.Failed, Reason = "Manual identity verification is required." },
                new EducationAccountSweepTarget { Id = 5, SweepReportId = 2, Nric = SingaporeNricUtil.Generate(5), Action = SweepAction.Create, Status = SweepTargetStatus.Success, Reason = "Eligible citizen account created." },
                new EducationAccountSweepTarget { Id = 6, SweepReportId = 2, Nric = SingaporeNricUtil.Generate(6), Action = SweepAction.Extend, Status = SweepTargetStatus.Success, Reason = "Active enrollment requires an extension." },
                new EducationAccountSweepTarget { Id = 7, SweepReportId = 2, Nric = SingaporeNricUtil.Generate(7), Action = SweepAction.Close, Status = SweepTargetStatus.Failed, Reason = "Overdue charge reconciliation requires manual handling." });

            return modelBuilder;
        }
    }
}
