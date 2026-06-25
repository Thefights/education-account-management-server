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
                new SystemTopup { Id = 30, Name = "Household Support Education Credit", TopupAmount = 300m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 31, Name = "Continuing Education Credit", TopupAmount = 160m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 32, Name = "Campus Essentials Support", TopupAmount = 90m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 33, Name = "Apprenticeship Preparation Grant", TopupAmount = 240m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 34, Name = "Technical Skills Support", TopupAmount = 210m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 35, Name = "Academic Recovery Credit", TopupAmount = 140m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 36, Name = "Independent Learning Grant", TopupAmount = 170m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 37, Name = "Inclusive Education Support", TopupAmount = 280m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 38, Name = "Course Materials Top-up", TopupAmount = 110m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 39, Name = "Youth Employability Credit", TopupAmount = 230m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 40, Name = "Family Assistance Education Credit", TopupAmount = 260m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 41, Name = "Bridge-to-Work Learning Grant", TopupAmount = 190m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 42, Name = "School Participation Support", TopupAmount = 70m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 43, Name = "Community Learning Credit", TopupAmount = 125m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 44, Name = "Practical Training Allowance", TopupAmount = 215m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 45, Name = "Academic Milestone Credit", TopupAmount = 175m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 46, Name = "Student Resilience Grant", TopupAmount = 145m, Status = SystemTopupStatus.Inactive, CreatedAt = createdAt },
                new SystemTopup { Id = 47, Name = "Applied Learning Credit", TopupAmount = 205m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 48, Name = "Education Access Credit", TopupAmount = 95m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 49, Name = "Holiday Learning Support", TopupAmount = 115m, Status = SystemTopupStatus.Inactive, CreatedAt = createdAt },
                new SystemTopup { Id = 50, Name = "Progression Support Grant", TopupAmount = 275m, Status = SystemTopupStatus.Active, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
