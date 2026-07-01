using DTOs.Base;
using DTOs.FasSchemes;
using Interfaces.Audit;
using Interfaces.Base;
using Interfaces.FasSchemes;
using Interfaces.Storage;
using Mappers.FasSchemes;
using Results;
using Services.Base;
using Validators;
using Helpers.FasSchemes;
using Utils;

namespace Services.FasSchemes
{
    public class FasSchemeService(
        IUnitOfWork unitOfWork,
        FasSchemeMapper mapper,
        SchoolScopeResolver schoolScopeResolver,
        IAuditLogWriter auditLogWriter,
        TimeProvider timeProvider,
        IFileValidator fileValidator,
        IManagementActionLogService managementActionLogService,
        IUploadService uploadService)
        : BaseService<FasScheme, CreateFasSchemeDTO, GetFasSchemeDTO, UpdateFasSchemeDTO>(
            unitOfWork,
            mapper,
            uploadService,
            includes: [nameof(FasScheme.Tiers), nameof(FasScheme.RequiredDocuments), $"{nameof(FasScheme.SchemeCourses)}.{nameof(FasSchemeCourse.Course)}", nameof(FasScheme.AdditionalQuestions)]),
          IFasSchemeService
    {
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly TimeProvider _timeProvider = timeProvider;
        private readonly IFileValidator _fileValidator = fileValidator;
        private readonly IManagementActionLogService _managementActionLogService = managementActionLogService;

        private readonly IGenericRepository<FasSchemeConditionGroup> _groupRepository =
            unitOfWork.Repository<FasSchemeConditionGroup>();
        private readonly IGenericRepository<FasSchemeCondition> _conditionRepository =
            unitOfWork.Repository<FasSchemeCondition>();
        private readonly IGenericRepository<FasSchemeTier> _tierRepository =
            unitOfWork.Repository<FasSchemeTier>();
        private readonly IGenericRepository<FasSchemeRequiredDocument> _requiredDocRepository =
            unitOfWork.Repository<FasSchemeRequiredDocument>();
        private readonly IGenericRepository<FasSchemeCourse> _courseLinkRepository =
            unitOfWork.Repository<FasSchemeCourse>();
        private readonly IGenericRepository<Course> _courseRepository =
            unitOfWork.Repository<Course>();
        private readonly IGenericRepository<FasSchemeAdditionalQuestion> _additionalQuestionRepository =
            unitOfWork.Repository<FasSchemeAdditionalQuestion>();

        public override async Task<GetFasSchemeDTO> CreateAsync(
            CreateFasSchemeDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            ValidateInput(createDTO.SchemeName, createDTO.Tiers, createDTO.RootConditionGroup, createDTO.SubsidyType, createDTO.IsPerComponent);
            FasConditionTreeUtility.Validate(createDTO.RootConditionGroup);
            FasConditionSemanticAnalyzer.Validate(createDTO.RootConditionGroup);
            await ValidateCoursesExistAsync(createDTO.SchemeCourses.Select(c => c.CourseId).ToList(), schoolId, cancellationToken);

            var id = await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                var uploadedTemplateKeys = await UploadDocumentTemplatesAsync(createDTO.RequiredDocuments, token);
                foreach (var uploadedTemplateKey in uploadedTemplateKeys)
                {
                    ImageTransactionHookHelper.RegisterUploadedImageRollback(transaction, _uploadService, uploadedTemplateKey);
                }

                var scheme = _mapper.MapFromCreateDTO(createDTO);
                scheme.SchoolId = schoolId;
                scheme.Status = FasSchemeStatus.Draft;
                scheme.SchemeCode = await BusinessCodeGenerator.GenerateUniqueAsync(
                    BusinessCodeGenerator.FasSchemePrefix,
                    (code, innerToken) => _repository.AnyAsync(s => s.SchemeCode == code, innerToken),
                    conflictMessage: "Unable to generate a unique FAS scheme code.",
                    cancellationToken: token);
                scheme.TryValidate();

                await UniqueConstraintValidator.ValidateAsync(_repository, scheme, cancellationToken: token);
                await _repository.AddAsync(scheme, token);
                await _unitOfWork.SaveChangeAsync(token);

                // Save condition tree
                var root = FasConditionTreeMapper.MapFasGroup(createDTO.RootConditionGroup, scheme.Id);
                await _groupRepository.AddAsync(root, token);

                // Save Tiers
                var tiers = createDTO.Tiers.Select(t => new FasSchemeTier
                {
                    FasSchemeId = scheme.Id,
                    TierName = t.TierName,
                    TierIncomeBasis = t.TierIncomeBasis,
                    MinPerCapitaIncome = t.MinPerCapitaIncome,
                    MaxPerCapitaIncome = t.MaxPerCapitaIncome,
                    MinGrossHouseholdIncome = t.MinGrossHouseholdIncome,
                    MaxGrossHouseholdIncome = t.MaxGrossHouseholdIncome,
                    SubsidyValue = t.SubsidyValue,
                    CourseFeeSubsidyValue = t.CourseFeeSubsidyValue,
                    MiscFeeSubsidyValue = t.MiscFeeSubsidyValue,
                    DisplayOrder = t.DisplayOrder
                }).ToList();
                await _tierRepository.AddRangeAsync(tiers, token);

                // Save Required Documents
                var docs = createDTO.RequiredDocuments.Select(d => new FasSchemeRequiredDocument
                {
                    FasSchemeId = scheme.Id,
                    DocumentName = d.DocumentName,
                    TemplateFileKey = d.TemplateFileKey,
                    DisplayOrder = d.DisplayOrder
                }).ToList();
                await _requiredDocRepository.AddRangeAsync(docs, token);

                // Save Course links
                var links = createDTO.SchemeCourses.Select(c => new FasSchemeCourse
                {
                    FasSchemeId = scheme.Id,
                    CourseId = c.CourseId
                }).ToList();
                await _courseLinkRepository.AddRangeAsync(links, token);

                // Save Additional Questions
                var questions = createDTO.AdditionalQuestions.Select(q => new FasSchemeAdditionalQuestion
                {
                    FasSchemeId = scheme.Id,
                    QuestionText = q.QuestionText,
                    IsRequired = q.IsRequired
                }).ToList();
                await _additionalQuestionRepository.AddRangeAsync(questions, token);

                await _unitOfWork.SaveChangeAsync(token);
                ValidatePersistedTree(root);

                return scheme.Id;
            }, cancellationToken);

