using Models;

namespace Persistence.SqlServer.ModelConfigurations;

public static class CheckConstraintConfigurator
{
    public static void ConfigureCheckConstraints(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AiAssistantSetting>().ToTable(table =>
            table.HasCheckConstraint("CK_AiAssistantSetting_Singleton", "[Id] = 1"));

        modelBuilder.Entity<EducationAccount>().ToTable(table =>
            table.HasCheckConstraint(
                "CK_EducationAccount_Balance_NonNegative",
                "[EducationCreditBalance] >= 0"));

        modelBuilder.Entity<EducationCreditTransaction>().ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_EducationCreditTransaction_Amounts_NonNegative",
                "[Amount] >= 0 AND [BalanceBefore] >= 0 AND [BalanceAfter] >= 0");
            table.HasCheckConstraint(
                "CK_EducationCreditTransaction_BalanceEquation",
                "([Direction] = 1 AND [BalanceAfter] = [BalanceBefore] + [Amount]) OR " +
                "([Direction] = 2 AND [BalanceAfter] = [BalanceBefore] - [Amount])");
        });

        modelBuilder.Entity<Course>().ToTable(table =>
            table.HasCheckConstraint(
                "CK_Course_Amounts_NonNegative",
                "[CourseFeeAmount] >= 0 AND [MiscFeeAmount] >= 0 AND [GstAmount] >= 0"));

        modelBuilder.Entity<Charge>().ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_Charge_Amounts_NonNegative",
                "[GrossAmount] >= 0 AND [SubsidyAmount] >= 0 AND [NetAmount] >= 0 " +
                "AND [PaidAmount] >= 0 AND [RemainingAmount] >= 0");
            table.HasCheckConstraint(
                "CK_Charge_AmountEquations",
                "[SubsidyAmount] <= [GrossAmount] AND " +
                "[NetAmount] = [GrossAmount] - [SubsidyAmount] AND " +
                "[PaidAmount] <= [NetAmount] AND " +
                "[RemainingAmount] = [NetAmount] - [PaidAmount]");
        });

        modelBuilder.Entity<Payment>().ToTable(table =>
            table.HasCheckConstraint("CK_Payment_TotalAmount_NonNegative", "[TotalAmount] >= 0"));

        modelBuilder.Entity<PaymentAllocation>().ToTable(table =>
            table.HasCheckConstraint("CK_PaymentAllocation_Amount_NonNegative", "[Amount] >= 0"));

        modelBuilder.Entity<TopupRule>().ToTable(table =>
            table.HasCheckConstraint(
                "CK_TopupRule_Amount_By_MatchMode",
                "([MatchMode] = 1 AND [TopupAmount] > 0) OR " +
                "([MatchMode] = 2 AND [TopupAmount] IS NULL)"));

        modelBuilder.Entity<TopupRuleCondition>().ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_TopupRuleCondition_Value_By_Field",
                "([Field] IN (1, 2) AND [ValueNumber] IS NOT NULL AND [ValueText] IS NULL) OR " +
                "([Field] = 3 AND [ValueText] IS NOT NULL AND [ValueNumber] IS NULL)");
            table.HasCheckConstraint(
                "CK_TopupRuleCondition_DisplayOrder_NonNegative",
                "[DisplayOrder] >= 0");
        });

        modelBuilder.Entity<TopupExecution>().ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_TopupExecution_Counts_And_Amount_NonNegative",
                "[TotalExecutedAmount] >= 0 AND [TotalTargetCount] >= 0 " +
                "AND [SuccessCount] >= 0 AND [FailedCount] >= 0 " +
                "AND [SuccessCount] + [FailedCount] <= [TotalTargetCount]");
            table.HasCheckConstraint(
                "CK_TopupExecution_Source_Fields",
                "([SourceType] = 1 AND [TopupRuleId] IS NOT NULL AND [TopupScheduleId] IS NULL " +
                "AND [ManualAmount] IS NULL AND [ManualReason] IS NULL) OR " +
                "([SourceType] = 2 AND [TopupRuleId] IS NOT NULL AND [TopupScheduleId] IS NOT NULL " +
                "AND [ManualAmount] IS NULL AND [ManualReason] IS NULL) OR " +
                "([SourceType] = 3 AND [TopupRuleId] IS NULL AND [TopupScheduleId] IS NULL " +
                "AND [ManualAmount] > 0 AND [ManualReason] IS NOT NULL)");
        });

        modelBuilder.Entity<TopupExecutionTarget>().ToTable(table =>
        {
            table.HasCheckConstraint("CK_TopupExecutionTarget_Amount_Positive", "[Amount] > 0");
            table.HasCheckConstraint(
                "CK_TopupExecutionTarget_Result",
                "([Status] = 3 AND [EducationAccountId] IS NOT NULL " +
                "AND [EducationCreditTransactionId] IS NOT NULL AND [FailureReason] IS NULL) OR " +
                "([Status] = 4 AND [EducationCreditTransactionId] IS NULL AND [FailureReason] IS NOT NULL) OR " +
                "([Status] IN (1, 2) AND [EducationCreditTransactionId] IS NULL)");
        });
    }
}
