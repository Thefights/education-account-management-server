using Enums;
using Models;

namespace Helpers.FasSchemes
{
    public static class FasConditionEvaluator
    {
        public static bool Evaluate(ICollection<FasSchemeConditionGroup>? conditionGroups, int studentAge, bool isSingaporean, NationalityCategory guardianNationality, decimal grossHouseholdIncome, int householdMemberCount)
        {
            if (conditionGroups == null || !conditionGroups.Any())
            {
                return true; 
            }

            var rootGroup = conditionGroups.FirstOrDefault(g => g.ParentGroupId == null);
            if (rootGroup == null)
            {
                return true;
            }

            return EvaluateGroup(rootGroup, studentAge, isSingaporean, guardianNationality, grossHouseholdIncome, householdMemberCount);
        }

        private static bool EvaluateGroup(FasSchemeConditionGroup group, int studentAge, bool isSingaporean, NationalityCategory guardianNationality, decimal grossHouseholdIncome, int householdMemberCount)
        {
            bool result = group.LogicalOperator == TopupLogicalOperator.And;

            if (group.Conditions != null && group.Conditions.Any())
            {
                foreach (var condition in group.Conditions)
                {
                    bool conditionResult = EvaluateCondition(condition, studentAge, isSingaporean, guardianNationality, grossHouseholdIncome, householdMemberCount);
                    if (group.LogicalOperator == TopupLogicalOperator.And)
                    {
                        result = result && conditionResult;
                        if (!result) return false;
                    }
                    else
                    {
                        result = result || conditionResult;
                        if (result) return true;
                    }
                }
            }

            if (group.ChildGroups != null && group.ChildGroups.Any())
            {
                foreach (var childGroup in group.ChildGroups)
                {
                    bool childGroupResult = EvaluateGroup(childGroup, studentAge, isSingaporean, guardianNationality, grossHouseholdIncome, householdMemberCount);
                    if (group.LogicalOperator == TopupLogicalOperator.And)
                    {
                        result = result && childGroupResult;
                        if (!result) return false;
                    }
                    else
                    {
                        result = result || childGroupResult;
                        if (result) return true;
                    }
                }
            }

            return result;
        }

        private static bool EvaluateCondition(FasSchemeCondition condition, int studentAge, bool isSingaporean, NationalityCategory guardianNationality, decimal grossHouseholdIncome, int householdMemberCount)
        {
            decimal valueToCompare = 0;
            string? textToCompare = null;

            switch (condition.Field)
            {
                case FasConditionField.StudentAge:
                    valueToCompare = studentAge;
                    break;
                case FasConditionField.StudentNationality:
                    textToCompare = isSingaporean ? "Singapore" : "Other";
                    break;
                case FasConditionField.GuardianNationality:
                    textToCompare = guardianNationality == NationalityCategory.SingaporeCitizen ? "Singapore" : "Other";
                    break;
                case FasConditionField.GrossHouseholdIncome:
                    valueToCompare = grossHouseholdIncome;
                    break;
                case FasConditionField.PerCapitaIncome:
                    valueToCompare = householdMemberCount > 0 ? grossHouseholdIncome / householdMemberCount : 0;
                    break;
            }

            if (textToCompare != null)
            {
                string conditionText = condition.ValueText ?? "";
                return condition.Operator switch
                {
                    FasConditionOperator.Equal => string.Equals(textToCompare, conditionText, StringComparison.OrdinalIgnoreCase),
                    FasConditionOperator.NotEqual => !string.Equals(textToCompare, conditionText, StringComparison.OrdinalIgnoreCase),
                    _ => false
                };
            }

            decimal targetValue = condition.ValueNumber ?? 0;

            return condition.Operator switch
            {
                FasConditionOperator.Equal => valueToCompare == targetValue,
                FasConditionOperator.NotEqual => valueToCompare != targetValue,
                FasConditionOperator.LessThan => valueToCompare < targetValue,
                FasConditionOperator.LessThanOrEqual => valueToCompare <= targetValue,
                FasConditionOperator.GreaterThan => valueToCompare > targetValue,
                FasConditionOperator.GreaterThanOrEqual => valueToCompare >= targetValue,
                FasConditionOperator.Between => valueToCompare >= targetValue && valueToCompare <= (condition.ValueNumberTo ?? 0),
                _ => false
            };
        }
    }
}
