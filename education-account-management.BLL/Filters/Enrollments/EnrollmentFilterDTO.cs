namespace Filters.Enrollments;

public class EnrollmentFilterDTO : FilterDTO
{
    private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = nameof(Enrollment.Id),
            ["courseCode"] = $"{nameof(Enrollment.Course)}.{nameof(Course.CourseCode)}",
            ["courseName"] = nameof(Enrollment.CourseNameSnapshot),
            ["citizenNric"] = nameof(Enrollment.CitizenNricSnapshot),
            ["citizenFullName"] = nameof(Enrollment.CitizenFullNameSnapshot),
            ["accountNumber"] = nameof(Enrollment.AccountNumberSnapshot),
            ["enrolledAt"] = nameof(Enrollment.CreatedAt),
            ["chargeStatus"] = $"{nameof(Enrollment.Charge)}.{nameof(Charge.Status)}"
        };

    public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

    [FilterField(FilterOperationEnum.Equal, nameof(Enrollment.CourseId))]
    public int? CourseId { get; set; }

    [FilterField(FilterOperationEnum.Contains, $"{nameof(Enrollment.Course)}.{nameof(Course.CourseCode)}")]
    [SearchField($"{nameof(Enrollment.Course)}.{nameof(Course.CourseCode)}")]
    public string? CourseCode { get; set; }

    [FilterField(FilterOperationEnum.Contains, nameof(Enrollment.CourseNameSnapshot))]
    [SearchField(nameof(Enrollment.CourseNameSnapshot))]
    public string? CourseName { get; set; }

    [FilterField(FilterOperationEnum.Contains, nameof(Enrollment.CitizenNricSnapshot))]
    [SearchField(nameof(Enrollment.CitizenNricSnapshot))]
    public string? CitizenNric { get; set; }

    [FilterField(FilterOperationEnum.Contains, nameof(Enrollment.CitizenFullNameSnapshot))]
    [SearchField(nameof(Enrollment.CitizenFullNameSnapshot))]
    public string? CitizenFullName { get; set; }

    [FilterField(FilterOperationEnum.Contains, nameof(Enrollment.AccountNumberSnapshot))]
    [SearchField(nameof(Enrollment.AccountNumberSnapshot))]
    public string? AccountNumber { get; set; }

    [FilterField(FilterOperationEnum.In, $"{nameof(Enrollment.Charge)}.{nameof(Charge.Status)}")]
    public List<ChargeStatus>? ChargeStatuses { get; set; }
}