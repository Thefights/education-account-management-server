namespace Models
{
    public class UserRole : Entity
    {
        [NotDefaultValue]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [NotDefaultValue]
        public int RoleId { get; set; } = 1;
        public Role Role { get; set; } = null!;
    }
}
