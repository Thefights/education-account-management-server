using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class OutstandingDeductionRunSeedBuilder : ISeedBuilder
{
    public int Priority => 140;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OutstandingDeductionRun>().HasData(
            new OutstandingDeductionRun { Id = 1, RunMonth = "2025-09", RunDate = new DateOnly(2025, 9, 5), Status = OutstandingDeductionRunStatus.Completed, TotalScannedCharges = 1, TotalDeductedAmount = 0m, SuccessCount = 0, FailedCount = 0, StartedAt = new DateTime(2025, 9, 5, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2025, 9, 5, 0, 2, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2025, 9, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionRun { Id = 2, RunMonth = "2025-10", RunDate = new DateOnly(2025, 10, 5), Status = OutstandingDeductionRunStatus.Completed, TotalScannedCharges = 1, TotalDeductedAmount = 0m, SuccessCount = 0, FailedCount = 1, StartedAt = new DateTime(2025, 10, 5, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2025, 10, 5, 0, 2, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2025, 10, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionRun { Id = 3, RunMonth = "2025-11", RunDate = new DateOnly(2025, 11, 5), Status = OutstandingDeductionRunStatus.Completed, TotalScannedCharges = 1, TotalDeductedAmount = 0m, SuccessCount = 0, FailedCount = 0, StartedAt = new DateTime(2025, 11, 5, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2025, 11, 5, 0, 2, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2025, 11, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionRun { Id = 4, RunMonth = "2025-12", RunDate = new DateOnly(2025, 12, 5), Status = OutstandingDeductionRunStatus.Completed, TotalScannedCharges = 1, TotalDeductedAmount = 0m, SuccessCount = 0, FailedCount = 0, StartedAt = new DateTime(2025, 12, 5, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2025, 12, 5, 0, 2, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2025, 12, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionRun { Id = 5, RunMonth = "2026-01", RunDate = new DateOnly(2026, 1, 5), Status = OutstandingDeductionRunStatus.Completed, TotalScannedCharges = 1, TotalDeductedAmount = 0m, SuccessCount = 0, FailedCount = 0, StartedAt = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2026, 1, 5, 0, 2, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionRun { Id = 6, RunMonth = "2026-02", RunDate = new DateOnly(2026, 2, 5), Status = OutstandingDeductionRunStatus.Completed, TotalScannedCharges = 1, TotalDeductedAmount = 0m, SuccessCount = 0, FailedCount = 0, StartedAt = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2026, 2, 5, 0, 2, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionRun { Id = 7, RunMonth = "2026-03", RunDate = new DateOnly(2026, 3, 5), Status = OutstandingDeductionRunStatus.Completed, TotalScannedCharges = 1, TotalDeductedAmount = 50m, SuccessCount = 1, FailedCount = 0, StartedAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2026, 3, 5, 0, 2, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionRun { Id = 8, RunMonth = "2026-04", RunDate = new DateOnly(2026, 4, 5), Status = OutstandingDeductionRunStatus.Completed, TotalScannedCharges = 1, TotalDeductedAmount = 70m, SuccessCount = 1, FailedCount = 0, StartedAt = new DateTime(2026, 4, 5, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2026, 4, 5, 0, 2, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 4, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionRun { Id = 9, RunMonth = "2026-05", RunDate = new DateOnly(2026, 5, 5), Status = OutstandingDeductionRunStatus.Failed, TotalScannedCharges = 1, TotalDeductedAmount = 0m, SuccessCount = 0, FailedCount = 1, StartedAt = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc), FailureReason = "Database connection was interrupted.", CreatedAt = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionRun { Id = 10, RunMonth = "2026-06", RunDate = new DateOnly(2026, 6, 5), Status = OutstandingDeductionRunStatus.Completed, TotalScannedCharges = 1, TotalDeductedAmount = 0m, SuccessCount = 0, FailedCount = 0, StartedAt = new DateTime(2026, 6, 5, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2026, 6, 5, 0, 2, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 6, 5, 0, 0, 0, DateTimeKind.Utc) });

        return modelBuilder;
    }
}