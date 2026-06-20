using DTOs.TopUp;
using Interfaces.Audit;
using Interfaces.TopUp;
using Services.Base;
using Validators;

namespace Services.TopUp
{
    public class TopupScheduleService(
        IUnitOfWork unitOfWork,
        TopupScheduleMapper mapper,
        IAuditLogWriter auditLogWriter)
        : BaseService<TopupSchedule, CreateTopupScheduleDTO, GetTopupScheduleDTO, UpdateTopupScheduleDTO>(
            unitOfWork, mapper, includes: [nameof(TopupSchedule.TopupRule)]),
        ITopupScheduleService
    {
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;

        public override async Task<GetTopupScheduleDTO> CreateAsync(CreateTopupScheduleDTO createDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            ValidateSchedule(createDTO.Frequency, createDTO.OneTimeExecutionAt, createDTO.ExecuteAtDay,
                createDTO.ExecuteAtMonth, createDTO.ExecutionTime, TopupScheduleStatus.Active, createDTO.TopupRuleId);

            // Ensure TopupRule exists
            var rule = await _unitOfWork.Repository<TopupRule>().Query()
                .FirstOrDefaultAsync(r => r.Id == createDTO.TopupRuleId, cancellationToken);
            if (rule == null)
            {
                throw new ValidationFailureException(nameof(createDTO.TopupRuleId), "Referenced Topup Rule does not exist.");
            }
            if (rule.Type != TopupRuleType.Schedule)
            {
                throw new ValidationFailureException(nameof(createDTO.TopupRuleId), "Only Schedule rules can have a schedule.");
            }

            var scheduleId = await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                var schedule = _mapper.MapFromCreateDTO(createDTO);
                schedule.NextExecutionAt = ComputeFirstExecutionAt(schedule);

                schedule.TryValidate();
                await UniqueConstraintValidator.ValidateAsync(_repository, schedule, cancellationToken: token);

                var resultEntity = await _repository.AddAsync(schedule, token);
                await _unitOfWork.SaveChangeAsync(token);

                return resultEntity.Id;
            }, cancellationToken);

            var createdSchedule = await GetByIdAsync(scheduleId, cancellationToken);

            // Audit Log
            await _auditLogWriter.LogAsync(
                AuditLogCategory.TopupConfig,
                "CreateSchedule",
                cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return createdSchedule;
        }

        public override async Task<GetTopupScheduleDTO> UpdateAsync(int id, UpdateTopupScheduleDTO updateDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            ValidateSchedule(updateDTO.Frequency, updateDTO.OneTimeExecutionAt, updateDTO.ExecuteAtDay,
                updateDTO.ExecuteAtMonth, updateDTO.ExecutionTime, updateDTO.Status, updateDTO.TopupRuleId);

            var schedule = await _repository.Query()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken)
                ?? throw new DataNotFoundException(typeof(TopupSchedule), id);

            if (schedule.Frequency == TopupScheduleType.OneTime
                && schedule.Status == TopupScheduleStatus.Completed)
            {
                throw new ValidationFailureException(nameof(schedule.Status),
                    "A completed OneTime schedule cannot be changed or activated again.");
            }

            // Ensure TopupRule exists
            var rule = await _unitOfWork.Repository<TopupRule>().Query()
                .FirstOrDefaultAsync(r => r.Id == updateDTO.TopupRuleId, cancellationToken);
            if (rule == null)
            {
                throw new ValidationFailureException(nameof(updateDTO.TopupRuleId), "Referenced Topup Rule does not exist.");
            }
            if (rule.Type != TopupRuleType.Schedule)
            {
                throw new ValidationFailureException(nameof(updateDTO.TopupRuleId), "Only Schedule rules can have a schedule.");
            }

            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                _mapper.MapFromUpdateDTO(updateDTO, schedule);
                schedule.NextExecutionAt = ComputeFirstExecutionAt(schedule);

