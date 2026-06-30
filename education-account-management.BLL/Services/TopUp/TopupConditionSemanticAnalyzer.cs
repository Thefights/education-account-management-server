using DTOs.TopUp;

namespace Services.TopUp
{
    internal static class TopupConditionSemanticAnalyzer
    {
        private const int MaximumScenarioCount = 1000;
        private const decimal BalanceStep = 0.01m;

        public static IReadOnlyList<string> Analyze(TopupConditionGroupRequestDTO root)
        {
            var scenarioCount = 0;
            var exceededLimit = false;
            var scenarios = BuildScenarioCases(root, ref scenarioCount, ref exceededLimit);
            if (exceededLimit)
                return [$"Condition logic cannot expand to more than {MaximumScenarioCount} scenarios."];

            var errors = new List<string>();
            for (var index = 0; index < scenarios.Count; index++)
            {
                foreach (var fieldGroup in scenarios[index].GroupBy(condition => condition.Field))
                {
                    if (IsSatisfiable(fieldGroup.Key, fieldGroup.ToList())) continue;
                    var conditionNumbers = string.Join(", ", fieldGroup
                        .Select(condition => condition.DisplayOrder + 1)
                        .Distinct()
                        .OrderBy(number => number));
                    errors.Add(
                        $"Scenario {index + 1} can never match because {fieldGroup.Key} conditions {conditionNumbers} conflict.");
                }
            }
            return errors;
        }

        private static List<List<TopupConditionRequestDTO>> BuildScenarioCases(
            TopupConditionGroupRequestDTO group,
            ref int scenarioCount,
            ref bool exceededLimit)
        {
            var items = new List<List<List<TopupConditionRequestDTO>>>();
            items.AddRange(group.Conditions.Select(condition =>
                new List<List<TopupConditionRequestDTO>> { new() { condition } }));
            foreach (var child in group.Groups)
                items.Add(BuildScenarioCases(child, ref scenarioCount, ref exceededLimit));

            if (exceededLimit || items.Count == 0) return [];
            if (group.LogicalOperator == TopupLogicalOperator.Or)
            {
                var union = items.SelectMany(item => item).ToList();
                scenarioCount += union.Count;
                if (scenarioCount > MaximumScenarioCount) exceededLimit = true;
                return exceededLimit ? [] : union;
            }

            var cases = new List<List<TopupConditionRequestDTO>>
            {
                new List<TopupConditionRequestDTO>()
            };
            foreach (var item in items)
            {
                var combined = new List<List<TopupConditionRequestDTO>>();
                foreach (var current in cases)
                foreach (var branch in item)
                {
                    if (++scenarioCount > MaximumScenarioCount)
                    {
                        exceededLimit = true;
                        return [];
                    }
                    combined.Add([.. current, .. branch]);
                }
                cases = combined;
            }
            return cases;
        }

        private static bool IsSatisfiable(
            TopupConditionField field,
            IReadOnlyList<TopupConditionRequestDTO> conditions) => field switch
        {
            TopupConditionField.Age => IsAgeSatisfiable(conditions),
            TopupConditionField.Balance => IsBalanceSatisfiable(conditions),
            TopupConditionField.SchoolingStatus => IsTextSatisfiable(conditions),
            _ => true
        };

        private static bool IsAgeSatisfiable(IReadOnlyList<TopupConditionRequestDTO> conditions)
        {
            for (var age = TopupEligibilityPolicy.MinimumAge;
                 age <= TopupEligibilityPolicy.MaximumAge;
                 age++)
            {
                if (conditions.All(condition => MatchesNumber(age, condition))) return true;
            }
            return false;
        }

        private static bool IsBalanceSatisfiable(IReadOnlyList<TopupConditionRequestDTO> conditions)
        {
            var lower = 0m;
            var upper = TopupEligibilityPolicy.MaximumBalance;
            var excluded = new HashSet<decimal>();

            foreach (var condition in conditions)
            {
                var value = condition.ValueNumber!.Value;
                switch (condition.Operator)
                {
                    case TopupConditionOperator.Equals:
                        lower = Math.Max(lower, value);
                        upper = Math.Min(upper, value);
                        break;
                    case TopupConditionOperator.NotEquals:
                        excluded.Add(value);
                        break;
                    case TopupConditionOperator.GreaterThan:
                        lower = Math.Max(lower, value + BalanceStep);
                        break;
                    case TopupConditionOperator.GreaterThanOrEqual:
                        lower = Math.Max(lower, value);
                        break;
                    case TopupConditionOperator.LessThan:
                        upper = Math.Min(upper, value - BalanceStep);
                        break;
                    case TopupConditionOperator.LessThanOrEqual:
                        upper = Math.Min(upper, value);
                        break;
                    case TopupConditionOperator.Between:
                        lower = Math.Max(lower, value);
                        upper = Math.Min(upper, condition.ValueNumberTo!.Value);
                        break;
                }
            }

            if (lower > upper) return false;
            var excludedInRange = excluded.Count(value => value >= lower && value <= upper);
            var availableValueCount = decimal.Floor((upper - lower) / BalanceStep) + 1;
            return availableValueCount > excludedInRange;
        }

        private static bool IsTextSatisfiable(IReadOnlyList<TopupConditionRequestDTO> conditions)
        {
            var equals = conditions
                .Where(condition => condition.Operator == TopupConditionOperator.Equals)
                .Select(condition => condition.ValueText!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (equals.Count > 1) return false;
            if (equals.Count == 0) return true;
            return !conditions.Any(condition =>
                condition.Operator == TopupConditionOperator.NotEquals &&
                string.Equals(condition.ValueText?.Trim(), equals[0], StringComparison.OrdinalIgnoreCase));
        }

        private static bool MatchesNumber(decimal current, TopupConditionRequestDTO condition)
        {
            var expected = condition.ValueNumber!.Value;
            return condition.Operator switch
            {
                TopupConditionOperator.Equals => current == expected,
                TopupConditionOperator.NotEquals => current != expected,
                TopupConditionOperator.GreaterThan => current > expected,
                TopupConditionOperator.GreaterThanOrEqual => current >= expected,
                TopupConditionOperator.LessThan => current < expected,
                TopupConditionOperator.LessThanOrEqual => current <= expected,
                TopupConditionOperator.Between => current >= expected &&
                                                  current <= condition.ValueNumberTo!.Value,
                _ => false
            };
        }
    }
}
