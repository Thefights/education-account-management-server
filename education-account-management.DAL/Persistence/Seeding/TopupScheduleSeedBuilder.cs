using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;
using System;
using System.Linq;

namespace Persistence.Seeding
{
    public sealed class TopupScheduleSeedBuilder : ISeedBuilder
    {
        public int Priority => 100;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var random = new Random(12345);
            var schedules = new List<TopupSchedule>();

            for (int i = 1; i <= 20; i++)
            {
                var type = (TopupScheduleType)random.Next(0, 3);
                var schedule = new TopupSchedule
                {
                    Id = i,
                    ScheduleName = $"Random Schedule {i:000}",
                    ScheduleType = type,
                    IsActive = random.NextDouble() > 0.2, // 80% active
                    SpecificExecutionTime = new TimeSpan(random.Next(0, 24), random.Next(0, 60), 0),
                    CreatedAt = createdAt
                };

                if (type == TopupScheduleType.OneTime)
                {
                    schedule.OneTimeExecutionDate = new DateTime(2027, random.Next(1, 13), random.Next(1, 28), 0, 0, 0, DateTimeKind.Utc);
                    schedule.NextExecutionAt = schedule.OneTimeExecutionDate.Value.Add(schedule.SpecificExecutionTime);
                }
                else if (type == TopupScheduleType.Monthly)
                {
                    schedule.ExecuteAtDay = random.Next(1, 29);
                    schedule.NextExecutionAt = new DateTime(2027, random.Next(1, 13), schedule.ExecuteAtDay.Value, schedule.SpecificExecutionTime.Hours, schedule.SpecificExecutionTime.Minutes, 0, DateTimeKind.Utc);
                }
                else if (type == TopupScheduleType.Yearly)
                {
                    schedule.ExecuteAtDay = random.Next(1, 29);
                    schedule.ExecuteAtMonth = random.Next(1, 13);
                    schedule.NextExecutionAt = new DateTime(2027, schedule.ExecuteAtMonth.Value, schedule.ExecuteAtDay.Value, schedule.SpecificExecutionTime.Hours, schedule.SpecificExecutionTime.Minutes, 0, DateTimeKind.Utc);
                }

                schedules.Add(schedule);
            }

            modelBuilder.Entity<TopupSchedule>().HasData(schedules);

            return modelBuilder;
        }
    }
}
