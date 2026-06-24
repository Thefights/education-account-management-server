using DTOs.TopUp;

namespace Services.TopUp
{
    internal static class TopupConditionTreeUtility
    {
        // Root group plus one child-group level.
        private const int MaximumDepth = 2;
        private const int MaximumNodeCount = 100;

        public static void Validate(TopupConditionGroupRequestDTO root)
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
            TopupConditionGroupRequestDTO group,
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
            TopupConditionRequestDTO condition,
            string path,
            Dictionary<string, string> errors)
        {
            if (condition.DisplayOrder < 0)
                errors[$"{path}.{nameof(condition.DisplayOrder)}"] = "Display order cannot be negative.";
            if (!Enum.IsDefined(condition.Field))
                errors[$"{path}.{nameof(condition.Field)}"] = "Condition field is invalid.";
            if (!Enum.IsDefined(condition.Operator))
                errors[$"{path}.{nameof(condition.Operator)}"] = "Condition operator is invalid.";

            if (condition.Field is TopupConditionField.Age or TopupConditionField.Balance)
            {
                if (condition.ValueNumber is null or < 0)
                    errors[$"{path}.{nameof(condition.ValueNumber)}"] = "A non-negative numeric value is required.";
                if (condition.ValueText != null)
                    errors[$"{path}.{nameof(condition.ValueText)}"] = "Text value must be null for numeric fields.";
                if (condition.Field == TopupConditionField.Age && condition.ValueNumber % 1 != 0)
                    errors[$"{path}.{nameof(condition.ValueNumber)}"] = "Age must be a whole number.";

                if (condition.Operator == TopupConditionOperator.Between)
                {
                    if (condition.ValueNumberTo is null or < 0)
                        errors[$"{path}.{nameof(condition.ValueNumberTo)}"] = "Between requires a non-negative upper value.";
                    else if (condition.ValueNumber.HasValue && condition.ValueNumberTo < condition.ValueNumber)
                        errors[$"{path}.{nameof(condition.ValueNumberTo)}"] = "Between upper value must be greater than or equal to the lower value.";
                    if (condition.Field == TopupConditionField.Age && condition.ValueNumberTo % 1 != 0)
                        errors[$"{path}.{nameof(condition.ValueNumberTo)}"] = "Age must be a whole number.";
                }
                else if (condition.ValueNumberTo != null)
                {
                    errors[$"{path}.{nameof(condition.ValueNumberTo)}"] = "Upper value is only allowed for Between.";
                }
                return;
            }

            if (condition.Operator is not TopupConditionOperator.Equals and not TopupConditionOperator.NotEquals)
                errors[$"{path}.{nameof(condition.Operator)}"] = "Schooling status only supports Equals or NotEquals.";
            if (!string.Equals(condition.ValueText, "Enrolled", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(condition.ValueText, "Not Enrolled", StringComparison.OrdinalIgnoreCase))
                errors[$"{path}.{nameof(condition.ValueText)}"] = "Schooling status must be Enrolled or Not Enrolled.";
            if (condition.ValueNumber != null || condition.ValueNumberTo != null)
                errors[$"{path}.{nameof(condition.ValueNumber)}"] = "Numeric values must be null for schooling status.";
        }
    }
}
