using DTOs.FasApplications;
using Filters.FasApplications;
using Interfaces.FasApplications;
using Mappers.FasApplications;
using Results;
using System.Linq.Expressions;

namespace Services.FasApplications
{
    public class FasApplicationManagementService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        SchoolScopeResolver schoolScopeResolver,
        FasApplicationMapper mapper) : IFasApplicationManagementService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly FasApplicationMapper _mapper = mapper;
        private readonly IGenericRepository<FasApplication> _fasApplicationRepository = unitOfWork.Repository<FasApplication>();

        public async Task<PaginationResult<GetFasApplicationSchoolAdminDTO>> GetApplicationPaginatedAsync(FasApplicationFilterDTO request, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            // 1. Build combined security + status expression filter
            var statusFilter = BuildStatusFilter(request.Status, now);

            Expression<Func<FasApplication, bool>> combinedFilter = a =>
                a.SchoolStudent.SchoolId == adminSchoolId && statusFilter.Compile().Invoke(a);

            // Use a composable approach for EF translation
            combinedFilter = CombineFilters(
                a => a.SchoolStudent.SchoolId == adminSchoolId,
                statusFilter);

            // 3. Define search fields
            string[] searchFields = [
                nameof(FasApplication.ApplicationNumber),
                "SchoolStudent.EducationAccount.AccountNumber",
                "SchoolStudent.EducationAccount.Citizen.FullName",
                "FasScheme.SchemeName"
            ];

            // 4. Handle sorting
            string order = string.IsNullOrWhiteSpace(request.SortExpression) ? "CreatedAt asc" : request.SortExpression;

            // 5. Build projection
            var pageSize = Math.Clamp(request.PageSize, 1, 100);
            Func<IQueryable<FasApplication>, IQueryable<GetFasApplicationSchoolAdminDTO>> projectionFunc = query => query.Select(a => new GetFasApplicationSchoolAdminDTO
            {
                Id = a.Id,
                ApplicationNumber = a.ApplicationNumber,
                AccountName = a.SchoolStudent.EducationAccount.Citizen.FullName,
                AccountNumber = a.SchoolStudent.EducationAccount.AccountNumber,
                SchemeName = a.FasScheme.SchemeName,
                SubmittedAt = a.CreatedAt,
                Status = a.Status == FasApplicationStatus.Approved
                         && a.ValidityEndDate != null && a.ValidityEndDate < now
                    ? FasApplicationStatus.Expired
                    : a.Status
            });

            // 6. Execute query using Overload 4 (filterExpr + filterStr + search + searchFields)
            var result = await _fasApplicationRepository.GetProjectedPaginatedAsync<GetFasApplicationSchoolAdminDTO>(
                projection: projectionFunc,
                filterExpr: combinedFilter,
                filterStr: request.Filter,
                search: request.Search,
                searchFields: searchFields,
                order: order,
                page: request.Page,
                pageSize: pageSize,
                includes: null,
                cancellationToken: cancellationToken
            );

            return new PaginationResult<GetFasApplicationSchoolAdminDTO>(result.Count, pageSize, result.Items);
        }

        private static Expression<Func<FasApplication, bool>> BuildStatusFilter(FasApplicationStatus? status, DateTime now)
        {
            if (!status.HasValue)
            {
                return a => true;
            }

            if (status.Value == FasApplicationStatus.Pending)
            {
                return a => a.Status == FasApplicationStatus.Pending;
            }

            if (status.Value == FasApplicationStatus.Approved)
            {
                return a => a.Status == FasApplicationStatus.Approved
                            && (a.ValidityEndDate == null || a.ValidityEndDate >= now);
            }

            if (status.Value == FasApplicationStatus.Expired)
            {
                return a => a.Status == FasApplicationStatus.Approved
                            && a.ValidityEndDate != null && a.ValidityEndDate < now;
            }

            if (status.Value == FasApplicationStatus.Rejected)
            {
                return a => a.Status == FasApplicationStatus.Rejected;
            }

            // Withdrawn
            return a => a.Status == FasApplicationStatus.Withdrawn;
        }

        /// <summary>
        /// Combines two expression filters with AND logic in a way that EF Core can translate to SQL.
        /// </summary>
        private static Expression<Func<FasApplication, bool>> CombineFilters(
            Expression<Func<FasApplication, bool>> first,
            Expression<Func<FasApplication, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(FasApplication), "a");

            var firstBody = Expression.Invoke(first, parameter);
            var secondBody = Expression.Invoke(second, parameter);

            var combined = Expression.AndAlso(firstBody, secondBody);

            return Expression.Lambda<Func<FasApplication, bool>>(combined, parameter);
        }


        public async Task<GetFasApplicationSchoolAdminDetailDTO> GetApplicationDetailsAsync(int applicationId, CancellationToken cancellationToken = default)
        {
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var application = await _fasApplicationRepository.Query()
                .Include(a => a.SchoolStudent)
                .Include(a => a.SchoolStudent.EducationAccount)
                .Include(a => a.SchoolStudent.EducationAccount.Citizen)
                .Include(a => a.RecommendedTier)
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

            // Determine enum status (including Expired)
            var now = DateTime.UtcNow;
            var finalStatus = application.Status;
            if (application.Status == FasApplicationStatus.Approved && application.ValidityEndDate < now)
            {
                finalStatus = FasApplicationStatus.Expired;
            }

            var isPercent = application.FasScheme.SubsidyType == FasSubsidyType.Percent;
            var symbol = isPercent ? "%" : " S$";

            // Map DTO with Mapperly
            var dto = _mapper.MapToDetailDTO(application);
            dto.Status = finalStatus;
            dto.Scheme = new SchemeDetailsDTO
            {
                Id = application.FasSchemeId,
                SchemeName = application.FasScheme.SchemeName,
                Tiers = application.FasScheme.Tiers.Select(_mapper.MapTierToDTO).ToList()
            };

            // Map dynamic/computed fields for Scheme Tiers
            for (int i = 0; i < application.FasScheme.Tiers.Count; i++)
            {
                var t = application.FasScheme.Tiers.ElementAt(i);
                var tierDto = dto.Scheme.Tiers[i];

                tierDto.ConditionText = t.MaxPerCapitaIncome.HasValue ? $"PCI <= {t.MaxPerCapitaIncome.Value:0.00}" : "No limit";
                tierDto.SubsidyDescription = application.FasScheme.IsPerComponent
                    ? $"Course: {t.CourseFeeSubsidyValue:0.00}{symbol}, Misc: {t.MiscFeeSubsidyValue:0.00}{symbol}"
                    : $"{t.SubsidyValue:0.00}{symbol}";
            }

            // Map documents list
            dto.Scheme.RequiredDocuments = application.FasScheme.RequiredDocuments.Select(rd =>
            {
                var attachedDoc = application.Documents.FirstOrDefault(d => d.FasSchemeRequiredDocumentId == rd.Id);
                return new ApplicationDocumentDTO
                {
                    Id = rd.Id,
                    DocumentName = rd.DocumentName,
                    Status = attachedDoc != null ? "attached" : "missing",
                    FileUrl = attachedDoc?.FileKey // returning FileKey as FileUrl
                };
            }).ToList();

            // Set SystemSuggestedTier from RecommendedTier
            if (application.RecommendedTier != null)
            {
                dto.SystemSuggestedTier = new SystemSuggestedTierDTO
                {
                    Id = application.RecommendedTier.Id,
                    TierName = application.RecommendedTier.TierName,
                    Reason = application.RecommendationReason ?? $"Matches {application.RecommendedTier.TierName} bracket."
                };
            }

            return dto;
        }

        public async Task RejectAsync(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {

            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

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

        public async Task ApproveAsync(int id, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

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


    }
}
