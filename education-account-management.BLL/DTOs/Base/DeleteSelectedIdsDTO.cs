namespace DTOs.Base
{
    public class DeleteSelectedIdsDTO
    {
        [MessageRequired, MessageMinLength(1)]
        public List<int> Ids { get; set; } = [];

        [MessageRequired, MessageMinLength(10), MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }
}
