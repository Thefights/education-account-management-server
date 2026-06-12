namespace Persistence.Seeding.Constants
{
    public interface ISeedBuilder
    {
        int Priority { get; }
        ModelBuilder Seed(ModelBuilder modelBuilder);
    }
}
