using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class CourseFeeSeedBuilder : ISeedBuilder
{
    public int Priority => 110;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<CourseFee>().HasData(
            new CourseFee { Id = 1, CourseId = 1, CourseFeeAmount = 100m, MiscFeeAmount = 10m, GstAmount = 10m, CreatedAt = createdAt },
            new CourseFee { Id = 2, CourseId = 2, CourseFeeAmount = 115m, MiscFeeAmount = 12m, GstAmount = 13m, CreatedAt = createdAt },
            new CourseFee { Id = 3, CourseId = 3, CourseFeeAmount = 130m, MiscFeeAmount = 15m, GstAmount = 15m, CreatedAt = createdAt },
            new CourseFee { Id = 4, CourseId = 4, CourseFeeAmount = 145m, MiscFeeAmount = 17m, GstAmount = 18m, CreatedAt = createdAt },
            new CourseFee { Id = 5, CourseId = 5, CourseFeeAmount = 160m, MiscFeeAmount = 20m, GstAmount = 20m, CreatedAt = createdAt },
            new CourseFee { Id = 6, CourseId = 6, CourseFeeAmount = 175m, MiscFeeAmount = 22m, GstAmount = 23m, CreatedAt = createdAt },
            new CourseFee { Id = 7, CourseId = 7, CourseFeeAmount = 190m, MiscFeeAmount = 25m, GstAmount = 25m, CreatedAt = createdAt },
            new CourseFee { Id = 8, CourseId = 8, CourseFeeAmount = 205m, MiscFeeAmount = 27m, GstAmount = 28m, CreatedAt = createdAt },
            new CourseFee { Id = 9, CourseId = 9, CourseFeeAmount = 220m, MiscFeeAmount = 30m, GstAmount = 30m, CreatedAt = createdAt },
            new CourseFee { Id = 10, CourseId = 10, CourseFeeAmount = 235m, MiscFeeAmount = 32m, GstAmount = 33m, CreatedAt = createdAt });

        return modelBuilder;
    }
}
