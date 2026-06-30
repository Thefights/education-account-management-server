using Models;
using Persistence.Seeding.Constants;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Enums;

namespace Persistence.Seeding
{
    public sealed class FasSeedBuilder : ISeedBuilder
    {
        public int Priority => 100;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var publishedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var approvedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc);
            
            var schemes = new List<FasScheme>();
            var conditionGroups = new List<FasSchemeConditionGroup>();
            var conditions = new List<FasSchemeCondition>();
            var tiers = new List<FasSchemeTier>();
            var documents = new List<FasSchemeRequiredDocument>();
            var courses = new List<FasSchemeCourse>();
            var questions = new List<FasSchemeAdditionalQuestion>();

            for (int i = 1; i <= 60; i++)
            {
                var status = FasSchemeStatus.Active;
                if (i >= 16 && i <= 20) status = FasSchemeStatus.Draft;
                if (i >= 21 && i <= 23) status = FasSchemeStatus.Inactive;

                schemes.Add(new FasScheme 
                { 
                    Id = i, SchoolId = 1, Status = status, 
                    SchemeCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.FasSchemePrefix, i), 
                    SchemeName = $"Seed FAS Scheme {i:D2}", 
                    Description = "Seed financial assistance scheme.", 
                    DurationInMonths = 12, SubsidyType = FasSubsidyType.FixedAmount, 
                    IsPerComponent = false, 
                    PublishedAt = status == FasSchemeStatus.Active ? publishedAt.AddDays(i) : null, 
                    CreatedAt = createdAt 
                });

                conditionGroups.Add(new FasSchemeConditionGroup 
                { 
                    Id = i, FasSchemeId = i, ParentGroupId = null, 
                    LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 1, CreatedAt = createdAt 
                });

                conditions.Add(new FasSchemeCondition 
                { 
                    Id = i, GroupId = i, Field = FasConditionField.PerCapitaIncome, 
                    Operator = FasConditionOperator.LessThanOrEqual, 
                    ValueNumber = 5000m, DisplayOrder = 1, CreatedAt = createdAt 
                });

                tiers.Add(new FasSchemeTier 
                { 
                    Id = i, FasSchemeId = i, TierName = "Tier 1", 
                    MaxPerCapitaIncome = 5000m, SubsidyValue = 100m, 
                    DisplayOrder = 1, CreatedAt = createdAt 
                });

                documents.Add(new FasSchemeRequiredDocument 
                { 
                    Id = i, FasSchemeId = i, DocumentName = "Income Statement", 
                    TemplateFileKey = $"fas/templates/document-{i:D2}.pdf", 
                    DisplayOrder = 1, CreatedAt = createdAt 
                });

                courses.Add(new FasSchemeCourse 
                { 
                    Id = i, FasSchemeId = i, CourseId = (i % 10) + 1, CreatedAt = createdAt 
                });

                questions.Add(new FasSchemeAdditionalQuestion
                {
                    Id = i * 2 - 1, FasSchemeId = i,
                    QuestionText = "Briefly explain the primary reason for your financial assistance application.",
                    IsRequired = true, CreatedAt = createdAt
                });
                
