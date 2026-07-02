using DTOs.FasApplications;
using Interfaces.Audit;
using Interfaces.FasApplications;
using Mappers.FasApplications;
using Results;
using Services.Base;
using Interfaces.Email;
using Interfaces.Notifications;

namespace Services.FasApplications
{
    public class FasApplicationManagementService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        SchoolScopeResolver schoolScopeResolver,
        FasApplicationMapper mapper,
        IAuditLogWriter auditLogWriter,
        IManagementActionLogService managementActionLogService,
        INotificationWriter notificationWriter,
        IOutboxWriter outboxWriter,
        EmailTemplateBuilder emailTemplateBuilder,
        AppConfiguration configuration) : BaseGetService<FasApplication, GetFasApplicationSchoolAdminDTO>(unitOfWork, mapper), IFasApplicationManagementService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly FasApplicationMapper _mapper = mapper;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IManagementActionLogService _managementActionLogService = managementActionLogService;
        private readonly INotificationWriter _notificationWriter = notificationWriter;
        private readonly IOutboxWriter _outboxWriter = outboxWriter;
        private readonly EmailTemplateBuilder _emailTemplateBuilder = emailTemplateBuilder;
        private readonly AppConfiguration _configuration = configuration;
        private readonly IGenericRepository<FasTierOverrideHistory> _tierOverrideRepository =
            unitOfWork.Repository<FasTierOverrideHistory>();

        public override async Task<PaginationResult<GetFasApplicationSchoolAdminDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);

            var (total, items) = await _repository.GetProjectedPaginatedAsync(
                _mapper.ProjectToGetDTO,
                a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status != FasApplicationStatus.Draft,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                _includes,
                cancellationToken);

            return new PaginationResult<GetFasApplicationSchoolAdminDTO>(total, pageSize, items);
        }

        public async Task<GetFasApplicationSchoolAdminDetailDTO> GetApplicationDetailsAsync(int applicationId, CancellationToken cancellationToken = default)
        {
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var application = await _repository.Query()
                .Include(a => a.SchoolStudent)
                .Include(a => a.SchoolStudent.EducationAccount)
                .Include(a => a.SchoolStudent.EducationAccount.Citizen)
                .Include(a => a.RecommendedTier)
                .Include(a => a.ApprovedTier)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.Tiers)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.RequiredDocuments)
                .Include(a => a.Documents)
                .Include(a => a.AdditionalQuestionAnswers)
                .Include(a => a.TierOverrideHistories)
                    .ThenInclude(h => h.OldTier)
                .Include(a => a.TierOverrideHistories)
                    .ThenInclude(h => h.NewTier)
                .Include(a => a.TierOverrideHistories)
                    .ThenInclude(h => h.ModifiedByUser)
                        .ThenInclude(u => u.AdminProfile)
                .FirstOrDefaultAsync(
                    a => a.Id == applicationId &&
                        a.SchoolStudent.SchoolId == adminSchoolId &&
                        a.Status != FasApplicationStatus.Draft,
                    cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), applicationId);
            }

            return _mapper.MapToDetailDTO(application);
        }

        public async Task RejectAsync(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var errors = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(dto.ExternalRejectionReason))
            {
                errors[nameof(dto.ExternalRejectionReason)] = "External rejection reason is required.";
            }

            if (string.IsNullOrWhiteSpace(dto.InternalRejectionReason))
            {
                errors[nameof(dto.InternalRejectionReason)] = "Internal rejection reason is required.";
            }

            if (errors.Count > 0)
            {
                throw new ValidationFailureException(errors);
            }

            var application = await _repository
                .Query()
                .Include(a => a.FasScheme)
                .Include(a => a.SchoolStudent)
                    .ThenInclude(student => student.EducationAccount)
                        .ThenInclude(account => account.Citizen)
                            .ThenInclude(citizen => citizen.User)
                .FirstOrDefaultAsync(
                    a => a.Id == id &&
                        a.SchoolStudent.SchoolId == schoolId &&
                        a.FasScheme.SchoolId == schoolId,
                    cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (application.Status != FasApplicationStatus.Pending)
            {
                throw new DataConflictException("Only pending applications can be rejected.");
            }

            application.Status = FasApplicationStatus.Rejected;
            application.ExternalRejectionReason = dto.ExternalRejectionReason;
            application.InternalRejectionReason = dto.InternalRejectionReason;

            application.TryValidate();

            var recipientUserId = application.SchoolStudent.EducationAccount.Citizen.User?.Id;
            if (recipientUserId.HasValue)
            {
                await _notificationWriter.CreateAsync(
                    recipientUserId.Value,
                    NotificationType.FasApplicationRejected,
                    NotificationSeverity.Warning,
                    "FAS application rejected",
                    $"Your FAS application {application.ApplicationNumber} was rejected. Reason: {application.ExternalRejectionReason}",
                    nameof(FasApplication),
                    application.Id,
                    new
                    {
                        application.ApplicationNumber,
                        application.ExternalRejectionReason
                    },
                    cancellationToken);
            }

            var recipientEmail = application.SchoolStudent.EducationAccount.Citizen.Email;
            if (!string.IsNullOrWhiteSpace(recipientEmail))
            {
                var template = _emailTemplateBuilder.BuildFasApplicationRejectedEmail(
                    application.SchoolStudent.EducationAccount.Citizen.FullName,
                    application.FasScheme.SchemeName,
                    application.ApplicationNumber,
                    application.ExternalRejectionReason,
                    BuildAccountHolderPortalLink("/account-holder/fas/management"));

                await _outboxWriter.EnqueueEmailAsync(recipientEmail, template, cancellationToken);
            }

            _repository.Update(application);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task ApproveAsync(
            int id,
            ApproveFasApplicationDTO dto,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var application = await _repository
                    .Query(tracking: true)
                    .Include(a => a.FasScheme)
                        .ThenInclude(s => s.Tiers)
                    .Include(a => a.SchoolStudent)
                        .ThenInclude(student => student.EducationAccount)
                            .ThenInclude(account => account.Citizen)
                                .ThenInclude(citizen => citizen.User)
                    .Include(a => a.RecommendedTier)
                    .FirstOrDefaultAsync(
                        a => a.Id == id &&
                            a.SchoolStudent.SchoolId == schoolId &&
                            a.FasScheme.SchoolId == schoolId,
                        token);

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

                var selectedTierId = dto.ApprovedTierId ?? application.RecommendedTierId.Value;
                var selectedTier = application.FasScheme.Tiers.FirstOrDefault(t => t.Id == selectedTierId);
                if (selectedTier == null)
                {
                    throw new ValidationFailureException(nameof(dto.ApprovedTierId), "Selected tier does not belong to this FAS scheme.");
                }

                var isOverride = selectedTier.Id != application.RecommendedTierId.Value;
                var reason = dto.Reason?.Trim() ?? string.Empty;
                if (isOverride)
                {
                    ValidateOverrideReason(reason);
                }

                var currentUserId = _currentUserService.UserId;
                var now = DateTime.UtcNow;
                var oldStatus = application.Status;

                application.Status = FasApplicationStatus.Approved;
                application.ApprovedAt = now;
                application.ApprovedByUserId = currentUserId;
                application.ApprovedTierId = selectedTier.Id;
                application.DurationInMonthsSnapshot = application.FasScheme.DurationInMonths;
                application.ValidityStartDate = now.Date;
                application.ValidityEndDate = application.FasScheme.DurationInMonths > 0
                    ? application.ValidityStartDate.Value.AddMonths(application.FasScheme.DurationInMonths)
                    : null;

                if (isOverride)
                {
                    var history = new FasTierOverrideHistory
                    {
                        FasApplicationId = application.Id,
                        OldTierId = application.RecommendedTierId,
                        NewTierId = selectedTier.Id,
                        ModifiedByUserId = currentUserId,
                        ModifiedAt = now,
                        Reason = reason
                    };
                    history.TryValidate();
                    await _tierOverrideRepository.AddAsync(history, token);
                }

                application.TryValidate();
                _repository.Update(application);

                var batchId = Guid.NewGuid();
                await _managementActionLogService.LogAsync(
                    batchId,
                    ManagementActionEntityType.FasApplication,
                    application.Id,
                    isOverride ? ManagementAction.Override : ManagementAction.Approve,
                    isOverride
                        ? BuildOverrideAuditReason(
                            reason,
                            application.RecommendationReason,
                            application.RecommendedTier?.TierName,
                            selectedTier.TierName)
                        : $"Approved system-recommended tier {selectedTier.TierName}.",
                    oldStatus.ToString(),
                    application.Status.ToString(),
                    cancellationToken: token);

                await _auditLogWriter.LogAsync(
                    AuditLogCategory.StatusChange,
                    isOverride
                        ? $"OverrideFasApplicationTier: ApplicationId {application.Id}, OldTierId {application.RecommendedTierId}, NewTierId {selectedTier.Id}"
                        : $"ApproveFasApplication: ApplicationId {application.Id}, TierId {selectedTier.Id}",
                    cancellationToken: token);

                var recipientUserId = application.SchoolStudent.EducationAccount.Citizen.User?.Id;
                if (recipientUserId.HasValue)
                {
                    await _notificationWriter.CreateAsync(
                        recipientUserId.Value,
                        NotificationType.FasApplicationApproved,
                        NotificationSeverity.Success,
                        "FAS application approved",
                        $"Your FAS application {application.ApplicationNumber} has been approved under {selectedTier.TierName}.",
                        nameof(FasApplication),
                        application.Id,
                        new
                        {
                            application.ApplicationNumber,
                            approvedTierId = selectedTier.Id,
                            selectedTier.TierName,
                            application.ValidityStartDate,
                            application.ValidityEndDate
                        },
                        token);
                }

                var recipientEmail = application.SchoolStudent.EducationAccount.Citizen.Email;
                if (!string.IsNullOrWhiteSpace(recipientEmail))
                {
                    var template = _emailTemplateBuilder.BuildFasApplicationApprovedEmail(
                        application.SchoolStudent.EducationAccount.Citizen.FullName,
                        application.FasScheme.SchemeName,
                        application.ApplicationNumber,
                        selectedTier.TierName,
                        FormatApprovedSubsidy(selectedTier),
                        application.ValidityStartDate,
                        application.ValidityEndDate,
                        BuildAccountHolderPortalLink("/account-holder/fas/management"));

                    await _outboxWriter.EnqueueEmailAsync(recipientEmail, template, token);
                }

                await _unitOfWork.SaveChangeAsync(token);
            }, cancellationToken);
        }

        public override async Task<List<GetFasApplicationSchoolAdminDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                a => a.SchoolStudent.SchoolId == schoolId && a.Status != FasApplicationStatus.Draft,
                _includes,
                cancellationToken);
        }

        public override async Task<List<GetFasApplicationSchoolAdminDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                a => ids.Contains(a.Id) &&
                    a.SchoolStudent.SchoolId == schoolId &&
                    a.Status != FasApplicationStatus.Draft,
                _includes,
                cancellationToken);
        }

        public override async Task<GetFasApplicationSchoolAdminDTO> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.FirstOrDefaultProjectedAsync(
                _mapper.ProjectToGetDTO,
                a => a.Id == id &&
                    a.SchoolStudent.SchoolId == schoolId &&
                    a.Status != FasApplicationStatus.Draft,
                _includes,
                cancellationToken)
                ?? throw new DataNotFoundException(typeof(FasApplication), id);
        }

        private static void ValidateOverrideReason(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ValidationFailureException(nameof(ApproveFasApplicationDTO.Reason), "Override reason is required.");
            }

            if (reason.Length < 10)
            {
                throw new ValidationFailureException(nameof(ApproveFasApplicationDTO.Reason), "Override reason must be at least 10 characters.");
            }

            if (reason.Length > 500)
            {
                throw new ValidationFailureException(nameof(ApproveFasApplicationDTO.Reason), "Override reason must be 500 characters or fewer.");
            }
        }

        private static string BuildOverrideAuditReason(
            string overrideReason,
            string? recommendationReason,
            string? oldTierName,
            string newTierName)
        {
            var reason = $"Override reason: {overrideReason}; Recommendation reason: {recommendationReason ?? "N/A"}; Tier: {oldTierName ?? "N/A"} -> {newTierName}";
            return reason.Length <= 500 ? reason : reason[..500];
        }

        private static string FormatApprovedSubsidy(FasSchemeTier tier)
        {
            static string FormatValue(FasSubsidyType type, decimal? value)
            {
                if (!value.HasValue)
                {
                    return "N/A";
                }

                return type == FasSubsidyType.Percent
                    ? $"{value.Value:N2}%"
                    : $"SGD {value.Value:N2}";
            }

            if (!tier.IsPerComponent)
            {
                return FormatValue(tier.SubsidyType, tier.SubsidyValue);
            }

            return $"Course Fee: {FormatValue(tier.SubsidyType, tier.CourseFeeSubsidyValue)}, Misc Fee: {FormatValue(tier.SubsidyType, tier.MiscFeeSubsidyValue)}";
        }

        private string BuildAccountHolderPortalLink(string path)
        {
            var frontendUrl = _configuration.UrlsConfig?.FrontendUrl?.Trim();
            if (string.IsNullOrWhiteSpace(frontendUrl))
            {
                return "#";
            }

            return $"{frontendUrl.TrimEnd('/')}{path}";
        }
    }
}
