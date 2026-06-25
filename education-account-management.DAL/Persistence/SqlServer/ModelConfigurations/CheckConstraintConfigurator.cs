using Models;

namespace Persistence.SqlServer.ModelConfigurations
{
    public static class CheckConstraintConfigurator
    {
        public static void ConfigureCheckConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AiAssistantSetting>().ToTable(table =>
            {
                table.HasCheckConstraint("CK_AiAssistantSetting_Singleton", "[Id] = 1");
                table.HasCheckConstraint("CK_AiAssistantSetting_TaxRate", "[TaxRate] >= 0");
            });

            modelBuilder.Entity<EducationAccount>().ToTable(table =>
                table.HasCheckConstraint(
                    "CK_EducationAccount_Balance_NonNegative",
                    "[EducationCreditBalance] >= 0"));

            modelBuilder.Entity<EducationCreditTransaction>().ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_EducationCreditTransaction_Amounts_NonNegative",
                    "[Amount] > 0 AND [BalanceBefore] >= 0 AND [BalanceAfter] >= 0");
                table.HasCheckConstraint(
                    "CK_EducationCreditTransaction_BalanceEquation",
                    "([Direction] = 1 AND [BalanceAfter] = [BalanceBefore] + [Amount]) OR " +
                    "([Direction] = 2 AND [BalanceAfter] = [BalanceBefore] - [Amount])");
            });

            modelBuilder.Entity<Course>().ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Course_Amounts_NonNegative",
                    "[CourseFeeAmount] >= 0 AND [MiscFeeAmount] >= 0 AND [GstAmount] >= 0");
                table.HasCheckConstraint(
                    "CK_Course_Date_Order",
                    "[EnrollmentDeadline] <= [StartDate] AND [StartDate] <= [EndDate]");
            });

            modelBuilder.Entity<Charge>().ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Charge_Amounts_NonNegative",
                    "[CourseFeeAmountSnapshot] >= 0 AND [MiscFeeAmountSnapshot] >= 0 " +
                    "AND [GstAmountSnapshot] >= 0 AND [GrossAmount] >= 0 " +
                    "AND [SubsidyAmount] >= 0 AND [NetAmount] >= 0 " +
                    "AND [PaidAmount] >= 0 AND [RemainingAmount] >= 0");
                table.HasCheckConstraint(
                    "CK_Charge_AmountEquations",
                    "[SubsidyAmount] <= [GrossAmount] AND " +
                    "[NetAmount] = [GrossAmount] - [SubsidyAmount] AND " +
                    "[PaidAmount] <= [NetAmount] AND " +
                    "[RemainingAmount] = [NetAmount] - [PaidAmount]");
            });

            modelBuilder.Entity<Payment>().ToTable(table =>
                table.HasCheckConstraint("CK_Payment_TotalAmount_Positive", "[TotalAmount] > 0"));

            modelBuilder.Entity<PaymentAllocation>().ToTable(table =>
                table.HasCheckConstraint(
                    "CK_PaymentAllocation_Amounts",
                    "[Amount] > 0 AND [ChargeGrossAmountSnapshot] >= 0 " +
                    "AND [ChargeNetAmountSnapshot] >= 0 AND [ChargeRemainingAmountSnapshot] >= 0"));

            modelBuilder.Entity<ChargeInstallment>().ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_ChargeInstallment_Amounts",
                    "[Amount] > 0 AND [PaidAmount] >= 0 AND [RemainingAmount] >= 0 " +
                    "AND [PaidAmount] <= [Amount] AND [RemainingAmount] = [Amount] - [PaidAmount]");
                table.HasCheckConstraint("CK_ChargeInstallment_Number_Positive", "[InstallmentNumber] > 0");
            });

            modelBuilder.Entity<OutstandingDeductionRun>().ToTable(table =>
                table.HasCheckConstraint(
                    "CK_OutstandingDeductionRun_Totals_NonNegative",
                    "[TotalScannedCharges] >= 0 AND [TotalDeductedAmount] >= 0 " +
                    "AND [SuccessCount] >= 0 AND [FailedCount] >= 0 " +
                    "AND [SuccessCount] + [FailedCount] <= [TotalScannedCharges]"));

            modelBuilder.Entity<OutstandingDeductionTarget>().ToTable(table =>
                table.HasCheckConstraint(
                    "CK_OutstandingDeductionTarget_Amounts_NonNegative",
                    "[BalanceBefore] >= 0 AND [RemainingBefore] >= 0 AND [DeductedAmount] >= 0 " +
                    "AND [BalanceAfter] >= 0 AND [RemainingAfter] >= 0"));

            modelBuilder.Entity<SystemTopup>().ToTable(table =>
                table.HasCheckConstraint(
                    "CK_SystemTopup_Amount_By_Status",
                    "([Status] = 1 AND [TopupAmount] > 0) OR " +
                    "([Status] = 2 AND ([TopupAmount] IS NULL OR [TopupAmount] > 0))"));

            modelBuilder.Entity<ScheduleTopUp>().ToTable(table =>
                table.HasCheckConstraint(
                    "CK_ScheduleTopUp_Amount_By_Status",
                    "([Status] IN (1, 3) AND [TopupAmount] > 0) OR " +
                    "([Status] = 2 AND ([TopupAmount] IS NULL OR [TopupAmount] > 0))"));

            ConfigureConditionConstraints<SystemTopupCondition>(modelBuilder, "SystemTopupCondition");
            ConfigureConditionConstraints<ScheduleTopUpCondition>(modelBuilder, "ScheduleTopUpCondition");
            ConfigureGroupConstraints<SystemTopupConditionGroup>(modelBuilder, "SystemTopupConditionGroup");
            ConfigureGroupConstraints<ScheduleTopUpConditionGroup>(modelBuilder, "ScheduleTopUpConditionGroup");
            ConfigureFasConditionConstraints(modelBuilder);
            ConfigureGroupConstraints<FasSchemeConditionGroup>(modelBuilder, "FasSchemeConditionGroup");

            modelBuilder.Entity<TopupExecution>().ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_TopupExecution_Counts_And_Amount_NonNegative",
                    "[TotalExecutedAmount] >= 0 AND [TotalTargetCount] >= 0 " +
                    "AND [SuccessCount] >= 0 AND [FailedCount] >= 0 " +
                    "AND [SuccessCount] + [FailedCount] <= [TotalTargetCount]");
                table.HasCheckConstraint(
                    "CK_TopupExecution_Source_Fields",
                    "([SourceType] = 1 AND [SystemTopupId] IS NOT NULL AND [ScheduleTopUpId] IS NULL " +
                    "AND [ManualAmount] IS NULL AND [ManualReason] IS NULL) OR " +
                    "([SourceType] = 2 AND [SystemTopupId] IS NULL AND [ScheduleTopUpId] IS NOT NULL " +
                    "AND [ManualAmount] IS NULL AND [ManualReason] IS NULL) OR " +
                    "([SourceType] = 3 AND [SystemTopupId] IS NULL AND [ScheduleTopUpId] IS NULL " +
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

        private static void ConfigureFasConditionConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FasSchemeCondition>().ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_FasSchemeCondition_Value_By_Field",
                    "([Field] IN (1, 4, 5) AND [ValueNumber] IS NOT NULL AND [CountryId] IS NULL AND " +
                    "(([Operator] = 7 AND [ValueNumberTo] IS NOT NULL AND [ValueNumberTo] >= [ValueNumber]) OR " +
                    "([Operator] <> 7 AND [ValueNumberTo] IS NULL))) OR " +
                    "([Field] IN (2, 3) AND [CountryId] IS NOT NULL AND [Operator] IN (1, 2) " +
                    "AND [ValueNumber] IS NULL AND [ValueNumberTo] IS NULL)");
                table.HasCheckConstraint("CK_FasSchemeCondition_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
            });
        }

        private static void ConfigureConditionConstraints<TCondition>(ModelBuilder modelBuilder, string tableName)
            where TCondition : BaseEntity
        {
            modelBuilder.Entity<TCondition>().ToTable(table =>
            {
                table.HasCheckConstraint(
                    $"CK_{tableName}_Value_By_Field",
                    "([Field] IN (1, 2) AND [ValueNumber] IS NOT NULL AND [ValueText] IS NULL AND " +
                    "(([Operator] = 7 AND [ValueNumberTo] IS NOT NULL AND [ValueNumberTo] >= [ValueNumber]) OR " +
                    "([Operator] <> 7 AND [ValueNumberTo] IS NULL))) OR " +
                    "([Field] = 3 AND [Operator] IN (1, 2) AND [ValueText] IS NOT NULL " +
                    "AND [ValueNumber] IS NULL AND [ValueNumberTo] IS NULL)");
                table.HasCheckConstraint($"CK_{tableName}_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
            });
        }

        private static void ConfigureGroupConstraints<TGroup>(ModelBuilder modelBuilder, string tableName)
            where TGroup : BaseEntity
        {
            modelBuilder.Entity<TGroup>().ToTable(table =>
                table.HasCheckConstraint($"CK_{tableName}_DisplayOrder_NonNegative", "[DisplayOrder] >= 0"));
        }
    }
}