                questions.Add(new FasSchemeAdditionalQuestion
                {
                    Id = i * 2, FasSchemeId = i,
                    QuestionText = "Are there any specific medical conditions in your household?",
                    IsRequired = false, CreatedAt = createdAt
                });
            }

            modelBuilder.Entity<FasScheme>().HasData(schemes);
            modelBuilder.Entity<FasSchemeConditionGroup>().HasData(conditionGroups);
            modelBuilder.Entity<FasSchemeCondition>().HasData(conditions);
            modelBuilder.Entity<FasSchemeTier>().HasData(tiers);
            modelBuilder.Entity<FasSchemeRequiredDocument>().HasData(documents);
            modelBuilder.Entity<FasSchemeCourse>().HasData(courses);
            modelBuilder.Entity<FasSchemeAdditionalQuestion>().HasData(questions);

            var applications = new List<FasApplication>();
            var appDocs = new List<FasApplicationDocument>();
            var answers = new List<FasApplicationAdditionalQuestionAnswer>();
            
            int appId = 1;
            
            // Generate 25 applications for Student 2 to satisfy ChargeSeedBuilder
            for (int i = 1; i <= 25; i++)
            {
                int schemeId = i;
                applications.Add(new FasApplication
                {
                    Id = appId,
                    FasSchemeId = schemeId,
                    SchoolStudentId = 2, 
                    RecommendedTierId = schemeId,
                    ApplicationNumber = $"FASAPP-2026-{appId:D4}",
                    Status = FasApplicationStatus.Pending,
                    StudentAgeSnapshot = 18,
                    StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen,
                    GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen,
                    GrossHouseholdIncomeSnapshot = 2500m,
                    HouseholdMemberCountSnapshot = 4,
                    PerCapitaIncomeSnapshot = 625m,
                    RecommendationReason = "PCI <= 5000",
                    CreatedAt = createdAt
                });

                appDocs.Add(new FasApplicationDocument
                {
                    Id = appId,
                    FasApplicationId = appId,
                    FasSchemeRequiredDocumentId = schemeId,
                    DocumentNameSnapshot = "Income Statement",
                    FileKey = $"fas/applications/{appId}/document.pdf",
                    FileName = $"fas-application-{appId:D2}.pdf",
                    CreatedAt = createdAt
                });

                answers.Add(new FasApplicationAdditionalQuestionAnswer
                {
                    Id = appId * 2 - 1,
                    FasApplicationId = appId,
                    FasSchemeAdditionalQuestionId = schemeId * 2 - 1,
                    QuestionTextSnapshot = "Reason",
                    IsRequiredSnapshot = true,
                    AnswerText = "Need help.",
                    CreatedAt = createdAt
                });

                appId++;
            }

            // Generate 30 applications for Student 1 (current user)
            // 6 statuses * 5 records
            var statuses = new[] { 
                FasApplicationStatus.Pending, 
                FasApplicationStatus.Approved, 
                FasApplicationStatus.Rejected, 
                FasApplicationStatus.Withdrawn, 
                FasApplicationStatus.Draft, 
                FasApplicationStatus.Expired 
            };
            
            int schemeIdForUser = 26;
            foreach (var appStatus in statuses)
            {
                for (int i = 0; i < 5; i++)
                {
                    applications.Add(new FasApplication
                    {
                        Id = appId,
                        FasSchemeId = schemeIdForUser,
                        SchoolStudentId = 1, // Current User
                        RecommendedTierId = schemeIdForUser,
                        ApplicationNumber = $"FASAPP-2026-{appId:D4}",
                        Status = appStatus,
                        StudentAgeSnapshot = 18,
                        StudentNationalitySnapshot = NationalityCategory.SingaporeCitizen,
                        GuardianNationalitySnapshot = NationalityCategory.SingaporeCitizen,
                        GrossHouseholdIncomeSnapshot = 2500m,
                        HouseholdMemberCountSnapshot = 4,
                        PerCapitaIncomeSnapshot = 625m,
                        RecommendationReason = "PCI <= 5000",
                        CreatedAt = createdAt
                    });

                    appDocs.Add(new FasApplicationDocument
                    {
                        Id = appId,
                        FasApplicationId = appId,
                        FasSchemeRequiredDocumentId = schemeIdForUser,
                        DocumentNameSnapshot = "Income Statement",
                        FileKey = $"fas/applications/{appId}/document.pdf",
                        FileName = $"fas-application-{appId:D2}.pdf",
                        CreatedAt = createdAt
                    });

                    answers.Add(new FasApplicationAdditionalQuestionAnswer
                    {
                        Id = appId * 2 - 1,
                        FasApplicationId = appId,
                        FasSchemeAdditionalQuestionId = schemeIdForUser * 2 - 1,
                        QuestionTextSnapshot = "Reason",
                        IsRequiredSnapshot = true,
                        AnswerText = "Need help.",
                        CreatedAt = createdAt
                    });

                    appId++;
                    schemeIdForUser++;
                }
            }

            modelBuilder.Entity<FasApplication>().HasData(applications);
            modelBuilder.Entity<FasApplicationDocument>().HasData(appDocs);
            modelBuilder.Entity<FasApplicationAdditionalQuestionAnswer>().HasData(answers);

            modelBuilder.Entity<FasTierOverrideHistory>().HasData(
                new FasTierOverrideHistory { Id = 1, FasApplicationId = 2, OldTierId = 2, NewTierId = 2, ModifiedByUserId = 1, ModifiedAt = approvedAt.AddDays(2), Reason = "Seed tier review trail.", CreatedAt = createdAt },
                new FasTierOverrideHistory { Id = 2, FasApplicationId = 6, OldTierId = 6, NewTierId = 6, ModifiedByUserId = 1, ModifiedAt = approvedAt.AddDays(6), Reason = "Seed tier review trail.", CreatedAt = createdAt },
                new FasTierOverrideHistory { Id = 3, FasApplicationId = 10, OldTierId = 10, NewTierId = 10, ModifiedByUserId = 1, ModifiedAt = approvedAt.AddDays(10), Reason = "Seed tier review trail.", CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
