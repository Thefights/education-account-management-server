namespace Filters.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SearchFieldAttribute(string targetField) : Attribute
    {
        public string TargetField { get; } = targetField;
    }
}
