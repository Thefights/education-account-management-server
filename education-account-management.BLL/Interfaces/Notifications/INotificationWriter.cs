using Enums;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Interfaces.Notifications
{
    public interface INotificationWriter
    {
        Task CreateAsync(
            int recipientUserId,
            NotificationType type,
            NotificationSeverity severity,
            string title,
            string message,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            object? metadata = null,
            CancellationToken cancellationToken = default);

        Task CreateForUsersAsync(
            IReadOnlyCollection<int> recipientUserIds,
            NotificationType type,
            NotificationSeverity severity,
            string title,
            string message,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            object? metadata = null,
            CancellationToken cancellationToken = default);
    }
}
