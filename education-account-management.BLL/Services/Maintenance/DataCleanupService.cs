using Interfaces.Maintenance;

namespace Services.Maintenance
{
    public class DataCleanupService(
        IUnitOfWork unitOfWork,
        ILogger<DataCleanupService> logger)
        : IDataCleanupService
    {
        private const int TerminalFailedOutboxAttemptCount = 5;

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DataCleanupService> _logger = logger;
        private readonly IGenericRepository<OtpVerification> _otpVerificationRepository = unitOfWork.Repository<OtpVerification>();
        private readonly IGenericRepository<PasswordResetToken> _passwordResetTokenRepository = unitOfWork.Repository<PasswordResetToken>();
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
            var deletedPasswordResetTokenCount = await DeletePasswordResetTokensAsync(authTransientCutoff, normalizedBatchSize, cancellationToken);
            var deletedRefreshTokenCount = await DeleteRefreshTokensAsync(authTransientCutoff, normalizedBatchSize, cancellationToken);
            var deletedOutboxMessageCount = await DeleteOutboxMessagesAsync(outboxCutoff, normalizedBatchSize, cancellationToken);

            _logger.LogInformation(
                "Data cleanup completed. DeletedOtpCount: {DeletedOtpCount}, DeletedPasswordResetTokenCount: {DeletedPasswordResetTokenCount}, DeletedRefreshTokenCount: {DeletedRefreshTokenCount}, DeletedOutboxMessageCount: {DeletedOutboxMessageCount}.",
                deletedOtpCount,
                deletedPasswordResetTokenCount,
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
                    .Where(otp => otp.ExpiresAt < cutoff
                        || (otp.UsedAt != null && otp.UsedAt < cutoff)
                        || (otp.InvalidatedAt != null && otp.InvalidatedAt < cutoff))
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

        private async Task<int> DeletePasswordResetTokensAsync(
            DateTime cutoff,
            int batchSize,
            CancellationToken cancellationToken)
        {
            var totalDeleted = 0;
            while (true)
            {
                var items = await _passwordResetTokenRepository
                    .Query(tracking: true)
                    .Where(token => token.ExpiresAt < cutoff
                        || (token.UsedAt != null && token.UsedAt < cutoff))
                    .OrderBy(token => token.Id)
                    .Take(batchSize)
                    .ToListAsync(cancellationToken);

                if (items.Count == 0)
                {
                    return totalDeleted;
                }

                _passwordResetTokenRepository.RemoveRange(items);
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

                var itemIds = items.Select(token => token.Id).ToList();
                var referencingTokens = await _refreshTokenRepository
                    .Query(tracking: true)
                    .Where(token => token.ReplacedByRefreshTokenId != null
                        && itemIds.Contains(token.ReplacedByRefreshTokenId.Value))
                    .ToListAsync(cancellationToken);
                foreach (var referencingToken in referencingTokens)
                {
                    referencingToken.ReplacedByRefreshTokenId = null;
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
                    .Where(message => (message.Status == OutboxMessageStatus.Processed
                            && (message.ProcessedAt ?? message.CreatedAt) < cutoff)
                        || (message.Status == OutboxMessageStatus.Failed
                            && message.AttemptCount >= TerminalFailedOutboxAttemptCount
                            && message.CreatedAt < cutoff))
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
