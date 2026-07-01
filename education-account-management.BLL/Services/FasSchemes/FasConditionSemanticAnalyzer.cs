using DTOs.FasSchemes;

namespace Services.FasSchemes
{
    public static class FasConditionSemanticAnalyzer
    {
        private const decimal MinimumAge = 16m;
        private const decimal MaximumAge = 30m;

        public static void Validate(FasConditionGroupRequestDTO root)
        {
            var scenarios = Expand(root);
            var errors = new Dictionary<string, string>();

            for (var index = 0; index < scenarios.Count; index++)
            {
                var scenario = scenarios[index];
                ValidateNumericScenario(
                    scenario.Where(c => c.Field == FasConditionField.StudentAge),
                    MinimumAge,
                    MaximumAge,
                    $"Scenario[{index}].StudentAge",
                    errors);
                ValidateNumericScenario(
                    scenario.Where(c => c.Field == FasConditionField.GrossHouseholdIncome),
                    0m,
                    null,
                    $"Scenario[{index}].GrossHouseholdIncome",
                    errors);
                ValidateNumericScenario(
                    scenario.Where(c => c.Field == FasConditionField.PerCapitaIncome),
                    0m,
                    null,
                    $"Scenario[{index}].PerCapitaIncome",
                    errors);
                ValidateNationalityScenario(
                    scenario.Where(c => c.Field == FasConditionField.StudentNationality),
                    $"Scenario[{index}].StudentNationality",
                    errors);
                ValidateNationalityScenario(
                    scenario.Where(c => c.Field == FasConditionField.GuardianNationality),
                    $"Scenario[{index}].GuardianNationality",
                    errors);
            }

            if (errors.Count != 0)
            {
                throw new ValidationFailureException(errors);
            }
        }

        private static List<List<FasConditionRequestDTO>> Expand(FasConditionGroupRequestDTO group)
        {
            var ownConditions = group.Conditions
                .Select(condition => new List<FasConditionRequestDTO> { condition })
                .ToList();
            var childScenarios = group.Groups
                .Select(Expand)
                .ToList();

            if (group.LogicalOperator == TopupLogicalOperator.Or)
            {
                var result = new List<List<FasConditionRequestDTO>>();
                result.AddRange(ownConditions);
                result.AddRange(childScenarios.SelectMany(s => s));
                return result.Count == 0
                    ? new List<List<FasConditionRequestDTO>> { new() }
                    : result;
            }

            var combined = new List<List<FasConditionRequestDTO>> { new() };
            foreach (var condition in ownConditions)
            {
                combined = Combine(combined, [condition]);
            }

            foreach (var scenarios in childScenarios)
            {
                combined = Combine(combined, scenarios);
            }

            return combined;
        }

        private static List<List<FasConditionRequestDTO>> Combine(
            List<List<FasConditionRequestDTO>> left,
            List<List<FasConditionRequestDTO>> right)
        {
            var result = new List<List<FasConditionRequestDTO>>();
            foreach (var l in left)
            {
                foreach (var r in right)
                {
                    result.Add(l.Concat(r).ToList());
                }
            }

            return result;
        }

        private static void ValidateNumericScenario(
            IEnumerable<FasConditionRequestDTO> conditions,
            decimal domainMin,
            decimal? domainMax,
            string path,
            Dictionary<string, string> errors)
        {
            var constraint = new NumericConstraint(domainMin, false, domainMax, false);
            foreach (var condition in conditions)
            {
                ApplyNumeric(condition, constraint);
            }

            if (!constraint.IsFeasible())
            {
                errors[path] = "Condition scenario is impossible.";
            }
        }

        private static void ApplyNumeric(FasConditionRequestDTO condition, NumericConstraint constraint)
        {
            var value = condition.ValueNumber ?? 0m;
            switch (condition.Operator)
            {
                case FasConditionOperator.Equal:
                    constraint.EqualValues.Add(value);
                    break;
                case FasConditionOperator.NotEqual:
                    constraint.NotEqualValues.Add(value);
                    break;
                case FasConditionOperator.LessThan:
                    constraint.SetUpper(value, exclusive: true);
                    break;
                case FasConditionOperator.LessThanOrEqual:
                    constraint.SetUpper(value, exclusive: false);
                    break;
                case FasConditionOperator.GreaterThan:
                    constraint.SetLower(value, exclusive: true);
                    break;
                case FasConditionOperator.GreaterThanOrEqual:
                    constraint.SetLower(value, exclusive: false);
                    break;
                case FasConditionOperator.Between:
                    constraint.SetLower(value, exclusive: false);
                    constraint.SetUpper(condition.ValueNumberTo ?? value, exclusive: false);
                    break;
            }
        }

        private static void ValidateNationalityScenario(
            IEnumerable<FasConditionRequestDTO> conditions,
            string path,
            Dictionary<string, string> errors)
        {
            int? required = null;
            var excluded = new HashSet<int>();
            foreach (var condition in conditions)
            {
                var value = condition.CountryId ?? 0;
                if (value is not (1 or 2))
                {
                    errors[path] = "Nationality condition only supports Singapore Citizen or Foreigner.";
                    return;
                }

                if (condition.Operator == FasConditionOperator.Equal)
                {
                    if (required.HasValue && required.Value != value)
                    {
                        errors[path] = "Nationality condition scenario is impossible.";
                        return;
                    }

                    required = value;
                }
                else if (condition.Operator == FasConditionOperator.NotEqual)
                {
                    excluded.Add(value);
                }
            }

            if ((required.HasValue && excluded.Contains(required.Value)) || excluded.Count >= 2)
            {
                errors[path] = "Nationality condition scenario is impossible.";
            }
        }

        private sealed class NumericConstraint(
            decimal lower,
            bool lowerExclusive,
            decimal? upper,
            bool upperExclusive)
        {
            private decimal Lower { get; set; } = lower;
            private bool LowerExclusive { get; set; } = lowerExclusive;
            private decimal? Upper { get; set; } = upper;
            private bool UpperExclusive { get; set; } = upperExclusive;

            public List<decimal> EqualValues { get; } = [];
            public HashSet<decimal> NotEqualValues { get; } = [];

            public void SetLower(decimal value, bool exclusive)
            {
                if (value > Lower || (value == Lower && exclusive && !LowerExclusive))
                {
                    Lower = value;
                    LowerExclusive = exclusive;
                }
            }

            public void SetUpper(decimal value, bool exclusive)
            {
                if (!Upper.HasValue || value < Upper.Value || (value == Upper.Value && exclusive && !UpperExclusive))
                {
                    Upper = value;
                    UpperExclusive = exclusive;
                }
            }

            public bool IsFeasible()
            {
                if (Upper.HasValue)
                {
                    if (Lower > Upper.Value) return false;
                    if (Lower == Upper.Value && (LowerExclusive || UpperExclusive)) return false;
                }

                if (EqualValues.Distinct().Count() > 1) return false;
                if (EqualValues.Count == 1)
                {
                    var value = EqualValues[0];
                    if (value < Lower || (value == Lower && LowerExclusive)) return false;
                    if (Upper.HasValue && (value > Upper.Value || (value == Upper.Value && UpperExclusive))) return false;
                    if (NotEqualValues.Contains(value)) return false;
                }

                if (Upper.HasValue && Lower == Upper.Value && NotEqualValues.Contains(Lower))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
