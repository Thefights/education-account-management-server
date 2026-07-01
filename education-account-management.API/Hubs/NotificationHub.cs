using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
    }
}
