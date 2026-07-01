using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Hubs
{
    public class NameIdentifierUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
