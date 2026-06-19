using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;
using System.Linq;

namespace Persistence.Seeding
{
    public sealed class TopupScheduleRuleSeedBuilder : ISeedBuilder
    {
        public int Priority => 110;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var random = new Random(12345);
            var mappings = new List<TopupScheduleRule>();

            int mappingId = 1;
            for (int scheduleId = 1; scheduleId <= 20; scheduleId++)
            {
                int numRules = random.Next(1, 5); // 1 to 4 rules per schedule
                
                // Ensure unique rules per schedule
                var assignedRules = new HashSet<int>();
                while (assignedRules.Count < numRules)
                {
                    assignedRules.Add(random.Next(1, 51)); // 1 to 50
                }

                foreach (var ruleId in assignedRules)
                {
                    mappings.Add(new TopupScheduleRule
                    {
                        Id = mappingId++,
                        TopupScheduleId = scheduleId,
                        TopupRuleId = ruleId,
                        CreatedAt = createdAt
                    });
                }
            }

            modelBuilder.Entity<TopupScheduleRule>().HasData(mappings);

            return modelBuilder;
        }
    }
}
