using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ScheduleTopUpSeedBuilder : ISeedBuilder
    {
        public int Priority => 90;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<ScheduleTopUp>().HasData(
                new ScheduleTopUp { Id = 1, Name = "Monthly Transport Allowance", TopupAmount = 60m, Frequency = ScheduleTopUpFrequency.Monthly, ExecutionTime = new TimeOnly(8, 0), ExecuteAtDay = 1, NextExecutionAt = new DateTime(2027, 1, 1, 8, 0, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 2, Name = "Monthly Meal Support Credit", TopupAmount = 90m, Frequency = ScheduleTopUpFrequency.Monthly, ExecutionTime = new TimeOnly(8, 30), ExecuteAtDay = 3, NextExecutionAt = new DateTime(2027, 1, 3, 8, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 3, Name = "Semester Materials Allowance", TopupAmount = 180m, Frequency = ScheduleTopUpFrequency.OneTime, ExecutionTime = new TimeOnly(9, 0), OneTimeExecutionAt = new DateTime(2027, 1, 5, 9, 0, 0), NextExecutionAt = new DateTime(2027, 1, 5, 9, 0, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 4, Name = "Annual Study Resource Credit", TopupAmount = 240m, Frequency = ScheduleTopUpFrequency.Yearly, ExecutionTime = new TimeOnly(9, 30), ExecuteAtDay = 10, ExecuteAtMonth = 1, NextExecutionAt = new DateTime(2027, 1, 10, 9, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 5, Name = "Monthly Low-Balance Booster", TopupAmount = 75m, Frequency = ScheduleTopUpFrequency.Monthly, ExecutionTime = new TimeOnly(10, 0), ExecuteAtDay = 5, NextExecutionAt = new DateTime(2027, 1, 5, 10, 0, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 6, Name = "Course Start Support Credit", TopupAmount = 150m, Frequency = ScheduleTopUpFrequency.OneTime, ExecutionTime = new TimeOnly(10, 30), OneTimeExecutionAt = new DateTime(2027, 1, 12, 10, 30, 0), NextExecutionAt = new DateTime(2027, 1, 12, 10, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 7, Name = "Annual Technology Allowance", TopupAmount = 300m, Frequency = ScheduleTopUpFrequency.Yearly, ExecutionTime = new TimeOnly(11, 0), ExecuteAtDay = 15, ExecuteAtMonth = 2, NextExecutionAt = new DateTime(2027, 2, 15, 11, 0, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 8, Name = "Monthly Campus Essentials Credit", TopupAmount = 50m, Frequency = ScheduleTopUpFrequency.Monthly, ExecutionTime = new TimeOnly(11, 30), ExecuteAtDay = 8, NextExecutionAt = new DateTime(2027, 1, 8, 11, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 9, Name = "Exam Period Support Credit", TopupAmount = 120m, Frequency = ScheduleTopUpFrequency.OneTime, ExecutionTime = new TimeOnly(12, 0), OneTimeExecutionAt = new DateTime(2027, 2, 1, 12, 0, 0), NextExecutionAt = new DateTime(2027, 2, 1, 12, 0, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 10, Name = "Annual Enrichment Credit", TopupAmount = 220m, Frequency = ScheduleTopUpFrequency.Yearly, ExecutionTime = new TimeOnly(12, 30), ExecuteAtDay = 20, ExecuteAtMonth = 3, NextExecutionAt = new DateTime(2027, 3, 20, 12, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 11, Name = "Monthly Attendance Support", TopupAmount = 65m, Frequency = ScheduleTopUpFrequency.Monthly, ExecutionTime = new TimeOnly(13, 0), ExecuteAtDay = 11, NextExecutionAt = new DateTime(2027, 1, 11, 13, 0, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 12, Name = "One-Time Emergency Education Credit", TopupAmount = 250m, Frequency = ScheduleTopUpFrequency.OneTime, ExecutionTime = new TimeOnly(13, 30), OneTimeExecutionAt = new DateTime(2027, 2, 8, 13, 30, 0), NextExecutionAt = new DateTime(2027, 2, 8, 13, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 13, Name = "Annual Back-to-School Credit", TopupAmount = 200m, Frequency = ScheduleTopUpFrequency.Yearly, ExecutionTime = new TimeOnly(14, 0), ExecuteAtDay = 5, ExecuteAtMonth = 1, NextExecutionAt = new DateTime(2027, 1, 5, 14, 0, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 14, Name = "Monthly Skills Practice Credit", TopupAmount = 85m, Frequency = ScheduleTopUpFrequency.Monthly, ExecutionTime = new TimeOnly(14, 30), ExecuteAtDay = 14, NextExecutionAt = new DateTime(2027, 1, 14, 14, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 15, Name = "Workshop Materials Credit", TopupAmount = 110m, Frequency = ScheduleTopUpFrequency.OneTime, ExecutionTime = new TimeOnly(15, 0), OneTimeExecutionAt = new DateTime(2027, 2, 15, 15, 0, 0), NextExecutionAt = new DateTime(2027, 2, 15, 15, 0, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 16, Name = "Annual Progression Support", TopupAmount = 275m, Frequency = ScheduleTopUpFrequency.Yearly, ExecutionTime = new TimeOnly(15, 30), ExecuteAtDay = 18, ExecuteAtMonth = 6, NextExecutionAt = new DateTime(2027, 6, 18, 15, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 17, Name = "Monthly Participation Credit", TopupAmount = 55m, Frequency = ScheduleTopUpFrequency.Monthly, ExecutionTime = new TimeOnly(16, 0), ExecuteAtDay = 17, NextExecutionAt = new DateTime(2027, 1, 17, 16, 0, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 18, Name = "Graduation Preparation Credit", TopupAmount = 160m, Frequency = ScheduleTopUpFrequency.OneTime, ExecutionTime = new TimeOnly(16, 30), OneTimeExecutionAt = new DateTime(2027, 3, 1, 16, 30, 0), NextExecutionAt = new DateTime(2027, 3, 1, 16, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 19, Name = "Annual Household Relief Credit", TopupAmount = 320m, Frequency = ScheduleTopUpFrequency.Yearly, ExecutionTime = new TimeOnly(17, 0), ExecuteAtDay = 22, ExecuteAtMonth = 4, NextExecutionAt = new DateTime(2027, 4, 22, 17, 0, 0), Status = ScheduleTopUpStatus.Inactive, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 20, Name = "Monthly Learning Continuity Credit", TopupAmount = 95m, Frequency = ScheduleTopUpFrequency.Monthly, ExecutionTime = new TimeOnly(17, 30), ExecuteAtDay = 20, NextExecutionAt = new DateTime(2027, 1, 20, 17, 30, 0), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt });
            return modelBuilder;
        }
    }
}
