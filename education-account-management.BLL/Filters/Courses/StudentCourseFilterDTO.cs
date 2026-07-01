using Enums;
using Filters.Base;
using Models;

namespace Filters.Courses
{
    public class StudentCourseFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(Enrollment.Id),
                ["courseCode"] = $"{nameof(Enrollment.Course)}.{nameof(Course.CourseCode)}",
                ["courseName"] = $"{nameof(Enrollment.Course)}.{nameof(Course.CourseName)}",
                ["status"] = $"{nameof(Enrollment.Course)}.{nameof(Course.Status)}",
                ["startDate"] = $"{nameof(Enrollment.Course)}.{nameof(Course.StartDate)}",
                ["endDate"] = $"{nameof(Enrollment.Course)}.{nameof(Course.EndDate)}"
            };

        public CourseStatus Tab { get; set; } = CourseStatus.Upcoming;

        public override string Sort { get; set; } = "startDate desc";

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.Contains, $"{nameof(Course)}.{nameof(Course.CourseName)}")]
        [SearchField($"{nameof(Course)}.{nameof(Course.CourseName)}")]
        public string? CourseName { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(Course)}.{nameof(Course.CourseCode)}")]
        [SearchField($"{nameof(Course)}.{nameof(Course.CourseCode)}")]
        public string? CourseCode { get; set; }
    }
}
