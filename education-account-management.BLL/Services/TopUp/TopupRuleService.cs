using DTOs.TopUp;
using Interfaces.Audit;
using Interfaces.TopUp;
using Services.Base;
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
                rule.TryValidate();

                // The parent key is assigned on save; DTO validation and ValidateRule cover condition fields.
                await UniqueConstraintValidator.ValidateAsync(_repository, rule, cancellationToken: token);

                var resultEntity = await _repository.AddAsync(rule, token);
                await _unitOfWork.SaveChangeAsync(token);

                return resultEntity.Id;
            }, cancellationToken);

            var createdRule = await GetByIdAsync(ruleId, cancellationToken);

            await _auditLogWriter.LogAsync(
                AuditLogCategory.TopupConfig,
                "CreateRule",
                cancellationToken: cancellationToken
            );

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return createdRule;
        }

        public override async Task<GetTopupRuleDTO> UpdateAsync(int id, UpdateTopupRuleDTO updateDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);

            var rule = await _repository.Query()
                .Include(r => r.Conditions)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken)
                ?? throw new DataNotFoundException(typeof(TopupRule), id);

            ValidateRule(updateDTO.MatchMode, updateDTO.TopupAmount, updateDTO.Conditions);

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

            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                // Update rule metadata
                rule.RuleName = updateDTO.RuleName;
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
                foreach (var condition in rule.Conditions) condition.TryValidate();
                await UniqueConstraintValidator.ValidateAsync(_repository, rule, rule.Id, token);

                _repository.Update(rule);
            }, cancellationToken);

            var updatedRule = await GetByIdAsync(id, cancellationToken);

            await _auditLogWriter.LogAsync(
                AuditLogCategory.TopupConfig,
                "UpdateRule",
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

                    string action = rule.Status is TopupRuleStatus.Active ? "ActivateRule" : "InactivateRule";

                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.TopupConfig,
                        action,
                        cancellationToken: token
                    );
                }

            }, cancellationToken);
        }

        public override async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var rule = await GetByIdAsync(id, cancellationToken);
            await base.DeleteAsync(id, cancellationToken);
            await _auditLogWriter.LogAsync(
                AuditLogCategory.TopupConfig,
                "DeleteRule",
                cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        // DTO annotations handle: Required/MaxLength on RuleName, EnumDefined on Type/MatchMode/Status,
        // MinLength(1) on Conditions, EnumDefined on Field/Operator, Range(>=0) on DisplayOrder.
        // Only cross-field and field-specific business rules remain here.

        private static void ValidateRule(TopupMatchMode matchMode, decimal? topupAmount, IReadOnlyList<CreateTopupRuleConditionDTO> conditions)
        {
            var errors = new Dictionary<string, string>();
            if (conditions.Count == 0) errors[nameof(conditions)] = "At least one condition is required.";
            ValidateAmountMatchMode(matchMode, topupAmount, errors);
            ValidateConditions(conditions.Select(c => (c.Field, c.Operator, c.ValueText, c.ValueNumber, c.ConditionAmount, c.DisplayOrder)).ToList(), matchMode, errors);
            if (errors.Count != 0) throw new ValidationFailureException(errors);
        }

        private static void ValidateRule(TopupMatchMode matchMode, decimal? topupAmount, IReadOnlyList<UpdateTopupRuleConditionDTO> conditions)
        {
            var errors = new Dictionary<string, string>();
            if (conditions.Count == 0) errors[nameof(conditions)] = "At least one condition is required.";
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

                if (displayOrder < 0)
                    errors[$"{prefix}.{nameof(TopupRuleCondition.DisplayOrder)}"] = "Display order cannot be negative.";

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
