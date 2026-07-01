using Authorization;
using Common.HttpResults;
using Controllers.Base;
using Filters.Notifications;
using Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers
{
    [Authorize]
    public class NotificationController(INotificationService service) : BaseController
    {
        private readonly INotificationService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetMine(
            [FromQuery] NotificationFilterDTO filter,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetMineAsync(filter, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
        {
            var result = await _service.GetUnreadCountAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await _service.MarkAsReadAsync(id, cancellationToken);
            return Result.SuccessAction("Notification marked as read.");
        }

        [HttpPost("read-all")]
        public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
        {
            await _service.MarkAllAsReadAsync(cancellationToken);
            return Result.SuccessAction("All notifications marked as read.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(id, cancellationToken);
            return Result.SuccessAction("Notification deleted.");
        }
    }
}
