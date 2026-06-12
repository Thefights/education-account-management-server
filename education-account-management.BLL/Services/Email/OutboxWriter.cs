using DTOs.Email;
using Emails;
using Interfaces.Email;
using System.Text.Json;

namespace Services.Email
{
    public class OutboxWriter(IUnitOfWork unitOfWork) : IOutboxWriter
    {
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
                Type = OutboxMessageType.SendEmail,
                Status = OutboxMessageStatus.Pending,
                PayloadJson = JsonSerializer.Serialize(payload),
            };
            outboxMessage.TryValidate();

            await _outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);
        }
    }
}
