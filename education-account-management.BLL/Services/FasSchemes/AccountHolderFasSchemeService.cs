using DTOs.FasSchemes;
using Filters.FasSchemes;
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
                    student.EducationAccount.Citizen.DateOfBirth
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (studentInfo == null)
            {
                throw new DataNotFoundException("SchoolStudent for the current account holder was not found.");
            }

            var today = DateTime.UtcNow.Date;

            // 2. Tìm tất cả các Scheme đang ở trạng thái Active
            // Bao gồm luôn cả việc load các bảng liên quan như Tiers, RequiredDocuments, ConditionGroups và Conditions
            var activeSchemesQuery = _unitOfWork.Repository<FasScheme>()
                .Query()
                .Where(s => s.Status == FasSchemeStatus.Active)
                .Include(s => s.Tiers)
                .Include(s => s.RequiredDocuments)
                .Include(s => s.ConditionGroups)
                    .ThenInclude(cg => cg.Conditions)
                .Include(s => s.ConditionGroups)
                    .ThenInclude(cg => cg.ChildGroups)
                        .ThenInclude(child => child.Conditions);

            var activeSchemes = await activeSchemesQuery.ToListAsync(cancellationToken);

            // 3. Tìm danh sách Scheme ID mà học sinh này đã từng apply 
            // (Chỉ chặn những Scheme đang chờ duyệt (Pending) hoặc đã duyệt nhưng còn hạn (Approved + >= Today))
            var existingApplicationSchemeIds = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .Where(a => a.SchoolStudentId == studentInfo.Id 
                            && (a.Status == FasApplicationStatus.Pending || 
                               (a.Status == FasApplicationStatus.Approved && a.ValidityEndDate >= today)))
                .Select(a => a.FasSchemeId)
                .Distinct()
                .ToListAsync(cancellationToken);

            // Loại bỏ các Scheme đã apply thành công khỏi danh sách Active
            var availableSchemes = activeSchemes
                .Where(s => !existingApplicationSchemeIds.Contains(s.Id))
                .ToList();

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
                bool isEligible = Evaluate(scheme.ConditionGroups, filter, studentAge);
                if (isEligible)
                {
                    eligibleSchemes.Add(scheme);
                }
            }
            availableSchemes = eligibleSchemes;

            // 6. Map các Scheme đã qua bộ lọc sang DTO để trả về cho UI
            var schemeDTOs = availableSchemes
                .Select(s => new FasSchemeAvailableDTO
                {
                    Id = s.Id,
                    SchemeCode = s.SchemeCode,
                    SchemeName = s.SchemeName,
                    Description = s.Description,
                    DurationInMonths = s.DurationInMonths,
                    SubsidyType = s.SubsidyType.ToString(),
                    IsPerComponent = s.IsPerComponent,
                    PublishedAt = s.PublishedAt,
                    Tiers = s.Tiers.Select(t => new FasSchemeTierDTO
                    {
                        Id = t.Id,
                        TierName = t.TierName,
                        MaxPerCapitaIncome = t.MaxPerCapitaIncome,
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
                    ConditionsSummary = GenerateConditionsSummary(s.ConditionGroups)
                }).ToList();

            return new FasSchemeAvailableResponseDTO
            {
                CalculatedPerCapitaIncome = calculatedPerCapitaIncome,
                Schemes = schemeDTOs
            };
        }

        private bool Evaluate(ICollection<FasSchemeConditionGroup> conditionGroups, FasSchemeFilterDTO filter, int studentAge)
        {
            // Nếu Scheme không cấu hình bất kỳ điều kiện nào, coi như ai cũng được apply
            if (conditionGroups == null || !conditionGroups.Any())
            {
                return true; 
            }

            var rootGroup = conditionGroups.FirstOrDefault(g => g.ParentGroupId == null);
            if (rootGroup == null)
            {
                return true;
            }

            return EvaluateGroup(rootGroup, filter, studentAge);
        }

        private bool EvaluateGroup(FasSchemeConditionGroup group, FasSchemeFilterDTO filter, int studentAge)
        {
            bool result = group.LogicalOperator == TopupLogicalOperator.And;

            if (group.Conditions != null && group.Conditions.Any())
            {
                foreach (var condition in group.Conditions)
                {
                    bool conditionResult = EvaluateCondition(condition, filter, studentAge);
                    if (group.LogicalOperator == TopupLogicalOperator.And)
                    {
                        result = result && conditionResult;
                        if (!result) return false;
                    }
                    else
                    {
                        result = result || conditionResult;
                        if (result) return true;
                    }
                }
            }

            if (group.ChildGroups != null && group.ChildGroups.Any())
            {
                foreach (var childGroup in group.ChildGroups)
                {
                    bool childGroupResult = EvaluateGroup(childGroup, filter, studentAge);
                    if (group.LogicalOperator == TopupLogicalOperator.And)
                    {
                        result = result && childGroupResult;
                        if (!result) return false;
                    }
                    else
                    {
                        result = result || childGroupResult;
                        if (result) return true;
                    }
                }
            }

            return result;
        }

        private bool EvaluateCondition(FasSchemeCondition condition, FasSchemeFilterDTO filter, int studentAge)
        {
            decimal valueToCompare = 0;
            int? idToCompare = null;

            // Biến này để đánh dấu xem thông tin cần thiết đã được User gửi lên hay chưa (hoặc có sẵn trong DB không)
            bool isProvided = true;

            switch (condition.Field)
            {
                case FasConditionField.StudentAge:
                    valueToCompare = studentAge; // Tuổi học sinh lấy từ DB
                    break;
                case FasConditionField.StudentNationality:
                    idToCompare = 1; // Học sinh trong hệ thống này luôn được tính là Singapore Citizen (1)
                    break;
                case FasConditionField.ParentNationality:
                    if (!filter.ParentNationalityId.HasValue) isProvided = false;
                    idToCompare = filter.ParentNationalityId;
                    break;
                case FasConditionField.GrossHouseholdIncome:
                    if (!filter.GrossHouseholdIncome.HasValue) isProvided = false;
                    valueToCompare = filter.GrossHouseholdIncome ?? 0;
                    break;
                case FasConditionField.PerCapitaIncome:
                    if (!filter.HouseholdMemberCount.HasValue || !filter.GrossHouseholdIncome.HasValue) isProvided = false;
                    valueToCompare = (filter.HouseholdMemberCount ?? 0) > 0 
                        ? (filter.GrossHouseholdIncome ?? 0) / filter.HouseholdMemberCount.Value 
                        : 0;
                    break;
            }

            // [QUAN TRỌNG]: Nếu User chưa gửi các thông tin như Thu nhập, Số người (khi gọi API dạng "Get All")
            // Ta sẽ tự động BỎ QUA (Skip) điều kiện đó bằng cách trả về true.
            // Điều này giúp Scheme vẫn xuất hiện trên màn hình như một lựa chọn "tiềm năng" thay vì bị loại bỏ sai.
            if (!isProvided)
            {
                return true; 
            }

            if (idToCompare.HasValue)
            {
                int conditionCountryId = condition.CountryId ?? 0;
                return condition.Operator switch
                {
                    FasConditionOperator.Equal => idToCompare.Value == conditionCountryId,
                    FasConditionOperator.NotEqual => idToCompare.Value != conditionCountryId,
                    _ => false
                };
            }

            decimal targetValue = condition.ValueNumber ?? 0;

            return condition.Operator switch
            {
                FasConditionOperator.Equal => valueToCompare == targetValue,
                FasConditionOperator.NotEqual => valueToCompare != targetValue,
                FasConditionOperator.LessThan => valueToCompare < targetValue,
                FasConditionOperator.LessThanOrEqual => valueToCompare <= targetValue,
                FasConditionOperator.GreaterThan => valueToCompare > targetValue,
                FasConditionOperator.GreaterThanOrEqual => valueToCompare >= targetValue,
                FasConditionOperator.Between => valueToCompare >= targetValue && valueToCompare <= (condition.ValueNumberTo ?? 0),
                _ => false
            };
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

            if (condition.Field == FasConditionField.StudentNationality || condition.Field == FasConditionField.ParentNationality)
            {
                return $"{fieldName} {opStr} (Country {condition.CountryId})";
            }

            if (condition.Operator == FasConditionOperator.Between)
            {
                return $"{fieldName} {opStr} {condition.ValueNumber} and {condition.ValueNumberTo}";
            }

            return $"{fieldName} {opStr} {condition.ValueNumber}";
        }
    }
}
