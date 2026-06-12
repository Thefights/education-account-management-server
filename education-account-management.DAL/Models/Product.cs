namespace Models
{
    public class Product : EntityWithImage
    {
        [EnumDefined]
        public ProductStatus Status { get; set; } = ProductStatus.Active;

        [MessageRequired, MessageMaxLength(100), Unique]
        public string Name { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
