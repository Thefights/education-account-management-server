using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class SchoolStudentSeedBuilder : ISeedBuilder
    {
        public int Priority => 70;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<SchoolStudent>().HasData(
                new SchoolStudent { Id = 1, SchoolId = 1, EducationAccountId = 1, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 2, SchoolId = 1, EducationAccountId = 2, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 3, SchoolId = 1, EducationAccountId = 3, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 4, SchoolId = 1, EducationAccountId = 4, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 5, SchoolId = 1, EducationAccountId = 5, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 6, SchoolId = 1, EducationAccountId = 6, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 7, SchoolId = 1, EducationAccountId = 7, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 8, SchoolId = 1, EducationAccountId = 8, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 9, SchoolId = 1, EducationAccountId = 9, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 10, SchoolId = 1, EducationAccountId = 10, Status = SchoolStudentStatus.Inactive, CreatedAt = createdAt },
                new SchoolStudent { Id = 11, SchoolId = 1, EducationAccountId = 11, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 12, SchoolId = 1, EducationAccountId = 12, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 13, SchoolId = 1, EducationAccountId = 13, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 14, SchoolId = 1, EducationAccountId = 14, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 15, SchoolId = 1, EducationAccountId = 15, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 16, SchoolId = 1, EducationAccountId = 16, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 17, SchoolId = 1, EducationAccountId = 17, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 18, SchoolId = 1, EducationAccountId = 18, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 19, SchoolId = 1, EducationAccountId = 19, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 20, SchoolId = 1, EducationAccountId = 20, Status = SchoolStudentStatus.Inactive, CreatedAt = createdAt },
                new SchoolStudent { Id = 21, SchoolId = 1, EducationAccountId = 21, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 22, SchoolId = 1, EducationAccountId = 22, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 23, SchoolId = 1, EducationAccountId = 23, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 24, SchoolId = 1, EducationAccountId = 24, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 25, SchoolId = 1, EducationAccountId = 25, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 26, SchoolId = 1, EducationAccountId = 26, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 27, SchoolId = 1, EducationAccountId = 27, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 28, SchoolId = 1, EducationAccountId = 28, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 29, SchoolId = 1, EducationAccountId = 29, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 30, SchoolId = 1, EducationAccountId = 30, Status = SchoolStudentStatus.Inactive, CreatedAt = createdAt },
                new SchoolStudent { Id = 31, SchoolId = 1, EducationAccountId = 31, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 32, SchoolId = 1, EducationAccountId = 32, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 33, SchoolId = 1, EducationAccountId = 33, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 34, SchoolId = 1, EducationAccountId = 34, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 35, SchoolId = 1, EducationAccountId = 35, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 36, SchoolId = 1, EducationAccountId = 36, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 37, SchoolId = 1, EducationAccountId = 37, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 38, SchoolId = 1, EducationAccountId = 38, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 39, SchoolId = 1, EducationAccountId = 39, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 40, SchoolId = 1, EducationAccountId = 40, Status = SchoolStudentStatus.Inactive, CreatedAt = createdAt },
                new SchoolStudent { Id = 41, SchoolId = 1, EducationAccountId = 41, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 42, SchoolId = 1, EducationAccountId = 42, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 43, SchoolId = 1, EducationAccountId = 43, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 44, SchoolId = 1, EducationAccountId = 44, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 45, SchoolId = 1, EducationAccountId = 45, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 46, SchoolId = 1, EducationAccountId = 46, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 47, SchoolId = 1, EducationAccountId = 47, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 48, SchoolId = 1, EducationAccountId = 48, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 49, SchoolId = 1, EducationAccountId = 49, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
                new SchoolStudent { Id = 50, SchoolId = 1, EducationAccountId = 50, Status = SchoolStudentStatus.Inactive, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
