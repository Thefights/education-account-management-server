using Enums;
using Interfaces.Maintenance;
using Models;
using Repositories.Interfaces;

namespace Services.Maintenance
{
    public class DataCleanupService(
        IUnitOfWork unitOfWork,
        ILogger<DataCleanupService> logger)
        : IDataCleanupService
    {
        private const int TerminalFailedOutboxRetryCount = 5;

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DataCleanupService> _logger = logger;
        private readonly IGenericRepository<OtpVerification> _otpVerificationRepository = unitOfWork.Repository<OtpVerification>();
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository = unitOfWork.Repository<RefreshToken>();
        private readonly IGenericRepository<OutboxMessage> _outboxMessageRepository = unitOfWork.Repository<OutboxMessage>();

        public async Task CleanupAsync(
            int authTransientRetentionDays,
            int outboxRetentionDays,
            int batchSize,
            CancellationToken cancellationToken = default)
        {
            var normalizedBatchSize = Math.Max(1, batchSize);
            var now = DateTime.UtcNow;
            var authTransientCutoff = now.AddDays(-Math.Max(1, authTransientRetentionDays));
            var outboxCutoff = now.AddDays(-Math.Max(1, outboxRetentionDays));

            var deletedOtpCount = await DeleteOtpVerificationsAsync(authTransientCutoff, normalizedBatchSize, cancellationToken);
            var deletedRefreshTokenCount = await DeleteRefreshTokensAsync(authTransientCutoff, normalizedBatchSize, cancellationToken);
            var deletedOutboxMessageCount = await DeleteOutboxMessagesAsync(outboxCutoff, normalizedBatchSize, cancellationToken);

            _logger.LogInformation(
                "Data cleanup completed. DeletedOtpCount: {DeletedOtpCount}, DeletedRefreshTokenCount: {DeletedRefreshTokenCount}, DeletedOutboxMessageCount: {DeletedOutboxMessageCount}.",
                deletedOtpCount,
                deletedRefreshTokenCount,
                deletedOutboxMessageCount);
        }

        private async Task<int> DeleteOtpVerificationsAsync(
            DateTime cutoff,
            int batchSize,
            CancellationToken cancellationToken)
        {
            var totalDeleted = 0;
            while (true)
            {
                var items = await _otpVerificationRepository
                    .Query(tracking: true)
                    .Where(otp => otp.ExpiresAt < cutoff)
                    .OrderBy(otp => otp.Id)
                    .Take(batchSize)
                    .ToListAsync(cancellationToken);

                if (items.Count == 0)
                {
                    return totalDeleted;
                }

                _otpVerificationRepository.RemoveRange(items);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                totalDeleted += items.Count;
            }
        }

        private async Task<int> DeleteRefreshTokensAsync(
            DateTime cutoff,
            int batchSize,
            CancellationToken cancellationToken)
        {
            var totalDeleted = 0;
            while (true)
            {
                var items = await _refreshTokenRepository
                    .Query(tracking: true)
                    .Where(token => token.ExpiresAt < cutoff
                        || (token.RevokedAt != null && token.RevokedAt < cutoff))
                    .OrderBy(token => token.Id)
                    .Take(batchSize)
                    .ToListAsync(cancellationToken);

                if (items.Count == 0)
                {
                    return totalDeleted;
                }

                _refreshTokenRepository.RemoveRange(items);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                totalDeleted += items.Count;
            }
        }

        private async Task<int> DeleteOutboxMessagesAsync(
            DateTime cutoff,
            int batchSize,
            CancellationToken cancellationToken)
        {
            var totalDeleted = 0;
            while (true)
            {
                var items = await _outboxMessageRepository
                    .Query(tracking: true)
                    .Where(message => (message.Status == OutboxMessageStatus.Completed
                            && message.OccurredAt < cutoff)
                        || (message.Status == OutboxMessageStatus.Failed
                            && message.RetryCount >= TerminalFailedOutboxRetryCount
                            && message.OccurredAt < cutoff))
                    .OrderBy(message => message.Id)
                    .Take(batchSize)
                    .ToListAsync(cancellationToken);

                if (items.Count == 0)
                {
                    return totalDeleted;
                }

                _outboxMessageRepository.RemoveRange(items);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                totalDeleted += items.Count;
            }
        }
    }
}
