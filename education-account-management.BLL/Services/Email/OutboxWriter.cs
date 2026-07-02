using DTOs.Email;
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
            var outboxMessage = CreateOutboxMessage(toEmail, template);
            outboxMessage.TryValidate();

            await _outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);
        }

        public async Task EnqueueEmailOnceAsync(
            string toEmail,
            EmailTemplate template,
            CancellationToken cancellationToken = default)
        {
            var outboxMessage = CreateOutboxMessage(toEmail, template);
            var exists = await _outboxMessageRepository.AnyAsync(
                message => message.Type == SendEmailMessageType
                    && message.PayloadJson == outboxMessage.PayloadJson,
                cancellationToken);
            if (exists)
            {
                return;
            }

            outboxMessage.TryValidate();
            await _outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);
        }

        private static OutboxMessage CreateOutboxMessage(
            string toEmail,
            EmailTemplate template)
        {
            var payload = new EmailOutboxPayloadDTO
            {
                ToEmail = toEmail,
                Subject = template.Subject,
                HtmlBody = template.HtmlBody,
                TextBody = template.TextBody
            };

            return new OutboxMessage
            {
                Type = SendEmailMessageType,
                PayloadJson = JsonSerializer.Serialize(payload)
            };
        }
    }
}
