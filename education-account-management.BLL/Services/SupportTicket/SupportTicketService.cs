using DTOs.SupportTicket;
using Enums;
using Exceptions;
using Filters.SupportTickets;
using Interfaces.Audit;
using Interfaces.SupportTicket;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.SupportTicket
{
    public class SupportTicketService(
        IUnitOfWork unitOfWork,
        IAuditUserContext auditUserContext) : ISupportTicketService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuditUserContext _auditUserContext = auditUserContext;
        private readonly IGenericRepository<Models.SupportTicket> _supportTicketRepository = unitOfWork.Repository<Models.SupportTicket>();
        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();
        private readonly IGenericRepository<Notification> _notificationRepository = unitOfWork.Repository<Notification>();

        public async Task CreateTicketAsync(CreateSupportTicketDTO request, CancellationToken cancellationToken = default)
        {
            var userId = _auditUserContext.CurrentUserId;
            if (userId == null) throw new UnauthorizedAccessException();

            var user = await _userRepository.Query()
                .Include(u => u.Citizen)
                .ThenInclude(c => c.EducationAccount)
                .FirstOrDefaultAsync(u => u.Id == userId.Value, cancellationToken)
                ?? throw new DataNotFoundException(typeof(User), userId.Value);

            var educationAccountId = user.Citizen?.EducationAccount?.Id;
            if (educationAccountId == null) throw new InvalidOperationException("No education account found for this user.");

            var ticket = new Models.SupportTicket
            {
                AccountHolderId = educationAccountId.Value,
                Title = request.Title,
                QuestionMessage = request.QuestionMessage,
                Status = SupportTicketStatus.Pending
            };

            await _supportTicketRepository.AddAsync(ticket, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task<PaginationResult<GetSupportTicketDTO>> GetPendingTicketsAsync(SupportTicketFilterDTO filterDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);

            var (total, tickets) = await _supportTicketRepository.GetProjectedPaginatedAsync(
                ProjectToGetDTO,
                t => t.Status == SupportTicketStatus.Pending,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression ?? string.Empty,
                filterDTO.Page,
                pageSize,
                new string[] { nameof(Models.SupportTicket.AccountHolder), $"{nameof(Models.SupportTicket.AccountHolder)}.{nameof(EducationAccount.Citizen)}" },
                cancellationToken);

            return new PaginationResult<GetSupportTicketDTO>(total, pageSize, tickets);
        }

        public async Task<PaginationResult<GetSupportTicketDTO>> GetMyTicketsAsync(SupportTicketFilterDTO filterDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            var userId = _auditUserContext.CurrentUserId;
            if (userId == null) throw new UnauthorizedAccessException();

            var user = await _userRepository.Query()
                .Include(u => u.Citizen)
                .ThenInclude(c => c.EducationAccount)
                .FirstOrDefaultAsync(u => u.Id == userId.Value, cancellationToken)
                ?? throw new DataNotFoundException(typeof(User), userId.Value);

            var educationAccountId = user.Citizen?.EducationAccount?.Id;
            if (educationAccountId == null) throw new InvalidOperationException("No education account found for this user.");

            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);

            var (total, tickets) = await _supportTicketRepository.GetProjectedPaginatedAsync(
                ProjectToGetDTO,
                t => t.AccountHolderId == educationAccountId.Value,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression ?? string.Empty,
                filterDTO.Page,
                pageSize,
                new string[] { nameof(Models.SupportTicket.AccountHolder), $"{nameof(Models.SupportTicket.AccountHolder)}.{nameof(EducationAccount.Citizen)}" },
                cancellationToken);

            return new PaginationResult<GetSupportTicketDTO>(total, pageSize, tickets);
        }

        public async Task ReplyTicketAsync(int id, ReplySupportTicketDTO request, CancellationToken cancellationToken = default)
        {
            var adminId = _auditUserContext.CurrentUserId;
            if (adminId == null) throw new UnauthorizedAccessException();

            var ticket = await _supportTicketRepository.Query(tracking: true)
                .Include(t => t.AccountHolder)
                .ThenInclude(ea => ea.Citizen)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
                ?? throw new DataNotFoundException(typeof(Models.SupportTicket), id);

            if (ticket.Status == SupportTicketStatus.Resolved)
                throw new InvalidOperationException("Ticket is already resolved.");

            ticket.AdminResponse = request.ReplyMessage;
            ticket.ResponsedBy = adminId;
            ticket.Status = SupportTicketStatus.Resolved;
            ticket.ResolvedAt = DateTime.UtcNow;

            _supportTicketRepository.Update(ticket);

            var recipientUserId = ticket.AccountHolder.Citizen?.User?.Id;

            if (recipientUserId != null)
            {
                var notification = new Notification
                {
                    RecipientUserId = recipientUserId.Value,
                    Type = NotificationType.SupportTicketReply,
                    Severity = NotificationSeverity.Info,
                    Title = "Reply to your Support Ticket",
                    Message = $"Admin responded to your question: {request.ReplyMessage}"
                };
                await _notificationRepository.AddAsync(notification, cancellationToken);
            }

            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        private static IQueryable<GetSupportTicketDTO> ProjectToGetDTO(IQueryable<Models.SupportTicket> query)
        {
            return query.Select(t => new GetSupportTicketDTO
            {
                Id = t.Id,
                Title = t.Title,
                QuestionMessage = t.QuestionMessage,
                AdminResponse = t.AdminResponse,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                ResolvedAt = t.ResolvedAt,
                AccountHolderName = t.AccountHolder.Citizen != null ? t.AccountHolder.Citizen.FullName : "Unknown"
            });
        }
    }
}
