using Enums;
using Filters.Base;
using Models;
using System;
using System.Collections.Generic;

namespace Filters.Notifications
{
    public class NotificationFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(Notification.Id),
                ["createdAt"] = nameof(Notification.CreatedAt),
                ["type"] = nameof(Notification.Type),
                ["isRead"] = nameof(Notification.IsRead)
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(TargetField: nameof(Notification.IsRead))]
        public bool? IsRead { get; set; }

        [FilterField(TargetField: nameof(Notification.Type))]
        public NotificationType? Type { get; set; }

        [SearchField(nameof(Notification.Title))]
        [SearchField(nameof(Notification.Message))]
        public string? Keyword { get; set; }
    }
}
