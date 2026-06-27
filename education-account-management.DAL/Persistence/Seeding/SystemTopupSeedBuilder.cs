using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class SystemTopupSeedBuilder : ISeedBuilder
    {
        public int Priority => 90;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<SystemTopup>().HasData(
                new SystemTopup { Id = 21, Name = "Post-Secondary Study Support", TopupAmount = 200m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 22, Name = "Youth Skills Development Grant", TopupAmount = 180m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 23, Name = "Low-Balance Learning Credit", TopupAmount = 120m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 24, Name = "Transport Support Credit", TopupAmount = 80m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 25, Name = "Learning Materials Allowance", TopupAmount = 150m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 26, Name = "Digital Learning Access Grant", TopupAmount = 250m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 27, Name = "Exam Readiness Support", TopupAmount = 100m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 28, Name = "Training Pathway Credit", TopupAmount = 220m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 29, Name = "Student Welfare Credit", TopupAmount = 130m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 30, Name = "Household Support Education Credit", TopupAmount = 300m, Status = SystemTopupStatus.Active, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
