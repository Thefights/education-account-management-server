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
        public bool? IsInstallment { get; set; }

        public List<int>? EnrollmentIds { get; set; }

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;
    }
}
