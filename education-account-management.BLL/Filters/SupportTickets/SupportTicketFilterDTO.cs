using Enums;
using Filters.Base;
using Models;
using System;
using System.Collections.Generic;

namespace Filters.SupportTickets
{
    public class SupportTicketFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(SupportTicket.Id),
                ["status"] = nameof(SupportTicket.Status),
                ["createdAt"] = nameof(SupportTicket.CreatedAt),
                ["accountHolderName"] = $"{nameof(SupportTicket.AccountHolder)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}",
                ["resolvedAt"] = nameof(SupportTicket.ResolvedAt)
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(SupportTicket.Status))]
        public List<SupportTicketStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(SupportTicket.QuestionMessage))]
        [SearchField(nameof(SupportTicket.QuestionMessage))]
        public string? QuestionMessage { get; set; }
    }
}
