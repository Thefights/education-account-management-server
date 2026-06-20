using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;
using System;
using System.Collections.Generic;
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
                var type = (TopupScheduleType)random.Next(1, 4); // 1 = OneTime, 2 = Monthly, 3 = Yearly
                var executionTime = new TimeOnly(random.Next(0, 24), random.Next(0, 60), 0);
                var schedule = new TopupSchedule
                {
                    Id = i,
                    TopupRuleId = i,
                    Frequency = type,
                    Status = random.NextDouble() > 0.2 ? TopupScheduleStatus.Active : TopupScheduleStatus.Inactive, // 80% active
                    ExecutionTime = executionTime,
                    CreatedAt = createdAt
                };

                if (type == TopupScheduleType.OneTime)
                {
                    schedule.OneTimeExecutionAt = DateTime.SpecifyKind(createdAt, DateTimeKind.Unspecified);
                    schedule.NextExecutionAt = schedule.OneTimeExecutionAt;
                }
                else if (type == TopupScheduleType.Monthly)
                {
                    schedule.ExecuteAtDay = random.Next(1, 29);
                    schedule.NextExecutionAt = new DateTime(2027, random.Next(1, 13), schedule.ExecuteAtDay.Value, executionTime.Hour, executionTime.Minute, 0, DateTimeKind.Unspecified);
                }
                else if (type == TopupScheduleType.Yearly)
                {
                    schedule.ExecuteAtDay = random.Next(1, 29);
                    schedule.ExecuteAtMonth = random.Next(1, 13);
                    schedule.NextExecutionAt = new DateTime(2027, schedule.ExecuteAtMonth.Value, schedule.ExecuteAtDay.Value, executionTime.Hour, executionTime.Minute, 0, DateTimeKind.Unspecified);
                }

                if (schedule.Status == TopupScheduleStatus.Inactive)
                    schedule.NextExecutionAt = null;

                schedules.Add(schedule);
            }

            modelBuilder.Entity<TopupSchedule>().HasData(schedules);

            return modelBuilder;
        }
    }
}
