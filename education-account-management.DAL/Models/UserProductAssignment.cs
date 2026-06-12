namespace Models
{
    public class UserProductAssignment : Entity
    {
        [NotDefaultValue]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [NotDefaultValue]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [EnumDefined]
        public ProductAssignmentRole RoleInProduct { get; set; }
    }
}
