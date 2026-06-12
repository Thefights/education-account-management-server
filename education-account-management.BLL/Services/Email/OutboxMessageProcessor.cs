using DTOs.Email;
using Emails;
using Interfaces.Email;
using System.Text.Json;

namespace Services.Email
{
    public class OutboxMessageProcessor(
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<OutboxMessageProcessor> logger)
        : IOutboxMessageProcessor
    {
        private const int BatchSize = 20;
        private const int MaxAttempts = 5;

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<OutboxMessageProcessor> _logger = logger;
        private readonly IGenericRepository<OutboxMessage> _outboxMessageRepository = unitOfWork.Repository<OutboxMessage>();

        public async Task ProcessPendingAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var messages = await _outboxMessageRepository
                .Query(tracking: true)
                .Where(message => message.Type == OutboxMessageType.SendEmail
                    && (message.Status == OutboxMessageStatus.Pending || message.Status == OutboxMessageStatus.Failed)
                    && (message.NextAttemptAt == null || message.NextAttemptAt <= now)
                    && message.AttemptCount < MaxAttempts)
                .OrderBy(message => message.CreatedAt)
                .Take(BatchSize)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                await ProcessEmailMessageAsync(message, cancellationToken);
            }

            if (messages.Count != 0)
            {
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }
        }

        private async Task ProcessEmailMessageAsync(
            OutboxMessage message,
            CancellationToken cancellationToken)
        {
            message.Status = OutboxMessageStatus.Processing;
            message.AttemptCount++;

            try
            {
                var payload = JsonSerializer.Deserialize<EmailOutboxPayloadDTO>(message.PayloadJson)
                    ?? throw new InternalAppException("Outbox email payload is invalid.");
                var template = new EmailTemplate(payload.Subject, payload.HtmlBody, payload.TextBody);

                await _emailService.SendAsync(payload.ToEmail, template, cancellationToken);

                message.Status = OutboxMessageStatus.Processed;
                message.ProcessedAt = DateTime.UtcNow;
                message.NextAttemptAt = null;
                message.LastError = null;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to process outbox email message {OutboxMessageId}.",
                    message.Id);

                message.Status = message.AttemptCount >= MaxAttempts
                    ? OutboxMessageStatus.Failed
                    : OutboxMessageStatus.Pending;
                message.NextAttemptAt = DateTime.UtcNow.AddMinutes(Math.Min(message.AttemptCount * 5, 60));
                message.LastError = StringUtil.Truncate(ex.Message, 1000);
            }
        }
    }
}
