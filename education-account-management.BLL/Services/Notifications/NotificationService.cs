using DTOs.Notifications;
using Enums;
using Exceptions;
using Filters.Notifications;
using Infrastructure.Interface;
using Interfaces.Notifications;
using Mappers.Notifications;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Notifications
{
    public class NotificationService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        NotificationMapper mapper,
        INotificationRealtimeSender realtimeSender) : INotificationService, INotificationWriter
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly NotificationMapper _mapper = mapper;
        private readonly INotificationRealtimeSender _realtimeSender = realtimeSender;
        private readonly IGenericRepository<Notification> _repository =
            unitOfWork.Repository<Notification>();

        public async Task<PaginationResult<GetNotificationDTO>> GetMineAsync(
            NotificationFilterDTO filter,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filter);
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);

            var (total, items) = await _repository.GetProjectedPaginatedAsync(
                _mapper.ProjectToGetDTO,
                notification => notification.RecipientUserId == _currentUserService.UserId,
                filter.Filter,
                filter.Search,
                filter.SearchFields,
                filter.SortExpression,
                filter.Page,
                pageSize,
                cancellationToken: cancellationToken);

            return new PaginationResult<GetNotificationDTO>(total, pageSize, items);
        }

        public async Task<NotificationUnreadCountDTO> GetUnreadCountAsync(
            CancellationToken cancellationToken = default)
        {
            var count = await _repository.CountAsync(
                notification => notification.RecipientUserId == _currentUserService.UserId &&
                    !notification.IsRead,
                cancellationToken);

            return new NotificationUnreadCountDTO { Count = count };
        }

        private Task<int> GetUnreadCountAsync(
            int recipientUserId,
            CancellationToken cancellationToken)
        {
            return _repository.CountAsync(
                notification => notification.RecipientUserId == recipientUserId &&
                    !notification.IsRead,
                cancellationToken);
        }

        public async Task MarkAsReadAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var notification = await _repository.Query(tracking: true).FirstOrDefaultAsync(
                item => item.Id == id &&
                    item.RecipientUserId == _currentUserService.UserId,
                cancellationToken)
                ?? throw new DataNotFoundException(typeof(Notification), id);

            if (notification.IsRead)
            {
                return;
            }

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            notification.TryValidate();
            _repository.Update(notification);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var unreadCount = await GetUnreadCountAsync(
                _currentUserService.UserId,
                cancellationToken);
            await _realtimeSender.SendUnreadCountChangedAsync(
                _currentUserService.UserId,
                unreadCount,
                cancellationToken);
        }

        public async Task MarkAllAsReadAsync(
            CancellationToken cancellationToken = default)
        {
            var unreadNotifications = await _repository.Query(tracking: true)
                .Where(notification => notification.RecipientUserId == _currentUserService.UserId &&
                    !notification.IsRead)
                .ToListAsync(cancellationToken);

            if (unreadNotifications.Count == 0)
            {
                return;
            }

            var now = DateTime.UtcNow;
            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadAt = now;
                notification.TryValidate();
            }

            _repository.UpdateRange(unreadNotifications);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            await _realtimeSender.SendUnreadCountChangedAsync(
                _currentUserService.UserId,
                0,
                cancellationToken);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var notification = await _repository.Query(tracking: true).FirstOrDefaultAsync(
                item => item.Id == id &&
                    item.RecipientUserId == _currentUserService.UserId,
                cancellationToken)
                ?? throw new DataNotFoundException(typeof(Notification), id);

            notification.IsDeleted = true;
            notification.DeletedAt = DateTime.UtcNow;
            notification.TryValidate();
            _repository.Update(notification);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var unreadCount = await GetUnreadCountAsync(
                _currentUserService.UserId,
                cancellationToken);
            await _realtimeSender.SendUnreadCountChangedAsync(
                _currentUserService.UserId,
                unreadCount,
                cancellationToken);
        }

        public async Task CreateAsync(
            int recipientUserId,
            NotificationType type,
            NotificationSeverity severity,
            string title,
            string message,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            object? metadata = null,
            CancellationToken cancellationToken = default)
        {
            var notification = new Notification
            {
                RecipientUserId = recipientUserId,
                Type = type,
                Severity = severity,
                Title = title,
                Message = message,
                RelatedEntityType = relatedEntityType,
                RelatedEntityId = relatedEntityId,
                MetadataJson = metadata == null ? null : JsonSerializer.Serialize(metadata)
            };

            notification.TryValidate();
            await _repository.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var unreadCount = await GetUnreadCountAsync(recipientUserId, cancellationToken);
            await _realtimeSender.SendNotificationCreatedAsync(
                recipientUserId,
                _mapper.MapToRealtimeDTO(notification),
                unreadCount,
                cancellationToken);
        }

        public async Task CreateForUsersAsync(
            IReadOnlyCollection<int> recipientUserIds,
            NotificationType type,
            NotificationSeverity severity,
            string title,
            string message,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            object? metadata = null,
            CancellationToken cancellationToken = default)
        {
            var distinctIds = recipientUserIds
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (distinctIds.Count == 0)
            {
                return;
            }

            var notifications = distinctIds.Select(userId => new Notification
            {
                RecipientUserId = userId,
                Type = type,
                Severity = severity,
                Title = title,
                Message = message,
                RelatedEntityType = relatedEntityType,
                RelatedEntityId = relatedEntityId,
                MetadataJson = metadata == null ? null : JsonSerializer.Serialize(metadata)
            }).ToList();

            foreach (var notification in notifications)
            {
                notification.TryValidate();
            }

            await _repository.AddRangeAsync(notifications, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            foreach (var notification in notifications)
            {
                var unreadCount = await GetUnreadCountAsync(
                    notification.RecipientUserId,
                    cancellationToken);
                await _realtimeSender.SendNotificationCreatedAsync(
                    notification.RecipientUserId,
                    _mapper.MapToRealtimeDTO(notification),
                    unreadCount,
                    cancellationToken);
            }
        }
    }
}
