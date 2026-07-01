using DTOs.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Interfaces.Notifications
{
    public interface INotificationRealtimeSender
    {
        Task SendNotificationCreatedAsync(
            int recipientUserId,
            NotificationRealtimeDTO notification,
            int unreadCount,
            CancellationToken cancellationToken = default);

        Task SendUnreadCountChangedAsync(
            int recipientUserId,
            int unreadCount,
            CancellationToken cancellationToken = default);
    }
}