                schedule.TryValidate();
                await UniqueConstraintValidator.ValidateAsync(_repository, schedule, schedule.Id, token);
                _repository.Update(schedule);
            }, cancellationToken);

            var updatedSchedule = await GetByIdAsync(id, cancellationToken);

            // Audit Log
            await _auditLogWriter.LogAsync(
                AuditLogCategory.TopupConfig,
                "UpdateSchedule",
                cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return updatedSchedule;
        }

        public override async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var schedule = await GetByIdAsync(id, cancellationToken);

            await base.DeleteAsync(id, cancellationToken);

            // Audit Log
            await _auditLogWriter.LogAsync(
                AuditLogCategory.TopupConfig,
                "DeleteSchedule",
                cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        private DateTime? ComputeFirstExecutionAt(TopupSchedule schedule)
        {
            if (schedule.Status != TopupScheduleStatus.Active)
            {
                return null;
            }

            var now = DateTime.UtcNow.AddHours(8); // Singapore Time SGT
            var time = schedule.ExecutionTime;

            if (schedule.Frequency == TopupScheduleType.OneTime)
            {
                if (schedule.OneTimeExecutionAt == null) return null;
                var occurrence = DateTime.SpecifyKind(schedule.OneTimeExecutionAt.Value, DateTimeKind.Unspecified);
                schedule.OneTimeExecutionAt = occurrence;
                return occurrence;
            }

            if (schedule.Frequency == TopupScheduleType.Monthly)
            {
                if (schedule.ExecuteAtDay == null) return null;
                var date = CreateOccurrence(now.Year, now.Month, schedule.ExecuteAtDay.Value, time);
                if (date <= now)
                {
                    var nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                    date = CreateOccurrence(nextMonth.Year, nextMonth.Month, schedule.ExecuteAtDay.Value, time);
                }
                return date;
            }

            if (schedule.Frequency == TopupScheduleType.Yearly)
            {
                if (schedule.ExecuteAtDay == null || schedule.ExecuteAtMonth == null) return null;
                var month = schedule.ExecuteAtMonth.Value;
                var date = CreateOccurrence(now.Year, month, schedule.ExecuteAtDay.Value, time);
                if (date <= now)
                {
                    date = CreateOccurrence(now.Year + 1, month, schedule.ExecuteAtDay.Value, time);
                }
                return date;
            }

            return null;
        }

        private static void ValidateSchedule(
            TopupScheduleType frequency,
            DateTime? oneTimeExecutionAt,
            int? executeAtDay,
            int? executeAtMonth,
            TimeOnly executionTime,
            TopupScheduleStatus status,
            int ruleId)
        {
            var errors = new Dictionary<string, string>();
            if (ruleId <= 0) errors[nameof(ruleId)] = "Top-up rule ID must be positive.";
            if (!Enum.IsDefined(frequency)) errors[nameof(frequency)] = "Schedule frequency is invalid.";
            if (!Enum.IsDefined(status)) errors[nameof(status)] = "Schedule status is invalid.";

            if (frequency == TopupScheduleType.OneTime)
            {
                if (!oneTimeExecutionAt.HasValue) errors[nameof(oneTimeExecutionAt)] = "One-time execution date and time is required.";
                if (executeAtDay.HasValue) errors[nameof(executeAtDay)] = "Execution day must be null for OneTime.";
                if (executeAtMonth.HasValue) errors[nameof(executeAtMonth)] = "Execution month must be null for OneTime.";
                if (status == TopupScheduleStatus.Completed)
                    errors[nameof(status)] = "A new OneTime schedule cannot start as Completed.";
            }
            else if (frequency == TopupScheduleType.Monthly)
            {
                if (oneTimeExecutionAt.HasValue) errors[nameof(oneTimeExecutionAt)] = "One-time date must be null for Monthly.";
                if (executeAtDay is < 1 or > 31) errors[nameof(executeAtDay)] = "Execution day must be between 1 and 31.";
                if (executeAtMonth.HasValue) errors[nameof(executeAtMonth)] = "Execution month must be null for Monthly.";
                if (status == TopupScheduleStatus.Completed) errors[nameof(status)] = "Monthly schedules cannot be Completed.";
            }
            else if (frequency == TopupScheduleType.Yearly)
            {
                if (oneTimeExecutionAt.HasValue) errors[nameof(oneTimeExecutionAt)] = "One-time date must be null for Yearly.";
                if (executeAtDay is < 1 or > 31) errors[nameof(executeAtDay)] = "Execution day must be between 1 and 31.";
                if (executeAtMonth is < 1 or > 12) errors[nameof(executeAtMonth)] = "Execution month must be between 1 and 12.";
                if (executeAtDay.HasValue && executeAtMonth.HasValue
                    && executeAtDay > DateTime.DaysInMonth(2024, executeAtMonth.Value))
                    errors[nameof(executeAtDay)] = "Execution day is invalid for the selected month.";
                if (status == TopupScheduleStatus.Completed) errors[nameof(status)] = "Yearly schedules cannot be Completed.";
            }

            if (errors.Count != 0) throw new ValidationFailureException(errors);
        }

        private static DateTime CreateOccurrence(int year, int month, int requestedDay, TimeOnly time)
        {
            var day = Math.Min(requestedDay, DateTime.DaysInMonth(year, month));
            return new DateTime(year, month, day, time.Hour, time.Minute, time.Second, DateTimeKind.Unspecified);
        }
    }
}
