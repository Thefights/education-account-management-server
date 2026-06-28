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
                new SystemTopup { Id = 1, Name = "Seed System Topup 01", TopupAmount = 100m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 2, Name = "Seed System Topup 02", TopupAmount = 120m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 3, Name = "Seed System Topup 03", TopupAmount = 140m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 4, Name = "Seed System Topup 04", TopupAmount = 160m, Status = SystemTopupStatus.Inactive, CreatedAt = createdAt },
                new SystemTopup { Id = 5, Name = "Seed System Topup 05", TopupAmount = 180m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 6, Name = "Seed System Topup 06", TopupAmount = 200m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 7, Name = "Seed System Topup 07", TopupAmount = 220m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 8, Name = "Seed System Topup 08", TopupAmount = 240m, Status = SystemTopupStatus.Inactive, CreatedAt = createdAt },
                new SystemTopup { Id = 9, Name = "Seed System Topup 09", TopupAmount = 260m, Status = SystemTopupStatus.Active, CreatedAt = createdAt },
                new SystemTopup { Id = 10, Name = "Seed System Topup 10", TopupAmount = 280m, Status = SystemTopupStatus.Active, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
