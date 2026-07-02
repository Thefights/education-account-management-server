using DTOs.FasSchemes;
using Enums;
using Filters.FasSchemes;
using Helpers.FasSchemes;
using Interfaces.Base;
using Interfaces.FasSchemes;
using Results;

namespace Services.FasSchemes
{
    public class AccountHolderFasSchemeService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService) : IAccountHolderFasSchemeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private sealed record BlockingApplicationInfo(int Id, FasApplicationStatus Status);
        private sealed record CurrentCourseInfo(int Id, string CourseCode, string CourseName, CourseStatus Status);

        public async Task<FasSchemeAvailableResponseDTO> GetAvailableSchemesAsync(FasSchemeFilterDTO filter, CancellationToken cancellationToken = default)
        {
            // 1. Xác định User đang đăng nhập và lấy thông tin cơ bản của học sinh (như Ngày sinh) từ bảng Citizen
            var currentAccountHolderId = _currentUserService.UserId;
            
            var studentInfo = await _unitOfWork.Repository<SchoolStudent>()
                .Query()
                .Where(student => student.EducationAccount.Citizen.User != null 
                    && student.EducationAccount.Citizen.User.Id == currentAccountHolderId)
                .Select(student => new {
                    student.Id,
                    student.SchoolId,
                    student.EducationAccount.Citizen.FullName,
                    student.EducationAccount.Citizen.IsSingaporean,
                    student.EducationAccount.Citizen.DateOfBirth
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (studentInfo == null)
            {
                throw new DataNotFoundException("SchoolStudent for the current account holder was not found.");
            }

            // 2. Tìm tất cả các Scheme đang ở trạng thái Active
            // Bao gồm luôn cả việc load các bảng liên quan như Tiers, RequiredDocuments, ConditionGroups và Conditions
            var activeSchemesQuery = _unitOfWork.Repository<FasScheme>()
                .Query()
                .Where(s => s.Status == FasSchemeStatus.Active && s.SchoolId == studentInfo.SchoolId)
                .Include(s => s.Tiers)
                .Include(s => s.RequiredDocuments)
                .Include(s => s.AdditionalQuestions)
                .Include(s => s.SchemeCourses)
                    .ThenInclude(link => link.Course)
                .Include(s => s.ConditionGroups)
                    .ThenInclude(cg => cg.Conditions)
                .Include(s => s.ConditionGroups)
                    .ThenInclude(cg => cg.ChildGroups)
                        .ThenInclude(child => child.Conditions);

            var activeSchemes = await activeSchemesQuery.ToListAsync(cancellationToken);

            var today = DateTime.UtcNow.Date;
            var blockingApplications = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .Where(application => application.SchoolStudentId == studentInfo.Id
                    && (application.Status == FasApplicationStatus.Pending
                        || (application.Status == FasApplicationStatus.Approved
                            && (application.ValidityEndDate == null || application.ValidityEndDate >= today))))
                .OrderBy(application => application.Status == FasApplicationStatus.Pending ? 0 : 1)
                .ThenByDescending(application => application.CreatedAt)
                .Select(application => new
                {
                    application.Id,
                    application.FasSchemeId,
                    application.Status
                })
                .ToListAsync(cancellationToken);
            var blockingApplicationBySchemeId = blockingApplications
                .GroupBy(application => application.FasSchemeId)
                .ToDictionary(
                    group => group.Key,
                    group =>
                    {
                        var application = group.First();
                        return new BlockingApplicationInfo(application.Id, application.Status);
                    });

            var currentCourses = await _unitOfWork.Repository<Enrollment>()
                .Query()
                .Where(enrollment => enrollment.SchoolStudentId == studentInfo.Id
                    && enrollment.Status == EnrollmentStatus.Active
                    && (enrollment.Course.Status == CourseStatus.Enrolling
                        || enrollment.Course.Status == CourseStatus.Upcoming
                        || enrollment.Course.Status == CourseStatus.InProgress))
                .Select(enrollment => new CurrentCourseInfo(
                    enrollment.CourseId,
                    enrollment.Course.CourseCode,
                    enrollment.Course.CourseName,
                    enrollment.Course.Status))
                .ToListAsync(cancellationToken);
            var currentCourseIds = currentCourses
                .Select(course => course.Id)
                .ToHashSet();

            // 3. Keep all active schemes visible. The frontend disables Apply for schemes that
            // already have a pending or currently valid approved application.
            var availableSchemes = activeSchemes;

            // 4. Tính toán Per-capita Income (PCI) nếu FE có gửi param GrossHouseholdIncome và HouseholdMemberCount
            decimal? calculatedPerCapitaIncome = null;
            if (filter.GrossHouseholdIncome.HasValue && filter.HouseholdMemberCount.HasValue)
            {
                calculatedPerCapitaIncome = filter.HouseholdMemberCount.Value > 0 
                    ? filter.GrossHouseholdIncome.Value / filter.HouseholdMemberCount.Value 
                    : 0;
            }

            // Tính số tuổi chính xác của học sinh tính tới thời điểm hiện tại
            int studentAge = DateTime.UtcNow.Year - studentInfo.DateOfBirth.Year;
            if (studentInfo.DateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-studentAge)))
            {
                studentAge--;
            }

            // 5. Quét qua từng Scheme khả dụng để kiểm tra điều kiện (Eligibility)
            var eligibleSchemes = new List<FasScheme>();
            foreach (var scheme in availableSchemes)
            {
                bool isEligible = Evaluate(scheme.ConditionGroups, filter, studentAge, studentInfo.IsSingaporean);
                if (isEligible)
                {
                    eligibleSchemes.Add(scheme);
                }
            }
            availableSchemes = eligibleSchemes;

            // 6. Map các Scheme đã qua bộ lọc sang DTO để trả về cho UI
            var schemeDTOs = availableSchemes
                .Select(s =>
                {
                    var matchedCurrentCourses = currentCourses
                        .Where(course => s.SchemeCourses.Any(link => link.CourseId == course.Id))
                        .OrderBy(course => course.CourseName)
                        .Select(course => new FasSchemeCurrentCourseDTO
                        {
                            Id = course.Id,
                            CourseCode = course.CourseCode,
                            CourseName = course.CourseName,
                            Status = course.Status.ToString()
                        })
                        .ToList();
                    var appliesToCurrentCourses = currentCourseIds.Count > 0
                        && matchedCurrentCourses.Count > 0;
                    var hasBlockingApplication = blockingApplicationBySchemeId.TryGetValue(
                        s.Id,
                        out var blockingApplication);

                    return new FasSchemeAvailableDTO
                    {
                        Id = s.Id,
                        SchemeCode = s.SchemeCode,
                        SchemeName = s.SchemeName,
                        Description = s.Description,
                        DurationInMonths = s.DurationInMonths,
                        PublishedAt = s.PublishedAt,
                        HasBlockingApplication = hasBlockingApplication,
                        BlockingApplicationId = blockingApplication?.Id,
                        BlockingApplicationStatus = blockingApplication?.Status,
                        AppliesToCurrentCourses = appliesToCurrentCourses,
                        MatchedCurrentCourseCount = matchedCurrentCourses.Count,
                        MatchedCurrentCourses = matchedCurrentCourses,
                        ApplyUnavailableReason = blockingApplication != null
                            ? "You already have a pending or active approved application for this scheme."
                            : null,
                        Tiers = s.Tiers.Select(t => new FasSchemeTierDTO
                    {
                        Id = t.Id,
                        TierName = t.TierName,
                        TierIncomeBasis = t.TierIncomeBasis,
                        MinPerCapitaIncome = t.MinPerCapitaIncome,
                        MaxPerCapitaIncome = t.MaxPerCapitaIncome,
                        MinGrossHouseholdIncome = t.MinGrossHouseholdIncome,
                        MaxGrossHouseholdIncome = t.MaxGrossHouseholdIncome,
                        SubsidyType = t.SubsidyType,
                        IsPerComponent = t.IsPerComponent,
                        SubsidyValue = t.SubsidyValue,
                        CourseFeeSubsidyValue = t.CourseFeeSubsidyValue,
                        MiscFeeSubsidyValue = t.MiscFeeSubsidyValue,
                        DisplayOrder = t.DisplayOrder
                    }).ToList(),
                    RequiredDocuments = s.RequiredDocuments.Select(d => new FasSchemeRequiredDocumentDTO
                    {
                        Id = d.Id,
                        DocumentName = d.DocumentName,
                        TemplateUrl = d.TemplateFileKey
                    }).ToList(),
                    AdditionalQuestions = s.AdditionalQuestions.Select(q => new FasSchemeAdditionalQuestionDTO
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionText,
                        IsRequired = q.IsRequired
                    }).ToList(),
                        ConditionsSummary = GenerateConditionsSummary(s.ConditionGroups)
                    };
                })
                .OrderBy(s => s.HasBlockingApplication)
                .ThenByDescending(s => s.AppliesToCurrentCourses)
                .ThenByDescending(s => s.PublishedAt)
                .ThenBy(s => s.SchemeName)
                .ToList();

            return new FasSchemeAvailableResponseDTO
            {
                CalculatedPerCapitaIncome = calculatedPerCapitaIncome,
                StudentProfile = new FasStudentProfileDTO
                {
                    FullName = studentInfo.FullName,
                    Age = studentAge,
                    Nationality = studentInfo.IsSingaporean
                        ? NationalityCategory.SingaporeCitizen
                        : NationalityCategory.Other
                },
                Schemes = schemeDTOs
            };
        }

        private bool Evaluate(ICollection<FasSchemeConditionGroup> conditionGroups, FasSchemeFilterDTO filter, int studentAge, bool isSingaporean)
        {
            // If filter values are not provided, we consider them eligible for the list view
            // to show potential schemes.
            if (!filter.GrossHouseholdIncome.HasValue || !filter.HouseholdMemberCount.HasValue || !filter.GuardianNationality.HasValue)
            {
                return true;
            }

            return FasConditionEvaluator.Evaluate(
                conditionGroups, 
                studentAge, 
                isSingaporean,
                filter.GuardianNationality.Value, 
                filter.GrossHouseholdIncome.Value, 
                filter.HouseholdMemberCount.Value);
        }

        private List<string> GenerateConditionsSummary(ICollection<FasSchemeConditionGroup> conditionGroups)
        {
            var summaryList = new List<string>();
            var rootGroup = conditionGroups?.FirstOrDefault(g => g.ParentGroupId == null);
            if (rootGroup == null) return summaryList;

            GenerateGroupSummary(rootGroup, summaryList, 0);
            return summaryList;
        }

        private void GenerateGroupSummary(FasSchemeConditionGroup group, List<string> summaryList, int level)
        {
            string prefix = new string(' ', level * 2);
            string logicOp = group.LogicalOperator == TopupLogicalOperator.And ? "AND" : "OR";
            bool isFirst = true;

            if (group.Conditions != null)
            {
                foreach (var condition in group.Conditions)
                {
                    string condStr = isFirst && level == 0 ? $"{FormatCondition(condition)}" : $"{logicOp} {FormatCondition(condition)}";
                    if (isFirst) isFirst = false;
                    summaryList.Add(prefix + condStr);
                }
            }

            if (group.ChildGroups != null)
            {
                foreach (var childGroup in group.ChildGroups)
                {
                    GenerateGroupSummary(childGroup, summaryList, level + 1);
                }
            }
        }

        private string FormatCondition(FasSchemeCondition condition)
        {
            string fieldName = condition.Field.ToString();
            string opStr = condition.Operator switch
            {
                FasConditionOperator.Equal => "=",
                FasConditionOperator.NotEqual => "!=",
                FasConditionOperator.LessThan => "<",
                FasConditionOperator.LessThanOrEqual => "<=",
                FasConditionOperator.GreaterThan => ">",
                FasConditionOperator.GreaterThanOrEqual => ">=",
                FasConditionOperator.Between => "Between",
                _ => ""
            };

            if (condition.Field == FasConditionField.StudentNationality || condition.Field == FasConditionField.GuardianNationality)
            {
                return $"{fieldName} {opStr} {condition.ValueText}";
            }

            if (condition.Operator == FasConditionOperator.Between)
            {
                return $"{fieldName} {opStr} {condition.ValueNumber} and {condition.ValueNumberTo}";
            }

            return $"{fieldName} {opStr} {condition.ValueNumber}";
        }
    }
}
