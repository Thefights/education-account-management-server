using DTOs.FasApplications;
using Enums;
using Interfaces.FasApplications;
using Mappers.FasApplications;
using System.Linq.Expressions;
using Exceptions;

using Utils;
using Results;

namespace Services.FasApplications
{
    public class ManagementFasApplicationService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        FasApplicationMapper mapper,
        SchoolScopeResolver schoolScopeResolver) : IManagementFasApplicationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly FasApplicationMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly IGenericRepository<FasApplication> _fasApplicationRepository = unitOfWork.Repository<FasApplication>();

        public async Task<PaginationResult<FasApplicationItemDTO>> GetApplicationQueueAsync(GetFasApplicationListRequestDTO request, int adminSchoolId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow.AddHours(8);

            // 1. Build Base Filter for Admin's School
            Expression<Func<FasApplication, bool>> baseFilter = a => a.SchoolStudent.SchoolId == adminSchoolId;

            // 2. Status Filter based on Request
            var requestedStatus = request.Status?.ToLower();
            Expression<Func<FasApplication, bool>> dbFilter = requestedStatus switch
            {
                "approved" => a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Approved && (a.ValidityEndDate == null || a.ValidityEndDate >= now),
                "expired" => a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Approved && a.ValidityEndDate < now,
                "rejected" => a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Rejected,
                "pending" => a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Pending,
                _ => a => a.SchoolStudent.SchoolId == adminSchoolId
            };

            // 3. Search Filter
            string[] searchFields = [nameof(FasApplication.ApplicationNumber), "SchoolStudent.EducationAccount.AccountNumber", "SchoolStudent.EducationAccount.Citizen.FullName", "FasScheme.SchemeName"];

            // 4. Sort Order
            string order = string.IsNullOrWhiteSpace(request.Sort) ? "CreatedAt asc" : request.Sort;

            // 5. Pagination & Projection
            var result = await _fasApplicationRepository.GetProjectedPaginatedAsync(
                _mapper.ProjectToListItemDTO,
                dbFilter,
                null,
                request.Search,
                searchFields,
                order,
                request.Page,
                request.PageSize,
                null,
                cancellationToken
            );

            // Override mapped status to properly label 'expired' or just lowercase
            foreach (var item in result.Items)
            {
                if (string.Equals(item.Status, "Approved", StringComparison.OrdinalIgnoreCase) && item.ValidityEndDate < now)
                {
                    item.Status = "expired";
                }
                else
                {
                    item.Status = item.Status.ToLower();
                }
            }

            return new PaginationResult<FasApplicationItemDTO>(result.Count, request.PageSize, result.Items);
        }

        public async Task<FasApplicationDetailDTO> GetApplicationDetailsAsync(int applicationId, int adminSchoolId, CancellationToken cancellationToken = default)
        {
            var repo = _unitOfWork.Repository<FasApplication>();

            var application = await repo.Query()
                .Include(a => a.SchoolStudent)
                .Include(a => a.SchoolStudent.EducationAccount)
                .Include(a => a.SchoolStudent.EducationAccount.Citizen)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.Tiers)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.RequiredDocuments)
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.Id == applicationId && a.SchoolStudent.SchoolId == adminSchoolId, cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException($"Application {applicationId} not found.");
            }

            // Determine status string (including 'expired')
            var now = DateTime.UtcNow;
            var statusStr = application.Status.ToString().ToLower();
            if (application.Status == FasApplicationStatus.Approved && application.ValidityEndDate < now)
            {
                statusStr = "expired";
            }

            var isPercent = application.FasScheme.SubsidyType == FasSubsidyType.Percent;
            var symbol = isPercent ? "%" : " S$";

            // Map DTO
            var dto = new FasApplicationDetailDTO
            {
                ApplicationId = application.ApplicationNumber,
                Status = statusStr,
                StudentProfile = new StudentProfileDTO
                {
                    Age = application.StudentAgeSnapshot,
                    StudentNationality = application.StudentNationalitySnapshot,
                    GuardianNationality = application.GuardianNationalitySnapshot,
                    GrossHouseholdIncome = application.GrossHouseholdIncomeSnapshot,
                    HouseholdMembers = application.HouseholdMemberCountSnapshot,
                    PerCapitaIncome = application.PerCapitaIncomeSnapshot
                },
                Scheme = new SchemeDetailsDTO
                {
                    Id = application.FasSchemeId,
                    SchemeName = application.FasScheme.SchemeName,
                    Tiers = application.FasScheme.Tiers.Select(t => new TierDetailsDTO
                    {
                        Id = t.Id,
                        TierName = t.TierName,
                        ConditionText = t.MaxPerCapitaIncome.HasValue ? $"PCI <= {t.MaxPerCapitaIncome.Value:0.00}" : "No limit",
                        SubsidyDescription = application.FasScheme.IsPerComponent
                            ? $"Course: {t.CourseFeeSubsidyValue:0.00}{symbol}, Misc: {t.MiscFeeSubsidyValue:0.00}{symbol}"
                            : $"{t.SubsidyValue:0.00}{symbol}",
                        MaxPerCapitaIncome = t.MaxPerCapitaIncome
                    }).ToList(),
                    RequiredDocuments = application.FasScheme.RequiredDocuments.Select(rd =>
                    {
                        var attachedDoc = application.Documents.FirstOrDefault(d => d.FasSchemeRequiredDocumentId == rd.Id);
                        return new ApplicationDocumentDTO
                        {
                            Id = rd.Id,
                            DocumentName = rd.DocumentName,
                            Status = attachedDoc != null ? "attached" : "missing",
                            FileUrl = attachedDoc?.FileKey // returning FileKey as FileUrl
                        };
                    }).ToList()
                }
            };

            // Calculate Recommended Tier
            //citizen-only -> Tier 1 if Citizen": For a citizenship grant scheme, the MaxPerCapitaIncome field on the tier in the database is set to null
            var eligibleTier = application.FasScheme.Tiers
                .Where(t => t.MaxPerCapitaIncome == null || application.PerCapitaIncomeSnapshot <= t.MaxPerCapitaIncome)
                .OrderBy(t => t.MaxPerCapitaIncome ?? decimal.MaxValue)
                .FirstOrDefault();

            if (eligibleTier != null)
            {
                dto.SystemSuggestedTier = new SystemSuggestedTierDTO
                {
                    Id = eligibleTier.Id,
                    TierName = eligibleTier.TierName,
                    Reason = $"Calculated PCI (${application.PerCapitaIncomeSnapshot:0.00}) matches {eligibleTier.TierName} bracket."
                };
            }

            return dto;
        }

        public async Task ApproveAsync(int schoolId, int id, CancellationToken cancellationToken = default)
        {

            var application = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .Include(a => a.FasScheme)
                .Include(a => a.SchoolStudent)
                .FirstOrDefaultAsync(a => a.Id == id && a.FasScheme.SchoolId == schoolId, cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (application.Status != FasApplicationStatus.Pending)
            {
                throw new DataConflictException("Only pending applications can be approved.");
            }

            if (!application.RecommendedTierId.HasValue)
            {
                throw new DataConflictException("Cannot approve application because no tier is eligible.");
            }

            var currentUserId = _currentUserService.UserId;
            var now = DateTime.UtcNow;

            application.Status = FasApplicationStatus.Approved;
            application.ApprovedAt = now;
            application.ApprovedByUserId = currentUserId;
            application.ApprovedTierId = application.RecommendedTierId;
            application.DurationInMonthsSnapshot = application.FasScheme.DurationInMonths;
            application.ValidityStartDate = now.Date;
            application.ValidityEndDate = application.FasScheme.DurationInMonths > 0
                ? application.ValidityStartDate.Value.AddMonths(application.FasScheme.DurationInMonths)
                : null;

            application.TryValidate();

            _unitOfWork.Repository<FasApplication>().Update(application);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task RejectAsync(int schoolId, int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {

            var application = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .Include(a => a.FasScheme)
                .FirstOrDefaultAsync(a => a.Id == id && a.FasScheme.SchoolId == schoolId, cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (application.Status != FasApplicationStatus.Pending)
            {
                throw new DataConflictException("Only pending applications can be rejected.");
            }

            if (string.IsNullOrWhiteSpace(dto.RejectionReason))
            {
                throw new DataConflictException("Rejection reason is required.");
            }

            application.Status = FasApplicationStatus.Rejected;
            application.RejectionReason = dto.RejectionReason;

            application.TryValidate();

            _unitOfWork.Repository<FasApplication>().Update(application);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task<FasApplicationDetailDTO> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await GetApplicationDetailsAsync(id, adminSchoolId, cancellationToken);
        }

        public Task<PaginationResult<FasApplicationDetailDTO>> GetAllPaginatedAsync(Filters.Base.FilterDTO filterDTO, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<FasApplicationDetailDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<FasApplicationDetailDTO>> GetAllByIdsAsync(List<int> ids, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}