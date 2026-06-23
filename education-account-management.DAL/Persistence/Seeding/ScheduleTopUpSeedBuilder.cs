using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class ScheduleTopUpSeedBuilder : ISeedBuilder
{
    public int Priority => 90;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var random = new Random(54321);
        var schedules = new List<ScheduleTopUp>();
        for (var id = 1; id <= 20; id++)
        {
            var frequency = (ScheduleTopUpFrequency)random.Next(1, 4);
            var executionTime = new TimeOnly(random.Next(0, 24), random.Next(0, 60));
            var schedule = new ScheduleTopUp
            {
                Id = id,
                Name = $"Scheduled Top-up {id:000}",
                TopupAmount = random.Next(1, 50) * 10m,
                Frequency = frequency,
                ExecutionTime = executionTime,
                Status = random.NextDouble() > 0.2 ? ScheduleTopUpStatus.Active : ScheduleTopUpStatus.Inactive,
                CreatedAt = SeedDataConstants.CreatedAt
            };

            if (frequency == ScheduleTopUpFrequency.OneTime)
            {
                schedule.OneTimeExecutionAt = new DateTime(2027, 1, Math.Min(id, 28), executionTime.Hour, executionTime.Minute, 0);
                schedule.NextExecutionAt = schedule.OneTimeExecutionAt;
            }
            else if (frequency == ScheduleTopUpFrequency.Monthly)
            {
                schedule.ExecuteAtDay = Math.Min(id, 28);
                schedule.NextExecutionAt = new DateTime(2027, 1, schedule.ExecuteAtDay.Value, executionTime.Hour, executionTime.Minute, 0);
            }
            else
            {
                schedule.ExecuteAtDay = Math.Min(id, 28);
                schedule.ExecuteAtMonth = (id % 12) + 1;
                schedule.NextExecutionAt = new DateTime(2027, schedule.ExecuteAtMonth.Value, schedule.ExecuteAtDay.Value, executionTime.Hour, executionTime.Minute, 0);
            }

            if (schedule.Status == ScheduleTopUpStatus.Inactive)
                schedule.NextExecutionAt = null;
            schedules.Add(schedule);
        }

        modelBuilder.Entity<ScheduleTopUp>().HasData(schedules);
        return modelBuilder;
    }
}