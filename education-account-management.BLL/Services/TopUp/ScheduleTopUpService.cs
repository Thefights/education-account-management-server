using DTOs.TopUp;
using Interfaces.Audit;
using Interfaces.TopUp;
using Results;
using Services.Base;
using Validators;

namespace Services.TopUp;

public class ScheduleTopUpService(
    IUnitOfWork unitOfWork,
    ScheduleTopUpMapper mapper,
    IAuditLogWriter auditLogWriter)
    : BaseService<ScheduleTopUp, CreateScheduleTopUpDTO, GetScheduleTopUpDTO, UpdateScheduleTopUpDTO>(unitOfWork, mapper),
      IScheduleTopUpService
{
    private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
    private readonly IGenericRepository<ScheduleTopUpConditionGroup> _groupRepository =
        unitOfWork.Repository<ScheduleTopUpConditionGroup>();
    private readonly IGenericRepository<ScheduleTopUpCondition> _conditionRepository =
        unitOfWork.Repository<ScheduleTopUpCondition>();

    public override async Task<GetScheduleTopUpDTO> CreateAsync(
        CreateScheduleTopUpDTO createDTO,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(createDTO);
        ValidateAmount(createDTO.TopupAmount, ScheduleTopUpStatus.Active);
        ValidateSchedule(createDTO.Frequency, createDTO.OneTimeExecutionAt, createDTO.ExecuteAtDay,
            createDTO.ExecuteAtMonth, ScheduleTopUpStatus.Active);
        TopupConditionTreeUtility.Validate(createDTO.RootConditionGroup);

        var id = await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            var schedule = _mapper.MapFromCreateDTO(createDTO);
            schedule.Status = ScheduleTopUpStatus.Active;
            schedule.NextExecutionAt = ComputeFirstExecutionAt(schedule);
            schedule.TryValidate();
            await UniqueConstraintValidator.ValidateAsync(_repository, schedule, cancellationToken: token);
            await _repository.AddAsync(schedule, token);
            await _unitOfWork.SaveChangeAsync(token);

            var root = TopupConditionTreeMapper.MapScheduleGroup(createDTO.RootConditionGroup, schedule.Id);
            await _groupRepository.AddAsync(root, token);
            await _unitOfWork.SaveChangeAsync(token);
            ValidatePersistedTree(root);
            return schedule.Id;
        }, cancellationToken);

        await LogAsync("CreateScheduleTopUp", cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public override async Task<GetScheduleTopUpDTO> UpdateAsync(
        int id,
        UpdateScheduleTopUpDTO updateDTO,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(updateDTO);
        ValidateAmount(updateDTO.TopupAmount, updateDTO.Status);
        ValidateSchedule(updateDTO.Frequency, updateDTO.OneTimeExecutionAt, updateDTO.ExecuteAtDay,
            updateDTO.ExecuteAtMonth, updateDTO.Status);
        TopupConditionTreeUtility.Validate(updateDTO.RootConditionGroup);

        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            var schedule = await _repository.GetTrackedByIdAsync(id, cancellationToken: token)
                ?? throw new DataNotFoundException(typeof(ScheduleTopUp), id);
            if (schedule.Frequency == ScheduleTopUpFrequency.OneTime && schedule.Status == ScheduleTopUpStatus.Completed)
                throw new ValidationFailureException(nameof(schedule.Status), "A completed one-time schedule cannot be changed.");

            var groups = await _groupRepository.Query(tracking: true)
                .Include(group => group.Conditions)
                .Where(group => group.ScheduleTopUpId == id)
                .ToListAsync(token);
            _conditionRepository.RemoveRange(groups.SelectMany(group => group.Conditions).ToList());
            _groupRepository.RemoveRange(groups);
            await _unitOfWork.SaveChangeAsync(token);

            _mapper.MapFromUpdateDTO(updateDTO, schedule);
            schedule.NextExecutionAt = ComputeFirstExecutionAt(schedule);
            schedule.TryValidate();
            await UniqueConstraintValidator.ValidateAsync(_repository, schedule, schedule.Id, token);
            _repository.Update(schedule);

            var root = TopupConditionTreeMapper.MapScheduleGroup(updateDTO.RootConditionGroup, schedule.Id);
            await _groupRepository.AddAsync(root, token);
            await _unitOfWork.SaveChangeAsync(token);
            ValidatePersistedTree(root);
        }, cancellationToken);

        await LogAsync("UpdateScheduleTopUp", cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task UpdateStatusesAsync(
        BatchUpdateScheduleTopUpStatusDTO dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (dto.Ids.Count == 0) return;
        if (dto.Status == ScheduleTopUpStatus.Completed)
            throw new ValidationFailureException(nameof(dto.Status), "Completed status is managed by schedule execution.");

        var schedules = await _repository.GetByIdsAsync(dto.Ids, cancellationToken: cancellationToken);
        if (schedules.Count != dto.Ids.Distinct().Count())
            throw new ValidationFailureException(nameof(dto.Ids), "One or more scheduled top-ups do not exist.");
        if (dto.Status == ScheduleTopUpStatus.Active)
        {
            var completed = schedules.FirstOrDefault(schedule =>
                schedule.Frequency == ScheduleTopUpFrequency.OneTime && schedule.Status == ScheduleTopUpStatus.Completed);
            if (completed != null)
                throw new ValidationFailureException(nameof(dto.Ids), $"Completed schedule {completed.Id} cannot be activated again.");
            var missingAmount = schedules.FirstOrDefault(schedule => schedule.TopupAmount is null or <= 0);
            if (missingAmount != null)
                throw new ValidationFailureException(nameof(dto.Ids), $"Schedule {missingAmount.Id} requires a positive amount before activation.");
        }

        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            foreach (var schedule in schedules)
            {
                schedule.Status = dto.Status;
                schedule.NextExecutionAt = ComputeFirstExecutionAt(schedule);
                schedule.TryValidate();
            }
            _repository.UpdateRange(schedules);
            await _auditLogWriter.LogAsync(
                AuditLogCategory.Topup,
                dto.Status == ScheduleTopUpStatus.Active ? "ActivateScheduleTopUp" : "InactivateScheduleTopUp",
                cancellationToken: token);
        }, cancellationToken);
    }

    public override async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.DeleteAsync(id, cancellationToken);
        await LogAsync("DeleteScheduleTopUp", cancellationToken);
    }

    public override async Task<GetScheduleTopUpDTO> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dto = await base.GetByIdAsync(id, cancellationToken);
        await PopulateTreesAsync([dto], cancellationToken);
        return dto;
    }

    public override async Task<List<GetScheduleTopUpDTO>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await base.GetAllAsync(cancellationToken);
        await PopulateTreesAsync(items, cancellationToken);
        return items;
    }

    public override async Task<List<GetScheduleTopUpDTO>> GetAllByIdsAsync(
        List<int> ids,
        CancellationToken cancellationToken = default)
    {
        var items = await base.GetAllByIdsAsync(ids, cancellationToken);
        await PopulateTreesAsync(items, cancellationToken);
        return items;
    }

    public override async Task<PaginationResult<GetScheduleTopUpDTO>> GetAllPaginatedAsync(
        FilterDTO filterDTO,
        CancellationToken cancellationToken = default)
    {
        var result = await base.GetAllPaginatedAsync(filterDTO, cancellationToken);
        await PopulateTreesAsync(result.Collection, cancellationToken);
        return result;
    }

    private async Task PopulateTreesAsync(
        List<GetScheduleTopUpDTO> items,
        CancellationToken cancellationToken)
    {
        if (items.Count == 0) return;
        var ids = items.Select(item => item.Id).ToList();
        var groups = await _groupRepository.Query()
            .Include(group => group.Conditions)
            .Where(group => ids.Contains(group.ScheduleTopUpId))
            .ToListAsync(cancellationToken);
        var groupsByOwner = groups.GroupBy(group => group.ScheduleTopUpId)
            .ToDictionary(group => group.Key, group => (IReadOnlyCollection<ScheduleTopUpConditionGroup>)group.ToList());
        foreach (var item in items)
        {
            if (groupsByOwner.TryGetValue(item.Id, out var ownedGroups) && ownedGroups.Count != 0)
                item.RootConditionGroup = TopupConditionTreeMapper.MapScheduleTree(ownedGroups);
        }
    }

    private async Task LogAsync(string action, CancellationToken cancellationToken)
    {
        await _auditLogWriter.LogAsync(AuditLogCategory.Topup, action, cancellationToken: cancellationToken);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }

    private static void ValidateAmount(decimal? amount, ScheduleTopUpStatus status)
    {
        if (status is ScheduleTopUpStatus.Active or ScheduleTopUpStatus.Completed && amount is null or <= 0)
            throw new ValidationFailureException(nameof(ScheduleTopUp.TopupAmount), "Top-up amount must be positive before activation.");
        if (amount < 0)
            throw new ValidationFailureException(nameof(ScheduleTopUp.TopupAmount), "Top-up amount cannot be negative.");
    }

    private static void ValidateSchedule(
        ScheduleTopUpFrequency frequency,
        DateTime? oneTimeExecutionAt,
        int? executeAtDay,
        int? executeAtMonth,
        ScheduleTopUpStatus status)
    {
        var errors = new Dictionary<string, string>();
        if (status == ScheduleTopUpStatus.Completed)
            errors[nameof(status)] = "Completed status is managed by schedule execution.";
        if (frequency == ScheduleTopUpFrequency.OneTime)
        {
            if (!oneTimeExecutionAt.HasValue) errors[nameof(oneTimeExecutionAt)] = "One-time execution date and time is required.";
            if (executeAtDay.HasValue) errors[nameof(executeAtDay)] = "Execution day must be null for OneTime.";
            if (executeAtMonth.HasValue) errors[nameof(executeAtMonth)] = "Execution month must be null for OneTime.";
        }
        else if (frequency == ScheduleTopUpFrequency.Monthly)
        {
            if (oneTimeExecutionAt.HasValue) errors[nameof(oneTimeExecutionAt)] = "One-time date must be null for Monthly.";
            if (executeAtDay is < 1 or > 31) errors[nameof(executeAtDay)] = "Execution day must be between 1 and 31.";
            if (executeAtMonth.HasValue) errors[nameof(executeAtMonth)] = "Execution month must be null for Monthly.";
        }
        else if (frequency == ScheduleTopUpFrequency.Yearly)
        {
            if (oneTimeExecutionAt.HasValue) errors[nameof(oneTimeExecutionAt)] = "One-time date must be null for Yearly.";
            if (executeAtDay is < 1 or > 31) errors[nameof(executeAtDay)] = "Execution day must be between 1 and 31.";
            if (executeAtMonth is < 1 or > 12) errors[nameof(executeAtMonth)] = "Execution month must be between 1 and 12.";
            if (executeAtDay.HasValue && executeAtMonth.HasValue &&
                executeAtDay > DateTime.DaysInMonth(2024, executeAtMonth.Value))
                errors[nameof(executeAtDay)] = "Execution day is invalid for the selected month.";
        }
        if (errors.Count != 0) throw new ValidationFailureException(errors);
    }

    private static DateTime? ComputeFirstExecutionAt(ScheduleTopUp schedule)
    {
        if (schedule.Status != ScheduleTopUpStatus.Active) return null;
        var now = DateTime.UtcNow.AddHours(8);
        if (schedule.Frequency == ScheduleTopUpFrequency.OneTime)
            return schedule.OneTimeExecutionAt.HasValue
                ? DateTime.SpecifyKind(schedule.OneTimeExecutionAt.Value, DateTimeKind.Unspecified)
                : null;
        if (schedule.Frequency == ScheduleTopUpFrequency.Monthly)
        {
            if (!schedule.ExecuteAtDay.HasValue) return null;
            var occurrence = CreateOccurrence(now.Year, now.Month, schedule.ExecuteAtDay.Value, schedule.ExecutionTime);
            if (occurrence <= now)
            {
                var nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                occurrence = CreateOccurrence(nextMonth.Year, nextMonth.Month, schedule.ExecuteAtDay.Value, schedule.ExecutionTime);
            }
            return occurrence;
        }
        if (!schedule.ExecuteAtDay.HasValue || !schedule.ExecuteAtMonth.HasValue) return null;
        var yearly = CreateOccurrence(now.Year, schedule.ExecuteAtMonth.Value, schedule.ExecuteAtDay.Value, schedule.ExecutionTime);
        return yearly <= now
            ? CreateOccurrence(now.Year + 1, schedule.ExecuteAtMonth.Value, schedule.ExecuteAtDay.Value, schedule.ExecutionTime)
            : yearly;
    }

    private static DateTime CreateOccurrence(int year, int month, int requestedDay, TimeOnly time)
    {
        var day = Math.Min(requestedDay, DateTime.DaysInMonth(year, month));
        return new DateTime(year, month, day, time.Hour, time.Minute, time.Second, DateTimeKind.Unspecified);
    }

    private static void ValidatePersistedTree(ScheduleTopUpConditionGroup root)
    {
        root.TryValidate();
        foreach (var condition in root.Conditions) condition.TryValidate();
        foreach (var child in root.ChildGroups) ValidatePersistedTree(child);
    }
}
