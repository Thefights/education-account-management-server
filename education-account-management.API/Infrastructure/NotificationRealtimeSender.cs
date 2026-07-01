using DTOs.Notifications;
using Hubs;
using Interfaces.Notifications;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class NotificationRealtimeSender(
        IHubContext<NotificationHub> hubContext) : INotificationRealtimeSender
    {
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;

        public async Task SendNotificationCreatedAsync(
            int recipientUserId,
            NotificationRealtimeDTO notification,
            int unreadCount,
            CancellationToken cancellationToken = default)
        {
            var userId = recipientUserId.ToString();
            await _hubContext.Clients.User(userId).SendAsync(
                "notificationCreated",
                notification,
                cancellationToken);

            await _hubContext.Clients.User(userId).SendAsync(
                "notificationUnreadCountChanged",
                new { count = unreadCount },
                cancellationToken);
        }

        public Task SendUnreadCountChangedAsync(
            int recipientUserId,
            int unreadCount,
            CancellationToken cancellationToken = default)
        {
            return _hubContext.Clients.User(recipientUserId.ToString()).SendAsync(
                "notificationUnreadCountChanged",
                new { count = unreadCount },
                cancellationToken);
        }
    }
}
