using DTOs.FasSchemes;

namespace Services.FasSchemes
{
    internal static class FasConditionTreeUtility
    {
        private const int MaximumDepth = 2;
        private const int MaximumNodeCount = 100;

        public static void Validate(FasConditionGroupRequestDTO root)
        {
            ArgumentNullException.ThrowIfNull(root);
            var errors = new Dictionary<string, string>();
            var nodeCount = 0;
            ValidateGroup(root, nameof(root), 1, ref nodeCount, errors);
            if (nodeCount > MaximumNodeCount)
                errors[nameof(root)] = $"A condition tree cannot contain more than {MaximumNodeCount} nodes.";
            if (errors.Count != 0) throw new ValidationFailureException(errors);
        }

        private static void ValidateGroup(
            FasConditionGroupRequestDTO group,
            string path,
            int depth,
            ref int nodeCount,
            Dictionary<string, string> errors)
        {
            nodeCount++;
            if (depth > MaximumDepth)
            {
                errors[path] = $"A condition tree cannot be deeper than {MaximumDepth} levels.";
                return;
            }
            if (!Enum.IsDefined(group.LogicalOperator))
                errors[$"{path}.{nameof(group.LogicalOperator)}"] = "Logical operator is invalid.";
            if (group.DisplayOrder < 0)
                errors[$"{path}.{nameof(group.DisplayOrder)}"] = "Display order cannot be negative.";
            if (group.Conditions.Count + group.Groups.Count == 0)
                errors[path] = "A condition group must contain at least one condition or child group.";

            var displayOrders = new HashSet<int>();
            for (var index = 0; index < group.Conditions.Count; index++)
            {
                var condition = group.Conditions[index];
                var conditionPath = $"{path}.{nameof(group.Conditions)}[{index}]";
                nodeCount++;
                if (!displayOrders.Add(condition.DisplayOrder))
                    errors[$"{conditionPath}.{nameof(condition.DisplayOrder)}"] = "Display order must be unique within a group.";
                ValidateCondition(condition, conditionPath, errors);
            }

            for (var index = 0; index < group.Groups.Count; index++)
            {
                var child = group.Groups[index];
                var childPath = $"{path}.{nameof(group.Groups)}[{index}]";
                if (!displayOrders.Add(child.DisplayOrder))
                    errors[$"{childPath}.{nameof(child.DisplayOrder)}"] = "Display order must be unique within a group.";
                ValidateGroup(child, childPath, depth + 1, ref nodeCount, errors);
            }
        }

        private static void ValidateCondition(
            FasConditionRequestDTO condition,
            string path,
            Dictionary<string, string> errors)
        {
            if (condition.DisplayOrder < 0)
                errors[$"{path}.{nameof(condition.DisplayOrder)}"] = "Display order cannot be negative.";
            if (!Enum.IsDefined(condition.Field))
                errors[$"{path}.{nameof(condition.Field)}"] = "Condition field is invalid.";
            if (!Enum.IsDefined(condition.Operator))
                errors[$"{path}.{nameof(condition.Operator)}"] = "Condition operator is invalid.";

            if (condition.Field is FasConditionField.StudentAge or FasConditionField.GrossHouseholdIncome or FasConditionField.PerCapitaIncome)
            {
                if (condition.ValueNumber is null or < 0)
                    errors[$"{path}.{nameof(condition.ValueNumber)}"] = "A non-negative numeric value is required.";
                if (condition.Nationality != null)
                    errors[$"{path}.{nameof(condition.Nationality)}"] = "Nationality must be null for numeric fields.";
                if (condition.Field == FasConditionField.StudentAge && condition.ValueNumber.HasValue && condition.ValueNumber.Value % 1 != 0)
                    errors[$"{path}.{nameof(condition.ValueNumber)}"] = "Age must be a whole number.";

                if (condition.Operator == FasConditionOperator.Between)
                {
                    if (condition.ValueNumberTo is null or < 0)
                        errors[$"{path}.{nameof(condition.ValueNumberTo)}"] = "Between requires a non-negative upper value.";
                    else if (condition.ValueNumber.HasValue && condition.ValueNumberTo < condition.ValueNumber)
                        errors[$"{path}.{nameof(condition.ValueNumberTo)}"] = "Between upper value must be greater than or equal to the lower value.";
                    if (condition.Field == FasConditionField.StudentAge && condition.ValueNumberTo.HasValue && condition.ValueNumberTo.Value % 1 != 0)
                        errors[$"{path}.{nameof(condition.ValueNumberTo)}"] = "Age must be a whole number.";
                }
                else if (condition.ValueNumberTo != null)
                {
                    errors[$"{path}.{nameof(condition.ValueNumberTo)}"] = "Upper value is only allowed for Between.";
                }
                return;
            }

            // Nationality fields: StudentNationality or GuardianNationality
            if (condition.Operator is not FasConditionOperator.Equal and not FasConditionOperator.NotEqual)
                errors[$"{path}.{nameof(condition.Operator)}"] = "Nationality fields only support Equal or NotEqual.";
            if (!condition.Nationality.HasValue || !Enum.IsDefined(condition.Nationality.Value))
                errors[$"{path}.{nameof(condition.Nationality)}"] = "Nationality is required.";
            if (condition.ValueNumber != null || condition.ValueNumberTo != null)
                errors[$"{path}.{nameof(condition.ValueNumber)}"] = "Numeric values must be null for nationality fields.";
        }
    }
}
