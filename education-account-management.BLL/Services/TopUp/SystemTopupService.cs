using DTOs.Base;
using DTOs.TopUp;
using Interfaces.Audit;
using Interfaces.TopUp;
using Mappers.TopUp;
using Results;
using Services.Base;
using Validators;

namespace Services.TopUp
{
    public class SystemTopupService(
        IUnitOfWork unitOfWork,
        SystemTopupMapper mapper,
        IAuditLogWriter auditLogWriter,
        IManagementActionLogService managementActionLogService)
        : BaseService<SystemTopup, CreateSystemTopupDTO, GetSystemTopupDTO, UpdateSystemTopupDTO>(unitOfWork, mapper),
          ISystemTopupService
    {
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IManagementActionLogService _managementActionLogService = managementActionLogService;
        private readonly IGenericRepository<SystemTopupConditionGroup> _groupRepository =
            unitOfWork.Repository<SystemTopupConditionGroup>();
        private readonly IGenericRepository<SystemTopupCondition> _conditionRepository =
            unitOfWork.Repository<SystemTopupCondition>();

        public override async Task<GetSystemTopupDTO> CreateAsync(
            CreateSystemTopupDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            ValidateAmount(createDTO.TopupAmount, SystemTopupStatus.Active);
            TopupConditionTreeUtility.Validate(createDTO.RootConditionGroup);

            var id = await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var topup = _mapper.MapFromCreateDTO(createDTO);
                topup.Status = SystemTopupStatus.Active;
                topup.TryValidate();
                await UniqueConstraintValidator.ValidateAsync(_repository, topup, cancellationToken: token);
                await _repository.AddAsync(topup, token);
                await _unitOfWork.SaveChangeAsync(token);

                var root = TopupConditionTreeMapper.MapSystemGroup(createDTO.RootConditionGroup, topup.Id);
                await _groupRepository.AddAsync(root, token);
                await _unitOfWork.SaveChangeAsync(token);
                ValidatePersistedTree(root);
                return topup.Id;
            }, cancellationToken);

