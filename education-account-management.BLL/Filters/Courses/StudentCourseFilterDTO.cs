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
                ["startDate"] = $"{nameof(Course)}.{nameof(Course.StartDate)}"
            };

        public CourseStatus Tab { get; set; } = CourseStatus.Upcoming;

        public override string Sort { get; set; } = "startDate desc";

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;
    }
}
