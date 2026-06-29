using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ScheduleTopUpSeedBuilder : ISeedBuilder
    {
        public int Priority => 180;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<ScheduleTopUp>().HasData(
                new ScheduleTopUp { Id = 1, Name = "July Intake Welcome Credit", TopupAmount = 65m, Frequency = ScheduleTopUpFrequency.OneTime, OneTimeExecutionAt = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), ExecuteAtDay = null, ExecuteAtMonth = null, ExecutionTime = new TimeOnly(10, 0), NextExecutionAt = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 2, Name = "Monthly Meal Allowance", TopupAmount = 80m, Frequency = ScheduleTopUpFrequency.Monthly, OneTimeExecutionAt = null, ExecuteAtDay = 15, ExecuteAtMonth = null, ExecutionTime = new TimeOnly(11, 0), NextExecutionAt = new DateTime(2026, 7, 2, 0, 0, 0, DateTimeKind.Utc), Status = ScheduleTopUpStatus.Inactive, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 3, Name = "Annual Materials Grant", TopupAmount = 95m, Frequency = ScheduleTopUpFrequency.Yearly, OneTimeExecutionAt = null, ExecuteAtDay = null, ExecuteAtMonth = 6, ExecutionTime = new TimeOnly(12, 0), NextExecutionAt = null, Status = ScheduleTopUpStatus.Completed, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 4, Name = "Orientation Transport Credit", TopupAmount = 110m, Frequency = ScheduleTopUpFrequency.OneTime, OneTimeExecutionAt = new DateTime(2026, 7, 4, 0, 0, 0, DateTimeKind.Utc), ExecuteAtDay = null, ExecuteAtMonth = null, ExecutionTime = new TimeOnly(13, 0), NextExecutionAt = new DateTime(2026, 7, 4, 0, 0, 0, DateTimeKind.Utc), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 5, Name = "Monthly Attendance Support", TopupAmount = 125m, Frequency = ScheduleTopUpFrequency.Monthly, OneTimeExecutionAt = null, ExecuteAtDay = 15, ExecuteAtMonth = null, ExecutionTime = new TimeOnly(14, 0), NextExecutionAt = new DateTime(2026, 7, 5, 0, 0, 0, DateTimeKind.Utc), Status = ScheduleTopUpStatus.Inactive, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 6, Name = "Year-End Progress Award", TopupAmount = 140m, Frequency = ScheduleTopUpFrequency.Yearly, OneTimeExecutionAt = null, ExecuteAtDay = null, ExecuteAtMonth = 6, ExecutionTime = new TimeOnly(9, 0), NextExecutionAt = null, Status = ScheduleTopUpStatus.Completed, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 7, Name = "One-Time Device Support", TopupAmount = 155m, Frequency = ScheduleTopUpFrequency.OneTime, OneTimeExecutionAt = new DateTime(2026, 7, 7, 0, 0, 0, DateTimeKind.Utc), ExecuteAtDay = null, ExecuteAtMonth = null, ExecutionTime = new TimeOnly(10, 0), NextExecutionAt = new DateTime(2026, 7, 7, 0, 0, 0, DateTimeKind.Utc), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 8, Name = "Monthly Commuter Credit", TopupAmount = 170m, Frequency = ScheduleTopUpFrequency.Monthly, OneTimeExecutionAt = null, ExecuteAtDay = 15, ExecuteAtMonth = null, ExecutionTime = new TimeOnly(11, 0), NextExecutionAt = new DateTime(2026, 7, 8, 0, 0, 0, DateTimeKind.Utc), Status = ScheduleTopUpStatus.Inactive, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 9, Name = "Annual Skills Grant", TopupAmount = 185m, Frequency = ScheduleTopUpFrequency.Yearly, OneTimeExecutionAt = null, ExecuteAtDay = null, ExecuteAtMonth = 6, ExecutionTime = new TimeOnly(12, 0), NextExecutionAt = null, Status = ScheduleTopUpStatus.Completed, CreatedAt = createdAt },
                new ScheduleTopUp { Id = 10, Name = "Workshop Participation Credit", TopupAmount = 200m, Frequency = ScheduleTopUpFrequency.OneTime, OneTimeExecutionAt = new DateTime(2026, 7, 10, 0, 0, 0, DateTimeKind.Utc), ExecuteAtDay = null, ExecuteAtMonth = null, ExecutionTime = new TimeOnly(13, 0), NextExecutionAt = new DateTime(2026, 7, 10, 0, 0, 0, DateTimeKind.Utc), Status = ScheduleTopUpStatus.Active, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