            await LogAsync("CreateSystemTopup", cancellationToken);
            return await GetByIdAsync(id, cancellationToken);
        }

        public override async Task<GetSystemTopupDTO> UpdateAsync(
            int id,
            UpdateSystemTopupDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            ValidateAmount(updateDTO.TopupAmount, updateDTO.Status);
            TopupConditionTreeUtility.Validate(updateDTO.RootConditionGroup);

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var topup = await _repository.GetTrackedByIdAsync(id, cancellationToken: token)
                    ?? throw new DataNotFoundException(typeof(SystemTopup), id);
                var groups = await _groupRepository.Query(tracking: true)
                    .Include(group => group.Conditions)
                    .Where(group => group.SystemTopupId == id)
                    .ToListAsync(token);

                _conditionRepository.RemoveRange(groups.SelectMany(group => group.Conditions).ToList());
                _groupRepository.RemoveRange(groups);
                await _unitOfWork.SaveChangeAsync(token);

                _mapper.MapFromUpdateDTO(updateDTO, topup);
                topup.TryValidate();
                await UniqueConstraintValidator.ValidateAsync(_repository, topup, topup.Id, token);
                _repository.Update(topup);

                var root = TopupConditionTreeMapper.MapSystemGroup(updateDTO.RootConditionGroup, topup.Id);
                await _groupRepository.AddAsync(root, token);
                await _unitOfWork.SaveChangeAsync(token);
                ValidatePersistedTree(root);
            }, cancellationToken);

            await LogAsync("UpdateSystemTopup", cancellationToken);
            return await GetByIdAsync(id, cancellationToken);
        }

        public async Task UpdateStatusesAsync(
            BatchUpdateSystemTopupStatusDTO dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (dto.Ids.Count == 0) return;
            var batchId = Guid.NewGuid();
            var topups = await _repository.GetByIdsAsync(dto.Ids, cancellationToken: cancellationToken);
            if (topups.Count != dto.Ids.Distinct().Count())
                throw new ValidationFailureException(nameof(dto.Ids), "One or more System top-ups do not exist.");
            if (dto.Status == SystemTopupStatus.Active)
            {
                var invalid = topups.FirstOrDefault(topup => topup.TopupAmount is null or <= 0);
                if (invalid != null)
                    throw new ValidationFailureException(nameof(dto.Ids), $"System top-up {invalid.Id} requires a positive amount before activation.");
            }

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                foreach (var topup in topups)
                {
                    var oldStatus = topup.Status;
                    topup.Status = dto.Status;
                    topup.TryValidate();
                    await _managementActionLogService.LogAsync(
                        batchId,
                        "SystemTopup",
                        topup.Id,
                        dto.Status == SystemTopupStatus.Active ? "Activate" : "Deactivate",
                        dto.Reason,
                        oldStatus.ToString(),
                        topup.Status.ToString(),
                        cancellationToken: token);
                }
                _repository.UpdateRange(topups);
                await _auditLogWriter.LogAsync(
                    AuditLogCategory.Topup,
                    dto.Status == SystemTopupStatus.Active ? "ActivateSystemTopup" : "InactivateSystemTopup",
                    cancellationToken: token);
            }, cancellationToken);
        }

        public override async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(id, cancellationToken);
            await LogAsync("DeleteSystemTopup", cancellationToken);
        }

        public override async Task DeleteSelectedIdsAsync(DeleteSelectedIdsDTO dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var batchId = Guid.NewGuid();
            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var topups = await _repository.GetTrackedByIdsAsync(dto.Ids, cancellationToken: token);
                if (topups.Count != dto.Ids.Distinct().Count())
                    throw new ValidationFailureException(nameof(dto.Ids), "One or more System top-ups do not exist.");

                foreach (var topup in topups)
                {
                    await _managementActionLogService.LogAsync(
                        batchId,
                        "SystemTopup",
                        topup.Id,
                        "Delete",
                        dto.Reason,
                        topup.Status.ToString(),
                        null,
                        cancellationToken: token);
                }

                _repository.RemoveRange(topups);
                await _auditLogWriter.LogAsync(AuditLogCategory.Topup, "DeleteSelectedSystemTopup", cancellationToken: token);
            }, cancellationToken);
        }

        public override async Task<GetSystemTopupDTO> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var dto = await base.GetByIdAsync(id, cancellationToken);
            await PopulateTreesAsync([dto], cancellationToken);
            return dto;
        }

        public override async Task<List<GetSystemTopupDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var items = await base.GetAllAsync(cancellationToken);
            await PopulateTreesAsync(items, cancellationToken);
            return items;
        }

        public override async Task<List<GetSystemTopupDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            var items = await base.GetAllByIdsAsync(ids, cancellationToken);
            await PopulateTreesAsync(items, cancellationToken);
            return items;
        }

        public override async Task<PaginationResult<GetSystemTopupDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            var result = await base.GetAllPaginatedAsync(filterDTO, cancellationToken);
            await PopulateTreesAsync(result.Collection, cancellationToken);
            return result;
        }

        private async Task PopulateTreesAsync(
            List<GetSystemTopupDTO> items,
            CancellationToken cancellationToken)
        {
            if (items.Count == 0) return;
            var ids = items.Select(item => item.Id).ToList();
            var groups = await _groupRepository.Query()
                .Include(group => group.Conditions)
                .Where(group => ids.Contains(group.SystemTopupId))
                .ToListAsync(cancellationToken);
            var groupsByOwner = groups.GroupBy(group => group.SystemTopupId)
                .ToDictionary(group => group.Key, group => (IReadOnlyCollection<SystemTopupConditionGroup>)group.ToList());
            foreach (var item in items)
            {
                if (groupsByOwner.TryGetValue(item.Id, out var ownedGroups) && ownedGroups.Count != 0)
                    item.RootConditionGroup = TopupConditionTreeMapper.MapSystemTree(ownedGroups);
            }
        }

        private async Task LogAsync(string action, CancellationToken cancellationToken)
        {
            await _auditLogWriter.LogAsync(AuditLogCategory.Topup, action, cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        private static void ValidateAmount(decimal? amount, SystemTopupStatus status)
        {
            if (status == SystemTopupStatus.Active && amount is null or <= 0)
                throw new ValidationFailureException(nameof(SystemTopup.TopupAmount), "Top-up amount must be positive before activation.");
            if (amount < 0)
                throw new ValidationFailureException(nameof(SystemTopup.TopupAmount), "Top-up amount cannot be negative.");
        }

        private static void ValidatePersistedTree(SystemTopupConditionGroup root)
        {
            root.TryValidate();
            foreach (var condition in root.Conditions) condition.TryValidate();
            foreach (var child in root.ChildGroups) ValidatePersistedTree(child);
        }
    }
}
