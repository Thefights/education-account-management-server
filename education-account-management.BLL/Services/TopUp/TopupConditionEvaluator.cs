namespace Services.TopUp
{
    public static class TopupConditionEvaluator
    {
        public sealed record ConditionNode(
            TopupConditionField Field,
            TopupConditionOperator Operator,
            string? ValueText,
            decimal? ValueNumber,
            decimal? ValueNumberTo,
            int DisplayOrder);

        public sealed record GroupNode(
            TopupLogicalOperator LogicalOperator,
            int DisplayOrder,
            IReadOnlyList<Node> Nodes);

        public sealed record Node(int DisplayOrder, ConditionNode? Condition, GroupNode? Group);

        public sealed record EvaluationResult(bool IsSatisfied, IReadOnlyList<ConditionNode> MatchedConditions);

        public static GroupNode BuildSystemTree(IEnumerable<SystemTopupConditionGroup> source)
        {
            var groups = source.ToList();
            var root = groups.Single(group => group.ParentGroupId == null);
            var children = groups.Where(group => group.ParentGroupId.HasValue)
                .GroupBy(group => group.ParentGroupId!.Value)
                .ToDictionary(group => group.Key, group => group.ToList());
            return BuildSystemGroup(root, children);
        }

        public static GroupNode BuildScheduleTree(IEnumerable<ScheduleTopUpConditionGroup> source)
        {
            var groups = source.ToList();
            var root = groups.Single(group => group.ParentGroupId == null);
            var children = groups.Where(group => group.ParentGroupId.HasValue)
                .GroupBy(group => group.ParentGroupId!.Value)
                .ToDictionary(group => group.Key, group => group.ToList());
            return BuildScheduleGroup(root, children);
        }

        public static EvaluationResult Evaluate(EducationAccount account, GroupNode root, DateTime now)
        {
            var age = CalculateAge(account.Citizen.DateOfBirth, DateOnly.FromDateTime(now));
            if (!TopupEligibilityPolicy.IsEligibleAge(age))
                return new EvaluationResult(false, []);

            var matched = new List<ConditionNode>();
            var isSatisfied = EvaluateGroup(account, root, now, matched);
            return new EvaluationResult(isSatisfied, isSatisfied ? matched : []);
        }

        private static bool EvaluateGroup(
            EducationAccount account,
            GroupNode group,
            DateTime now,
            List<ConditionNode> matched)
        {
            foreach (var node in group.Nodes.OrderBy(node => node.DisplayOrder))
            {
                var branchMatches = new List<ConditionNode>();
                var isSatisfied = node.Condition != null
                    ? EvaluateCondition(account, node.Condition, now, branchMatches)
                    : EvaluateGroup(account, node.Group!, now, branchMatches);

                if (group.LogicalOperator == TopupLogicalOperator.And)
                {
                    if (!isSatisfied) return false;
                    matched.AddRange(branchMatches);
                    continue;
                }

                if (isSatisfied)
                {
                    matched.AddRange(branchMatches);
                    return true;
                }
            }

            return group.LogicalOperator == TopupLogicalOperator.And;
        }

        private static bool EvaluateCondition(
            EducationAccount account,
            ConditionNode condition,
            DateTime now,
            List<ConditionNode> matched)
        {
            decimal? currentNumber = condition.Field switch
            {
                TopupConditionField.Age => CalculateAge(account.Citizen.DateOfBirth, DateOnly.FromDateTime(now)),
                TopupConditionField.Balance => account.EducationCreditBalance,
                _ => null
            };

            bool result;
            if (currentNumber.HasValue)
            {
                result = Compare(currentNumber.Value, condition);
            }
            else
            {
                var currentText = account.Citizen.SchoolingStatus?.Trim();
                var expectedText = condition.ValueText?.Trim();
                result = condition.Operator switch
                {
                    TopupConditionOperator.Equals => string.Equals(currentText, expectedText, StringComparison.OrdinalIgnoreCase),
                    TopupConditionOperator.NotEquals => !string.Equals(currentText, expectedText, StringComparison.OrdinalIgnoreCase),
                    _ => false
                };
            }

            if (result) matched.Add(condition);
            return result;
        }

        private static bool Compare(decimal current, ConditionNode condition)
        {
            if (!condition.ValueNumber.HasValue) return false;
            var expected = condition.ValueNumber.Value;
            return condition.Operator switch
            {
                TopupConditionOperator.Equals => current == expected,
                TopupConditionOperator.NotEquals => current != expected,
                TopupConditionOperator.GreaterThan => current > expected,
                TopupConditionOperator.GreaterThanOrEqual => current >= expected,
                TopupConditionOperator.LessThan => current < expected,
                TopupConditionOperator.LessThanOrEqual => current <= expected,
                TopupConditionOperator.Between => condition.ValueNumberTo.HasValue &&
                                                  current >= expected && current <= condition.ValueNumberTo.Value,
                _ => false
            };
        }

        private static int CalculateAge(DateOnly dateOfBirth, DateOnly today)
        {
            var age = today.Year - dateOfBirth.Year;
            if (today < dateOfBirth.AddYears(age)) age--;
            return age;
        }

        private static GroupNode BuildSystemGroup(
            SystemTopupConditionGroup group,
            IReadOnlyDictionary<int, List<SystemTopupConditionGroup>> children)
        {
            var nodes = group.Conditions.Select(condition => new Node(
                    condition.DisplayOrder,
                    new ConditionNode(condition.Field, condition.Operator, condition.ValueText,
                        condition.ValueNumber, condition.ValueNumberTo, condition.DisplayOrder),
                    null))
                .Concat(children.GetValueOrDefault(group.Id, []).Select(child =>
                    new Node(child.DisplayOrder, null, BuildSystemGroup(child, children))))
                .OrderBy(node => node.DisplayOrder)
                .ToList();
            return new GroupNode(group.LogicalOperator, group.DisplayOrder, nodes);
        }

        private static GroupNode BuildScheduleGroup(
            ScheduleTopUpConditionGroup group,
            IReadOnlyDictionary<int, List<ScheduleTopUpConditionGroup>> children)
        {
            var nodes = group.Conditions.Select(condition => new Node(
                    condition.DisplayOrder,
                    new ConditionNode(condition.Field, condition.Operator, condition.ValueText,
                        condition.ValueNumber, condition.ValueNumberTo, condition.DisplayOrder),
                    null))
                .Concat(children.GetValueOrDefault(group.Id, []).Select(child =>
                    new Node(child.DisplayOrder, null, BuildScheduleGroup(child, children))))
                .OrderBy(node => node.DisplayOrder)
                .ToList();
            return new GroupNode(group.LogicalOperator, group.DisplayOrder, nodes);
        }
    }
}
