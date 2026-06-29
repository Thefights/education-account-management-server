using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class SystemTopupSeedBuilder : ISeedBuilder
    {
        public int Priority => 160;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<SystemTopup>().HasData(
                new SystemTopup { Id = 1, Name = "Monthly Learning Credit Boost", TopupAmount = 100m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 2, Name = "Transport Support Credit", TopupAmount = 120m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 3, Name = "Digital Access Allowance", TopupAmount = 140m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 4, Name = "Back-to-School Credit", TopupAmount = 160m, Status = SystemTopupStatus.Inactive, CreatedAt = createdAt },
                new SystemTopup { Id = 5, Name = "Exam Preparation Support", TopupAmount = 180m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 6, Name = "SkillsFuture Starter Credit", TopupAmount = 200m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 7, Name = "STEM Enrichment Credit", TopupAmount = 220m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 8, Name = "Community Learning Grant", TopupAmount = 240m, Status = SystemTopupStatus.Inactive, CreatedAt = createdAt },
                new SystemTopup { Id = 9, Name = "Attendance Reward Credit", TopupAmount = 260m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 10, Name = "Holiday Programme Support", TopupAmount = 280m, Status = SystemTopupStatus.Active, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
