using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class AdminProfileSeedBuilder : ISeedBuilder
{
    public int Priority => 70;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<AdminProfile>().HasData(
            new AdminProfile { Id = 1, StaffCode = "STAFF-001", FullName = "System Administrator", Email = "admin001@example.com", PhoneNumber = "+6591000001", UserId = 1, CreatedAt = createdAt },
            new AdminProfile { Id = 2, StaffCode = "STAFF-002", FullName = "Finance Administrator", Email = "admin002@example.com", PhoneNumber = "+6591000002", UserId = 2, CreatedAt = createdAt },
            new AdminProfile { Id = 3, StaffCode = "STAFF-003", FullName = "School Administrator", Email = "admin003@example.com", PhoneNumber = "+6591000003", UserId = 3, SchoolId = 1, CreatedAt = createdAt },
            new AdminProfile { Id = 4, StaffCode = "STAFF-004", FullName = "Admin Profile 004", Email = "admin004@example.com", PhoneNumber = "+6591000004", UserId = 4, CreatedAt = createdAt },
            new AdminProfile { Id = 5, StaffCode = "STAFF-005", FullName = "Admin Profile 005", Email = "admin005@example.com", PhoneNumber = "+6591000005", UserId = 5, CreatedAt = createdAt },
            new AdminProfile { Id = 6, StaffCode = "STAFF-006", FullName = "Admin Profile 006", Email = "admin006@example.com", PhoneNumber = "+6591000006", UserId = 6, CreatedAt = createdAt },
            new AdminProfile { Id = 7, StaffCode = "STAFF-007", FullName = "Admin Profile 007", Email = "admin007@example.com", PhoneNumber = "+6591000007", UserId = 7, CreatedAt = createdAt },
            new AdminProfile { Id = 8, StaffCode = "STAFF-008", FullName = "Admin Profile 008", Email = "admin008@example.com", PhoneNumber = "+6591000008", UserId = 8, CreatedAt = createdAt },
            new AdminProfile { Id = 9, StaffCode = "STAFF-009", FullName = "Admin Profile 009", Email = "admin009@example.com", PhoneNumber = "+6591000009", UserId = 9, CreatedAt = createdAt },
            new AdminProfile { Id = 10, StaffCode = "STAFF-010", FullName = "Admin Profile 010", Email = "admin010@example.com", PhoneNumber = "+6591000010", UserId = 10, CreatedAt = createdAt });

        return modelBuilder;
    }

}