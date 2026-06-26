using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ChargeSeedBuilder : ISeedBuilder
    {
        public int Priority => 130;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var schoolNames = new[]
            {
                "Northview Secondary School", "Eastbridge Secondary School", "Westhaven Secondary School",
                "Southpoint Secondary School", "Central Heights School", "Riverside Learning Institute",
                "Lakeside Technical School", "Greenfield Academy", "Harbourfront School",
                "Hillcrest Education Centre"
            };
            var baseCourseNames = new[]
            {
                "Quantitative Problem Solving", "Software Foundations with C#", "Professional Communication Lab",
                "Sustainability Science Workshop", "Digital Media Production", "Service Operations Practicum",
                "Electrical Systems Fundamentals", "Creative Writing Studio", "Data Analytics Essentials",
                "Office Productivity for Business"
            };
            var topics = new[]
            {
                "Academic Writing", "Business Numeracy", "Digital Literacy", "Career Readiness",
                "Applied Science", "Financial Literacy", "Project Collaboration", "Data Skills",
                "Workplace Communication"
            };
            var baseCodes = new[]
            {
                "CRS-2026-A1B2C3D", "CRS-2026-B2C3D4E", "CRS-2026-C3D4E5F", "CRS-2026-D4E5F6G",
                "CRS-2026-E5F6G7H", "CRS-2026-F6G7H8J", "CRS-2026-G7H8J9K", "CRS-2026-H8J9K0L",
                "CRS-2026-J9K0L1M", "CRS-2026-K0L1M2N"
            };
            var baseDescriptions = new[]
            {
                "Applied numeracy and structured problem-solving for academic pathways.",
                "Core programming concepts, debugging, and application structure.",
                "Practical writing, presentation, and workplace communication skills.",
                "Environmental systems, resource planning, and sustainability practice.",
                "Digital storytelling, layout, and media production workflows.",
                "Customer operations, service standards, and scenario-based practice.",
                "Foundational electrical theory, components, and safety practices.",
                "Narrative craft, editing practice, and guided writing critique.",
                "Data preparation, analysis, visualization, and reporting fundamentals.",
                "Document, spreadsheet, and presentation workflows for business users."
            };

            static int GetCourseSchoolId(int courseId)
            {
                return courseId <= 10
                    ? courseId
                    : 1 + (courseId - 11) / 9;
            }

            static int[] GetSchoolStudentIds(int schoolId)
            {
                var ids = new int[10];
                ids[0] = schoolId;
                for (var index = 0; index < 9; index++)
                {
                    ids[index + 1] = 11 + (schoolId - 1) * 9 + index;
                }

                return ids;
            }

            static bool IsChargeGeneratedCourse(int courseId)
            {
                return courseId <= 10
                    || (courseId > 10 && (courseId - 11) % 9 >= 4);
            }

            static (string Code, string Name, string Description, decimal CourseFee, decimal MiscFee, decimal Gst, DateTime StartDate, DateTime EndDate) GetCourseMeta(
                int courseId,
                string[] baseCodes,
                string[] baseCourseNames,
                string[] baseDescriptions,
                string[] topics)
            {
                if (courseId <= 10)
                {
                    var courseFee = 85m + courseId * 15m;
                    var miscFee = courseId % 2 == 0 ? 12m + (courseId / 2 - 1) * 5m : 10m + (courseId - 1) / 2 * 5m;
                    var gst = decimal.Round((courseFee + miscFee) * 0.09m, 2);
                    var startDate = courseId <= 3
                        ? new DateTime(2026, 8, courseId, 0, 0, 0, DateTimeKind.Utc)
                        : courseId <= 6
                            ? new DateTime(2026, 5, courseId, 0, 0, 0, DateTimeKind.Utc)
                            : new DateTime(2026, 3, courseId, 0, 0, 0, DateTimeKind.Utc);

                    return (
                        baseCodes[courseId - 1],
                        baseCourseNames[courseId - 1],
                        baseDescriptions[courseId - 1],
                        courseFee,
                        miscFee,
                        gst,
                        startDate,
                        startDate.AddMonths(courseId <= 6 ? 3 : 2));
                }

                var offset = courseId - 11;
                var schoolId = 1 + offset / 9;
                var index = offset % 9;
                var topic = topics[index];
                var courseFeeAmount = 120m + schoolId * 10m + index * 15m;
                var miscFeeAmount = 15m + index * 2m;
                var startBase = index switch
                {
                    <= 3 => new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    <= 5 => new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    <= 7 => new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    _ => new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                };
                var start = startBase.AddDays((schoolId - 1) * 3 + index * 7);

                return (
                    $"CRS-2026-S{schoolId:00}{index + 1:00}X{(char)('A' + index)}",
                    $"{topic} - School {schoolId} Cohort {index + 1}",
                    "Structured lessons with guided practice and applied assessments.",
                    courseFeeAmount,
                    miscFeeAmount,
                    decimal.Round((courseFeeAmount + miscFeeAmount) * 0.09m, 2),
                    start,
                    start.AddMonths(index is 6 or 7 ? 3 : 2));
            }

            var enrollmentRows = new List<(int Id, int CourseId, int SchoolStudentId)>
            {
                (1, 1, 1), (2, 2, 2), (3, 3, 3), (4, 4, 4), (5, 5, 5),
                (6, 6, 6), (7, 7, 7), (8, 8, 8), (9, 9, 9), (10, 10, 10),
                (11, 1, 9), (13, 2, 9), (14, 3, 9), (12, 4, 9), (15, 5, 9),
                (16, 6, 9), (17, 7, 9), (18, 8, 9), (19, 10, 9)
            };

            var enrollmentId = 20;
            for (var schoolId = 1; schoolId <= 10; schoolId++)
            {
                for (var index = 0; index < 9; index++)
                {
                    enrollmentRows.Add((enrollmentId++, 11 + (schoolId - 1) * 9 + index, 11 + (schoolId - 1) * 9 + index));
                }
            }

            var existingPairs = enrollmentRows
                .Select(enrollment => (enrollment.CourseId, enrollment.SchoolStudentId))
                .ToHashSet();
            var enrollmentCountsByCourse = enrollmentRows
                .GroupBy(enrollment => enrollment.CourseId)
                .ToDictionary(group => group.Key, group => group.Count());

            for (var courseId = 1; courseId <= 100; courseId++)
            {
                enrollmentCountsByCourse.TryGetValue(courseId, out var enrollmentCount);
                var schoolId = GetCourseSchoolId(courseId);

                foreach (var schoolStudentId in GetSchoolStudentIds(schoolId))
                {
                    if (enrollmentCount >= 10)
                    {
                        break;
                    }

                    if (!existingPairs.Add((courseId, schoolStudentId)))
                    {
                        continue;
                    }

                    enrollmentRows.Add((enrollmentId++, courseId, schoolStudentId));
                    enrollmentCount++;
                }

                enrollmentCountsByCourse[courseId] = enrollmentCount;
            }

            var tier1Amounts = new[] { 75m, 300m, 150m, 50m, 500m, 200m, 150m, 40m, 250m, 60m };
            var tier2Amounts = new[] { 35m, 120m, 90m, 25m, 180m, 80m, 70m, 20m, 100m, 30m };

            var charges = enrollmentRows
                .Where(enrollment => IsChargeGeneratedCourse(enrollment.CourseId))
                .OrderBy(enrollment => enrollment.Id)
                .Select(enrollment =>
                {
                    var schoolId = GetCourseSchoolId(enrollment.CourseId);
                    var meta = GetCourseMeta(enrollment.CourseId, baseCodes, baseCourseNames, baseDescriptions, topics);
                    var grossAmount = meta.CourseFee + meta.MiscFee + meta.Gst;
                    var primaryStudentId = schoolId;
                    var tier2StudentId = 11 + (schoolId - 1) * 9;
                    var subsidyAmount = 0m;
                    int? fasApplicationId = null;
                    string? tierName = null;
                    decimal? subsidyValue = null;

                    if (enrollment.SchoolStudentId == primaryStudentId)
                    {
                        subsidyAmount = Math.Min(grossAmount, tier1Amounts[schoolId - 1]);
                        fasApplicationId = 20 + (schoolId - 1) * 2 + 1;
                        tierName = "Tier 1";
                        subsidyValue = tier1Amounts[schoolId - 1];
                    }
                    else if (enrollment.SchoolStudentId == tier2StudentId)
                    {
                        subsidyAmount = Math.Min(grossAmount, tier2Amounts[schoolId - 1]);
                        fasApplicationId = 20 + (schoolId - 1) * 2 + 2;
                        tierName = "Tier 2";
                        subsidyValue = tier2Amounts[schoolId - 1];
                    }

                    var netAmount = grossAmount - subsidyAmount;
                    var isPastCourse = enrollment.CourseId is >= 7 and <= 10;

                    return new Charge
                    {
                        Id = enrollment.Id,
                        EnrollmentId = enrollment.Id,
                        Status = isPastCourse ? ChargeStatus.Overdue : ChargeStatus.PendingPayment,
                        SchoolNameSnapshot = schoolNames[schoolId - 1],
                        CourseCodeSnapshot = meta.Code,
                        CourseNameSnapshot = meta.Name,
                        CourseDescriptionSnapshot = meta.Description,
                        CourseStartDateSnapshot = meta.StartDate,
                        CourseEndDateSnapshot = meta.EndDate,
                        CourseFeeAmountSnapshot = meta.CourseFee,
                        MiscFeeAmountSnapshot = meta.MiscFee,
                        GstAmountSnapshot = meta.Gst,
                        TaxRateSnapshot = 0.09m,
                        GrossAmount = grossAmount,
                        SubsidyAmount = subsidyAmount,
                        NetAmount = netAmount,
                        PaidAmount = 0m,
                        RemainingAmount = netAmount,
                        AppliedFasSchemeNameSnapshot = subsidyAmount > 0m ? $"School {schoolId} FAS Demo" : null,
                        AppliedFasTierNameSnapshot = tierName,
                        AppliedFasSubsidyTypeSnapshot = subsidyAmount > 0m ? FasSubsidyType.FixedAmount : null,
                        AppliedFasIsPerComponentSnapshot = false,
                        AppliedFasSubsidyValueSnapshot = subsidyValue,
                        AppliedFasApplicationId = fasApplicationId,
                        CreatedAt = createdAt
                    };
                })
                .ToList();

            modelBuilder.Entity<Charge>().HasData(charges);

            return modelBuilder;
        }
    }
}
