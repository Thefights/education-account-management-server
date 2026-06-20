using DTOs.TopUp;
using Interfaces.Audit;
using Interfaces.TopUp;
using Services.Base;
using System.Text.Json;
using Validators;

namespace Services.TopUp
{
    public class TopupRuleService(
        IUnitOfWork unitOfWork,
        TopupRuleMapper mapper,
        IAuditLogWriter auditLogWriter)
        : BaseService<TopupRule, CreateTopupRuleDTO, GetTopupRuleDTO, UpdateTopupRuleDTO>(
            unitOfWork, mapper, includes: [nameof(TopupRule.Conditions)]),
          ITopupRuleService
    {
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;

        public override async Task<GetTopupRuleDTO> CreateAsync(CreateTopupRuleDTO createDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            ValidateRule(createDTO.MatchMode, createDTO.TopupAmount, createDTO.Conditions);

            var ruleId = await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                var rule = _mapper.MapFromCreateDTO(createDTO);
                rule.RuleName = rule.RuleName.Trim();

                rule.TryValidate();
                foreach (var condition in rule.Conditions)
                    condition.TryValidate();
                await UniqueConstraintValidator.ValidateAsync(_repository, rule, cancellationToken: token);

                var resultEntity = await _repository.AddAsync(rule, token);
                await _unitOfWork.SaveChangeAsync(token);

                return resultEntity.Id;
            }, cancellationToken);

            var createdRule = await GetByIdAsync(ruleId, cancellationToken);

            // Audit Log
            var payload = new
            {
                Rule = createdRule
            };

