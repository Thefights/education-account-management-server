using DTOs.TopUp;

namespace Mappers;

public static class TopupConditionTreeMapper
{
    public static SystemTopupConditionGroup MapSystemGroup(
        TopupConditionGroupRequestDTO source,
        int systemTopupId,
        SystemTopupConditionGroup? parent = null)
    {
        var group = new SystemTopupConditionGroup
        {
            SystemTopupId = systemTopupId,
            ParentGroup = parent,
            LogicalOperator = source.LogicalOperator,
            DisplayOrder = source.DisplayOrder,
            Conditions = source.Conditions.Select(condition => new SystemTopupCondition
            {
                Field = condition.Field,
                Operator = condition.Operator,
                ValueText = condition.ValueText,
                ValueNumber = condition.ValueNumber,
                ValueNumberTo = condition.ValueNumberTo,
                DisplayOrder = condition.DisplayOrder
            }).ToList()
        };
        group.ChildGroups = source.Groups
            .Select(child => MapSystemGroup(child, systemTopupId, group))
            .ToList();
        return group;
    }

    public static ScheduleTopUpConditionGroup MapScheduleGroup(
        TopupConditionGroupRequestDTO source,
        int scheduleTopUpId,
        ScheduleTopUpConditionGroup? parent = null)
    {
        var group = new ScheduleTopUpConditionGroup
        {
            ScheduleTopUpId = scheduleTopUpId,
            ParentGroup = parent,
            LogicalOperator = source.LogicalOperator,
            DisplayOrder = source.DisplayOrder,
            Conditions = source.Conditions.Select(condition => new ScheduleTopUpCondition
            {
                Field = condition.Field,
                Operator = condition.Operator,
                ValueText = condition.ValueText,
                ValueNumber = condition.ValueNumber,
                ValueNumberTo = condition.ValueNumberTo,
                DisplayOrder = condition.DisplayOrder
            }).ToList()
        };
        group.ChildGroups = source.Groups
            .Select(child => MapScheduleGroup(child, scheduleTopUpId, group))
            .ToList();
        return group;
    }

    public static TopupConditionGroupDTO MapSystemTree(IReadOnlyCollection<SystemTopupConditionGroup> groups)
    {
        var root = groups.Single(group => group.ParentGroupId == null);
        var children = groups.Where(group => group.ParentGroupId.HasValue)
            .GroupBy(group => group.ParentGroupId!.Value)
            .ToDictionary(group => group.Key, group => group.ToList());
        return MapSystemGroup(root, children);
    }

    public static TopupConditionGroupDTO MapScheduleTree(IReadOnlyCollection<ScheduleTopUpConditionGroup> groups)
    {
        var root = groups.Single(group => group.ParentGroupId == null);
        var children = groups.Where(group => group.ParentGroupId.HasValue)
            .GroupBy(group => group.ParentGroupId!.Value)
            .ToDictionary(group => group.Key, group => group.ToList());
        return MapScheduleGroup(root, children);
    }

    private static TopupConditionGroupDTO MapSystemGroup(
        SystemTopupConditionGroup group,
        IReadOnlyDictionary<int, List<SystemTopupConditionGroup>> children)
    {
        return new TopupConditionGroupDTO
        {
            Id = group.Id,
            LogicalOperator = group.LogicalOperator.ToString(),
            DisplayOrder = group.DisplayOrder,
            Conditions = group.Conditions.OrderBy(condition => condition.DisplayOrder).Select(condition => new TopupConditionDTO
            {
                Id = condition.Id,
                Field = condition.Field.ToString(),
                Operator = condition.Operator.ToString(),
                ValueText = condition.ValueText,
                ValueNumber = condition.ValueNumber,
                ValueNumberTo = condition.ValueNumberTo,
                DisplayOrder = condition.DisplayOrder
            }).ToList(),
            Groups = children.GetValueOrDefault(group.Id, [])
                .OrderBy(child => child.DisplayOrder)
                .Select(child => MapSystemGroup(child, children))
                .ToList()
        };
    }

    private static TopupConditionGroupDTO MapScheduleGroup(
        ScheduleTopUpConditionGroup group,
        IReadOnlyDictionary<int, List<ScheduleTopUpConditionGroup>> children)
    {
        return new TopupConditionGroupDTO
        {
            Id = group.Id,
            LogicalOperator = group.LogicalOperator.ToString(),
            DisplayOrder = group.DisplayOrder,
            Conditions = group.Conditions.OrderBy(condition => condition.DisplayOrder).Select(condition => new TopupConditionDTO
            {
                Id = condition.Id,
                Field = condition.Field.ToString(),
                Operator = condition.Operator.ToString(),
                ValueText = condition.ValueText,
                ValueNumber = condition.ValueNumber,
                ValueNumberTo = condition.ValueNumberTo,
                DisplayOrder = condition.DisplayOrder
            }).ToList(),
            Groups = children.GetValueOrDefault(group.Id, [])
                .OrderBy(child => child.DisplayOrder)
                .Select(child => MapScheduleGroup(child, children))
                .ToList()
        };
    }
}
