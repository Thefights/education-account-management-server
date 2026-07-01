using DTOs.Notifications;
using Filters.Notifications;
using Results;
using System.Threading;
using System.Threading.Tasks;

namespace Interfaces.Notifications
{
    public interface INotificationService
    {
        Task<PaginationResult<GetNotificationDTO>> GetMineAsync(
            NotificationFilterDTO filter,
            CancellationToken cancellationToken = default);

        Task<NotificationUnreadCountDTO> GetUnreadCountAsync(
            CancellationToken cancellationToken = default);

        Task MarkAsReadAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task MarkAllAsReadAsync(
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
