using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class FasSeedBuilder : ISeedBuilder
    {
        public int Priority => 100;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var approvedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<FasScheme>().HasData(
                new FasScheme { Id = 1, SchoolId = 1, Status = FasSchemeStatus.Draft, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 1), SchemeName = "Seed FAS Scheme 01", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 3, SubsidyType = FasSubsidyType.Percent, IsPerComponent = true, PublishedAt = null, CreatedAt = createdAt },
                new FasScheme { Id = 2, SchoolId = 1, Status = FasSchemeStatus.Active, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 2), SchemeName = "Seed FAS Scheme 02", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 6, SubsidyType = FasSubsidyType.FixedAmount, IsPerComponent = false, PublishedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 3, SchoolId = 1, Status = FasSchemeStatus.Inactive, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 3), SchemeName = "Seed FAS Scheme 03", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 9, SubsidyType = FasSubsidyType.Percent, IsPerComponent = true, PublishedAt = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 4, SchoolId = 1, Status = FasSchemeStatus.Draft, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 4), SchemeName = "Seed FAS Scheme 04", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 12, SubsidyType = FasSubsidyType.FixedAmount, IsPerComponent = false, PublishedAt = null, CreatedAt = createdAt },
                new FasScheme { Id = 5, SchoolId = 1, Status = FasSchemeStatus.Active, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 5), SchemeName = "Seed FAS Scheme 05", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 3, SubsidyType = FasSubsidyType.Percent, IsPerComponent = true, PublishedAt = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 6, SchoolId = 1, Status = FasSchemeStatus.Inactive, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 6), SchemeName = "Seed FAS Scheme 06", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 6, SubsidyType = FasSubsidyType.FixedAmount, IsPerComponent = false, PublishedAt = new DateTime(2026, 1, 6, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 7, SchoolId = 1, Status = FasSchemeStatus.Draft, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 7), SchemeName = "Seed FAS Scheme 07", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 9, SubsidyType = FasSubsidyType.Percent, IsPerComponent = true, PublishedAt = null, CreatedAt = createdAt },
                new FasScheme { Id = 8, SchoolId = 1, Status = FasSchemeStatus.Active, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 8), SchemeName = "Seed FAS Scheme 08", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 12, SubsidyType = FasSubsidyType.FixedAmount, IsPerComponent = false, PublishedAt = new DateTime(2026, 1, 8, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 9, SchoolId = 1, Status = FasSchemeStatus.Inactive, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 9), SchemeName = "Seed FAS Scheme 09", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 3, SubsidyType = FasSubsidyType.Percent, IsPerComponent = true, PublishedAt = new DateTime(2026, 1, 9, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasScheme { Id = 10, SchoolId = 1, Status = FasSchemeStatus.Draft, SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, 10), SchemeName = "Seed FAS Scheme 10", Description = "Seed financial assistance scheme for school-admin scope.", DurationInMonths = 6, SubsidyType = FasSubsidyType.FixedAmount, IsPerComponent = false, PublishedAt = null, CreatedAt = createdAt });

            modelBuilder.Entity<FasSchemeConditionGroup>().HasData(
                new FasSchemeConditionGroup { Id = 1, FasSchemeId = 1, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeConditionGroup { Id = 2, FasSchemeId = 2, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeConditionGroup { Id = 3, FasSchemeId = 3, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeConditionGroup { Id = 4, FasSchemeId = 4, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeConditionGroup { Id = 5, FasSchemeId = 5, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeConditionGroup { Id = 6, FasSchemeId = 6, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeConditionGroup { Id = 7, FasSchemeId = 7, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeConditionGroup { Id = 8, FasSchemeId = 8, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeConditionGroup { Id = 9, FasSchemeId = 9, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeConditionGroup { Id = 10, FasSchemeId = 10, ParentGroupId = null, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt });

            modelBuilder.Entity<FasSchemeCondition>().HasData(
                new FasSchemeCondition { Id = 1, GroupId = 1, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 750m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 2, GroupId = 2, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 800m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 3, GroupId = 3, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 850m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 4, GroupId = 4, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 900m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 5, GroupId = 5, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 950m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 6, GroupId = 6, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 1000m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 7, GroupId = 7, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 1050m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 8, GroupId = 8, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 1100m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 9, GroupId = 9, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 1150m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeCondition { Id = 10, GroupId = 10, Field = FasConditionField.PerCapitaIncome, Operator = FasConditionOperator.LessThanOrEqual, ValueNumber = 1200m, DisplayOrder = 1, CreatedAt = createdAt });

            modelBuilder.Entity<FasSchemeTier>().HasData(
                new FasSchemeTier { Id = 1, FasSchemeId = 1, TierName = "Tier 1", MaxPerCapitaIncome = 750m, SubsidyValue = 21m, CourseFeeSubsidyValue = 11m, MiscFeeSubsidyValue = 6m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 2, FasSchemeId = 2, TierName = "Tier 2", MaxPerCapitaIncome = 800m, SubsidyValue = 70m, CourseFeeSubsidyValue = null, MiscFeeSubsidyValue = null, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 3, FasSchemeId = 3, TierName = "Tier 3", MaxPerCapitaIncome = 850m, SubsidyValue = 23m, CourseFeeSubsidyValue = 13m, MiscFeeSubsidyValue = 8m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 4, FasSchemeId = 4, TierName = "Tier 1", MaxPerCapitaIncome = 900m, SubsidyValue = 90m, CourseFeeSubsidyValue = null, MiscFeeSubsidyValue = null, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 5, FasSchemeId = 5, TierName = "Tier 2", MaxPerCapitaIncome = 950m, SubsidyValue = 25m, CourseFeeSubsidyValue = 15m, MiscFeeSubsidyValue = 10m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 6, FasSchemeId = 6, TierName = "Tier 3", MaxPerCapitaIncome = 1000m, SubsidyValue = 110m, CourseFeeSubsidyValue = null, MiscFeeSubsidyValue = null, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 7, FasSchemeId = 7, TierName = "Tier 1", MaxPerCapitaIncome = 1050m, SubsidyValue = 27m, CourseFeeSubsidyValue = 17m, MiscFeeSubsidyValue = 12m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 8, FasSchemeId = 8, TierName = "Tier 2", MaxPerCapitaIncome = 1100m, SubsidyValue = 130m, CourseFeeSubsidyValue = null, MiscFeeSubsidyValue = null, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 9, FasSchemeId = 9, TierName = "Tier 3", MaxPerCapitaIncome = 1150m, SubsidyValue = 29m, CourseFeeSubsidyValue = 19m, MiscFeeSubsidyValue = 14m, DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeTier { Id = 10, FasSchemeId = 10, TierName = "Tier 1", MaxPerCapitaIncome = 1200m, SubsidyValue = 150m, CourseFeeSubsidyValue = null, MiscFeeSubsidyValue = null, DisplayOrder = 1, CreatedAt = createdAt });

            modelBuilder.Entity<FasSchemeRequiredDocument>().HasData(
                new FasSchemeRequiredDocument { Id = 1, FasSchemeId = 1, DocumentName = "Recent Payslip", TemplateFileKey = "fas/templates/document-01.pdf", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeRequiredDocument { Id = 2, FasSchemeId = 2, DocumentName = "Income Statement", TemplateFileKey = "fas/templates/document-02.pdf", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeRequiredDocument { Id = 3, FasSchemeId = 3, DocumentName = "Recent Payslip", TemplateFileKey = "fas/templates/document-03.pdf", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeRequiredDocument { Id = 4, FasSchemeId = 4, DocumentName = "Income Statement", TemplateFileKey = "fas/templates/document-04.pdf", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeRequiredDocument { Id = 5, FasSchemeId = 5, DocumentName = "Recent Payslip", TemplateFileKey = "fas/templates/document-05.pdf", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeRequiredDocument { Id = 6, FasSchemeId = 6, DocumentName = "Income Statement", TemplateFileKey = "fas/templates/document-06.pdf", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeRequiredDocument { Id = 7, FasSchemeId = 7, DocumentName = "Recent Payslip", TemplateFileKey = "fas/templates/document-07.pdf", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeRequiredDocument { Id = 8, FasSchemeId = 8, DocumentName = "Income Statement", TemplateFileKey = "fas/templates/document-08.pdf", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeRequiredDocument { Id = 9, FasSchemeId = 9, DocumentName = "Recent Payslip", TemplateFileKey = "fas/templates/document-09.pdf", DisplayOrder = 1, CreatedAt = createdAt },
                new FasSchemeRequiredDocument { Id = 10, FasSchemeId = 10, DocumentName = "Income Statement", TemplateFileKey = "fas/templates/document-10.pdf", DisplayOrder = 1, CreatedAt = createdAt });

            modelBuilder.Entity<FasSchemeCourse>().HasData(
                new FasSchemeCourse { Id = 1, FasSchemeId = 1, CourseId = 1, CreatedAt = createdAt },
                new FasSchemeCourse { Id = 2, FasSchemeId = 2, CourseId = 2, CreatedAt = createdAt },
                new FasSchemeCourse { Id = 3, FasSchemeId = 3, CourseId = 3, CreatedAt = createdAt },
                new FasSchemeCourse { Id = 4, FasSchemeId = 4, CourseId = 4, CreatedAt = createdAt },
                new FasSchemeCourse { Id = 5, FasSchemeId = 5, CourseId = 5, CreatedAt = createdAt },
                new FasSchemeCourse { Id = 6, FasSchemeId = 6, CourseId = 6, CreatedAt = createdAt },
                new FasSchemeCourse { Id = 7, FasSchemeId = 7, CourseId = 7, CreatedAt = createdAt },
                new FasSchemeCourse { Id = 8, FasSchemeId = 8, CourseId = 8, CreatedAt = createdAt },
                new FasSchemeCourse { Id = 9, FasSchemeId = 9, CourseId = 9, CreatedAt = createdAt },
                new FasSchemeCourse { Id = 10, FasSchemeId = 10, CourseId = 10, CreatedAt = createdAt });

            var applications = new List<FasApplication>
            {
                new FasApplication { Id = 1, FasSchemeId = 1, SchoolStudentId = 1, RecommendedTierId = 1, ApprovedTierId = 1, ApplicationNumber = "FASAPP-20260101-A1B2C3D", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 18, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 2500m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 625m, RecommendationReason = "PCI <= 750", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 6, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(6), CreatedAt = createdAt },
                new FasApplication { Id = 2, FasSchemeId = 2, SchoolStudentId = 2, RecommendedTierId = 2, ApprovedTierId = 2, ApplicationNumber = "FASAPP-20260102-E4F5G6H", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 17, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 3000m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 750m, RecommendationReason = "GHI <= 3500", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 3, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(3), CreatedAt = createdAt },
                new FasApplication { Id = 3, FasSchemeId = 3, SchoolStudentId = 3, RecommendedTierId = 3, ApprovedTierId = 3, ApplicationNumber = "FASAPP-20260103-I7J8K9L", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 19, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 2200m, HouseholdMemberCountSnapshot = 5, PerCapitaIncomeSnapshot = 440m, RecommendationReason = "Singapore citizen and PCI <= 690", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 12, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(12), CreatedAt = createdAt },
                new FasApplication { Id = 4, FasSchemeId = 4, SchoolStudentId = 4, RecommendedTierId = 4, ApplicationNumber = "FASAPP-20260104-M1N2P3Q", Status = FasApplicationStatus.Pending, StudentAgeSnapshot = 20, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 3600m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 900m, RecommendationReason = "Pending admin review", CreatedAt = createdAt },
                new FasApplication { Id = 5, FasSchemeId = 5, SchoolStudentId = 5, RecommendedTierId = 5, ApplicationNumber = "FASAPP-20260105-R4S5T6U", Status = FasApplicationStatus.Rejected, StudentAgeSnapshot = 21, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 7000m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 1750m, RecommendationReason = "No tier matched", RejectionReason = "Income exceeds supported threshold.", CreatedAt = createdAt },
                new FasApplication { Id = 6, FasSchemeId = 6, SchoolStudentId = 6, RecommendedTierId = 6, ApplicationNumber = "FASAPP-20260106-V7W8X9Y", Status = FasApplicationStatus.Withdrawn, StudentAgeSnapshot = 18, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 2800m, HouseholdMemberCountSnapshot = 5, PerCapitaIncomeSnapshot = 560m, RecommendationReason = "Student withdrew before review", WithdrawnAt = new DateTime(2026, 6, 3, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasApplication { Id = 7, FasSchemeId = 7, SchoolStudentId = 7, RecommendedTierId = 7, ApprovedTierId = 7, ApplicationNumber = "FASAPP-20260107-Z1A2B3C", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 16, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 4800m, HouseholdMemberCountSnapshot = 4, PerCapitaIncomeSnapshot = 1200m, RecommendationReason = "Special needs support threshold matched", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 12, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(12), CreatedAt = createdAt },
                new FasApplication { Id = 8, FasSchemeId = 8, SchoolStudentId = 8, RecommendedTierId = 8, ApprovedTierId = 8, ApplicationNumber = "FASAPP-20260108-D4E5F6G", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 22, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 3200m, HouseholdMemberCountSnapshot = 5, PerCapitaIncomeSnapshot = 640m, RecommendationReason = "PCI <= 850", ApprovedAt = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), ApprovedByUserId = 1, DurationInMonthsSnapshot = 6, ValidityStartDate = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), ValidityEndDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new FasApplication { Id = 9, FasSchemeId = 9, SchoolStudentId = 9, RecommendedTierId = 9, ApplicationNumber = "FASAPP-20260109-H7J8K9L", Status = FasApplicationStatus.Pending, StudentAgeSnapshot = 17, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.Other, GrossHouseholdIncomeSnapshot = 2600m, HouseholdMemberCountSnapshot = 3, PerCapitaIncomeSnapshot = 866.67m, RecommendationReason = "Emergency aid review required", CreatedAt = createdAt },
                new FasApplication { Id = 10, FasSchemeId = 10, SchoolStudentId = 10, RecommendedTierId = 10, ApprovedTierId = 10, ApplicationNumber = "FASAPP-20260110-M1N2P3Q", Status = FasApplicationStatus.Approved, StudentAgeSnapshot = 18, StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen, GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen, GrossHouseholdIncomeSnapshot = 3900m, HouseholdMemberCountSnapshot = 5, PerCapitaIncomeSnapshot = 780m, RecommendationReason = "PCI <= 1000", ApprovedAt = approvedAt, ApprovedByUserId = 1, DurationInMonthsSnapshot = 12, ValidityStartDate = approvedAt, ValidityEndDate = approvedAt.AddMonths(12), CreatedAt = createdAt }
            };

            int appId = 11;
            for (int i = 0; i < 25; i++)
            {
                var schemeId = (i % 10) + 1; // 1 to 10
                var studentId = 1; // Assigned to the mock singpass user's child for FE testing
                var categoryIndex = i % 5; 
                // 0: Pending, 1: Approved, 2: Rejected, 3: Withdrawn, 4: Expired (Approved but old dates)

                var status = categoryIndex == 0 ? FasApplicationStatus.Pending :
                             categoryIndex == 1 ? FasApplicationStatus.Approved :
                             categoryIndex == 2 ? FasApplicationStatus.Rejected :
                             categoryIndex == 3 ? FasApplicationStatus.Withdrawn :
                             FasApplicationStatus.Approved;

                var isApproved = status == FasApplicationStatus.Approved;
                var isRejected = status == FasApplicationStatus.Rejected;
                var isWithdrawn = status == FasApplicationStatus.Withdrawn;
                var isExpired = categoryIndex == 4;

                var currentApprovedAt = isExpired ? new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) : approvedAt;
                var duration = i % 2 == 0 ? 6 : 12;

                applications.Add(new FasApplication
                {
                    Id = appId++,
                    FasSchemeId = schemeId,
                    SchoolStudentId = studentId,
                    RecommendedTierId = schemeId,
                    ApprovedTierId = isApproved ? schemeId : null,
                    ApplicationNumber = $"FASAPP-GEN-{i:D4}",
                    Status = status,
                    StudentAgeSnapshot = 16 + (i % 6),
                    StudentNationalitySnapshot = i % 3 == 0 ? NationalityCategory.Other : NationalityCategory.SingaporeCitizen,
                    GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen,
                    GrossHouseholdIncomeSnapshot = 2000m + (i * 100 % 4000),
                    HouseholdMemberCountSnapshot = 3 + (i % 4),
                    PerCapitaIncomeSnapshot = (2000m + (i * 100 % 4000)) / (3 + (i % 4)),
                    RecommendationReason = $"Auto generated reason {i}",
                    RejectionReason = isRejected ? "Does not meet requirements." : null,
                    ApprovedAt = isApproved ? currentApprovedAt : null,
                    ApprovedByUserId = isApproved ? 1 : null,
                    DurationInMonthsSnapshot = isApproved ? duration : null,
                    ValidityStartDate = isApproved ? currentApprovedAt : null,
                    ValidityEndDate = isApproved ? currentApprovedAt.AddMonths(duration) : null,
                    WithdrawnAt = isWithdrawn ? approvedAt : null,
                    CreatedAt = createdAt.AddDays(-(i % 30))
                });
            }

            modelBuilder.Entity<FasApplication>().HasData(applications);

            var documents = new List<FasApplicationDocument>
            {
                new FasApplicationDocument { Id = 1, FasApplicationId = 1, FasSchemeRequiredDocumentId = 1, DocumentNameSnapshot = "Recent Payslip", FileKey = "fas/applications/1/document.pdf", FileName = "fas-application-01.pdf", CreatedAt = createdAt },
                new FasApplicationDocument { Id = 2, FasApplicationId = 2, FasSchemeRequiredDocumentId = 2, DocumentNameSnapshot = "Income Statement", FileKey = "fas/applications/2/document.pdf", FileName = "fas-application-02.pdf", CreatedAt = createdAt },
                new FasApplicationDocument { Id = 3, FasApplicationId = 3, FasSchemeRequiredDocumentId = 3, DocumentNameSnapshot = "Recent Payslip", FileKey = "fas/applications/3/document.pdf", FileName = "fas-application-03.pdf", CreatedAt = createdAt },
                new FasApplicationDocument { Id = 4, FasApplicationId = 4, FasSchemeRequiredDocumentId = 4, DocumentNameSnapshot = "Income Statement", FileKey = "fas/applications/4/document.pdf", FileName = "fas-application-04.pdf", CreatedAt = createdAt },
                new FasApplicationDocument { Id = 5, FasApplicationId = 5, FasSchemeRequiredDocumentId = 5, DocumentNameSnapshot = "Recent Payslip", FileKey = "fas/applications/5/document.pdf", FileName = "fas-application-05.pdf", CreatedAt = createdAt },
                new FasApplicationDocument { Id = 6, FasApplicationId = 6, FasSchemeRequiredDocumentId = 6, DocumentNameSnapshot = "Income Statement", FileKey = "fas/applications/6/document.pdf", FileName = "fas-application-06.pdf", CreatedAt = createdAt }
            };

            int docId = 7;
            foreach (var app in applications.Skip(10)) // Skip the first 10 manually seeded applications
            {
                documents.Add(new FasApplicationDocument
                {
                    Id = docId++,
                    FasApplicationId = app.Id,
                    FasSchemeRequiredDocumentId = app.FasSchemeId, // Each scheme has exactly one matching required document ID 1-10
                    DocumentNameSnapshot = app.FasSchemeId % 2 != 0 ? "Recent Payslip" : "Income Statement",
                    FileKey = $"fas/applications/{app.Id}/document.pdf",
                    FileName = $"fas-application-{app.Id:D2}.pdf",
                    CreatedAt = createdAt
                });
            }
            modelBuilder.Entity<FasApplicationDocument>().HasData(documents);

            modelBuilder.Entity<FasTierOverrideHistory>().HasData(
                new FasTierOverrideHistory { Id = 1, FasApplicationId = 2, OldTierId = 2, NewTierId = 2, ModifiedByUserId = 1, ModifiedAt = approvedAt.AddDays(2), Reason = "Seed tier review trail.", CreatedAt = createdAt },
                new FasTierOverrideHistory { Id = 2, FasApplicationId = 6, OldTierId = 6, NewTierId = 6, ModifiedByUserId = 1, ModifiedAt = approvedAt.AddDays(6), Reason = "Seed tier review trail.", CreatedAt = createdAt },
                new FasTierOverrideHistory { Id = 3, FasApplicationId = 10, OldTierId = 10, NewTierId = 10, ModifiedByUserId = 1, ModifiedAt = approvedAt.AddDays(10), Reason = "Seed tier review trail.", CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
