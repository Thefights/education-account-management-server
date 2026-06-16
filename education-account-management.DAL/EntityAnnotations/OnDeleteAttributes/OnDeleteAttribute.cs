namespace EntityAnnotations.OnDeleteAttributes
{
    /// <summary>
    /// Specifies the delete behavior for a navigation property (both soft delete and hard delete).
    /// Default behavior is <see cref="OnDeleteBehavior.Restrict"/> if not specified.
    /// </summary>
    /// <example>
    /// <code>
    /// // Restrict: Prevent deletion if related records exist (default)
    /// public class ParentRecord : AuditEntity
    /// {
    ///     public string Name { get; set; } = null!;
    ///     
    ///     // No attribute = Restrict by default
    ///     public ICollection<ChildRecord> Children { get; set; } = [];
    /// }
    /// 
    /// // Cascade: Auto soft-delete related records when parent is deleted
    /// public class ParentRecord : AuditEntity
    /// {
    ///     public string Name { get; set; } = null!;
    ///     
    ///     [OnDelete(OnDeleteBehavior.Cascade)]
    ///     public ICollection<ChildRecord> Children { get; set; } = [];
    /// }
    /// 
    /// // SetNull: Set FK to null on related records (FK must be nullable!)
    /// public class ParentRecord : AuditEntity
    /// {
    ///     public string Name { get; set; } = null!;
    ///     
    ///     [OnDelete(OnDeleteBehavior.SetNull)]
    ///     public ICollection<ChildRecord> Children { get; set; } = [];
    /// }
    /// 
    /// // Child record with nullable FK for SetNull to work
    /// public class ChildRecord : AuditEntity
    /// {
    ///     public string Name { get; set; } = null!;
    ///     public int? ParentRecordId { get; set; }  // Must be nullable!
    ///     public ParentRecord? ParentRecord { get; set; }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OnDeleteAttribute(OnDeleteBehavior mode) : Attribute
    {
        public OnDeleteBehavior Mode { get; } = mode;
    }
}