            return await GetByIdAsync(id, cancellationToken);
        }

        public override async Task<GetFasSchemeDTO> UpdateAsync(
            int id,
            UpdateFasSchemeDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            ValidateInput(updateDTO.SchemeName, updateDTO.Tiers, updateDTO.RootConditionGroup, updateDTO.SubsidyType, updateDTO.IsPerComponent);
            FasConditionTreeUtility.Validate(updateDTO.RootConditionGroup);
            FasConditionSemanticAnalyzer.Validate(updateDTO.RootConditionGroup);
            await ValidateCoursesExistAsync(updateDTO.SchemeCourses.Select(c => c.CourseId).ToList(), schoolId, cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                var scheme = await GetTrackedScopedSchemeAsync(id, schoolId, token);
                if (scheme.Status != FasSchemeStatus.Draft)
                {
                    throw new ValidationFailureException(nameof(scheme.Status), "Only draft schemes can be edited.");
                }

                var uploadedTemplateKeys = await UploadDocumentTemplatesAsync(updateDTO.RequiredDocuments, token);
                foreach (var uploadedTemplateKey in uploadedTemplateKeys)
                {
                    ImageTransactionHookHelper.RegisterUploadedImageRollback(transaction, _uploadService, uploadedTemplateKey);
                }

                // Clear condition tree
                var groups = await _groupRepository.Query(tracking: true)
                    .Include(g => g.Conditions)
                    .Where(g => g.FasSchemeId == id)
                    .ToListAsync(token);
                _conditionRepository.RemoveRange(groups.SelectMany(g => g.Conditions).ToList());
                _groupRepository.RemoveRange(groups);

                // Clear tiers
                var tiers = await _tierRepository.Query(tracking: true)
                    .Where(t => t.FasSchemeId == id)
                    .ToListAsync(token);
                _tierRepository.RemoveRange(tiers);

                // Clear docs
                var docs = await _requiredDocRepository.Query(tracking: true)
                    .Where(d => d.FasSchemeId == id)
                    .ToListAsync(token);
                var newTemplateKeys = updateDTO.RequiredDocuments
                    .Select(d => d.TemplateFileKey)
                    .Where(key => !string.IsNullOrWhiteSpace(key))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
                foreach (var oldTemplateKey in docs
                    .Select(d => d.TemplateFileKey)
                    .Where(key => !string.IsNullOrWhiteSpace(key) && !newTemplateKeys.Contains(key)))
                {
                    ImageTransactionHookHelper.RegisterImageDeleteAfterCommit(transaction, _uploadService, oldTemplateKey);
                }
                _requiredDocRepository.RemoveRange(docs);

                // Clear links
                var links = await _courseLinkRepository.Query(tracking: true)
                    .Where(l => l.FasSchemeId == id)
                    .ToListAsync(token);
                _courseLinkRepository.RemoveRange(links);

                // Clear additional questions
                var oldQuestions = await _additionalQuestionRepository.Query(tracking: true)
                    .Where(q => q.FasSchemeId == id)
                    .ToListAsync(token);
                _additionalQuestionRepository.RemoveRange(oldQuestions);

                await _unitOfWork.SaveChangeAsync(token);

                _mapper.MapFromUpdateDTO(updateDTO, scheme);
                scheme.TryValidate();

                await UniqueConstraintValidator.ValidateAsync(_repository, scheme, scheme.Id, token);
                _repository.Update(scheme);
                await _unitOfWork.SaveChangeAsync(token);

                var root = FasConditionTreeMapper.MapFasGroup(updateDTO.RootConditionGroup, scheme.Id);
                await _groupRepository.AddAsync(root, token);

                var newTiers = updateDTO.Tiers.Select(t => new FasSchemeTier
                {
                    FasSchemeId = scheme.Id,
                    TierName = t.TierName,
                    TierIncomeBasis = t.TierIncomeBasis,
                    MinPerCapitaIncome = t.MinPerCapitaIncome,
                    MaxPerCapitaIncome = t.MaxPerCapitaIncome,
                    MinGrossHouseholdIncome = t.MinGrossHouseholdIncome,
                    MaxGrossHouseholdIncome = t.MaxGrossHouseholdIncome,
                    SubsidyValue = t.SubsidyValue,
                    CourseFeeSubsidyValue = t.CourseFeeSubsidyValue,
                    MiscFeeSubsidyValue = t.MiscFeeSubsidyValue,
                    DisplayOrder = t.DisplayOrder
                }).ToList();
                await _tierRepository.AddRangeAsync(newTiers, token);

                var newDocs = updateDTO.RequiredDocuments.Select(d => new FasSchemeRequiredDocument
                {
                    FasSchemeId = scheme.Id,
                    DocumentName = d.DocumentName,
                    TemplateFileKey = d.TemplateFileKey,
                    DisplayOrder = d.DisplayOrder
                }).ToList();
                await _requiredDocRepository.AddRangeAsync(newDocs, token);

                var newLinks = updateDTO.SchemeCourses.Select(c => new FasSchemeCourse
                {
                    FasSchemeId = scheme.Id,
                    CourseId = c.CourseId
                }).ToList();
                await _courseLinkRepository.AddRangeAsync(newLinks, token);

                var newQuestions = updateDTO.AdditionalQuestions.Select(q => new FasSchemeAdditionalQuestion
                {
                    FasSchemeId = scheme.Id,
                    QuestionText = q.QuestionText,
                    IsRequired = q.IsRequired
                }).ToList();
                await _additionalQuestionRepository.AddRangeAsync(newQuestions, token);

                await _unitOfWork.SaveChangeAsync(token);
                ValidatePersistedTree(root);
            }, cancellationToken);

            return await GetByIdAsync(id, cancellationToken);
        }

        public async Task UpdateStatusesAsync(
            BatchUpdateFasSchemeStatusDTO dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (dto.Ids.Count == 0) return;
            var batchId = Guid.NewGuid();
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var schemes = await _repository.Query(tracking: true)
                    .Where(s => dto.Ids.Contains(s.Id) && s.SchoolId == schoolId)
                    .ToListAsync(token);

                if (schemes.Count != dto.Ids.Distinct().Count())
                    throw new ValidationFailureException(nameof(dto.Ids), "One or more FAS schemes do not exist or you do not have permission.");

                foreach (var scheme in schemes)
                {
                    var oldStatus = scheme.Status;
                    if (oldStatus == dto.Status) continue;

                    // Transition validations
                    if (dto.Status == FasSchemeStatus.Active)
                    {
                        if (scheme.DurationInMonths <= 0)
                        {
                            throw new ValidationFailureException(nameof(scheme.DurationInMonths), "A positive validity duration (in months) is required before publishing.");
                        }
                        await ValidateSchemeCanActivateAsync(scheme, token);
                    }

                    scheme.Status = dto.Status;
                    if (dto.Status == FasSchemeStatus.Active && oldStatus == FasSchemeStatus.Draft)
                    {
                        scheme.PublishedAt = _timeProvider.GetUtcNow().UtcDateTime;
                    }

                    scheme.TryValidate();
                    _repository.Update(scheme);
                    await _managementActionLogService.LogAsync(
                        batchId,
                        ManagementActionEntityType.FasScheme,
                        scheme.Id,
                        dto.Status == FasSchemeStatus.Active ? ManagementAction.Activate : ManagementAction.Deactivate,
                        dto.Reason,
                        oldStatus.ToString(),
                        scheme.Status.ToString(),
                        cancellationToken: token);

                    // Audit Log
                    string auditAction = dto.Status switch
                    {
                        FasSchemeStatus.Active => "PublishFasScheme",
                        FasSchemeStatus.Inactive => "DeactivateFasScheme",
                        _ => "UpdateStatusFasScheme"
                    };
                    await _auditLogWriter.LogAsync(AuditLogCategory.StatusChange, $"{auditAction}: SchemeId {scheme.Id}", cancellationToken: token);
                }
                await _unitOfWork.SaveChangeAsync(token);
            }, cancellationToken);
        }

        public async Task<GetFasSchemeDTO> DuplicateAsync(int id, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var newSchemeId = await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                // Load source scheme including condition tree
                var source = await _repository.Query(tracking: false)
                    .Include(s => s.Tiers)
                    .Include(s => s.RequiredDocuments)
                    .Include(s => s.SchemeCourses)
                    .Include(s => s.AdditionalQuestions)
                    .FirstOrDefaultAsync(s => s.Id == id && s.SchoolId == schoolId, token)
                    ?? throw new DataNotFoundException(typeof(FasScheme), id);

                var sourceGroups = await _groupRepository.Query(tracking: false)
                    .Include(g => g.Conditions)
                    .Where(g => g.FasSchemeId == id)
                    .ToListAsync(token);

                // Create new scheme
                var nameCopy = $"{source.SchemeName} (copy)";
                if (nameCopy.Length > 150)
                {
                    nameCopy = nameCopy[..143] + " (copy)";
                }

                var newScheme = new FasScheme
                {
                    SchoolId = schoolId,
                    Status = FasSchemeStatus.Draft,
                    SchemeCode = await BusinessCodeGenerator.GenerateUniqueAsync(
                        BusinessCodeGenerator.FasSchemePrefix,
                        (code, innerToken) => _repository.AnyAsync(s => s.SchemeCode == code, innerToken),
                        conflictMessage: "Unable to generate a unique FAS scheme code.",
                        cancellationToken: token),
                    SchemeName = nameCopy,
                    Description = source.Description,
                    DurationInMonths = source.DurationInMonths,
                    SubsidyType = source.SubsidyType,
                    IsPerComponent = source.IsPerComponent
                };
                newScheme.TryValidate();
                await _repository.AddAsync(newScheme, token);
                await _unitOfWork.SaveChangeAsync(token);

                // Duplicate tiers
                var newTiers = source.Tiers.Select(t => new FasSchemeTier
                {
                    FasSchemeId = newScheme.Id,
                    TierName = t.TierName,
                    TierIncomeBasis = t.TierIncomeBasis,
                    MinPerCapitaIncome = t.MinPerCapitaIncome,
                    MaxPerCapitaIncome = t.MaxPerCapitaIncome,
                    MinGrossHouseholdIncome = t.MinGrossHouseholdIncome,
                    MaxGrossHouseholdIncome = t.MaxGrossHouseholdIncome,
                    SubsidyValue = t.SubsidyValue,
                    CourseFeeSubsidyValue = t.CourseFeeSubsidyValue,
                    MiscFeeSubsidyValue = t.MiscFeeSubsidyValue,
                    DisplayOrder = t.DisplayOrder
                }).ToList();
                await _tierRepository.AddRangeAsync(newTiers, token);

                // Duplicate docs
                var newDocs = source.RequiredDocuments.Select(d => new FasSchemeRequiredDocument
                {
                    FasSchemeId = newScheme.Id,
                    DocumentName = d.DocumentName,
                    TemplateFileKey = d.TemplateFileKey,
                    DisplayOrder = d.DisplayOrder
                }).ToList();
                await _requiredDocRepository.AddRangeAsync(newDocs, token);

                // Duplicate links
                var newLinks = source.SchemeCourses.Select(c => new FasSchemeCourse
                {
                    FasSchemeId = newScheme.Id,
                    CourseId = c.CourseId
                }).ToList();
                await _courseLinkRepository.AddRangeAsync(newLinks, token);

                // Duplicate additional questions
                var newQuestions = source.AdditionalQuestions.Select(q => new FasSchemeAdditionalQuestion
                {
                    FasSchemeId = newScheme.Id,
                    QuestionText = q.QuestionText,
                    IsRequired = q.IsRequired
                }).ToList();
                await _additionalQuestionRepository.AddRangeAsync(newQuestions, token);

                // Duplicate condition tree
                var rootGroup = sourceGroups.SingleOrDefault(g => g.ParentGroupId == null);
                if (rootGroup != null)
                {
                    var clonedRoot = CloneGroup(rootGroup, sourceGroups, newScheme.Id);
                    await _groupRepository.AddAsync(clonedRoot, token);
                }

                await _unitOfWork.SaveChangeAsync(token);
                return newScheme.Id;
            }, cancellationToken);

            return await GetByIdAsync(newSchemeId, cancellationToken);
        }

        public override async Task<GetFasSchemeDTO> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var scheme = await _repository.FirstOrDefaultProjectedAsync(
                _mapper.ProjectToGetDTO,
                s => s.Id == id && s.SchoolId == schoolId,
                _includes,
                cancellationToken)
                ?? throw new DataNotFoundException(typeof(FasScheme), id);

            await PopulateTreesAsync([scheme], cancellationToken);
            return scheme;
        }

        public override async Task<List<GetFasSchemeDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var list = await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                s => s.SchoolId == schoolId,
                _includes,
                cancellationToken);

            await PopulateTreesAsync(list, cancellationToken);
            return list;
        }

        public override async Task<PaginationResult<GetFasSchemeDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);

            var (total, items) = await _repository.GetProjectedPaginatedAsync(
                _mapper.ProjectToGetDTO,
                s => s.SchoolId == schoolId,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                _includes,
                cancellationToken: cancellationToken);

            await PopulateTreesAsync(items, cancellationToken);
            return new PaginationResult<GetFasSchemeDTO>(total, pageSize, items);
        }

        public override async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                var scheme = await GetTrackedScopedSchemeAsync(id, schoolId, token);
                if (scheme.Status != FasSchemeStatus.Draft)
                {
                    throw new ValidationFailureException(nameof(scheme.Status), "Only draft schemes can be deleted.");
                }

                var groups = await _groupRepository.Query(tracking: true)
                    .Include(g => g.Conditions)
                    .Where(g => g.FasSchemeId == id)
                    .ToListAsync(token);
                _conditionRepository.RemoveRange(groups.SelectMany(g => g.Conditions).ToList());
                _groupRepository.RemoveRange(groups);

                var Tiers = await _tierRepository.Query(tracking: true)
                    .Where(t => t.FasSchemeId == id)
                    .ToListAsync(token);
                _tierRepository.RemoveRange(Tiers);

                var docs = await _requiredDocRepository.Query(tracking: true)
                    .Where(d => d.FasSchemeId == id)
                    .ToListAsync(token);
                foreach (var templateKey in docs
                    .Select(d => d.TemplateFileKey)
                    .Where(key => !string.IsNullOrWhiteSpace(key)))
                {
                    ImageTransactionHookHelper.RegisterImageDeleteAfterCommit(transaction, _uploadService, templateKey);
                }
                _requiredDocRepository.RemoveRange(docs);

                var links = await _courseLinkRepository.Query(tracking: true)
                    .Where(l => l.FasSchemeId == id)
                    .ToListAsync(token);
                _courseLinkRepository.RemoveRange(links);

                _repository.Remove(scheme);
                await _unitOfWork.SaveChangeAsync(token);
            }, cancellationToken);
        }

        public override async Task DeleteSelectedIdsAsync(DeleteSelectedIdsDTO dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (dto.Ids.Count == 0) return;
            var batchId = Guid.NewGuid();
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                var schemes = await _repository.Query(tracking: true)
                    .Where(s => dto.Ids.Contains(s.Id) && s.SchoolId == schoolId)
                    .ToListAsync(token);
                if (schemes.Count != dto.Ids.Distinct().Count())
                    throw new ValidationFailureException(nameof(dto.Ids), "One or more FAS schemes do not exist or you do not have permission.");

                if (schemes.Any(s => s.Status != FasSchemeStatus.Draft))
                {
                    throw new ValidationFailureException(nameof(FasScheme.Status), "Only draft schemes can be deleted.");
                }

                foreach (var scheme in schemes)
                {
                    var groups = await _groupRepository.Query(tracking: true)
                        .Include(g => g.Conditions)
                        .Where(g => g.FasSchemeId == scheme.Id)
                        .ToListAsync(token);
                    _conditionRepository.RemoveRange(groups.SelectMany(g => g.Conditions).ToList());
                    _groupRepository.RemoveRange(groups);

                    var Tiers = await _tierRepository.Query(tracking: true)
                        .Where(t => t.FasSchemeId == scheme.Id)
                        .ToListAsync(token);
                    _tierRepository.RemoveRange(Tiers);

                    var docs = await _requiredDocRepository.Query(tracking: true)
                        .Where(d => d.FasSchemeId == scheme.Id)
                        .ToListAsync(token);
                    foreach (var templateKey in docs
                        .Select(d => d.TemplateFileKey)
                        .Where(key => !string.IsNullOrWhiteSpace(key)))
                    {
                        ImageTransactionHookHelper.RegisterImageDeleteAfterCommit(transaction, _uploadService, templateKey);
                    }
                    _requiredDocRepository.RemoveRange(docs);

                    var links = await _courseLinkRepository.Query(tracking: true)
                        .Where(l => l.FasSchemeId == scheme.Id)
                        .ToListAsync(token);
                    _courseLinkRepository.RemoveRange(links);

                    await _managementActionLogService.LogAsync(
                        batchId,
                        ManagementActionEntityType.FasScheme,
                        scheme.Id,
                        ManagementAction.Delete,
                        dto.Reason,
                        scheme.Status.ToString(),
                        null,
                        cancellationToken: token);
                    _repository.Remove(scheme);
                }
                await _unitOfWork.SaveChangeAsync(token);
            }, cancellationToken);
        }

        #region Helpers

        private async Task<FasScheme> GetTrackedScopedSchemeAsync(
            int id,
            int schoolId,
            CancellationToken cancellationToken)
        {
            return await _repository.Query(tracking: true)
                .Include(s => s.Tiers)
                .Include(s => s.RequiredDocuments)
                .Include(s => s.SchemeCourses)
                .FirstOrDefaultAsync(
                    s => s.Id == id && s.SchoolId == schoolId,
                    cancellationToken)
                ?? throw new DataNotFoundException(typeof(FasScheme), id);
        }

        private async Task PopulateTreesAsync(
            List<GetFasSchemeDTO> items,
            CancellationToken cancellationToken)
        {
            if (items.Count == 0) return;
            var ids = items.Select(item => item.Id).ToList();
            var groups = await _groupRepository.Query()
                .Include(group => group.Conditions)
                .Where(group => ids.Contains(group.FasSchemeId))
                .ToListAsync(cancellationToken);
            var groupsByOwner = groups.GroupBy(group => group.FasSchemeId)
                .ToDictionary(group => group.Key, group => (IReadOnlyCollection<FasSchemeConditionGroup>)group.ToList());
            foreach (var item in items)
            {
                if (groupsByOwner.TryGetValue(item.Id, out var ownedGroups) && ownedGroups.Count != 0)
                    item.RootConditionGroup = FasConditionTreeMapper.MapFasTree(ownedGroups);
            }
        }

        private static void ValidateInput(
            string name,
            List<FasSchemeTierRequestDTO> tiers,
            FasConditionGroupRequestDTO rootConditionGroup,
            FasSubsidyType subsidyType,
            bool isPerComponent)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationFailureException(nameof(CreateFasSchemeDTO.SchemeName), "Scheme name is required.");
            if (tiers.Count == 0)
                throw new ValidationFailureException(nameof(CreateFasSchemeDTO.Tiers), "At least one tier is required.");
            if (rootConditionGroup.Conditions.Count + rootConditionGroup.Groups.Count == 0)
                throw new ValidationFailureException(nameof(CreateFasSchemeDTO.RootConditionGroup), "At least one condition is required.");
            FasTierMatcher.ValidateTierConfiguration(tiers, subsidyType, isPerComponent);
        }

        private async Task ValidateCoursesExistAsync(List<int> courseIds, int schoolId, CancellationToken cancellationToken)
        {
            if (courseIds.Count == 0) return;
            var count = await _courseRepository.Query()
                .Where(c => courseIds.Contains(c.Id) && c.SchoolId == schoolId)
                .CountAsync(cancellationToken);
            if (count != courseIds.Distinct().Count())
            {
                throw new ValidationFailureException(nameof(CreateFasSchemeDTO.SchemeCourses), "One or more selected courses do not exist or belong to another school.");
            }
        }

        private async Task ValidateSchemeCanActivateAsync(FasScheme scheme, CancellationToken cancellationToken)
        {
            var tiers = await _tierRepository.Query()
                .Where(t => t.FasSchemeId == scheme.Id)
                .Select(t => new FasSchemeTierRequestDTO
                {
                    TierName = t.TierName,
                    TierIncomeBasis = t.TierIncomeBasis,
                    MinPerCapitaIncome = t.MinPerCapitaIncome,
                    MaxPerCapitaIncome = t.MaxPerCapitaIncome,
                    MinGrossHouseholdIncome = t.MinGrossHouseholdIncome,
                    MaxGrossHouseholdIncome = t.MaxGrossHouseholdIncome,
                    SubsidyValue = t.SubsidyValue,
                    CourseFeeSubsidyValue = t.CourseFeeSubsidyValue,
                    MiscFeeSubsidyValue = t.MiscFeeSubsidyValue,
                    DisplayOrder = t.DisplayOrder
                })
                .ToListAsync(cancellationToken);

            FasTierMatcher.ValidateTierConfiguration(tiers, scheme.SubsidyType, scheme.IsPerComponent);

            var groups = await _groupRepository.Query()
                .Include(g => g.Conditions)
                .Where(g => g.FasSchemeId == scheme.Id)
                .ToListAsync(cancellationToken);
            var root = groups.SingleOrDefault(g => g.ParentGroupId == null);
            if (root == null)
            {
                throw new ValidationFailureException(nameof(CreateFasSchemeDTO.RootConditionGroup), "A root condition group is required.");
            }

            var children = groups.Where(g => g.ParentGroupId.HasValue)
                .GroupBy(g => g.ParentGroupId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());
            var requestRoot = BuildConditionRequest(root, children);
            FasConditionTreeUtility.Validate(requestRoot);
            FasConditionSemanticAnalyzer.Validate(requestRoot);
        }

        private static FasConditionGroupRequestDTO BuildConditionRequest(
            FasSchemeConditionGroup group,
            IReadOnlyDictionary<int, List<FasSchemeConditionGroup>> children)
        {
            return new FasConditionGroupRequestDTO
            {
                LogicalOperator = group.LogicalOperator,
                DisplayOrder = group.DisplayOrder,
                Conditions = group.Conditions.Select(condition => new FasConditionRequestDTO
                {
                    Field = condition.Field,
                    Operator = condition.Operator,
                    ValueNumber = condition.ValueNumber,
                    ValueNumberTo = condition.ValueNumberTo,
                    CountryId = MapValueTextToCountryId(condition.Field, condition.ValueText),
                    DisplayOrder = condition.DisplayOrder
                }).ToList(),
                Groups = children.GetValueOrDefault(group.Id, [])
                    .Select(child => BuildConditionRequest(child, children))
                    .ToList()
            };
        }

        private static int? MapValueTextToCountryId(FasConditionField field, string? valueText)
        {
            if (field is not (FasConditionField.StudentNationality or FasConditionField.GuardianNationality))
            {
                return null;
            }

            return valueText?.Trim().ToLowerInvariant() switch
            {
                "singapore" => 1,
                "other" => 2,
                _ => null
            };
        }

        private async Task<List<string>> UploadDocumentTemplatesAsync(
            List<FasRequiredDocumentRequestDTO> documents,
            CancellationToken cancellationToken)
        {
            var uploadedTemplateKeys = new List<string>();
            foreach (var doc in documents)
            {
                if (doc.TemplateFile == null) continue;
                var extension = Path.GetExtension(doc.TemplateFile.FileName).ToLowerInvariant();
                if (extension is not ".docx" and not ".pdf")
                    throw new ValidationFailureException(nameof(doc.TemplateFile), "Only .docx and .pdf templates are allowed.");

                var uploadResult = await _uploadService!.UploadAsync(doc.TemplateFile, "fas/templates", cancellationToken);
                doc.TemplateFileKey = uploadResult.FileName;
                uploadedTemplateKeys.Add(uploadResult.FileName);
            }

            return uploadedTemplateKeys;
        }

        private static void ValidatePersistedTree(FasSchemeConditionGroup root)
        {
            root.TryValidate();
            foreach (var condition in root.Conditions) condition.TryValidate();
            foreach (var child in root.ChildGroups) ValidatePersistedTree(child);
        }

        private FasSchemeConditionGroup CloneGroup(
            FasSchemeConditionGroup sourceGroup,
            List<FasSchemeConditionGroup> allGroups,
            int newSchemeId,
            FasSchemeConditionGroup? parent = null)
        {
            var newGroup = new FasSchemeConditionGroup
            {
                FasSchemeId = newSchemeId,
                ParentGroup = parent,
                LogicalOperator = sourceGroup.LogicalOperator,
                DisplayOrder = sourceGroup.DisplayOrder,
                Conditions = sourceGroup.Conditions.Select(c => new FasSchemeCondition
                {
                    Field = c.Field,
                    Operator = c.Operator,
                    ValueNumber = c.ValueNumber,
                    ValueNumberTo = c.ValueNumberTo,
                    ValueText = c.ValueText,
                    DisplayOrder = c.DisplayOrder
                }).ToList()
            };

            var children = allGroups.Where(g => g.ParentGroupId == sourceGroup.Id).ToList();
            newGroup.ChildGroups = children
                .Select(cg => CloneGroup(cg, allGroups, newSchemeId, newGroup))
                .ToList();

            return newGroup;
        }

        #endregion
    }
}
