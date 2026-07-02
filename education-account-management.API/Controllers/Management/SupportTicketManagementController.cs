using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.SupportTicket;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Filters.SupportTickets;
using Interfaces.SupportTicket;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Controllers.Management
{
    [Authorize]
    public class SupportTicketManagementController(ISupportTicketService service) : BaseController
    {
        private readonly ISupportTicketService _service = service;

        [HttpGet("pending")]
        [Authorize(Roles = RolePolicy.SystemAdmin)]
        public async Task<IActionResult> GetPendingTickets([FromQuery] SupportTicketFilterDTO filterDTO, CancellationToken cancellationToken)
        {
            var tickets = await _service.GetPendingTicketsAsync(filterDTO, cancellationToken);
            return Result.SuccessData(tickets);
        }

        [HttpPost("{id}/reply")]
        [Authorize(Roles = RolePolicy.SystemAdmin)]
        public async Task<IActionResult> ReplyTicket(int id, ReplySupportTicketDTO request, CancellationToken cancellationToken)
        {
            await _service.ReplyTicketAsync(id, request, cancellationToken);
            return Result.SuccessAction("Ticket replied successfully");
        }
    }
}
