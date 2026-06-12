namespace Models
{
    public class UserFavoriteProduct : Entity
    {
        [NotDefaultValue]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [NotDefaultValue]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
