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

        private sealed record AccountHolderStudentInfo(int Id, int SchoolId, bool IsSingaporean, DateOnly DateOfBirth);

        public async Task<string> SubmitApplicationAsync(SubmitFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {
            var studentInfo = await GetCurrentStudentInfoAsync(cancellationToken);
            var scheme = await GetActiveSchemeAsync(dto.FasSchemeId, studentInfo.SchoolId, cancellationToken);

            await EnsureNoActiveApplicationAsync(studentInfo.Id, dto.FasSchemeId, null, cancellationToken);

            var applicationNumber = await GenerateApplicationNumberAsync(cancellationToken);

            var application = new FasApplication
            {
                FasSchemeId = dto.FasSchemeId,
                SchoolStudentId = studentInfo.Id,
                ApplicationNumber = applicationNumber,
                Status = FasApplicationStatus.Pending
            };

            ApplySubmission(application, dto, studentInfo, scheme, FasApplicationStatus.Pending);
            application.TryValidate();

            await _unitOfWork.Repository<FasApplication>().AddAsync(application, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return application.ApplicationNumber;
        }

        public async Task<int> SaveDraftApplicationAsync(SubmitFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {
            var studentInfo = await GetCurrentStudentInfoAsync(cancellationToken);
            var scheme = await GetActiveSchemeAsync(dto.FasSchemeId, studentInfo.SchoolId, cancellationToken);

            await EnsureNoActiveApplicationAsync(studentInfo.Id, dto.FasSchemeId, null, cancellationToken);

            var applicationNumber = await GenerateApplicationNumberAsync(cancellationToken);

            var application = new FasApplication
            {
                FasSchemeId = dto.FasSchemeId,
                SchoolStudentId = studentInfo.Id,
                ApplicationNumber = applicationNumber,
                Status = FasApplicationStatus.Draft
            };

            ApplySubmission(application, dto, studentInfo, scheme, FasApplicationStatus.Draft);
            application.TryValidate();

            await _unitOfWork.Repository<FasApplication>().AddAsync(application, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return application.Id;
        }

        public async Task UpdateDraftApplicationAsync(int id, SubmitFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {
            var studentInfo = await GetCurrentStudentInfoAsync(cancellationToken);

            var draft = await _unitOfWork.Repository<FasApplication>()
                .Query(tracking: true)
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.Id == id && a.SchoolStudentId == studentInfo.Id, cancellationToken);

            if (draft == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (draft.Status != FasApplicationStatus.Draft)
            {
                throw new DataConflictException("Only draft applications can be updated.");
            }

            if (dto.FasSchemeId != draft.FasSchemeId)
            {
                throw new DataConflictException("Draft application scheme does not match the submitted scheme.");
            }

            var scheme = await GetActiveSchemeAsync(dto.FasSchemeId, studentInfo.SchoolId, cancellationToken);

            ApplySubmission(draft, dto, studentInfo, scheme, FasApplicationStatus.Draft);
            draft.TryValidate();

            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task DeleteDraftApplicationAsync(int id, CancellationToken cancellationToken = default)
        {
            var studentInfo = await GetCurrentStudentInfoAsync(cancellationToken);

            var draft = await _unitOfWork.Repository<FasApplication>()
                .Query(tracking: true)
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.Id == id && a.SchoolStudentId == studentInfo.Id, cancellationToken);

            if (draft == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (draft.Status != FasApplicationStatus.Draft)
            {
                throw new DataConflictException("Only draft applications can be deleted.");
            }

            _unitOfWork.Repository<FasApplication>().Remove(draft);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task<int> CreateReapplyDraftAsync(int sourceApplicationId, CancellationToken cancellationToken = default)
        {
            var studentInfo = await GetCurrentStudentInfoAsync(cancellationToken);
            var today = DateTime.UtcNow.Date;

            var sourceApplication = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.Id == sourceApplicationId && a.SchoolStudentId == studentInfo.Id, cancellationToken);

            if (sourceApplication == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), sourceApplicationId);
            }

            var canReapply =
                sourceApplication.Status == FasApplicationStatus.Rejected ||
                sourceApplication.Status == FasApplicationStatus.Expired ||
                (sourceApplication.Status == FasApplicationStatus.Approved &&
                 sourceApplication.ValidityEndDate.HasValue &&
                 sourceApplication.ValidityEndDate.Value.Date < today);

            if (!canReapply)
            {
                throw new DataConflictException("Only rejected or expired applications can be used for reapply.");
            }

            var existingDraft = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .FirstOrDefaultAsync(a => a.SchoolStudentId == studentInfo.Id
                                          && a.FasSchemeId == sourceApplication.FasSchemeId
                                          && a.Status == FasApplicationStatus.Draft,
                    cancellationToken);

            if (existingDraft != null)
            {
                return existingDraft.Id;
            }

            await EnsureNoActiveApplicationAsync(studentInfo.Id, sourceApplication.FasSchemeId, null, cancellationToken);

            var applicationNumber = await GenerateApplicationNumberAsync(cancellationToken);

            var draft = new FasApplication
            {
                FasSchemeId = sourceApplication.FasSchemeId,
                SchoolStudentId = studentInfo.Id,
                ApplicationNumber = applicationNumber,
                Status = FasApplicationStatus.Draft,
                StudentAgeSnapshot = sourceApplication.StudentAgeSnapshot,
                StudentNationalitySnapshot = sourceApplication.StudentNationalitySnapshot,
                GuardianNationalitySnapshot = sourceApplication.GuardianNationalitySnapshot,
                GrossHouseholdIncomeSnapshot = sourceApplication.GrossHouseholdIncomeSnapshot,
                HouseholdMemberCountSnapshot = sourceApplication.HouseholdMemberCountSnapshot,
                PerCapitaIncomeSnapshot = sourceApplication.PerCapitaIncomeSnapshot,
                RecommendedTierId = sourceApplication.RecommendedTierId,
                RecommendationReason = sourceApplication.RecommendationReason,
                Documents = CloneDocuments(sourceApplication.Documents, -1)
            };

            draft.TryValidate();

            await _unitOfWork.Repository<FasApplication>().AddAsync(draft, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return draft.Id;
        }

        public async Task<string> PublishDraftApplicationAsync(int id, SubmitFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {
            var studentInfo = await GetCurrentStudentInfoAsync(cancellationToken);

            var draft = await _unitOfWork.Repository<FasApplication>()
                .Query(tracking: true)
                .Include(a => a.Documents)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.Tiers)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.RequiredDocuments)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.ConditionGroups)
                        .ThenInclude(cg => cg.Conditions)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.ConditionGroups)
                        .ThenInclude(cg => cg.ChildGroups)
                            .ThenInclude(child => child.Conditions)
                .FirstOrDefaultAsync(a => a.Id == id && a.SchoolStudentId == studentInfo.Id, cancellationToken);

            if (draft == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (draft.Status != FasApplicationStatus.Draft)
            {
                throw new DataConflictException("Only draft applications can be submitted.");
            }

            if (dto.FasSchemeId != draft.FasSchemeId)
            {
                throw new DataConflictException("Draft application scheme does not match the submitted scheme.");
            }

            await EnsureNoActiveApplicationAsync(studentInfo.Id, draft.FasSchemeId, draft.Id, cancellationToken);

            ApplySubmission(draft, dto, studentInfo, draft.FasScheme, FasApplicationStatus.Pending);
            draft.CreatedAt = DateTime.UtcNow;
            draft.TryValidate();

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return draft.ApplicationNumber;
        }

        public async Task<PaginationResult<FasApplicationSummaryDTO>> GetMyApplicationsAsync(FasApplicationFilterDTO filter, CancellationToken cancellationToken = default)
        {
            var studentInfo = await GetCurrentStudentInfoAsync(cancellationToken);
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);

            var (total, items) = await _unitOfWork.Repository<FasApplication>().GetProjectedPaginatedAsync(
                query => query.Select(a => new FasApplicationSummaryDTO
                {
                    Id = a.Id,
                    ApplicationNumber = a.ApplicationNumber,
                    SchemeId = a.FasSchemeId,
                    SchemeName = a.FasScheme.SchemeName,
                    Status = a.Status,
                    SubmittedAt = a.CreatedAt,
                    ApprovedDate = a.ApprovedAt,
                    ValidityEndDate = a.ValidityEndDate,
                    RejectionReason = a.RejectionReason
                }),
                a => a.SchoolStudentId == studentInfo.Id && (filter.Status.HasValue || a.Status != FasApplicationStatus.Draft),
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
            var studentInfo = await GetCurrentStudentInfoAsync(cancellationToken);

            var application = await _unitOfWork.Repository<FasApplication>()
                .Query(tracking: true)
                .FirstOrDefaultAsync(a => a.Id == id && a.SchoolStudentId == studentInfo.Id, cancellationToken);

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
            var studentInfo = await GetCurrentStudentInfoAsync(cancellationToken);

            var application = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .Include(a => a.FasScheme)
                .Include(a => a.ApprovedTier)
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.Id == id && a.SchoolStudentId == studentInfo.Id, cancellationToken);

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
                    RequiredDocumentId = d.FasSchemeRequiredDocumentId,
                    DocumentNameSnapshot = d.DocumentNameSnapshot,
                    FileName = d.FileName,
                    FileKey = d.FileKey
                }).ToList()
            };

            return result;
        }


        private async Task<AccountHolderStudentInfo> GetCurrentStudentInfoAsync(CancellationToken cancellationToken)
        {
            var currentAccountHolderId = _currentUserService.UserId;

            var studentInfo = await _unitOfWork.Repository<SchoolStudent>()
                .Query()
                .Where(student => student.EducationAccount.Citizen.User != null
                    && student.EducationAccount.Citizen.User.Id == currentAccountHolderId)
                .Select(student => new AccountHolderStudentInfo(
                    student.Id,
                    student.SchoolId,
                    student.EducationAccount.Citizen.IsSingaporean,
                    student.EducationAccount.Citizen.DateOfBirth))
                .SingleOrDefaultAsync(cancellationToken);

            return studentInfo ?? throw new DataNotFoundException("SchoolStudent for the current account holder was not found.");
        }

        private async Task<FasScheme> GetActiveSchemeAsync(int schemeId, int schoolId, CancellationToken cancellationToken)
        {
            var scheme = await _unitOfWork.Repository<FasScheme>()
                .Query()
                .Include(s => s.Tiers)
                .Include(s => s.RequiredDocuments)
                .Include(s => s.ConditionGroups)
                    .ThenInclude(cg => cg.Conditions)
                .Include(s => s.ConditionGroups)
                    .ThenInclude(cg => cg.ChildGroups)
                        .ThenInclude(child => child.Conditions)
                .FirstOrDefaultAsync(s => s.Id == schemeId
                    && s.Status == FasSchemeStatus.Active
                    && s.SchoolId == schoolId,
                    cancellationToken);

            return scheme ?? throw new DataNotFoundException(typeof(FasScheme), schemeId);
        }

        private async Task EnsureNoActiveApplicationAsync(
            int studentId,
            int schemeId,
            int? excludeApplicationId,
            CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;
            var exists = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .AnyAsync(a => a.SchoolStudentId == studentId
                    && a.FasSchemeId == schemeId
                    && (!excludeApplicationId.HasValue || a.Id != excludeApplicationId.Value)
                    && (a.Status == FasApplicationStatus.Pending
                        || a.Status == FasApplicationStatus.Draft
                        || (a.Status == FasApplicationStatus.Approved
                            && (a.ValidityEndDate == null || a.ValidityEndDate >= today))),
                    cancellationToken);

            if (exists)
            {
                throw new DataConflictException("You already have an active (draft, pending, or approved) application for this scheme.");
            }
        }

        private void ApplySubmission(
            FasApplication application,
            SubmitFasApplicationDTO dto,
            AccountHolderStudentInfo studentInfo,
            FasScheme scheme,
            FasApplicationStatus status)
        {
            var studentAge = GetStudentAge(studentInfo.DateOfBirth);
            var pci = dto.HouseholdMemberCount > 0 ? dto.GrossHouseholdIncome / dto.HouseholdMemberCount : 0;
            var (recommendedTierId, recommendationReason) = GetRecommendation(dto, studentInfo, scheme, studentAge, pci);

            RemoveDocuments(application.Documents);

            application.FasSchemeId = dto.FasSchemeId;
            application.Status = status;
            application.StudentAgeSnapshot = studentAge;
            application.StudentNationalitySnapshot = studentInfo.IsSingaporean
                ? NationalityCategory.SingaporeCitizen
                : NationalityCategory.Other;
            application.GuardianNationalitySnapshot = dto.GuardianNationality;
            application.GrossHouseholdIncomeSnapshot = dto.GrossHouseholdIncome;
            application.HouseholdMemberCountSnapshot = dto.HouseholdMemberCount;
            application.PerCapitaIncomeSnapshot = pci;
            application.RecommendedTierId = recommendedTierId;
            application.ApprovedTierId = null;
            application.RecommendationReason = recommendationReason;
            application.RejectionReason = null;
            application.ApprovedAt = null;
            application.ApprovedByUserId = null;
            application.DurationInMonthsSnapshot = null;
            application.ValidityStartDate = null;
            application.ValidityEndDate = null;
            application.WithdrawnAt = null;
            application.Documents = BuildDocuments(dto.Documents, scheme, application.Id == 0 ? -1 : application.Id);
        }

        private (int? RecommendedTierId, string RecommendationReason) GetRecommendation(
            SubmitFasApplicationDTO dto,
            AccountHolderStudentInfo studentInfo,
            FasScheme scheme,
            int studentAge,
            decimal pci)
        {
            var recommendationReason = "Failed scheme baseline conditions";

            var isEligible = FasConditionEvaluator.Evaluate(
                scheme.ConditionGroups,
                studentAge,
                studentInfo.IsSingaporean,
                dto.GuardianNationality,
                dto.GrossHouseholdIncome,
                dto.HouseholdMemberCount);

            if (!isEligible)
            {
                return (null, recommendationReason);
            }

            var eligibleTier = scheme.Tiers
                .Where(t => !t.MaxPerCapitaIncome.HasValue || pci <= t.MaxPerCapitaIncome)
                .OrderBy(t => t.MaxPerCapitaIncome ?? decimal.MaxValue)
                .FirstOrDefault();

            if (eligibleTier == null)
            {
                return (null, "Eligible for scheme but exceeded all tier PCI limits");
            }

            recommendationReason = eligibleTier.MaxPerCapitaIncome.HasValue
                ? $"PCI <= {eligibleTier.MaxPerCapitaIncome}"
                : "Matched tier with no PCI limit";

            return (eligibleTier.Id, recommendationReason);
        }

        private static int GetStudentAge(DateOnly dateOfBirth)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateOnly.FromDateTime(today.AddYears(-age))) age--;
            return age;
        }

        private static List<FasApplicationDocument> BuildDocuments(
            IEnumerable<SubmitFasApplicationDocumentDTO> documents,
            FasScheme scheme,
            int applicationId)
        {
            var result = new List<FasApplicationDocument>();
            foreach (var document in documents)
            {
                var requiredDocument = scheme.RequiredDocuments.FirstOrDefault(r => r.Id == document.RequiredDocumentId);
                if (requiredDocument != null)
                {
                    result.Add(new FasApplicationDocument
                    {
                        FasApplicationId = applicationId,
                        FasSchemeRequiredDocumentId = document.RequiredDocumentId,
                        FileKey = document.FileKey,
                        FileName = document.FileName,
                        DocumentNameSnapshot = requiredDocument.DocumentName
                    });
                }
            }
            return result;
        }

        private static List<FasApplicationDocument> CloneDocuments(
            IEnumerable<FasApplicationDocument> documents,
            int applicationId)
        {
            return documents.Select(document => new FasApplicationDocument
            {
                FasApplicationId = applicationId,
                FasSchemeRequiredDocumentId = document.FasSchemeRequiredDocumentId,
                FileKey = document.FileKey,
                FileName = document.FileName,
                DocumentNameSnapshot = document.DocumentNameSnapshot
            }).ToList();
        }

        private void RemoveDocuments(IEnumerable<FasApplicationDocument> documents)
        {
            var existingDocuments = documents.ToList();
            if (existingDocuments.Count > 0)
            {
                _unitOfWork.Repository<FasApplicationDocument>().RemoveRange(existingDocuments);
            }
        }

        private Task<string> GenerateApplicationNumberAsync(CancellationToken cancellationToken)
        {
            var applicationRepository = _unitOfWork.Repository<FasApplication>();
            return BusinessCodeGenerator.GenerateUniqueAsync(
                BusinessCodeGenerator.FasApplicationPrefix,
                (candidate, token) => applicationRepository.AnyAsync(
                    application => application.ApplicationNumber == candidate,
                    token),
                conflictMessage: "Unable to generate a unique FAS application number.",
                cancellationToken: cancellationToken);
        }
    }
}
