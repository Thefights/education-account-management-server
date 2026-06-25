using Enums;
using Filters.Base;
using Models;
using System;
using System.Collections.Generic;

namespace Filters.Courses
{
    public class StudentTuitionFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(Enrollment.Id),
                ["createdAt"] = $"{nameof(Enrollment.Charge)}.{nameof(Charge.CreatedAt)}"
            };

        public StudentTuitionFilterStatus Status { get; set; } = StudentTuitionFilterStatus.All;

        public override string Sort { get; set; } = "createdAt desc";

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;
    }
}
