using DTOs.SupportTicket;
using Filters.SupportTickets;
using Results;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Interfaces.SupportTicket
{
    public interface ISupportTicketService
    {
        Task CreateTicketAsync(CreateSupportTicketDTO request, CancellationToken cancellationToken = default);
        Task<PaginationResult<GetSupportTicketDTO>> GetPendingTicketsAsync(SupportTicketFilterDTO filterDTO, CancellationToken cancellationToken = default);
        Task<PaginationResult<GetSupportTicketDTO>> GetMyTicketsAsync(SupportTicketFilterDTO filterDTO, CancellationToken cancellationToken = default);
        Task ReplyTicketAsync(int id, ReplySupportTicketDTO request, CancellationToken cancellationToken = default);
    }
}
