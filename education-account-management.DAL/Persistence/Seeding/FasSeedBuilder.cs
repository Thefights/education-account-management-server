using Enums;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class FasSeedBuilder : ISeedBuilder
    {
        public int Priority => 125;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var approvedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<FasScheme>().HasData(
                new FasScheme { Id = 1, SchoolId = 1, Status = FasSchemeStatus.Active, SchemeCode = "FAS-001", SchemeName = "Household Income Subsidy", Description = "Support for students from lower-income households.", DurationInMonths = 6, SubsidyType = FasSubsidyType.Percent, PublishedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 2, SchoolId = 2, Status = FasSchemeStatus.Active, SchemeCode = "FAS-002", SchemeName = "Transport Assistance", Description = "Transport support for eligible students.", DurationInMonths = 3, SubsidyType = FasSubsidyType.FixedAmount, PublishedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 3, SchoolId = 3, Status = FasSchemeStatus.Active, SchemeCode = "FAS-003", SchemeName = "Study Grant", Description = "Course and misc fee assistance.", DurationInMonths = 12, SubsidyType = FasSubsidyType.Percent, IsPerComponent = true, PublishedAt = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 4, SchoolId = 4, Status = FasSchemeStatus.Draft, SchemeCode = "FAS-004", SchemeName = "Meal Subsidy", Description = "Draft meal support programme.", DurationInMonths = 6, SubsidyType = FasSubsidyType.Percent, CreatedAt = createdAt },
                new FasScheme { Id = 5, SchoolId = 5, Status = FasSchemeStatus.Active, SchemeCode = "FAS-005", SchemeName = "Digital Device Grant", Description = "Support for digital learning devices.", DurationInMonths = 9, SubsidyType = FasSubsidyType.FixedAmount, PublishedAt = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 6, SchoolId = 6, Status = FasSchemeStatus.Inactive, SchemeCode = "FAS-006", SchemeName = "Uniform Grant", Description = "Inactive uniform support programme.", DurationInMonths = 3, SubsidyType = FasSubsidyType.FixedAmount, PublishedAt = new DateTime(2026, 1, 6, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 7, SchoolId = 7, Status = FasSchemeStatus.Active, SchemeCode = "FAS-007", SchemeName = "Special Needs Support", Description = "Support for students with special learning needs.", DurationInMonths = 12, SubsidyType = FasSubsidyType.Percent, IsPerComponent = true, PublishedAt = new DateTime(2026, 1, 7, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 8, SchoolId = 8, Status = FasSchemeStatus.Active, SchemeCode = "FAS-008", SchemeName = "Learning Materials Grant", Description = "Support for textbooks and materials.", DurationInMonths = 6, SubsidyType = FasSubsidyType.Percent, PublishedAt = new DateTime(2026, 1, 8, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 9, SchoolId = 9, Status = FasSchemeStatus.Active, SchemeCode = "FAS-009", SchemeName = "Emergency Financial Aid", Description = "Short term emergency support.", DurationInMonths = 3, SubsidyType = FasSubsidyType.FixedAmount, PublishedAt = new DateTime(2026, 1, 9, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 10, SchoolId = 10, Status = FasSchemeStatus.Active, SchemeCode = "FAS-010", SchemeName = "STEM Programme Grant", Description = "Support for STEM related courses.", DurationInMonths = 12, SubsidyType = FasSubsidyType.Percent, PublishedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt });

            modelBuilder.Entity<FasSchemeConditionGroup>().HasData(
                Enumerable.Range(1, 10).Select(id => new FasSchemeConditionGroup { Id = id, FasSchemeId = id, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = id, CreatedAt = createdAt }).ToArray());

            modelBuilder.Entity<FasSchemeCondition>().HasData(
                new FasSchemeCondition { Id = 1, GroupId = 1, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 750m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 2, GroupId = 2, Field = FasConditionField.GrossHouseholdIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 3500m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 3, GroupId = 3, Field = FasConditionField.StudentNationality, Operator = FasConditionOperator.Equal, ValueText = "Singapore", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 4, GroupId = 4, Field = FasConditionField.StudentAge, Operator = FasConditionOperator.Between, ValueNumber = 16m, ValueNumberTo = 30m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 5, GroupId = 5, Field = FasConditionField.GuardianNationality, Operator = FasConditionOperator.Equal, ValueText = "Singapore", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 6, GroupId = 6, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 1000m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 7, GroupId = 7, Field = FasConditionField.GrossHouseholdIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 5000m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 8, GroupId = 8, Field = FasConditionField.StudentNationality, Operator = FasConditionOperator.Equal, ValueText = "Singapore", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 9, GroupId = 9, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 1200m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 10, GroupId = 10, Field = FasConditionField.StudentAge, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 25m, DisplayOrder = 1, CreatedAt = createdAt });

            modelBuilder.Entity<FasSchemeTier>().HasData(
                new FasSchemeTier { Id = 1, FasSchemeId = 1, TierName = "Tier 1", MaxPerCapitaIncome = 750m, SubsidyValue = 75m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 2, FasSchemeId = 2, TierName = "Tier 1", MaxPerCapitaIncome = 900m, SubsidyValue = 300m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 3, FasSchemeId = 3, TierName = "Tier 1", MaxPerCapitaIncome = 690m, CourseFeeSubsidyValue = 100m, MiscFeeSubsidyValue = 50m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 4, FasSchemeId = 4, TierName = "Tier 1", MaxPerCapitaIncome = 800m, SubsidyValue = 50m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 5, FasSchemeId = 5, TierName = "Tier 1", MaxPerCapitaIncome = 1000m, SubsidyValue = 500m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 6, FasSchemeId = 6, TierName = "Tier 1", MaxPerCapitaIncome = 750m, SubsidyValue = 200m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 7, FasSchemeId = 7, TierName = "Tier 1", MaxPerCapitaIncome = 1500m, CourseFeeSubsidyValue = 75m, MiscFeeSubsidyValue = 75m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 8, FasSchemeId = 8, TierName = "Tier 1", MaxPerCapitaIncome = 850m, SubsidyValue = 40m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 9, FasSchemeId = 9, TierName = "Tier 1", MaxPerCapitaIncome = 1200m, SubsidyValue = 250m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 10, FasSchemeId = 10, TierName = "Tier 1", MaxPerCapitaIncome = 1000m, SubsidyValue = 60m, DisplayOrder = 1, CreatedAt = createdAt });

            modelBuilder.Entity<FasSchemeRequiredDocument>().HasData(
                Enumerable.Range(1, 10).Select(id => new FasSchemeRequiredDocument { Id = id, FasSchemeId = id, DocumentName = id % 2 == 0 ? "Income Statement" : "Recent Payslip", TemplateFileKey = $"fas/templates/document-{id}.pdf", DisplayOrder = 1, CreatedAt = createdAt }).ToArray());

            var schemeCourses = Enumerable.Range(1, 10)
                .Select(id => new FasSchemeCourse
                {
                    Id = id,
                    FasSchemeId = id,
                    CourseId = id,
                    CreatedAt = createdAt
                })
                .ToList();
            var schemeCourseId = 11;
            for (var courseId = 11; courseId <= 100; courseId++)
            {
                var schoolId = ((courseId - 11) / 9) + 1;
                schemeCourses.Add(new FasSchemeCourse
                {
                    Id = schemeCourseId++,
                    FasSchemeId = schoolId,
                    CourseId = courseId,
                    CreatedAt = createdAt
                });
            }

            modelBuilder.Entity<FasSchemeCourse>().HasData(schemeCourses);

            modelBuilder.Entity<FasApplication>().HasData(
                new FasApplication { Id = 1, FasSchemeId = 1, SchoolStudentId = 1, RecommendedTierId = 1, ApprovedTierId = 1, ApplicationNumber = "FASAPP-20260101-A1B2C3D", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 18, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 2500m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 625m, RecommendationReason = "PCI <= 750", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 6, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(6), CreatedAt = createdAt },
                new FasApplication { Id = 2, FasSchemeId = 2, SchoolStudentId = 2, RecommendedTierId = 2, ApprovedTierId = 2, ApplicationNumber = "FASAPP-20260102-E4F5G6H", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 17, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 3000m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 750m, RecommendationReason = "GHI <= 3500", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 3, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(3), CreatedAt = createdAt },
                new FasApplication { Id = 3, FasSchemeId = 3, SchoolStudentId = 3, RecommendedTierId = 3, ApprovedTierId = 3, ApplicationNumber = "FASAPP-20260103-I7J8K9L", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 19, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 2200m, HouseholdMemberCountSnapshot = 5, PerCapitaIncomeSnapshot = 440m, RecommendationReason = "Singapore citizen and PCI <= 690", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 12, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(12), CreatedAt = createdAt },
                new FasApplication { Id = 4, FasSchemeId = 4, SchoolStudentId = 4, RecommendedTierId = 4, ApplicationNumber = "FASAPP-20260104-M1N2P3Q", Status = FasApplicationStatus.Pending, StudentAgeSnapshot = 20, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 3600m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 900m, RecommendationReason = "Pending admin review", CreatedAt = createdAt },
                new FasApplication { Id = 5, FasSchemeId = 5, SchoolStudentId = 5, RecommendedTierId = 5, ApplicationNumber = "FASAPP-20260105-R4S5T6U", Status = FasApplicationStatus.Rejected, StudentAgeSnapshot = 21, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 7000m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 1750m, RecommendationReason = "No tier matched", RejectionReason = "Income exceeds supported threshold.", CreatedAt = createdAt },
                new FasApplication { Id = 6, FasSchemeId = 6, SchoolStudentId = 6, RecommendedTierId = 6, ApplicationNumber = "FASAPP-20260106-V7W8X9Y", Status = FasApplicationStatus.Withdrawn, StudentAgeSnapshot = 18, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 2800m, HouseholdMemberCountSnapshot = 5, PerCapitaIncomeSnapshot = 560m, RecommendationReason = "Student withdrew before review", WithdrawnAt = new DateTime(2026, 6, 3, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasApplication { Id = 7, FasSchemeId = 7, SchoolStudentId = 7, RecommendedTierId = 7, ApprovedTierId = 7, ApplicationNumber = "FASAPP-20260107-Z1A2B3C", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 16, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 4800m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 1200m, RecommendationReason = "Special needs support threshold matched", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 12, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(12), CreatedAt = createdAt },
                new FasApplication { Id = 8, FasSchemeId = 8, SchoolStudentId = 8, RecommendedTierId = 8, ApprovedTierId = 8, ApplicationNumber = "FASAPP-20260108-D4E5F6G", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 22, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 3200m, HouseholdMemberCountSnapshot = 5, PerCapitaIncomeSnapshot = 640m, RecommendationReason = "PCI <= 850", ApprovedAt = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), ApprovedByUserId = 1, DurationInMonthsSnapshot = 6, ValidityStartDate = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), ValidityEndDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasApplication { Id = 9, FasSchemeId = 9, SchoolStudentId = 9, RecommendedTierId = 9, ApplicationNumber = "FASAPP-20260109-H7J8K9L", Status = FasApplicationStatus.Pending, StudentAgeSnapshot = 17, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 2600m, HouseholdMemberCountSnapshot = 3, PerCapitaIncomeSnapshot = 866.67m, RecommendationReason = "Emergency aid review required", CreatedAt = createdAt },
                new FasApplication { Id = 10, FasSchemeId = 10, SchoolStudentId = 10, RecommendedTierId = 10, ApprovedTierId = 10, ApplicationNumber = "FASAPP-20260110-M1N2P3Q", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 18, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 3900m, HouseholdMemberCountSnapshot = 5, PerCapitaIncomeSnapshot = 780m, RecommendationReason = "PCI <= 1000", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 12, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(12), CreatedAt = createdAt });

            modelBuilder.Entity<FasApplicationDocument>().HasData(
                Enumerable.Range(1, 10).Select(id => new FasApplicationDocument { Id = id, FasApplicationId = id, FasSchemeRequiredDocumentId = id, DocumentNameSnapshot = id % 2 == 0 ? "Income Statement" : "Recent Payslip", FileKey = $"fas/applications/{id}/document.pdf", FileName = $"fas-application-{id}.pdf", CreatedAt = createdAt }).ToArray());

            modelBuilder.Entity<FasTierOverrideHistory>().HasData(
                Enumerable.Range(1, 10).Select(id => new FasTierOverrideHistory { Id = id, FasApplicationId = id, OldTierId = id, NewTierId = id, ModifiedByUserId = 1, ModifiedAt = new DateTime(2026, 6, id, 0, 0, 0, DateTimeKind.Utc), Reason = "Seed audit trail for tier review.", CreatedAt = createdAt }).ToArray());

            return modelBuilder;
        }
    }
}
