using DTOs.FasSchemes;

namespace Mappers.FasSchemes
{
    public static class FasConditionTreeMapper
    {
        public static FasSchemeConditionGroup MapFasGroup(
            FasConditionGroupRequestDTO source,
            int fasSchemeId,
            FasSchemeConditionGroup? parent = null)
        {
            var group = new FasSchemeConditionGroup
            {
                FasSchemeId = fasSchemeId,
                ParentGroup = parent,
                LogicalOperator = source.LogicalOperator,
                DisplayOrder = source.DisplayOrder,
                Conditions = source.Conditions.Select(condition => new FasSchemeCondition
                {
                    Field = condition.Field,
                    Operator = condition.Operator,
                    ValueNumber = condition.ValueNumber,
                    ValueNumberTo = condition.ValueNumberTo,
                    ValueText = MapCountryIdToValueText(condition.Field, condition.CountryId),
                    DisplayOrder = condition.DisplayOrder
                }).ToList()
            };
            group.ChildGroups = source.Groups
                .Select(child => MapFasGroup(child, fasSchemeId, group))
                .ToList();
            return group;
        }

        public static FasConditionGroupDTO MapFasTree(IReadOnlyCollection<FasSchemeConditionGroup> groups)
        {
            if (groups.Count == 0) return null!;
            var root = groups.SingleOrDefault(group => group.ParentGroupId == null);
            if (root == null) return null!;
            var children = groups.Where(group => group.ParentGroupId.HasValue)
                .GroupBy(group => group.ParentGroupId!.Value)
                .ToDictionary(group => group.Key, group => group.ToList());
            return MapFasGroup(root, children);
        }

        private static FasConditionGroupDTO MapFasGroup(
            FasSchemeConditionGroup group,
            IReadOnlyDictionary<int, List<FasSchemeConditionGroup>> children)
        {
            return new FasConditionGroupDTO
            {
                Id = group.Id,
                LogicalOperator = group.LogicalOperator.ToString(),
                DisplayOrder = group.DisplayOrder,
                Conditions = group.Conditions.OrderBy(condition => condition.DisplayOrder).Select(condition => new FasConditionDTO
                {
                    Id = condition.Id,
                    Field = condition.Field.ToString(),
                    Operator = condition.Operator.ToString(),
                    ValueNumber = condition.ValueNumber,
                    ValueNumberTo = condition.ValueNumberTo,
                    CountryId = MapValueTextToCountryId(condition.Field, condition.ValueText),
                    DisplayOrder = condition.DisplayOrder
                }).ToList(),
                Groups = children.GetValueOrDefault(group.Id, [])
                    .OrderBy(child => child.DisplayOrder)
                    .Select(child => MapFasGroup(child, children))
                    .ToList()
            };
        }


        private static string? MapCountryIdToValueText(FasConditionField field, int? countryId)
        {
            if (field is not (FasConditionField.StudentNationality or FasConditionField.GuardianNationality))
            {
                return null;
            }

            return countryId switch
            {
                1 => "Singapore",
                2 => "Other",
                _ => null
            };        }

        private static int? MapValueTextToCountryId(FasConditionField field, string? valueText)
        {
            if (field is not (FasConditionField.StudentNationality or FasConditionField.GuardianNationality))
            {
                return null;
            }

            var normalized = valueText?.Trim();
            if (string.IsNullOrEmpty(normalized))
            {
                return null;
            }

            if (string.Equals(normalized, "Singapore", StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }

            if (string.Equals(normalized, "Other", StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }

            return null;

        }
    }
}
