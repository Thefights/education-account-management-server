using education_account_management.BLL.DTOs.Email;
using Emails;
using Enums;
using Interfaces.Email;
using Models;
using Repositories.Interfaces;
using System.Text.Json;
using Utils;

namespace Services.Email
{
    public class OutboxWriter(IUnitOfWork unitOfWork) : IOutboxWriter
    {
        public const string SendEmailMessageType = "SendEmail";

        private readonly IGenericRepository<OutboxMessage> _outboxMessageRepository = unitOfWork.Repository<OutboxMessage>();

        public async Task EnqueueEmailAsync(
            string toEmail,
            EmailTemplate template,
            CancellationToken cancellationToken = default)
        {
            var payload = new EmailOutboxPayloadDTO
            {
                ToEmail = toEmail,
                Subject = template.Subject,
                HtmlBody = template.HtmlBody,
                TextBody = template.TextBody
            };

            var outboxMessage = new OutboxMessage
            {
                Type = SendEmailMessageType,
                Status = OutboxMessageStatus.Pending,
                PayloadJson = JsonSerializer.Serialize(payload),
                RetryCount = 0,
                OccurredAt = DateTime.UtcNow
            };
            outboxMessage.TryValidate();

            await _outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);
        }
    }
}
