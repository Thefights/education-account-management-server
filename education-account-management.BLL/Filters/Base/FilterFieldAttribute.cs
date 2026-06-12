namespace Filters.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterFieldAttribute(FilterOperationEnum Operation = FilterOperationEnum.Equal, string? TargetField = null) : Attribute
    {
        public FilterOperationEnum Operation { get; } = Operation;
        public string? TargetField { get; } = TargetField;
    }
}
