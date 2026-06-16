using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class OtpVerificationSeedBuilder : ISeedBuilder
{
    public int Priority => 50;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<OtpVerification>().HasData(
            Enumerable.Range(1, 10).Select(id => new OtpVerification
            {
                Id = id,
                SessionId = $"otp-session-{id:000}",
                OtpHash = $"otp-hash-{id:000}",
                FailedAttemptCount = id % 3,
                ExpiresAt = createdAt.AddMinutes(10 + id),
                AuthAccountId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
