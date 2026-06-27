using DTOs.FasApplications;
using Enums;
using Exceptions;
using Filters.FasApplications;
using Helpers.FasSchemes;
using Interfaces.Base;
using Interfaces.FasApplications;
using Microsoft.EntityFrameworkCore;
using Models;
using Results;
using Utils;

namespace Services.FasApplications
{
    public class AccountHolderFasApplicationService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService) : IAccountHolderFasApplicationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<string> SubmitApplicationAsync(SubmitFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {
            var currentAccountHolderId = _currentUserService.UserId;

            // 1. Lấy thông tin học sinh liên kết với người dùng (Account Holder) hiện tại
            var studentInfo = await _unitOfWork.Repository<SchoolStudent>()
                .Query()
                .Where(student => student.EducationAccount.Citizen.User != null 
                    && student.EducationAccount.Citizen.User.Id == currentAccountHolderId)
                .Select(student => new {
                    student.Id,
                    student.SchoolId,
                    student.EducationAccount.Citizen.IsSingaporean,
                    student.EducationAccount.Citizen.DateOfBirth
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (studentInfo == null)
            {
                throw new DataNotFoundException("SchoolStudent for the current account holder was not found.");
            }

            // 2. Kiểm tra tính hợp lệ của Scheme (có tồn tại, đang hoạt động, và thuộc về trường của học sinh)
            var scheme = await _unitOfWork.Repository<FasScheme>()
                .Query()
                .Include(s => s.Tiers)
                .Include(s => s.RequiredDocuments)
                .Include(s => s.ConditionGroups)
                    .ThenInclude(cg => cg.Conditions)
                .Include(s => s.ConditionGroups)
                    .ThenInclude(cg => cg.ChildGroups)
                        .ThenInclude(child => child.Conditions)
                .FirstOrDefaultAsync(s => s.Id == dto.FasSchemeId && s.Status == FasSchemeStatus.Active && s.SchoolId == studentInfo.SchoolId, cancellationToken);

            if (scheme == null)
            {
                throw new DataNotFoundException(typeof(FasScheme), dto.FasSchemeId);
            }

            // 3. Kiểm tra xem học sinh đã có hồ sơ nào đang chờ duyệt hoặc đã được duyệt cho Scheme này chưa
            var existingApplication = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .AnyAsync(a => a.SchoolStudentId == studentInfo.Id 
                            && a.FasSchemeId == dto.FasSchemeId 
                            && (a.Status == FasApplicationStatus.Pending || 
                               (a.Status == FasApplicationStatus.Approved && (a.ValidityEndDate == null || a.ValidityEndDate >= DateTime.UtcNow))), cancellationToken);
            
            if (existingApplication)
            {
                throw new DataConflictException("You already have a pending or approved application for this scheme.");
            }

            // 4. Đánh giá các điều kiện cơ bản của Scheme
            int? recommendedTierId = null;
            string recommendationReason = "Failed scheme baseline conditions";
            int studentAge = DateTime.UtcNow.Year - studentInfo.DateOfBirth.Year;
            if (studentInfo.DateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-studentAge))) studentAge--;

            bool isEligible = FasConditionEvaluator.Evaluate(
                scheme.ConditionGroups, 
                studentAge, 
                studentInfo.IsSingaporean,
                dto.GuardianNationality, 
                dto.GrossHouseholdIncome, 
                dto.HouseholdMemberCount);

            decimal pci = dto.HouseholdMemberCount > 0 ? dto.GrossHouseholdIncome / dto.HouseholdMemberCount : 0;

            if (isEligible)
            {
                var eligibleTier = scheme.Tiers
                    .Where(t => !t.MaxPerCapitaIncome.HasValue || pci <= t.MaxPerCapitaIncome)
                    .OrderBy(t => t.MaxPerCapitaIncome ?? decimal.MaxValue)
                    .FirstOrDefault();

                if (eligibleTier != null)
                {
                    recommendedTierId = eligibleTier.Id;
                    recommendationReason = eligibleTier.MaxPerCapitaIncome.HasValue 
                        ? $"PCI <= {eligibleTier.MaxPerCapitaIncome}" 
                        : "Matched tier with no PCI limit";
                }
                else
                {
                    recommendationReason = "Eligible for scheme but exceeded all tier PCI limits";
                }
            }

            // Tạo mã hồ sơ (Application Number) ngẫu nhiên
            var applicationNumber = $"FASAPP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..7].ToUpper()}";

            // 5. Khởi tạo đối tượng hồ sơ (Entity) và lưu các thông tin Snapshot tại thời điểm nộp
            var application = new FasApplication
            {
                FasSchemeId = dto.FasSchemeId,
                SchoolStudentId = studentInfo.Id,
                ApplicationNumber = applicationNumber,
                Status = FasApplicationStatus.Pending,
                StudentAgeSnapshot = studentAge,
                StudentNationalitySnapshot = studentInfo.IsSingaporean ? NationalityCategory.SingaporeCitizen : NationalityCategory.Other,
                GuardianNationalitySnapshot = dto.GuardianNationality,
                GrossHouseholdIncomeSnapshot = dto.GrossHouseholdIncome,
                HouseholdMemberCountSnapshot = dto.HouseholdMemberCount,
                PerCapitaIncomeSnapshot = pci,
                RecommendedTierId = recommendedTierId,
                RecommendationReason = recommendationReason,
                Documents = dto.Documents.Select(d => new FasApplicationDocument
                {
                    FasSchemeRequiredDocumentId = d.RequiredDocumentId,
                    FileKey = d.FileKey,
                    FileName = d.FileName,
                    DocumentNameSnapshot = d.FileName // Tạm gán bằng tên file, sẽ được cập nhật chính xác từ cấu hình Scheme ở bước sau.
                }).ToList()
            };

            // Lấy tên tài liệu yêu cầu (DocumentName) từ cấu hình Scheme để lưu vào Snapshot cho chính xác.
            foreach (var doc in application.Documents)
            {
                var reqDoc = scheme.RequiredDocuments.FirstOrDefault(r => r.Id == doc.FasSchemeRequiredDocumentId);
                doc.DocumentNameSnapshot = reqDoc?.DocumentName ?? doc.FileName;
            }

            application.TryValidate();

            await _unitOfWork.Repository<FasApplication>().AddAsync(application, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return applicationNumber;
        }

        public async Task<PaginationResult<FasApplicationSummaryDTO>> GetMyApplicationsAsync(FasApplicationFilterDTO filter, CancellationToken cancellationToken = default)
        {
            var currentAccountHolderId = _currentUserService.UserId;

            // Lấy schoolStudentId của user hiện tại
            var studentId = await _unitOfWork.Repository<SchoolStudent>()
                .Query()
                .Where(student => student.EducationAccount.Citizen.User != null 
                    && student.EducationAccount.Citizen.User.Id == currentAccountHolderId)
                .Select(student => student.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (studentId == 0)
            {
                throw new DataNotFoundException("SchoolStudent for the current account holder was not found.");
            }

            var pageSize = Math.Clamp(filter.PageSize, 1, 100);

            var (total, items) = await _unitOfWork.Repository<FasApplication>().GetProjectedPaginatedAsync(
                query => query.Select(a => new FasApplicationSummaryDTO
                {
                    Id = a.Id,
                    ApplicationNumber = a.ApplicationNumber,
                    SchemeName = a.FasScheme.SchemeName,
                    Status = a.Status,
                    SubmittedAt = a.CreatedAt,
                    ApprovedDate = a.ApprovedAt,
                    ValidityEndDate = a.ValidityEndDate,
                    RejectionReason = a.RejectionReason
                }),
                a => a.SchoolStudentId == studentId,
                filter.Filter,
                filter.Search,
                filter.SearchFields,
                filter.SortExpression,
                filter.Page,
                pageSize,
                null,
                cancellationToken);

            return new PaginationResult<FasApplicationSummaryDTO>(total, pageSize, items);
        }
        public async Task WithdrawApplicationAsync(int id, CancellationToken cancellationToken = default)
        {
            var currentAccountHolderId = _currentUserService.UserId;

            // Lấy schoolStudentId của user hiện tại
            var studentId = await _unitOfWork.Repository<SchoolStudent>()
                .Query()
                .Where(student => student.EducationAccount.Citizen.User != null 
                    && student.EducationAccount.Citizen.User.Id == currentAccountHolderId)
                .Select(student => student.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (studentId == 0)
            {
                throw new DataNotFoundException("SchoolStudent for the current account holder was not found.");
            }

            var application = await _unitOfWork.Repository<FasApplication>()
                .Query(tracking: true)
                .FirstOrDefaultAsync(a => a.Id == id && a.SchoolStudentId == studentId, cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException("FAS Application not found.");
            }

            if (application.Status != FasApplicationStatus.Pending)
            {
                throw new ValidationFailureException(nameof(application.Status), "Only pending applications can be withdrawn.");
            }

            application.Status = FasApplicationStatus.Withdrawn;
            application.WithdrawnAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task<FasApplicationDetailDTO> GetApplicationDetailAsync(int id, CancellationToken cancellationToken = default)
        {
            var currentAccountHolderId = _currentUserService.UserId;

            // Lấy schoolStudentId của user hiện tại
            var studentId = await _unitOfWork.Repository<SchoolStudent>()
                .Query()
                .Where(student => student.EducationAccount.Citizen.User != null
                    && student.EducationAccount.Citizen.User.Id == currentAccountHolderId)
                .Select(student => student.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (studentId == 0)
            {
                throw new DataNotFoundException("SchoolStudent for the current account holder was not found.");
            }

            var application = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .Include(a => a.FasScheme)
                .Include(a => a.ApprovedTier)
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.Id == id && a.SchoolStudentId == studentId, cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException("FAS Application not found.");
            }

            var result = new FasApplicationDetailDTO
            {
                Id = application.Id,
                ApplicationNumber = application.ApplicationNumber,
                Status = application.Status,
                CreatedAt = application.CreatedAt,
                WithdrawnAt = application.WithdrawnAt,
                Scheme = new FasSchemeBasicInfoDTO
                {
                    Id = application.FasScheme.Id,
                    SchemeCode = application.FasScheme.SchemeCode,
                    SchemeName = application.FasScheme.SchemeName,
                    Description = application.FasScheme.Description
                },
                StudentAgeSnapshot = application.StudentAgeSnapshot,
                StudentNationalitySnapshot = application.StudentNationalitySnapshot,
                GuardianNationalitySnapshot = application.GuardianNationalitySnapshot,
                GrossHouseholdIncomeSnapshot = application.GrossHouseholdIncomeSnapshot,
                HouseholdMemberCountSnapshot = application.HouseholdMemberCountSnapshot,
                PerCapitaIncomeSnapshot = application.PerCapitaIncomeSnapshot,
                RejectionReason = application.RejectionReason,
                ApprovedAt = application.ApprovedAt,
                ValidityStartDate = application.ValidityStartDate,
                ValidityEndDate = application.ValidityEndDate,
                ApprovedTier = application.ApprovedTier != null ? new FasApplicationTierDetailDTO
                {
                    TierName = application.ApprovedTier.TierName,
                    SubsidyValue = application.ApprovedTier.SubsidyValue,
                    CourseFeeSubsidyValue = application.ApprovedTier.CourseFeeSubsidyValue,
                    MiscFeeSubsidyValue = application.ApprovedTier.MiscFeeSubsidyValue
                } : null,
                Documents = application.Documents.Select(d => new FasApplicationDocumentDetailDTO
                {
                    Id = d.Id,
                    DocumentNameSnapshot = d.DocumentNameSnapshot,
                    FileName = d.FileName,
                    FileKey = d.FileKey
                }).ToList()
            };

            return result;
        }
    }
}