            await _auditLogWriter.LogAsync(
                AuditLogCategory.TopupConfig,
                "CreateRule",
                payloadJson: JsonSerializer.Serialize(payload),
                cancellationToken: cancellationToken
            );

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return createdRule;
        }

        public override async Task<GetTopupRuleDTO> UpdateAsync(int id, UpdateTopupRuleDTO updateDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            ValidateRule(updateDTO.MatchMode, updateDTO.TopupAmount, updateDTO.Conditions);

            var rule = await _repository.Query()
                .Include(r => r.Conditions)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken)
                ?? throw new DataNotFoundException(typeof(TopupRule), id);

            if (rule.Type != updateDTO.Type)
            {
                var hasSuccessfulExecution = await _unitOfWork.Repository<TopupExecutionTarget>()
                    .Query()
                    .AnyAsync(t => t.TopupExecution.TopupRuleId == id && t.Status == TopupTargetStatus.Success, cancellationToken);
                var hasSchedule = await _unitOfWork.Repository<TopupSchedule>()
                    .Query()
                    .AnyAsync(s => s.TopupRuleId == id, cancellationToken);

                if (hasSuccessfulExecution || hasSchedule)
                {
                    throw new ValidationFailureException(nameof(updateDTO.Type),
                        "Cannot change rule type after a successful execution or after a schedule has been configured.");
                }
            }

            var existingConditionIds = rule.Conditions.Select(condition => condition.Id).ToHashSet();
            var invalidConditionId = updateDTO.Conditions
                .Where(condition => condition.Id.HasValue)
                .Select(condition => condition.Id!.Value)
                .FirstOrDefault(conditionId => !existingConditionIds.Contains(conditionId));
            if (invalidConditionId != 0)
            {
                throw new ValidationFailureException(nameof(updateDTO.Conditions),
                    $"Condition {invalidConditionId} does not belong to this rule.");
            }

            // Lock MatchMode check if it has executed successfully
            if (rule.MatchMode != updateDTO.MatchMode)
            {
                var hasExecutedSuccessfully = await _unitOfWork.Repository<TopupExecutionTarget>()
                    .Query()
                    .AnyAsync(t => t.TopupExecution.TopupRuleId == id && t.Status == TopupTargetStatus.Success, cancellationToken);

                if (hasExecutedSuccessfully)
                {
                    throw new ValidationFailureException("MatchMode", "Cannot change Match Mode because the rule has already executed successfully.");
                }
            }

            // Check name conflict
            if (!string.Equals(rule.RuleName, updateDTO.RuleName.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var nameConflict = await _repository.Query()
                    .AnyAsync(r => r.Id != id && r.RuleName == updateDTO.RuleName.Trim(), cancellationToken);

                if (nameConflict)
                {
                    throw new ValidationFailureException("RuleName", "A Top-up rule with this name already exists.");
                }
            }

            // Capture old values for audit log
            var oldPayload = new
            {
                RuleName = rule.RuleName,
                Type = rule.Type.ToString(),
                MatchMode = rule.MatchMode.ToString(),
                TopupAmount = rule.TopupAmount,
                Status = rule.Status.ToString(),
                Conditions = rule.Conditions.Select(c => new
                {
                    c.Id,
                    c.Field,
                    c.Operator,
                    c.ValueText,
                    c.ValueNumber,
                    c.ConditionAmount,
                    c.DisplayOrder
                }).ToList()
            };

            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                // Update rule metadata
                rule.RuleName = updateDTO.RuleName.Trim();
                rule.Type = updateDTO.Type;
                rule.MatchMode = updateDTO.MatchMode;
                rule.TopupAmount = updateDTO.TopupAmount;
                rule.Status = updateDTO.Status;

                // Synchronize Conditions
                var newConditions = updateDTO.Conditions ?? [];
                var existingConditions = rule.Conditions.ToList();

                // Identify conditions to delete
                var incomingIds = newConditions.Where(c => c.Id.HasValue).Select(c => c.Id!.Value).ToHashSet();
                var toDelete = existingConditions.Where(c => !incomingIds.Contains(c.Id)).ToList();
                foreach (var cond in toDelete)
                {
                    rule.Conditions.Remove(cond);
                }

                // Identify conditions to update or add
                foreach (var incoming in newConditions)
                {
                    if (incoming.Id.HasValue)
                    {
                        var existing = existingConditions.FirstOrDefault(c => c.Id == incoming.Id.Value);
                        if (existing != null)
                        {
                            existing.Field = incoming.Field;
                            existing.Operator = incoming.Operator;
                            existing.ValueText = incoming.ValueText;
                            existing.ValueNumber = incoming.ValueNumber;
                            existing.ConditionAmount = incoming.ConditionAmount;
                            existing.DisplayOrder = incoming.DisplayOrder;
                        }
                    }
                    else
                    {
                        rule.Conditions.Add(new TopupRuleCondition
                        {
                            TopupRuleId = rule.Id,
                            Field = incoming.Field,
                            Operator = incoming.Operator,
                            ValueText = incoming.ValueText,
                            ValueNumber = incoming.ValueNumber,
                            ConditionAmount = incoming.ConditionAmount,
                            DisplayOrder = incoming.DisplayOrder
                        });
                    }
                }

                rule.TryValidate();
                foreach (var condition in rule.Conditions)
                    condition.TryValidate();
                await UniqueConstraintValidator.ValidateAsync(_repository, rule, rule.Id, token);

                _repository.Update(rule);
            }, cancellationToken);

            var updatedRule = await GetByIdAsync(id, cancellationToken);

            var auditPayload = new
            {
                Old = oldPayload,
                New = updatedRule
            };

            await _auditLogWriter.LogAsync(
                AuditLogCategory.TopupConfig,
                "UpdateRule",
                payloadJson: JsonSerializer.Serialize(auditPayload),
                cancellationToken: cancellationToken
            );

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return updatedRule;
        }

        public async Task UpdateRulesStatusAsync(BatchUpdateTopupRuleStatusDTO dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (dto.Ids.Count == 0) return;

            var rules = await _repository.Query()
                .Where(r => dto.Ids.Contains(r.Id))
                .ToListAsync(cancellationToken);

            if (rules.Count != dto.Ids.Distinct().Count())
            {
                throw new ValidationFailureException(nameof(dto.Ids), "One or more Top-up rules do not exist.");
            }

            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                foreach (var rule in rules)
                {
                    if (rule.Status == dto.Status)
                    {
                        continue;
                    }

                    var oldStatus = rule.Status;
                    rule.Status = dto.Status;

                    _repository.Update(rule);

                    // Audit Log
                    var auditPayload = new
                    {
                        RuleId = rule.Id,
                        RuleName = rule.RuleName,
                        OldStatus = oldStatus.ToString(),
                        NewStatus = rule.Status.ToString()
                    };

                    string action = rule.Status is TopupRuleStatus.Active ? "ActivateRule" : "InactivateRule";

                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.TopupConfig,
                        action,
                        payloadJson: JsonSerializer.Serialize(auditPayload),
                        cancellationToken: token
                    );
                }

                await _unitOfWork.SaveChangeAsync(token);
            }, cancellationToken);
        }

        // DTO annotations handle: Required/MaxLength on RuleName, EnumDefined on Type/MatchMode/Status,
        // MinLength(1) on Conditions, EnumDefined on Field/Operator, Range(>=0) on DisplayOrder.
        // Only cross-field and field-specific business rules remain here.

        private static void ValidateRule(TopupMatchMode matchMode, decimal? topupAmount, IReadOnlyList<CreateTopupRuleConditionDTO> conditions)
        {
            var errors = new Dictionary<string, string>();
            ValidateAmountMatchMode(matchMode, topupAmount, errors);
            ValidateConditions(conditions.Select(c => (c.Field, c.Operator, c.ValueText, c.ValueNumber, c.ConditionAmount, c.DisplayOrder)).ToList(), matchMode, errors);
            if (errors.Count != 0) throw new ValidationFailureException(errors);
        }

        private static void ValidateRule(TopupMatchMode matchMode, decimal? topupAmount, IReadOnlyList<UpdateTopupRuleConditionDTO> conditions)
        {
            var errors = new Dictionary<string, string>();
            ValidateAmountMatchMode(matchMode, topupAmount, errors);
            ValidateConditions(conditions.Select(c => (c.Field, c.Operator, c.ValueText, c.ValueNumber, c.ConditionAmount, c.DisplayOrder)).ToList(), matchMode, errors);
            if (errors.Count != 0) throw new ValidationFailureException(errors);
        }

        private static void ValidateAmountMatchMode(TopupMatchMode matchMode, decimal? topupAmount, Dictionary<string, string> errors)
        {
            if (matchMode == TopupMatchMode.And && (topupAmount == null || topupAmount <= 0))
                errors[nameof(TopupRule.TopupAmount)] = "Top-up amount must be a positive number for AND match mode.";
            else if (matchMode == TopupMatchMode.Or && topupAmount != null)
                errors[nameof(TopupRule.TopupAmount)] = "Top-up amount must be null for OR match mode.";
        }

        private static void ValidateConditions(
            List<(TopupRuleConditionField Field, TopupRuleConditionOperator Operator, string? ValueText, decimal? ValueNumber, decimal? ConditionAmount, int DisplayOrder)> conditions,
            TopupMatchMode matchMode,
            Dictionary<string, string> errors)
        {
            var duplicateKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var displayOrders = new HashSet<int>();

            for (var i = 0; i < conditions.Count; i++)
            {
                var (field, op, valueText, valueNumber, conditionAmount, displayOrder) = conditions[i];
                var prefix = $"Conditions[{i}]";

                // Cross-field: ConditionAmount vs MatchMode
                if (matchMode == TopupMatchMode.And && conditionAmount != null)
                    errors[$"{prefix}.{nameof(TopupRuleCondition.ConditionAmount)}"] = "Condition amount must be null for AND match mode.";
                else if (matchMode == TopupMatchMode.Or && (conditionAmount == null || conditionAmount <= 0))
                    errors[$"{prefix}.{nameof(TopupRuleCondition.ConditionAmount)}"] = "Condition amount must be a positive number for OR match mode.";

                // Field-specific rules
                switch (field)
                {
                    case TopupRuleConditionField.Age:
                        if (valueNumber is null or < 0)
                            errors[$"{prefix}.{nameof(TopupRuleCondition.ValueNumber)}"] = "Age requires a non-negative value.";
                        else if (valueNumber % 1 != 0)
                            errors[$"{prefix}.{nameof(TopupRuleCondition.ValueNumber)}"] = "Age must be a whole number.";
                        break;

                    case TopupRuleConditionField.Balance:
                        if (valueNumber is null or < 0)
                            errors[$"{prefix}.{nameof(TopupRuleCondition.ValueNumber)}"] = "Account balance requires a non-negative value.";
                        break;

                    case TopupRuleConditionField.SchoolingStatus:
                        if (op is not TopupRuleConditionOperator.Equals and not TopupRuleConditionOperator.NotEquals)
                            errors[$"{prefix}.{nameof(TopupRuleCondition.Operator)}"] = "Schooling status only supports Equals or NotEquals.";
                        if (!string.Equals(valueText?.Trim(), "Enrolled", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(valueText?.Trim(), "Not Enrolled", StringComparison.OrdinalIgnoreCase))
                            errors[$"{prefix}.{nameof(TopupRuleCondition.ValueText)}"] = "Schooling status must be Enrolled or Not Enrolled.";
                        break;
                }

                // Duplicate condition check
                var key = $"{field}|{op}|{valueNumber}|{valueText?.Trim()}";
                if (!duplicateKeys.Add(key))
                    errors[prefix] = "Duplicate conditions are not allowed.";

                // Unique DisplayOrder check
                if (!displayOrders.Add(displayOrder))
                    errors[$"{prefix}.{nameof(TopupRuleCondition.DisplayOrder)}"] = "Display order must be unique within a rule.";
            }
        }
    }
}