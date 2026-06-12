using System.ComponentModel.DataAnnotations.Schema;

namespace Common
{
    public abstract class EntityWithImage : AuditEntity
    {
        [MessageMaxLength(255)]
        [Column(Order = 2)]
        public string? ImageUrl { get; set; }
    }
}
