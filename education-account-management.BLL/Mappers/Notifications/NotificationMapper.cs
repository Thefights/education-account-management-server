using DTOs.Notifications;
using Models;
using Riok.Mapperly.Abstractions;
using System.Linq;

namespace Mappers.Notifications
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class NotificationMapper
    {
        public IQueryable<GetNotificationDTO> ProjectToGetDTO(IQueryable<Notification> query)
        {
            return query.Select(notification => new GetNotificationDTO
            {
                Id = notification.Id,
                Type = notification.Type.ToString(),
                Severity = notification.Severity.ToString(),
                Title = notification.Title,
                Message = notification.Message,
                RelatedEntityType = notification.RelatedEntityType,
                RelatedEntityId = notification.RelatedEntityId,
                MetadataJson = notification.MetadataJson,
                IsRead = notification.IsRead,
                ReadAt = notification.ReadAt,
                CreatedAt = notification.CreatedAt
            });
        }

        public NotificationRealtimeDTO MapToRealtimeDTO(Notification notification)
        {
            return new NotificationRealtimeDTO
            {
                Id = notification.Id,
                Type = notification.Type.ToString(),
                Severity = notification.Severity.ToString(),
                Title = notification.Title,
                Message = notification.Message,
                RelatedEntityType = notification.RelatedEntityType,
                RelatedEntityId = notification.RelatedEntityId,
                MetadataJson = notification.MetadataJson,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}
