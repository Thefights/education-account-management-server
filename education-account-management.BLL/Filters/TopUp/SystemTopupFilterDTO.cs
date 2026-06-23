namespace Filters.TopUp;

public sealed class SystemTopupFilterDTO : FilterDTO
{
    private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = nameof(SystemTopup.Id),
            ["name"] = nameof(SystemTopup.Name),
            ["topupAmount"] = nameof(SystemTopup.TopupAmount),
            ["status"] = nameof(SystemTopup.Status),
            ["createdAt"] = nameof(SystemTopup.CreatedAt)
        };

    public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

    [FilterField(FilterOperationEnum.In, nameof(SystemTopup.Status))]
    public List<SystemTopupStatus>? Statuses { get; set; }

    [SearchField(nameof(SystemTopup.Name))]
    public string? Name { get; set; }
}
