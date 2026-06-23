using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class EducationAccountSeedBuilder : ISeedBuilder
{
    public int Priority => 80;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        var accounts = new List<EducationAccount>
        {
            new() { Id = 1, AccountNumber = "EDU-2026-00000000001", EducationCreditBalance = 0m, Status = EducationAccountStatus.Closed, OpenedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc), ClosedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc), CitizenId = 1, CreatedAt = createdAt },
            new() { Id = 2, AccountNumber = "EDU-2026-00000000002", EducationCreditBalance = 1130m, Status = EducationAccountStatus.Active, OpenedAt = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc), CitizenId = 2, CreatedAt = createdAt },
            new() { Id = 3, AccountNumber = "EDU-2026-00000000003", EducationCreditBalance = 1160m, Status = EducationAccountStatus.Active, OpenedAt = new DateTime(2026, 1, 4, 0, 0, 0, DateTimeKind.Utc), CitizenId = 3, CreatedAt = createdAt },
            new() { Id = 4, AccountNumber = "EDU-2026-00000000004", EducationCreditBalance = 1220m, Status = EducationAccountStatus.Active, OpenedAt = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), CitizenId = 4, CreatedAt = createdAt },
            new() { Id = 5, AccountNumber = "EDU-2026-00000000005", EducationCreditBalance = 1320m, Status = EducationAccountStatus.Active, OpenedAt = new DateTime(2026, 1, 6, 0, 0, 0, DateTimeKind.Utc), CitizenId = 5, CreatedAt = createdAt },
            new() { Id = 6, AccountNumber = "EDU-2026-00000000006", EducationCreditBalance = 450m, Status = EducationAccountStatus.Extended, OpenedAt = new DateTime(2026, 1, 7, 0, 0, 0, DateTimeKind.Utc), CitizenId = 6, CreatedAt = createdAt },
            new() { Id = 7, AccountNumber = "EDU-2026-00000000007", EducationCreditBalance = 1700m, Status = EducationAccountStatus.Active, OpenedAt = new DateTime(2026, 1, 8, 0, 0, 0, DateTimeKind.Utc), CitizenId = 7, CreatedAt = createdAt },
            new() { Id = 8, AccountNumber = "EDU-2026-00000000008", EducationCreditBalance = 0m, Status = EducationAccountStatus.Extended, OpenedAt = new DateTime(2026, 1, 9, 0, 0, 0, DateTimeKind.Utc), CitizenId = 8, CreatedAt = createdAt },
            new() { Id = 9, AccountNumber = "EDU-2026-00000000009", EducationCreditBalance = 1900m, Status = EducationAccountStatus.Active, OpenedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), CitizenId = 9, CreatedAt = createdAt },
            new() { Id = 10, AccountNumber = "EDU-2026-00000000010", EducationCreditBalance = 2000m, Status = EducationAccountStatus.Active, OpenedAt = new DateTime(2026, 1, 11, 0, 0, 0, DateTimeKind.Utc), CitizenId = 10, CreatedAt = createdAt }
        };

        // Add accounts for citizens 11 to 120
        accounts.AddRange(Enumerable.Range(11, 110).Select(id => new EducationAccount
        {
            Id = id,
            AccountNumber = $"EDU-2026-{id:00000000000}",
            EducationCreditBalance = 1000m,
            Status = EducationAccountStatus.Active,
            OpenedAt = createdAt,
            CitizenId = id,
            CreatedAt = createdAt
        }));

        modelBuilder.Entity<EducationAccount>().HasData(accounts.ToArray());

        return modelBuilder;
    }
}
