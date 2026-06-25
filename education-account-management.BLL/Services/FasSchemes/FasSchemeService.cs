using DTOs.FasSchemes;
using Interfaces.Audit;
using Interfaces.Base;
using Interfaces.FasSchemes;
using Mappers.FasSchemes;
using Results;
using Services.Base;
using Validators;

namespace Services.FasSchemes
{
    public class FasSchemeService(
        IUnitOfWork unitOfWork,
        FasSchemeMapper mapper,
        SchoolScopeResolver schoolScopeResolver,
        IAuditLogWriter auditLogWriter,
        TimeProvider timeProvider,
        IFileValidator fileValidator)
        : BaseService<FasScheme, CreateFasSchemeDTO, GetFasSchemeDTO, UpdateFasSchemeDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(FasScheme.Tiers), nameof(FasScheme.RequiredDocuments), $"{nameof(FasScheme.SchemeCourses)}.{nameof(FasSchemeCourse.Course)}"]),
          IFasSchemeService
    {
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly TimeProvider _timeProvider = timeProvider;
        private readonly IFileValidator _fileValidator = fileValidator;

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

        public override async Task<GetFasSchemeDTO> CreateAsync(
            CreateFasSchemeDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            ValidateInput(createDTO.SchemeName, createDTO.Tiers.Count, createDTO.RootConditionGroup);
            FasConditionTreeUtility.Validate(createDTO.RootConditionGroup);
            await ValidateCoursesExistAsync(createDTO.SchemeCourses.Select(c => c.CourseId).ToList(), schoolId, cancellationToken);
            await ValidateDocumentTemplatesAsync(createDTO.RequiredDocuments, cancellationToken);

            var id = await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var scheme = _mapper.MapFromCreateDTO(createDTO);
                scheme.SchoolId = schoolId;
                scheme.Status = FasSchemeStatus.Draft;
                scheme.SchemeCode = await GenerateUniqueSchemeCodeAsync(schoolId, token);
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
                    MaxPerCapitaIncome = t.MaxPerCapitaIncome,
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

            ValidateInput(updateDTO.SchemeName, updateDTO.Tiers.Count, updateDTO.RootConditionGroup);
            FasConditionTreeUtility.Validate(updateDTO.RootConditionGroup);
            await ValidateCoursesExistAsync(updateDTO.SchemeCourses.Select(c => c.CourseId).ToList(), schoolId, cancellationToken);
            await ValidateDocumentTemplatesAsync(updateDTO.RequiredDocuments, cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var scheme = await GetTrackedScopedSchemeAsync(id, schoolId, token);
                if (scheme.Status != FasSchemeStatus.Draft)
                {
                    throw new ValidationFailureException(nameof(scheme.Status), "Only draft schemes can be edited.");
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
                _requiredDocRepository.RemoveRange(docs);

                // Clear links
                var links = await _courseLinkRepository.Query(tracking: true)
                    .Where(l => l.FasSchemeId == id)
                    .ToListAsync(token);
                _courseLinkRepository.RemoveRange(links);

                await _unitOfWork.SaveChangeAsync(token);

                // Map updated properties
                _mapper.MapFromUpdateDTO(updateDTO, scheme);
                scheme.TryValidate();
                await UniqueConstraintValidator.ValidateAsync(_repository, scheme, scheme.Id, token);
                _repository.Update(scheme);

                // Insert new tree & collections
                var root = FasConditionTreeMapper.MapFasGroup(updateDTO.RootConditionGroup, scheme.Id);
                await _groupRepository.AddAsync(root, token);

                var newTiers = updateDTO.Tiers.Select(t => new FasSchemeTier
                {
                    FasSchemeId = scheme.Id,
                    TierName = t.TierName,
                    MaxPerCapitaIncome = t.MaxPerCapitaIncome,
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
                    }

                    scheme.Status = dto.Status;
                    if (dto.Status == FasSchemeStatus.Active && oldStatus == FasSchemeStatus.Draft)
                    {
                        scheme.PublishedAt = _timeProvider.GetUtcNow().UtcDateTime;
                    }

                    scheme.TryValidate();
                    _repository.Update(scheme);

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
                    SchemeCode = await GenerateUniqueSchemeCodeAsync(schoolId, token),
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
                    MaxPerCapitaIncome = t.MaxPerCapitaIncome,
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
            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
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
                _requiredDocRepository.RemoveRange(docs);

                var links = await _courseLinkRepository.Query(tracking: true)
                    .Where(l => l.FasSchemeId == id)
                    .ToListAsync(token);
                _courseLinkRepository.RemoveRange(links);

                _repository.Remove(scheme);
                await _unitOfWork.SaveChangeAsync(token);
            }, cancellationToken);
        }

        public override async Task DeleteSelectedIdsAsync(List<int> ids, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(ids);
            if (ids.Count == 0) return;
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var schemes = await _repository.Query(tracking: true)
                    .Where(s => ids.Contains(s.Id) && s.SchoolId == schoolId)
                    .ToListAsync(token);

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
                    _requiredDocRepository.RemoveRange(docs);

                    var links = await _courseLinkRepository.Query(tracking: true)
                        .Where(l => l.FasSchemeId == scheme.Id)
                        .ToListAsync(token);
                    _courseLinkRepository.RemoveRange(links);

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

        private async Task<string> GenerateUniqueSchemeCodeAsync(int schoolId, CancellationToken cancellationToken)
        {
            string code;
            bool exists;
            do
            {
                var randomSuffix = Random.Shared.Next(100000, 999999);
                code = $"FAS-{schoolId}-{randomSuffix}";
                exists = await _repository.AnyAsync(s => s.SchemeCode == code, cancellationToken);
            } while (exists);
            return code;
        }

        private static void ValidateInput(string name, int tiersCount, FasConditionGroupRequestDTO rootConditionGroup)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationFailureException(nameof(CreateFasSchemeDTO.SchemeName), "Scheme name is required.");
            if (tiersCount == 0)
                throw new ValidationFailureException(nameof(CreateFasSchemeDTO.Tiers), "At least one tier is required.");
            if (rootConditionGroup.Conditions.Count + rootConditionGroup.Groups.Count == 0)
                throw new ValidationFailureException(nameof(CreateFasSchemeDTO.RootConditionGroup), "At least one condition is required.");
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

        private async Task ValidateDocumentTemplatesAsync(
            List<FasRequiredDocumentRequestDTO> documents,
            CancellationToken cancellationToken)
        {
            foreach (var doc in documents)
            {
                if (string.IsNullOrWhiteSpace(doc.TemplateFileKey)) continue;

                var extension = Path.GetExtension(doc.TemplateFileKey).ToLowerInvariant();
                if (extension is not ".docx" and not ".pdf")
                {
                    throw new ValidationFailureException(nameof(doc.TemplateFileKey), "Only .docx and .pdf templates are allowed.");
                }

                byte[] signature;
                string contentType;

                if (extension == ".pdf")
                {
                    signature = System.Text.Encoding.ASCII.GetBytes("%PDF-");
                    contentType = "application/pdf";
                }
                else
                {
                    signature = new byte[] { 0x50, 0x4B, 0x03, 0x04 };
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                }

                using var stream = new MemoryStream(signature);
                var formFile = new FormFile(stream, 0, stream.Length, "file", doc.TemplateFileKey)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };

                var validationResult = await _fileValidator.ValidateAsync(formFile, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationFailureException(nameof(doc.TemplateFileKey), validationResult.ErrorMessage ?? "Invalid template file.");
                }
            }
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
                    //CountryId = c.CountryId,
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
