using Emails;
using System.Text.Json;
using Repositories.Interfaces;
using Enums;
using Models;
using Interfaces.Email;
using DTOs.Email;

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
            var messages = await _outboxMessageRepository
                .Query(tracking: true)
                .Where(message => message.Type == OutboxWriter.SendEmailMessageType
                    && (message.Status == OutboxMessageStatus.Pending || message.Status == OutboxMessageStatus.Failed)
                    && message.RetryCount < MaxAttempts)
                .OrderBy(message => message.OccurredAt)
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
            message.RetryCount++;

            try
            {
                var payload = JsonSerializer.Deserialize<EmailOutboxPayloadDTO>(message.PayloadJson)
                    ?? throw new InternalAppException("Outbox email payload is invalid.");
                var template = new EmailTemplate(payload.Subject, payload.HtmlBody, payload.TextBody);

                await _emailService.SendAsync(payload.ToEmail, template, cancellationToken);

                message.Status = OutboxMessageStatus.Completed;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to process outbox email message {OutboxMessageId}.",
                    message.Id);

                message.Status = message.RetryCount >= MaxAttempts
                    ? OutboxMessageStatus.Failed
                    : OutboxMessageStatus.Pending;
            }
        }
    }
}
