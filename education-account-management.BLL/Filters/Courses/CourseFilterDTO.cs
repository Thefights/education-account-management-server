


namespace Filters.Courses
{
    public class CourseFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(Course.Id),
                ["courseName"] = nameof(Course.CourseName),
                ["courseCode"] = nameof(Course.CourseCode),
                ["status"] = nameof(Course.Status),
                ["schoolName"] = $"{nameof(Course.School)}.{nameof(School.SchoolName)}",
                ["courseFeeAmount"] = nameof(Course.CourseFeeAmount),
                ["miscFeeAmount"] = nameof(Course.MiscFeeAmount),
                ["gstAmount"] = nameof(Course.GstAmount),
                ["enrollmentDueDate"] = nameof(Course.EnrollmentDueDate),
                ["paymentDueDate"] = nameof(Course.PaymentDueDate),
                ["startDate"] = nameof(Course.StartDate),
                ["endDate"] = nameof(Course.EndDate),
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(Course.Status))]
        public List<CourseStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(Course.SchoolId))]
        public int? SchoolId { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(Course.CourseName))]
        [SearchField(nameof(Course.CourseName))]
        public string? CourseName { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(Course.CourseCode))]
        [SearchField(nameof(Course.CourseCode))]
        public string? CourseCode { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(Course.School)}.{nameof(School.SchoolName)}")]
        [SearchField($"{nameof(Course.School)}.{nameof(School.SchoolName)}")]
        public string? SchoolName { get; set; }
    }
}
