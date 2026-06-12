namespace Filters
{
    public class ProductFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(Product.Id),
                ["name"] = nameof(Product.Name),
                ["description"] = nameof(Product.Description),
                ["status"] = nameof(Product.Status),
                ["createdAt"] = nameof(Product.CreationDate),
                ["updatedAt"] = nameof(Product.ModificationDate)
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(Product.Name))]
        public string? Name { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(Product.Description))]
        public string? Description { get; set; }

        [FilterField]
        public ProductStatus? Status { get; set; }
    }
}
