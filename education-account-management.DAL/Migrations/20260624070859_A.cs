using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AiAssistantSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiAssistantSetting", x => x.Id);
                    table.CheckConstraint("CK_AiAssistantSetting_Singleton", "[Id] = 1");
                });

            migrationBuilder.CreateTable(
                name: "Citizen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nric = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ResidentialAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MailingAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    CitizenshipStatus = table.Column<int>(type: "int", nullable: false),
                    SchoolingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationAccountSweepReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountsCreatedCount = table.Column<int>(type: "int", nullable: false),
                    AccountsClosedCount = table.Column<int>(type: "int", nullable: false),
                    AccountsExtendedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationAccountSweepReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutstandingDeductionRun",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunMonth = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    RunDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalScannedCharges = table.Column<int>(type: "int", nullable: false),
                    TotalDeductedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SuccessCount = table.Column<int>(type: "int", nullable: false),
                    FailedCount = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutstandingDeductionRun", x => x.Id);
                    table.CheckConstraint("CK_OutstandingDeductionRun_Totals_NonNegative", "[TotalScannedCharges] >= 0 AND [TotalDeductedAmount] >= 0 AND [SuccessCount] >= 0 AND [FailedCount] >= 0 AND [SuccessCount] + [FailedCount] <= [TotalScannedCharges]");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleTopUp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TopupAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    OneTimeExecutionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExecuteAtDay = table.Column<int>(type: "int", nullable: true),
                    ExecuteAtMonth = table.Column<int>(type: "int", nullable: true),
                    ExecutionTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    NextExecutionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleTopUp", x => x.Id);
                    table.CheckConstraint("CK_ScheduleTopUp_Amount_By_Status", "([Status] IN (1, 3) AND [TopupAmount] > 0) OR ([Status] = 2 AND ([TopupAmount] IS NULL OR [TopupAmount] > 0))");
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemTopup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TopupAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemTopup", x => x.Id);
                    table.CheckConstraint("CK_SystemTopup_Amount_By_Status", "([Status] = 1 AND [TopupAmount] > 0) OR ([Status] = 2 AND ([TopupAmount] IS NULL OR [TopupAmount] > 0))");
                });

            migrationBuilder.CreateTable(
                name: "EducationAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EducationCreditBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CitizenId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationAccount", x => x.Id);
                    table.CheckConstraint("CK_EducationAccount_Balance_NonNegative", "[EducationCreditBalance] >= 0");
                    table.ForeignKey(
                        name: "FK_EducationAccount_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false),
                    LockedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CitizenId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "EducationAccountSweepTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SweepReportId = table.Column<int>(type: "int", nullable: false),
                    Nric = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationAccountSweepTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationAccountSweepTargets_EducationAccountSweepReports_SweepReportId",
                        column: x => x.SweepReportId,
                        principalTable: "EducationAccountSweepReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleTopUpConditionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleTopUpId = table.Column<int>(type: "int", nullable: false),
                    ParentGroupId = table.Column<int>(type: "int", nullable: true),
                    LogicalOperator = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleTopUpConditionGroup", x => x.Id);
                    table.CheckConstraint("CK_ScheduleTopUpConditionGroup_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
                    table.ForeignKey(
                        name: "FK_ScheduleTopUpConditionGroup_ScheduleTopUpConditionGroup_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalTable: "ScheduleTopUpConditionGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScheduleTopUpConditionGroup_ScheduleTopUp_ScheduleTopUpId",
                        column: x => x.ScheduleTopUpId,
                        principalTable: "ScheduleTopUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CourseFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MiscFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GstAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FasApplicationDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.CheckConstraint("CK_Course_Amounts_NonNegative", "[CourseFeeAmount] >= 0 AND [MiscFeeAmount] >= 0 AND [GstAmount] >= 0");
                    table.CheckConstraint("CK_Course_Date_Order", "[FasApplicationDueDate] <= [StartDate] AND [StartDate] <= [EndDate]");
                    table.ForeignKey(
                        name: "FK_Course_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemTopupConditionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemTopupId = table.Column<int>(type: "int", nullable: false),
                    ParentGroupId = table.Column<int>(type: "int", nullable: true),
                    LogicalOperator = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemTopupConditionGroup", x => x.Id);
                    table.CheckConstraint("CK_SystemTopupConditionGroup_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
                    table.ForeignKey(
                        name: "FK_SystemTopupConditionGroup_SystemTopupConditionGroup_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalTable: "SystemTopupConditionGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemTopupConditionGroup_SystemTopup_SystemTopupId",
                        column: x => x.SystemTopupId,
                        principalTable: "SystemTopup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopupExecution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    SystemTopupId = table.Column<int>(type: "int", nullable: true),
                    ScheduleTopUpId = table.Column<int>(type: "int", nullable: true),
                    IdempotencyKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ManualAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ManualReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalTargetCount = table.Column<int>(type: "int", nullable: false),
                    SuccessCount = table.Column<int>(type: "int", nullable: false),
                    FailedCount = table.Column<int>(type: "int", nullable: false),
                    TotalExecutedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TopupNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TopupAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConditionsSnapshot = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopupExecution", x => x.Id);
                    table.CheckConstraint("CK_TopupExecution_Counts_And_Amount_NonNegative", "[TotalExecutedAmount] >= 0 AND [TotalTargetCount] >= 0 AND [SuccessCount] >= 0 AND [FailedCount] >= 0 AND [SuccessCount] + [FailedCount] <= [TotalTargetCount]");
                    table.CheckConstraint("CK_TopupExecution_Source_Fields", "([SourceType] = 1 AND [SystemTopupId] IS NOT NULL AND [ScheduleTopUpId] IS NULL AND [ManualAmount] IS NULL AND [ManualReason] IS NULL) OR ([SourceType] = 2 AND [SystemTopupId] IS NULL AND [ScheduleTopUpId] IS NOT NULL AND [ManualAmount] IS NULL AND [ManualReason] IS NULL) OR ([SourceType] = 3 AND [SystemTopupId] IS NULL AND [ScheduleTopUpId] IS NULL AND [ManualAmount] > 0 AND [ManualReason] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_TopupExecution_ScheduleTopUp_ScheduleTopUpId",
                        column: x => x.ScheduleTopUpId,
                        principalTable: "ScheduleTopUp",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopupExecution_SystemTopup_SystemTopupId",
                        column: x => x.SystemTopupId,
                        principalTable: "SystemTopup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EducationCreditTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    EducationAccountId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationCreditTransaction", x => x.Id);
                    table.CheckConstraint("CK_EducationCreditTransaction_Amounts_NonNegative", "[Amount] > 0 AND [BalanceBefore] >= 0 AND [BalanceAfter] >= 0");
                    table.CheckConstraint("CK_EducationCreditTransaction_BalanceEquation", "([Direction] = 1 AND [BalanceAfter] = [BalanceBefore] + [Amount]) OR ([Direction] = 2 AND [BalanceAfter] = [BalanceBefore] - [Amount])");
                    table.ForeignKey(
                        name: "FK_EducationCreditTransaction_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolStudent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    EducationAccountId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolStudent_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolStudent_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdminProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Nric = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminProfile_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminProfile_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nric = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActorUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLog_User_ActorUserId",
                        column: x => x.ActorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EducationAccountStatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EducationAccountId = table.Column<int>(type: "int", nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationAccountStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationAccountStatusHistory_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationAccountStatusHistory_User_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SsoIdentity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<int>(type: "int", nullable: false),
                    ProviderUserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsoIdentity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SsoIdentity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStatusHistory_User_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserStatusHistory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleTopUpCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Field = table.Column<int>(type: "int", nullable: false),
                    Operator = table.Column<int>(type: "int", nullable: false),
                    ValueText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValueNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValueNumberTo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleTopUpCondition", x => x.Id);
                    table.CheckConstraint("CK_ScheduleTopUpCondition_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
                    table.CheckConstraint("CK_ScheduleTopUpCondition_Value_By_Field", "([Field] IN (1, 2) AND [ValueNumber] IS NOT NULL AND [ValueText] IS NULL AND (([Operator] = 7 AND [ValueNumberTo] IS NOT NULL AND [ValueNumberTo] >= [ValueNumber]) OR ([Operator] <> 7 AND [ValueNumberTo] IS NULL))) OR ([Field] = 3 AND [Operator] IN (1, 2) AND [ValueText] IS NOT NULL AND [ValueNumber] IS NULL AND [ValueNumberTo] IS NULL)");
                    table.ForeignKey(
                        name: "FK_ScheduleTopUpCondition_ScheduleTopUpConditionGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ScheduleTopUpConditionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemTopupCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Field = table.Column<int>(type: "int", nullable: false),
                    Operator = table.Column<int>(type: "int", nullable: false),
                    ValueText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValueNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValueNumberTo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemTopupCondition", x => x.Id);
                    table.CheckConstraint("CK_SystemTopupCondition_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
                    table.CheckConstraint("CK_SystemTopupCondition_Value_By_Field", "([Field] IN (1, 2) AND [ValueNumber] IS NOT NULL AND [ValueText] IS NULL AND (([Operator] = 7 AND [ValueNumberTo] IS NOT NULL AND [ValueNumberTo] >= [ValueNumber]) OR ([Operator] <> 7 AND [ValueNumberTo] IS NULL))) OR ([Field] = 3 AND [Operator] IN (1, 2) AND [ValueText] IS NOT NULL AND [ValueNumber] IS NULL AND [ValueNumberTo] IS NULL)");
                    table.ForeignKey(
                        name: "FK_SystemTopupCondition_SystemTopupConditionGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "SystemTopupConditionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EducationCreditTransactionId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AccountNumberSnapshot = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CitizenNricSnapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CitizenFullNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.CheckConstraint("CK_Payment_TotalAmount_Positive", "[TotalAmount] > 0");
                    table.ForeignKey(
                        name: "FK_Payment_EducationCreditTransaction_EducationCreditTransactionId",
                        column: x => x.EducationCreditTransactionId,
                        principalTable: "EducationCreditTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopupExecutionTarget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopupExecutionId = table.Column<int>(type: "int", nullable: false),
                    EducationAccountId = table.Column<int>(type: "int", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MatchedConditionsSnapshot = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    EducationCreditTransactionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopupExecutionTarget", x => x.Id);
                    table.CheckConstraint("CK_TopupExecutionTarget_Amount_Positive", "[Amount] > 0");
                    table.CheckConstraint("CK_TopupExecutionTarget_Result", "([Status] = 3 AND [EducationAccountId] IS NOT NULL AND [EducationCreditTransactionId] IS NOT NULL AND [FailureReason] IS NULL) OR ([Status] = 4 AND [EducationCreditTransactionId] IS NULL AND [FailureReason] IS NOT NULL) OR ([Status] IN (1, 2) AND [EducationCreditTransactionId] IS NULL)");
                    table.ForeignKey(
                        name: "FK_TopupExecutionTarget_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopupExecutionTarget_EducationCreditTransaction_EducationCreditTransactionId",
                        column: x => x.EducationCreditTransactionId,
                        principalTable: "EducationCreditTransaction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopupExecutionTarget_TopupExecution_TopupExecutionId",
                        column: x => x.TopupExecutionId,
                        principalTable: "TopupExecution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SchoolStudentId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    SchoolNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseDescriptionSnapshot = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CitizenNricSnapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CitizenFullNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CitizenEmailSnapshot = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    CitizenPhoneNumberSnapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AccountNumberSnapshot = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollment_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_SchoolStudent_SchoolStudentId",
                        column: x => x.SchoolStudentId,
                        principalTable: "SchoolStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopupSystemApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemTopupId = table.Column<int>(type: "int", nullable: false),
                    EducationAccountId = table.Column<int>(type: "int", nullable: false),
                    TopupExecutionTargetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopupSystemApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopupSystemApplication_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopupSystemApplication_SystemTopup_SystemTopupId",
                        column: x => x.SystemTopupId,
                        principalTable: "SystemTopup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopupSystemApplication_TopupExecutionTarget_TopupExecutionTargetId",
                        column: x => x.TopupExecutionTargetId,
                        principalTable: "TopupExecutionTarget",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Charge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrollmentId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SchoolNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseCodeSnapshot = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CourseNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseDescriptionSnapshot = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CourseStartDateSnapshot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseEndDateSnapshot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseFeeAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MiscFeeAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GstAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubsidyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BecameOutstandingAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAutoDeductedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charge", x => x.Id);
                    table.CheckConstraint("CK_Charge_AmountEquations", "[SubsidyAmount] <= [GrossAmount] AND [NetAmount] = [GrossAmount] - [SubsidyAmount] AND [PaidAmount] <= [NetAmount] AND [RemainingAmount] = [NetAmount] - [PaidAmount]");
                    table.CheckConstraint("CK_Charge_Amounts_NonNegative", "[CourseFeeAmountSnapshot] >= 0 AND [MiscFeeAmountSnapshot] >= 0 AND [GstAmountSnapshot] >= 0 AND [GrossAmount] >= 0 AND [SubsidyAmount] >= 0 AND [NetAmount] >= 0 AND [PaidAmount] >= 0 AND [RemainingAmount] >= 0");
                    table.ForeignKey(
                        name: "FK_Charge_Enrollment_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutstandingDeductionTarget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutstandingDeductionRunId = table.Column<int>(type: "int", nullable: false),
                    ChargeId = table.Column<int>(type: "int", nullable: false),
                    EducationAccountId = table.Column<int>(type: "int", nullable: false),
                    EducationCreditTransactionId = table.Column<int>(type: "int", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeductedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutstandingDeductionTarget", x => x.Id);
                    table.CheckConstraint("CK_OutstandingDeductionTarget_Amounts_NonNegative", "[BalanceBefore] >= 0 AND [RemainingBefore] >= 0 AND [DeductedAmount] >= 0 AND [BalanceAfter] >= 0 AND [RemainingAfter] >= 0");
                    table.ForeignKey(
                        name: "FK_OutstandingDeductionTarget_Charge_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "Charge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutstandingDeductionTarget_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutstandingDeductionTarget_EducationCreditTransaction_EducationCreditTransactionId",
                        column: x => x.EducationCreditTransactionId,
                        principalTable: "EducationCreditTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutstandingDeductionTarget_OutstandingDeductionRun_OutstandingDeductionRunId",
                        column: x => x.OutstandingDeductionRunId,
                        principalTable: "OutstandingDeductionRun",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutstandingDeductionTarget_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentAllocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    ChargeId = table.Column<int>(type: "int", nullable: false),
                    CourseNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SchoolNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ChargeGrossAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChargeNetAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChargeRemainingAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentAllocation", x => x.Id);
                    table.CheckConstraint("CK_PaymentAllocation_Amounts", "[Amount] > 0 AND [ChargeGrossAmountSnapshot] >= 0 AND [ChargeNetAmountSnapshot] >= 0 AND [ChargeRemainingAmountSnapshot] >= 0");
                    table.ForeignKey(
                        name: "FK_PaymentAllocation_Charge_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "Charge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentAllocation_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AiAssistantSetting",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "IsEnabled", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, true, null, null });

            migrationBuilder.InsertData(
                table: "Citizen",
                columns: new[] { "Id", "CitizenshipStatus", "CreatedAt", "CreatedBy", "DateOfBirth", "DeletedAt", "Email", "FullName", "IsDeleted", "MailingAddress", "Nric", "PhoneNumber", "ResidentialAddress", "SchoolingStatus", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1991, 1, 2), null, "citizen001@example.com", "Citizen 001", false, "Mailing block 1, Singapore", "S0000001I", "+6590000001", "Residential block 1, Singapore", "Enrolled", null, null },
                    { 2, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1992, 2, 3), null, "citizen002@example.com", "Citizen 002", false, "Mailing block 2, Singapore", "S0000002G", "+6590000002", "Residential block 2, Singapore", "Not Enrolled", null, null },
                    { 3, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1993, 3, 4), null, "citizen003@example.com", "Citizen 003", false, "Mailing block 3, Singapore", "S0000003E", "+6590000003", "Residential block 3, Singapore", "Graduated", null, null },
                    { 4, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1994, 4, 5), null, "citizen004@example.com", "Citizen 004", false, "Mailing block 4, Singapore", "S0000004C", "+6590000004", "Residential block 4, Singapore", "Suspended", null, null },
                    { 5, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1995, 5, 6), null, "citizen005@example.com", "Citizen 005", false, "Mailing block 5, Singapore", "S0000005A", "+6590000005", "Residential block 5, Singapore", "Withdrawn", null, null },
                    { 6, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1996, 6, 7), null, "citizen006@example.com", "Citizen 006", false, "Mailing block 6, Singapore", "S0000006Z", "+6590000006", "Residential block 6, Singapore", "Enrolled", null, null },
                    { 7, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1997, 7, 8), null, "citizen007@example.com", "Citizen 007", false, "Mailing block 7, Singapore", "S0000007H", "+6590000007", "Residential block 7, Singapore", "Not Enrolled", null, null },
                    { 8, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1998, 8, 9), null, "citizen008@example.com", "Citizen 008", false, "Mailing block 8, Singapore", "S0000008F", "+6590000008", "Residential block 8, Singapore", "Graduated", null, null },
                    { 9, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1999, 9, 10), null, "citizen009@example.com", "Citizen 009", false, "Mailing block 9, Singapore", "S0000009D", "+6590000009", "Residential block 9, Singapore", "Suspended", null, null },
                    { 10, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 10, 11), null, "citizen010@example.com", "Citizen 010", false, "Mailing block 10, Singapore", "S0000010H", "+6590000010", "Residential block 10, Singapore", "Withdrawn", null, null },
                    { 11, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 11, 12), null, "citizen011@example.com", "Citizen 011", false, "Mailing block 11, Singapore", "S0000011F", "+6590000011", "Residential block 11, Singapore", "Enrolled", null, null },
                    { 12, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 12, 13), null, "citizen012@example.com", "Citizen 012", false, "Mailing block 12, Singapore", "S0000012D", "+6590000012", "Residential block 12, Singapore", "Not Enrolled", null, null },
                    { 13, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 1, 14), null, "citizen013@example.com", "Citizen 013", false, "Mailing block 13, Singapore", "S0000013B", "+6590000013", "Residential block 13, Singapore", "Graduated", null, null },
                    { 14, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 2, 15), null, "citizen014@example.com", "Citizen 014", false, "Mailing block 14, Singapore", "S0000014J", "+6590000014", "Residential block 14, Singapore", "Suspended", null, null },
                    { 15, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2005, 3, 16), null, "citizen015@example.com", "Citizen 015", false, "Mailing block 15, Singapore", "S0000015I", "+6590000015", "Residential block 15, Singapore", "Withdrawn", null, null },
                    { 16, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 1, 16), null, "unlinked.citizen@example.com", "Unlinked Test Citizen", false, "16 Test Avenue, Singapore", "S0000016G", "+6590000016", "16 Test Avenue, Singapore", "Not Enrolled", null, null },
                    { 17, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 5, 15), null, "student.citizen017@example.com", "School Student Citizen 017", false, "17 Student Avenue, Singapore", "S0000017E", "+6590000017", "17 Student Avenue, Singapore", "Enrolled", null, null },
                    { 18, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 6, 15), null, "student.citizen018@example.com", "School Student Citizen 018", false, "18 Student Avenue, Singapore", "S0000018C", "+6590000018", "18 Student Avenue, Singapore", "Enrolled", null, null },
                    { 19, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 7, 15), null, "student.citizen019@example.com", "School Student Citizen 019", false, "19 Student Avenue, Singapore", "S0000019A", "+6590000019", "19 Student Avenue, Singapore", "Enrolled", null, null },
                    { 20, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 8, 15), null, "student.citizen020@example.com", "School Student Citizen 020", false, "20 Student Avenue, Singapore", "S0000020E", "+6590000020", "20 Student Avenue, Singapore", "Enrolled", null, null },
                    { 21, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 9, 15), null, "student.citizen021@example.com", "School Student Citizen 021", false, "21 Student Avenue, Singapore", "S0000021C", "+6590000021", "21 Student Avenue, Singapore", "Enrolled", null, null },
                    { 22, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 10, 15), null, "student.citizen022@example.com", "School Student Citizen 022", false, "22 Student Avenue, Singapore", "S0000022A", "+6590000022", "22 Student Avenue, Singapore", "Enrolled", null, null },
                    { 23, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 11, 15), null, "student.citizen023@example.com", "School Student Citizen 023", false, "23 Student Avenue, Singapore", "S0000023Z", "+6590000023", "23 Student Avenue, Singapore", "Enrolled", null, null },
                    { 24, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 12, 15), null, "student.citizen024@example.com", "School Student Citizen 024", false, "24 Student Avenue, Singapore", "S0000024H", "+6590000024", "24 Student Avenue, Singapore", "Enrolled", null, null },
                    { 25, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 1, 15), null, "student.citizen025@example.com", "School Student Citizen 025", false, "25 Student Avenue, Singapore", "S0000025F", "+6590000025", "25 Student Avenue, Singapore", "Enrolled", null, null },
                    { 26, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 2, 15), null, "student.citizen026@example.com", "School Student Citizen 026", false, "26 Student Avenue, Singapore", "S0000026D", "+6590000026", "26 Student Avenue, Singapore", "Enrolled", null, null },
                    { 27, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 3, 15), null, "student.citizen027@example.com", "School Student Citizen 027", false, "27 Student Avenue, Singapore", "S0000027B", "+6590000027", "27 Student Avenue, Singapore", "Enrolled", null, null },
                    { 28, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 4, 15), null, "student.citizen028@example.com", "School Student Citizen 028", false, "28 Student Avenue, Singapore", "S0000028J", "+6590000028", "28 Student Avenue, Singapore", "Enrolled", null, null },
                    { 29, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 5, 15), null, "student.citizen029@example.com", "School Student Citizen 029", false, "29 Student Avenue, Singapore", "S0000029I", "+6590000029", "29 Student Avenue, Singapore", "Enrolled", null, null },
                    { 30, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 15), null, "student.citizen030@example.com", "School Student Citizen 030", false, "30 Student Avenue, Singapore", "S0000030B", "+6590000030", "30 Student Avenue, Singapore", "Enrolled", null, null },
                    { 31, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 15), null, "student.citizen031@example.com", "School Student Citizen 031", false, "31 Student Avenue, Singapore", "S0000031J", "+6590000031", "31 Student Avenue, Singapore", "Enrolled", null, null },
                    { 32, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 15), null, "student.citizen032@example.com", "School Student Citizen 032", false, "32 Student Avenue, Singapore", "S0000032I", "+6590000032", "32 Student Avenue, Singapore", "Enrolled", null, null },
                    { 33, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 15), null, "student.citizen033@example.com", "School Student Citizen 033", false, "33 Student Avenue, Singapore", "S0000033G", "+6590000033", "33 Student Avenue, Singapore", "Enrolled", null, null },
                    { 34, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 15), null, "student.citizen034@example.com", "School Student Citizen 034", false, "34 Student Avenue, Singapore", "S0000034E", "+6590000034", "34 Student Avenue, Singapore", "Enrolled", null, null },
                    { 35, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 11, 15), null, "student.citizen035@example.com", "School Student Citizen 035", false, "35 Student Avenue, Singapore", "S0000035C", "+6590000035", "35 Student Avenue, Singapore", "Enrolled", null, null },
                    { 36, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 12, 15), null, "student.citizen036@example.com", "School Student Citizen 036", false, "36 Student Avenue, Singapore", "S0000036A", "+6590000036", "36 Student Avenue, Singapore", "Enrolled", null, null },
                    { 37, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 1, 15), null, "student.citizen037@example.com", "School Student Citizen 037", false, "37 Student Avenue, Singapore", "S0000037Z", "+6590000037", "37 Student Avenue, Singapore", "Enrolled", null, null },
                    { 38, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 2, 15), null, "student.citizen038@example.com", "School Student Citizen 038", false, "38 Student Avenue, Singapore", "S0000038H", "+6590000038", "38 Student Avenue, Singapore", "Enrolled", null, null },
                    { 39, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 3, 15), null, "student.citizen039@example.com", "School Student Citizen 039", false, "39 Student Avenue, Singapore", "S0000039F", "+6590000039", "39 Student Avenue, Singapore", "Enrolled", null, null },
                    { 40, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 4, 15), null, "student.citizen040@example.com", "School Student Citizen 040", false, "40 Student Avenue, Singapore", "S0000040Z", "+6590000040", "40 Student Avenue, Singapore", "Enrolled", null, null },
                    { 41, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 5, 15), null, "student.citizen041@example.com", "School Student Citizen 041", false, "41 Student Avenue, Singapore", "S0000041H", "+6590000041", "41 Student Avenue, Singapore", "Enrolled", null, null },
                    { 42, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 6, 15), null, "student.citizen042@example.com", "School Student Citizen 042", false, "42 Student Avenue, Singapore", "S0000042F", "+6590000042", "42 Student Avenue, Singapore", "Enrolled", null, null },
                    { 43, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 7, 15), null, "student.citizen043@example.com", "School Student Citizen 043", false, "43 Student Avenue, Singapore", "S0000043D", "+6590000043", "43 Student Avenue, Singapore", "Enrolled", null, null },
                    { 44, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 8, 15), null, "student.citizen044@example.com", "School Student Citizen 044", false, "44 Student Avenue, Singapore", "S0000044B", "+6590000044", "44 Student Avenue, Singapore", "Enrolled", null, null },
                    { 45, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 9, 15), null, "student.citizen045@example.com", "School Student Citizen 045", false, "45 Student Avenue, Singapore", "S0000045J", "+6590000045", "45 Student Avenue, Singapore", "Enrolled", null, null },
                    { 46, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 10, 15), null, "student.citizen046@example.com", "School Student Citizen 046", false, "46 Student Avenue, Singapore", "S0000046I", "+6590000046", "46 Student Avenue, Singapore", "Enrolled", null, null },
                    { 47, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 11, 15), null, "student.citizen047@example.com", "School Student Citizen 047", false, "47 Student Avenue, Singapore", "S0000047G", "+6590000047", "47 Student Avenue, Singapore", "Enrolled", null, null },
                    { 48, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 12, 15), null, "student.citizen048@example.com", "School Student Citizen 048", false, "48 Student Avenue, Singapore", "S0000048E", "+6590000048", "48 Student Avenue, Singapore", "Enrolled", null, null },
                    { 49, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 1, 15), null, "student.citizen049@example.com", "School Student Citizen 049", false, "49 Student Avenue, Singapore", "S0000049C", "+6590000049", "49 Student Avenue, Singapore", "Enrolled", null, null },
                    { 50, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 2, 15), null, "student.citizen050@example.com", "School Student Citizen 050", false, "50 Student Avenue, Singapore", "S0000050G", "+6590000050", "50 Student Avenue, Singapore", "Enrolled", null, null },
                    { 51, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 3, 15), null, "student.citizen051@example.com", "School Student Citizen 051", false, "51 Student Avenue, Singapore", "S0000051E", "+6590000051", "51 Student Avenue, Singapore", "Enrolled", null, null },
                    { 52, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 4, 15), null, "student.citizen052@example.com", "School Student Citizen 052", false, "52 Student Avenue, Singapore", "S0000052C", "+6590000052", "52 Student Avenue, Singapore", "Enrolled", null, null },
                    { 53, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 5, 15), null, "student.citizen053@example.com", "School Student Citizen 053", false, "53 Student Avenue, Singapore", "S0000053A", "+6590000053", "53 Student Avenue, Singapore", "Enrolled", null, null },
                    { 54, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 6, 15), null, "student.citizen054@example.com", "School Student Citizen 054", false, "54 Student Avenue, Singapore", "S0000054Z", "+6590000054", "54 Student Avenue, Singapore", "Enrolled", null, null },
                    { 55, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 7, 15), null, "student.citizen055@example.com", "School Student Citizen 055", false, "55 Student Avenue, Singapore", "S0000055H", "+6590000055", "55 Student Avenue, Singapore", "Enrolled", null, null },
                    { 56, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 8, 15), null, "student.citizen056@example.com", "School Student Citizen 056", false, "56 Student Avenue, Singapore", "S0000056F", "+6590000056", "56 Student Avenue, Singapore", "Enrolled", null, null },
                    { 57, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 9, 15), null, "student.citizen057@example.com", "School Student Citizen 057", false, "57 Student Avenue, Singapore", "S0000057D", "+6590000057", "57 Student Avenue, Singapore", "Enrolled", null, null },
                    { 58, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 10, 15), null, "student.citizen058@example.com", "School Student Citizen 058", false, "58 Student Avenue, Singapore", "S0000058B", "+6590000058", "58 Student Avenue, Singapore", "Enrolled", null, null },
                    { 59, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 11, 15), null, "student.citizen059@example.com", "School Student Citizen 059", false, "59 Student Avenue, Singapore", "S0000059J", "+6590000059", "59 Student Avenue, Singapore", "Enrolled", null, null },
                    { 60, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 12, 15), null, "student.citizen060@example.com", "School Student Citizen 060", false, "60 Student Avenue, Singapore", "S0000060D", "+6590000060", "60 Student Avenue, Singapore", "Enrolled", null, null },
                    { 61, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 1, 15), null, "student.citizen061@example.com", "School Student Citizen 061", false, "61 Student Avenue, Singapore", "S0000061B", "+6590000061", "61 Student Avenue, Singapore", "Enrolled", null, null },
                    { 62, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 2, 15), null, "student.citizen062@example.com", "School Student Citizen 062", false, "62 Student Avenue, Singapore", "S0000062J", "+6590000062", "62 Student Avenue, Singapore", "Enrolled", null, null },
                    { 63, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 3, 15), null, "student.citizen063@example.com", "School Student Citizen 063", false, "63 Student Avenue, Singapore", "S0000063I", "+6590000063", "63 Student Avenue, Singapore", "Enrolled", null, null },
                    { 64, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 4, 15), null, "student.citizen064@example.com", "School Student Citizen 064", false, "64 Student Avenue, Singapore", "S0000064G", "+6590000064", "64 Student Avenue, Singapore", "Enrolled", null, null },
                    { 65, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 5, 15), null, "student.citizen065@example.com", "School Student Citizen 065", false, "65 Student Avenue, Singapore", "S0000065E", "+6590000065", "65 Student Avenue, Singapore", "Enrolled", null, null },
                    { 66, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 6, 15), null, "student.citizen066@example.com", "School Student Citizen 066", false, "66 Student Avenue, Singapore", "S0000066C", "+6590000066", "66 Student Avenue, Singapore", "Enrolled", null, null },
                    { 67, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 7, 15), null, "student.citizen067@example.com", "School Student Citizen 067", false, "67 Student Avenue, Singapore", "S0000067A", "+6590000067", "67 Student Avenue, Singapore", "Enrolled", null, null },
                    { 68, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 8, 15), null, "student.citizen068@example.com", "School Student Citizen 068", false, "68 Student Avenue, Singapore", "S0000068Z", "+6590000068", "68 Student Avenue, Singapore", "Enrolled", null, null },
                    { 69, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 9, 15), null, "student.citizen069@example.com", "School Student Citizen 069", false, "69 Student Avenue, Singapore", "S0000069H", "+6590000069", "69 Student Avenue, Singapore", "Enrolled", null, null },
                    { 70, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 10, 15), null, "student.citizen070@example.com", "School Student Citizen 070", false, "70 Student Avenue, Singapore", "S0000070A", "+6590000070", "70 Student Avenue, Singapore", "Enrolled", null, null },
                    { 71, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 11, 15), null, "student.citizen071@example.com", "School Student Citizen 071", false, "71 Student Avenue, Singapore", "S0000071Z", "+6590000071", "71 Student Avenue, Singapore", "Enrolled", null, null },
                    { 72, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 12, 15), null, "student.citizen072@example.com", "School Student Citizen 072", false, "72 Student Avenue, Singapore", "S0000072H", "+6590000072", "72 Student Avenue, Singapore", "Enrolled", null, null },
                    { 73, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 1, 15), null, "student.citizen073@example.com", "School Student Citizen 073", false, "73 Student Avenue, Singapore", "S0000073F", "+6590000073", "73 Student Avenue, Singapore", "Enrolled", null, null },
                    { 74, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 2, 15), null, "student.citizen074@example.com", "School Student Citizen 074", false, "74 Student Avenue, Singapore", "S0000074D", "+6590000074", "74 Student Avenue, Singapore", "Enrolled", null, null },
                    { 75, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 3, 15), null, "student.citizen075@example.com", "School Student Citizen 075", false, "75 Student Avenue, Singapore", "S0000075B", "+6590000075", "75 Student Avenue, Singapore", "Enrolled", null, null },
                    { 76, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 4, 15), null, "student.citizen076@example.com", "School Student Citizen 076", false, "76 Student Avenue, Singapore", "S0000076J", "+6590000076", "76 Student Avenue, Singapore", "Enrolled", null, null },
                    { 77, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 5, 15), null, "student.citizen077@example.com", "School Student Citizen 077", false, "77 Student Avenue, Singapore", "S0000077I", "+6590000077", "77 Student Avenue, Singapore", "Enrolled", null, null },
                    { 78, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 6, 15), null, "student.citizen078@example.com", "School Student Citizen 078", false, "78 Student Avenue, Singapore", "S0000078G", "+6590000078", "78 Student Avenue, Singapore", "Enrolled", null, null },
                    { 79, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 7, 15), null, "student.citizen079@example.com", "School Student Citizen 079", false, "79 Student Avenue, Singapore", "S0000079E", "+6590000079", "79 Student Avenue, Singapore", "Enrolled", null, null },
                    { 80, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 8, 15), null, "student.citizen080@example.com", "School Student Citizen 080", false, "80 Student Avenue, Singapore", "S0000080I", "+6590000080", "80 Student Avenue, Singapore", "Enrolled", null, null },
                    { 81, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 9, 15), null, "student.citizen081@example.com", "School Student Citizen 081", false, "81 Student Avenue, Singapore", "S0000081G", "+6590000081", "81 Student Avenue, Singapore", "Enrolled", null, null },
                    { 82, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 10, 15), null, "student.citizen082@example.com", "School Student Citizen 082", false, "82 Student Avenue, Singapore", "S0000082E", "+6590000082", "82 Student Avenue, Singapore", "Enrolled", null, null },
                    { 83, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 11, 15), null, "student.citizen083@example.com", "School Student Citizen 083", false, "83 Student Avenue, Singapore", "S0000083C", "+6590000083", "83 Student Avenue, Singapore", "Enrolled", null, null },
                    { 84, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 12, 15), null, "student.citizen084@example.com", "School Student Citizen 084", false, "84 Student Avenue, Singapore", "S0000084A", "+6590000084", "84 Student Avenue, Singapore", "Enrolled", null, null },
                    { 85, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 1, 15), null, "student.citizen085@example.com", "School Student Citizen 085", false, "85 Student Avenue, Singapore", "S0000085Z", "+6590000085", "85 Student Avenue, Singapore", "Enrolled", null, null },
                    { 86, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 2, 15), null, "student.citizen086@example.com", "School Student Citizen 086", false, "86 Student Avenue, Singapore", "S0000086H", "+6590000086", "86 Student Avenue, Singapore", "Enrolled", null, null },
                    { 87, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 3, 15), null, "student.citizen087@example.com", "School Student Citizen 087", false, "87 Student Avenue, Singapore", "S0000087F", "+6590000087", "87 Student Avenue, Singapore", "Enrolled", null, null },
                    { 88, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 4, 15), null, "student.citizen088@example.com", "School Student Citizen 088", false, "88 Student Avenue, Singapore", "S0000088D", "+6590000088", "88 Student Avenue, Singapore", "Enrolled", null, null },
                    { 89, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 5, 15), null, "student.citizen089@example.com", "School Student Citizen 089", false, "89 Student Avenue, Singapore", "S0000089B", "+6590000089", "89 Student Avenue, Singapore", "Enrolled", null, null },
                    { 90, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 15), null, "student.citizen090@example.com", "School Student Citizen 090", false, "90 Student Avenue, Singapore", "S0000090F", "+6590000090", "90 Student Avenue, Singapore", "Enrolled", null, null },
                    { 91, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 15), null, "student.citizen091@example.com", "School Student Citizen 091", false, "91 Student Avenue, Singapore", "S0000091D", "+6590000091", "91 Student Avenue, Singapore", "Enrolled", null, null },
                    { 92, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 15), null, "student.citizen092@example.com", "School Student Citizen 092", false, "92 Student Avenue, Singapore", "S0000092B", "+6590000092", "92 Student Avenue, Singapore", "Enrolled", null, null },
                    { 93, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 15), null, "student.citizen093@example.com", "School Student Citizen 093", false, "93 Student Avenue, Singapore", "S0000093J", "+6590000093", "93 Student Avenue, Singapore", "Enrolled", null, null },
                    { 94, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 15), null, "student.citizen094@example.com", "School Student Citizen 094", false, "94 Student Avenue, Singapore", "S0000094I", "+6590000094", "94 Student Avenue, Singapore", "Enrolled", null, null },
                    { 95, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 11, 15), null, "student.citizen095@example.com", "School Student Citizen 095", false, "95 Student Avenue, Singapore", "S0000095G", "+6590000095", "95 Student Avenue, Singapore", "Enrolled", null, null },
                    { 96, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 12, 15), null, "student.citizen096@example.com", "School Student Citizen 096", false, "96 Student Avenue, Singapore", "S0000096E", "+6590000096", "96 Student Avenue, Singapore", "Enrolled", null, null },
                    { 97, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 1, 15), null, "student.citizen097@example.com", "School Student Citizen 097", false, "97 Student Avenue, Singapore", "S0000097C", "+6590000097", "97 Student Avenue, Singapore", "Enrolled", null, null },
                    { 98, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 2, 15), null, "student.citizen098@example.com", "School Student Citizen 098", false, "98 Student Avenue, Singapore", "S0000098A", "+6590000098", "98 Student Avenue, Singapore", "Enrolled", null, null },
                    { 99, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 3, 15), null, "student.citizen099@example.com", "School Student Citizen 099", false, "99 Student Avenue, Singapore", "S0000099Z", "+6590000099", "99 Student Avenue, Singapore", "Enrolled", null, null },
                    { 100, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 4, 15), null, "student.citizen100@example.com", "School Student Citizen 100", false, "100 Student Avenue, Singapore", "S0000100G", "+6590000100", "100 Student Avenue, Singapore", "Enrolled", null, null },
                    { 101, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 5, 15), null, "free.citizen101@example.com", "Free Citizen 101", false, "101 Free Avenue, Singapore", "S0000101E", "+6590000101", "101 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 102, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 6, 15), null, "free.citizen102@example.com", "Free Citizen 102", false, "102 Free Avenue, Singapore", "S0000102C", "+6590000102", "102 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 103, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 7, 15), null, "free.citizen103@example.com", "Free Citizen 103", false, "103 Free Avenue, Singapore", "S0000103A", "+6590000103", "103 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 104, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 8, 15), null, "free.citizen104@example.com", "Free Citizen 104", false, "104 Free Avenue, Singapore", "S0000104Z", "+6590000104", "104 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 105, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 9, 15), null, "free.citizen105@example.com", "Free Citizen 105", false, "105 Free Avenue, Singapore", "S0000105H", "+6590000105", "105 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 106, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 10, 15), null, "free.citizen106@example.com", "Free Citizen 106", false, "106 Free Avenue, Singapore", "S0000106F", "+6590000106", "106 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 107, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 11, 15), null, "free.citizen107@example.com", "Free Citizen 107", false, "107 Free Avenue, Singapore", "S0000107D", "+6590000107", "107 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 108, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 12, 15), null, "free.citizen108@example.com", "Free Citizen 108", false, "108 Free Avenue, Singapore", "S0000108B", "+6590000108", "108 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 109, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 1, 15), null, "free.citizen109@example.com", "Free Citizen 109", false, "109 Free Avenue, Singapore", "S0000109J", "+6590000109", "109 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 110, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 2, 15), null, "free.citizen110@example.com", "Free Citizen 110", false, "110 Free Avenue, Singapore", "S0000110D", "+6590000110", "110 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 111, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 3, 15), null, "free.citizen111@example.com", "Free Citizen 111", false, "111 Free Avenue, Singapore", "S0000111B", "+6590000111", "111 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 112, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 4, 15), null, "free.citizen112@example.com", "Free Citizen 112", false, "112 Free Avenue, Singapore", "S0000112J", "+6590000112", "112 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 113, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 5, 15), null, "free.citizen113@example.com", "Free Citizen 113", false, "113 Free Avenue, Singapore", "S0000113I", "+6590000113", "113 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 114, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 6, 15), null, "free.citizen114@example.com", "Free Citizen 114", false, "114 Free Avenue, Singapore", "S0000114G", "+6590000114", "114 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 115, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 7, 15), null, "free.citizen115@example.com", "Free Citizen 115", false, "115 Free Avenue, Singapore", "S0000115E", "+6590000115", "115 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 116, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 8, 15), null, "free.citizen116@example.com", "Free Citizen 116", false, "116 Free Avenue, Singapore", "S0000116C", "+6590000116", "116 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 117, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 9, 15), null, "free.citizen117@example.com", "Free Citizen 117", false, "117 Free Avenue, Singapore", "S0000117A", "+6590000117", "117 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 118, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 10, 15), null, "free.citizen118@example.com", "Free Citizen 118", false, "118 Free Avenue, Singapore", "S0000118Z", "+6590000118", "118 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 119, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 11, 15), null, "free.citizen119@example.com", "Free Citizen 119", false, "119 Free Avenue, Singapore", "S0000119H", "+6590000119", "119 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 120, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 12, 15), null, "free.citizen120@example.com", "Free Citizen 120", false, "120 Free Avenue, Singapore", "S0000120A", "+6590000120", "120 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 121, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 1, 15), null, "free.citizen121@example.com", "Free Citizen 121", false, "121 Free Avenue, Singapore", "S0000121Z", "+6590000121", "121 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 122, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 2, 15), null, "free.citizen122@example.com", "Free Citizen 122", false, "122 Free Avenue, Singapore", "S0000122H", "+6590000122", "122 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 123, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 3, 15), null, "free.citizen123@example.com", "Free Citizen 123", false, "123 Free Avenue, Singapore", "S0000123F", "+6590000123", "123 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 124, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 4, 15), null, "free.citizen124@example.com", "Free Citizen 124", false, "124 Free Avenue, Singapore", "S0000124D", "+6590000124", "124 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 125, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 5, 15), null, "free.citizen125@example.com", "Free Citizen 125", false, "125 Free Avenue, Singapore", "S0000125B", "+6590000125", "125 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 126, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 6, 15), null, "free.citizen126@example.com", "Free Citizen 126", false, "126 Free Avenue, Singapore", "S0000126J", "+6590000126", "126 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 127, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 7, 15), null, "free.citizen127@example.com", "Free Citizen 127", false, "127 Free Avenue, Singapore", "S0000127I", "+6590000127", "127 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 128, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 8, 15), null, "free.citizen128@example.com", "Free Citizen 128", false, "128 Free Avenue, Singapore", "S0000128G", "+6590000128", "128 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 129, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 9, 15), null, "free.citizen129@example.com", "Free Citizen 129", false, "129 Free Avenue, Singapore", "S0000129E", "+6590000129", "129 Free Avenue, Singapore", "Not Enrolled", null, null },
                    { 130, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 10, 15), null, "free.citizen130@example.com", "Free Citizen 130", false, "130 Free Avenue, Singapore", "S0000130I", "+6590000130", "130 Free Avenue, Singapore", "Not Enrolled", null, null }
                });

            migrationBuilder.InsertData(
                table: "EducationAccountSweepReports",
                columns: new[] { "Id", "AccountsClosedCount", "AccountsCreatedCount", "AccountsExtendedCount", "BatchDate", "CompletedAt", "StartedAt" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, new DateOnly(2026, 6, 19), new DateTime(2026, 1, 1, 1, 4, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 0, 1, 1, new DateOnly(2026, 6, 20), new DateTime(2026, 1, 2, 1, 3, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 2, 1, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "OutboxMessage",
                columns: new[] { "Id", "OccurredAt", "PayloadJson", "RetryCount", "Status", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 1, 0, 0, DateTimeKind.Utc), "{\"messageId\":1}", 1, 1, "AuditProjection" },
                    { 2, new DateTime(2026, 1, 1, 0, 2, 0, 0, DateTimeKind.Utc), "{\"messageId\":2}", 2, 3, "EmailNotification" },
                    { 3, new DateTime(2026, 1, 1, 0, 3, 0, 0, DateTimeKind.Utc), "{\"messageId\":3}", 0, 1, "AuditProjection" },
                    { 4, new DateTime(2026, 1, 1, 0, 4, 0, 0, DateTimeKind.Utc), "{\"messageId\":4}", 1, 3, "EmailNotification" },
                    { 5, new DateTime(2026, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc), "{\"messageId\":5}", 2, 4, "AuditProjection" },
                    { 6, new DateTime(2026, 1, 1, 0, 6, 0, 0, DateTimeKind.Utc), "{\"messageId\":6}", 0, 3, "EmailNotification" },
                    { 7, new DateTime(2026, 1, 1, 0, 7, 0, 0, DateTimeKind.Utc), "{\"messageId\":7}", 1, 1, "AuditProjection" },
                    { 8, new DateTime(2026, 1, 1, 0, 8, 0, 0, DateTimeKind.Utc), "{\"messageId\":8}", 2, 3, "EmailNotification" },
                    { 9, new DateTime(2026, 1, 1, 0, 9, 0, 0, DateTimeKind.Utc), "{\"messageId\":9}", 0, 1, "AuditProjection" },
                    { 10, new DateTime(2026, 1, 1, 0, 10, 0, 0, DateTimeKind.Utc), "{\"messageId\":10}", 1, 4, "EmailNotification" }
                });

            migrationBuilder.InsertData(
                table: "OutstandingDeductionRun",
                columns: new[] { "Id", "CompletedAt", "CreatedAt", "CreatedBy", "DeletedAt", "FailedCount", "FailureReason", "IsDeleted", "RunDate", "RunMonth", "StartedAt", "Status", "SuccessCount", "TotalDeductedAmount", "TotalScannedCharges", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 5, 0, 2, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, null, false, new DateOnly(2025, 9, 5), "2025-09", new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 0, 0m, 1, null, null },
                    { 2, new DateTime(2025, 10, 5, 0, 2, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, null, false, new DateOnly(2025, 10, 5), "2025-10", new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 0, 0m, 1, null, null },
                    { 3, new DateTime(2025, 11, 5, 0, 2, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, null, false, new DateOnly(2025, 11, 5), "2025-11", new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 0, 0m, 1, null, null },
                    { 4, new DateTime(2025, 12, 5, 0, 2, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, null, false, new DateOnly(2025, 12, 5), "2025-12", new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 0, 0m, 1, null, null },
                    { 5, new DateTime(2026, 1, 5, 0, 2, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, null, false, new DateOnly(2026, 1, 5), "2026-01", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 0, 0m, 1, null, null },
                    { 6, new DateTime(2026, 2, 5, 0, 2, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, null, false, new DateOnly(2026, 2, 5), "2026-02", new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 0, 0m, 1, null, null },
                    { 7, new DateTime(2026, 3, 5, 0, 2, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, null, false, new DateOnly(2026, 3, 5), "2026-03", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1, 50m, 1, null, null },
                    { 8, new DateTime(2026, 4, 5, 0, 2, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, null, false, new DateOnly(2026, 4, 5), "2026-04", new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1, 70m, 1, null, null },
                    { 9, null, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Database connection was interrupted.", false, new DateOnly(2026, 5, 5), "2026-05", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, 0, 0m, 1, null, null },
                    { 10, new DateTime(2026, 6, 5, 0, 2, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, null, false, new DateOnly(2026, 6, 5), "2026-06", new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 0, 0m, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditTransactionId", "ExternalReference", "IsDeleted", "PaidAt", "PaymentMethod", "Status", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 5, "EDU-2026-00000000001", "Citizen 001", "S0000001I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-ONLINE-0005", false, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 120m, null, null },
                    { 6, "EDU-2026-00000000006", "Citizen 006", "S0000006Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-ONLINE-0006", false, new DateTime(2026, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 100m, null, null },
                    { 7, "EDU-2026-00000000007", "Citizen 007", "S0000007H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-ONLINE-0007", false, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 200m, null, null },
                    { 8, "EDU-2026-00000000008", "Citizen 008", "S0000008F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-ONLINE-0008", false, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 130m, null, null }
                });

            migrationBuilder.InsertData(
                table: "ScheduleTopUp",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExecuteAtDay", "ExecuteAtMonth", "ExecutionTime", "Frequency", "IsDeleted", "Name", "NextExecutionAt", "OneTimeExecutionAt", "Status", "TopupAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, null, new TimeOnly(14, 29, 0), 2, false, "Scheduled Top-up 001", new DateTime(2027, 1, 1, 14, 29, 0, 0, DateTimeKind.Unspecified), null, 1, 490m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, null, new TimeOnly(19, 26, 0), 2, false, "Scheduled Top-up 002", new DateTime(2027, 1, 2, 19, 26, 0, 0, DateTimeKind.Unspecified), null, 1, 200m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(15, 39, 0), 1, false, "Scheduled Top-up 003", null, new DateTime(2027, 1, 3, 15, 39, 0, 0, DateTimeKind.Unspecified), 2, 130m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, null, new TimeOnly(4, 58, 0), 2, false, "Scheduled Top-up 004", new DateTime(2027, 1, 4, 4, 58, 0, 0, DateTimeKind.Unspecified), null, 1, 90m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, null, new TimeOnly(18, 30, 0), 2, false, "Scheduled Top-up 005", new DateTime(2027, 1, 5, 18, 30, 0, 0, DateTimeKind.Unspecified), null, 1, 420m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 7, new TimeOnly(5, 24, 0), 3, false, "Scheduled Top-up 006", null, null, 2, 350m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(23, 4, 0), 1, false, "Scheduled Top-up 007", new DateTime(2027, 1, 7, 23, 4, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 7, 23, 4, 0, 0, DateTimeKind.Unspecified), 1, 420m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 9, new TimeOnly(19, 4, 0), 3, false, "Scheduled Top-up 008", new DateTime(2027, 9, 8, 19, 4, 0, 0, DateTimeKind.Unspecified), null, 1, 450m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, null, new TimeOnly(9, 41, 0), 2, false, "Scheduled Top-up 009", new DateTime(2027, 1, 9, 9, 41, 0, 0, DateTimeKind.Unspecified), null, 1, 230m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(12, 26, 0), 1, false, "Scheduled Top-up 010", new DateTime(2027, 1, 10, 12, 26, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 10, 12, 26, 0, 0, DateTimeKind.Unspecified), 1, 480m, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, null, new TimeOnly(5, 48, 0), 2, false, "Scheduled Top-up 011", new DateTime(2027, 1, 11, 5, 48, 0, 0, DateTimeKind.Unspecified), null, 1, 70m, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, new TimeOnly(2, 38, 0), 2, false, "Scheduled Top-up 012", new DateTime(2027, 1, 12, 2, 38, 0, 0, DateTimeKind.Unspecified), null, 1, 260m, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, null, new TimeOnly(9, 43, 0), 2, false, "Scheduled Top-up 013", new DateTime(2027, 1, 13, 9, 43, 0, 0, DateTimeKind.Unspecified), null, 1, 190m, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(14, 48, 0), 1, false, "Scheduled Top-up 014", new DateTime(2027, 1, 14, 14, 48, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 14, 14, 48, 0, 0, DateTimeKind.Unspecified), 1, 440m, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, 4, new TimeOnly(2, 3, 0), 3, false, "Scheduled Top-up 015", null, null, 2, 290m, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(1, 3, 0), 1, false, "Scheduled Top-up 016", new DateTime(2027, 1, 16, 1, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 16, 1, 3, 0, 0, DateTimeKind.Unspecified), 1, 470m, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, null, new TimeOnly(18, 26, 0), 2, false, "Scheduled Top-up 017", new DateTime(2027, 1, 17, 18, 26, 0, 0, DateTimeKind.Unspecified), null, 1, 460m, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(3, 56, 0), 1, false, "Scheduled Top-up 018", new DateTime(2027, 1, 18, 3, 56, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 18, 3, 56, 0, 0, DateTimeKind.Unspecified), 1, 480m, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, 8, new TimeOnly(3, 33, 0), 3, false, "Scheduled Top-up 019", new DateTime(2027, 8, 19, 3, 33, 0, 0, DateTimeKind.Unspecified), null, 1, 210m, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(15, 19, 0), 1, false, "Scheduled Top-up 020", new DateTime(2027, 1, 20, 15, 19, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 20, 15, 19, 0, 0, DateTimeKind.Unspecified), 1, 270m, null, null }
                });

            migrationBuilder.InsertData(
                table: "School",
                columns: new[] { "Id", "Address", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "IsDeleted", "PhoneNumber", "SchoolName", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "10 Northview Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@northview.edu.sg", false, "+6561000001", "Northview Secondary School", 1, null, null },
                    { 2, "20 Eastbridge Avenue, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@eastbridge.edu.sg", false, "+6561000002", "Eastbridge Secondary School", 1, null, null },
                    { 3, "30 Westhaven Street, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@westhaven.edu.sg", false, "+6561000003", "Westhaven Secondary School", 1, null, null },
                    { 4, "40 Southpoint Drive, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@southpoint.edu.sg", false, "+6561000004", "Southpoint Secondary School", 1, null, null },
                    { 5, "50 Central Heights, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@centralheights.edu.sg", false, "+6561000005", "Central Heights School", 1, null, null },
                    { 6, "60 Riverside Walk, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@riverside.edu.sg", false, "+6561000006", "Riverside Learning Institute", 1, null, null },
                    { 7, "70 Lakeside Crescent, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@lakeside.edu.sg", false, "+6561000007", "Lakeside Technical School", 1, null, null },
                    { 8, "80 Greenfield Lane, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@greenfield.edu.sg", false, "+6561000008", "Greenfield Academy", 1, null, null },
                    { 9, "90 Harbourfront Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@harbourfront.edu.sg", false, "+6561000009", "Harbourfront School", 1, null, null },
                    { 10, "100 Hillcrest Avenue, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "contact@hillcrest.edu.sg", false, "+6561000010", "Hillcrest Education Centre", 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "SystemTopup",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Name", "Status", "TopupAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 021", 2, 70m, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 022", 1, 770m, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 023", 1, 790m, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 024", 1, 170m, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 025", 1, 260m, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 026", 1, 230m, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 027", 2, 220m, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 028", 1, 140m, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 029", 1, 200m, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 030", 1, 860m, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 031", 1, 110m, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 032", 1, 360m, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 033", 1, 580m, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 034", 1, 730m, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 035", 1, 400m, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 036", 1, 680m, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 037", 1, 520m, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 038", 1, 260m, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 039", 1, 10m, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 040", 1, 450m, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 041", 1, 710m, null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 042", 1, 270m, null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 043", 1, 630m, null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 044", 1, 370m, null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 045", 1, 180m, null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 046", 1, 640m, null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 047", 1, 440m, null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 048", 1, 30m, null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 049", 2, 710m, null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "System Top-up 050", 1, 930m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupExecution",
                columns: new[] { "Id", "ConditionsSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutionCode", "FailedCount", "IdempotencyKey", "IsDeleted", "ManualAmount", "ManualReason", "ScheduleTopUpId", "SourceType", "Status", "SuccessCount", "SystemTopupId", "TopupAmountSnapshot", "TopupNameSnapshot", "TotalExecutedAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, null, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, "TOPUP-SEED-MANUAL-001", 1, "seed-manual-topup-001", false, 100m, "Seeded manual top-up execution.", null, 3, 3, 1, null, null, null, 100m, 2, null, null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CitizenId", "CreatedAt", "CreatedBy", "DeletedAt", "FailedLoginCount", "IsDeleted", "LastLoginAt", "LockedUntil", "Role", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 1, 1, null, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, null, null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 1, 1, null, null },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, null, null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, new DateTime(2026, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, 2, null, null },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, null, null },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 2, 1, null, null },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "AdminProfile",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "FullName", "IsDeleted", "Nric", "PhoneNumber", "SchoolId", "StaffCode", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin001@example.com", "System Administrator 001", false, "S0000001I", "+6591000001", null, "STAFF-K7M2Q", null, null, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin002@example.com", "System Administrator 002", false, "S0000002G", "+6591000002", null, "STAFF-R4T9X", null, null, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin003@example.com", "System Administrator 003", false, "S0000003E", "+6591000003", null, "STAFF-C8N5W", null, null, 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin004@example.com", "System Administrator 004", false, "S0000004C", "+6591000004", null, "STAFF-P3H7V", null, null, 4 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin005@example.com", "System Administrator 005", false, "S0000005A", "+6591000005", null, "STAFF-Y6D2K", null, null, 5 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin006@example.com", "System Administrator 006", false, "S0000006Z", "+6591000006", null, "STAFF-M9Q4A", null, null, 6 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin007@example.com", "Finance Administrator", false, "S0000007H", "+6591000007", null, "STAFF-T5X8C", null, null, 7 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin008@example.com", "School Administrator", false, "S0000008F", "+6591000008", 1, "STAFF-H2W6R", null, null, 8 }
                });

            migrationBuilder.InsertData(
                table: "AuditLog",
                columns: new[] { "Id", "Action", "ActorUserId", "Category", "IpAddress", "Nric", "OccurredAt" },
                values: new object[,]
                {
                    { 1, "AdminLoginSucceeded", 1, 4, "127.0.0.1", null, new DateTime(2026, 1, 1, 0, 1, 0, 0, DateTimeKind.Utc) },
                    { 2, "CreditTransactionCreated", 2, 5, "127.0.0.2", null, new DateTime(2026, 1, 1, 0, 2, 0, 0, DateTimeKind.Utc) },
                    { 3, "AdminLoginSucceeded", 3, 3, "127.0.0.3", null, new DateTime(2026, 1, 1, 0, 3, 0, 0, DateTimeKind.Utc) },
                    { 4, "CreditTransactionCreated", 4, 5, "127.0.0.4", null, new DateTime(2026, 1, 1, 0, 4, 0, 0, DateTimeKind.Utc) },
                    { 5, "AdminLoginSucceeded", 5, 4, "127.0.0.5", null, new DateTime(2026, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc) },
                    { 6, "CreditTransactionCreated", 6, 3, "127.0.0.6", null, new DateTime(2026, 1, 1, 0, 6, 0, 0, DateTimeKind.Utc) },
                    { 7, "AdminLoginSucceeded", 7, 4, "127.0.0.7", null, new DateTime(2026, 1, 1, 0, 7, 0, 0, DateTimeKind.Utc) },
                    { 8, "CreditTransactionCreated", 8, 5, "127.0.0.8", null, new DateTime(2026, 1, 1, 0, 8, 0, 0, DateTimeKind.Utc) },
                    { 11, "Auto Provisioning Sweep Completed", 1, 1, "127.0.0.1", null, new DateTime(2026, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "CourseCode", "CourseFeeAmount", "CourseName", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "EndDate", "FasApplicationDueDate", "GstAmount", "IsDeleted", "MiscFeeAmount", "SchoolId", "StartDate", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "CRS-2026-A1B2C3D", 100m, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation course in applied mathematics.", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9.90m, false, 10m, 1, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 2, "CRS-2026-B2C3D4E", 115m, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Introduction to programming and computing.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 11.43m, false, 12m, 2, new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 3, "CRS-2026-C3D4E5F", 130m, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Professional written and verbal communication.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 13.05m, false, 15m, 3, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 4, "CRS-2026-D4E5F6G", 145m, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Environmental systems and sustainability.", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 14.58m, false, 17m, 4, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 5, "CRS-2026-E5F6G7H", 160m, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Digital design principles and production.", new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 16.20m, false, 20m, 5, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 6, "CRS-2026-F6G7H8J", 175m, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Core hospitality service operations.", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 17.73m, false, 22m, 6, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 7, "CRS-2026-G7H8J9K", 190m, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Fundamentals of electrical systems.", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 19.35m, false, 25m, 7, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 8, "CRS-2026-H8J9K0L", 205m, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Writing techniques across common genres.", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 20.88m, false, 27m, 8, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 9, "CRS-2026-J9K0L1M", 220m, "Data Analytics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Data preparation, analysis and reporting.", new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 22.50m, false, 30m, 9, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 10, "CRS-2026-K0L1M2N", 235m, "Legacy Office Applications", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Archived office applications programme.", new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 24.03m, false, 32m, 10, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null }
                });

            migrationBuilder.InsertData(
                table: "EducationAccount",
                columns: new[] { "Id", "AccountNumber", "CitizenId", "ClosedAt", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditBalance", "IsDeleted", "OpenedAt", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-00000000001", 1, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0m, false, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 2, "EDU-2026-00000000002", 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1130m, false, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 3, "EDU-2026-00000000003", 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1160m, false, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 4, "EDU-2026-00000000004", 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1220m, false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 5, "EDU-2026-00000000005", 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1320m, false, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 6, "EDU-2026-00000000006", 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 450m, false, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 7, "EDU-2026-00000000007", 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1700m, false, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 8, "EDU-2026-00000000008", 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0m, false, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 9, "EDU-2026-00000000009", 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1900m, false, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 10, "EDU-2026-00000000010", 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2000m, false, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 11, "EDU-2026-00000000011", 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 12, "EDU-2026-00000000012", 12, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 13, "EDU-2026-00000000013", 13, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 14, "EDU-2026-00000000014", 14, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 15, "EDU-2026-00000000015", 15, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 16, "EDU-2026-00000000016", 16, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 17, "EDU-2026-00000000017", 17, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 18, "EDU-2026-00000000018", 18, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 19, "EDU-2026-00000000019", 19, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 20, "EDU-2026-00000000020", 20, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 21, "EDU-2026-00000000021", 21, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 22, "EDU-2026-00000000022", 22, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 23, "EDU-2026-00000000023", 23, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 24, "EDU-2026-00000000024", 24, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 25, "EDU-2026-00000000025", 25, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 26, "EDU-2026-00000000026", 26, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 27, "EDU-2026-00000000027", 27, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 28, "EDU-2026-00000000028", 28, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 29, "EDU-2026-00000000029", 29, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 30, "EDU-2026-00000000030", 30, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 31, "EDU-2026-00000000031", 31, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 32, "EDU-2026-00000000032", 32, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 33, "EDU-2026-00000000033", 33, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 34, "EDU-2026-00000000034", 34, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 35, "EDU-2026-00000000035", 35, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 36, "EDU-2026-00000000036", 36, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 37, "EDU-2026-00000000037", 37, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 38, "EDU-2026-00000000038", 38, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 39, "EDU-2026-00000000039", 39, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 40, "EDU-2026-00000000040", 40, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 41, "EDU-2026-00000000041", 41, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 42, "EDU-2026-00000000042", 42, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 43, "EDU-2026-00000000043", 43, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 44, "EDU-2026-00000000044", 44, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 45, "EDU-2026-00000000045", 45, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 46, "EDU-2026-00000000046", 46, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 47, "EDU-2026-00000000047", 47, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 48, "EDU-2026-00000000048", 48, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 49, "EDU-2026-00000000049", 49, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 50, "EDU-2026-00000000050", 50, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 51, "EDU-2026-00000000051", 51, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 52, "EDU-2026-00000000052", 52, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 53, "EDU-2026-00000000053", 53, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 54, "EDU-2026-00000000054", 54, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 55, "EDU-2026-00000000055", 55, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 56, "EDU-2026-00000000056", 56, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 57, "EDU-2026-00000000057", 57, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 58, "EDU-2026-00000000058", 58, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 59, "EDU-2026-00000000059", 59, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 60, "EDU-2026-00000000060", 60, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 61, "EDU-2026-00000000061", 61, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 62, "EDU-2026-00000000062", 62, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 63, "EDU-2026-00000000063", 63, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 64, "EDU-2026-00000000064", 64, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 65, "EDU-2026-00000000065", 65, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 66, "EDU-2026-00000000066", 66, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 67, "EDU-2026-00000000067", 67, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 68, "EDU-2026-00000000068", 68, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 69, "EDU-2026-00000000069", 69, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 70, "EDU-2026-00000000070", 70, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 71, "EDU-2026-00000000071", 71, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 72, "EDU-2026-00000000072", 72, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 73, "EDU-2026-00000000073", 73, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 74, "EDU-2026-00000000074", 74, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 75, "EDU-2026-00000000075", 75, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 76, "EDU-2026-00000000076", 76, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 77, "EDU-2026-00000000077", 77, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 78, "EDU-2026-00000000078", 78, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 79, "EDU-2026-00000000079", 79, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 80, "EDU-2026-00000000080", 80, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 81, "EDU-2026-00000000081", 81, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 82, "EDU-2026-00000000082", 82, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 83, "EDU-2026-00000000083", 83, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 84, "EDU-2026-00000000084", 84, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 85, "EDU-2026-00000000085", 85, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 86, "EDU-2026-00000000086", 86, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 87, "EDU-2026-00000000087", 87, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 88, "EDU-2026-00000000088", 88, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 89, "EDU-2026-00000000089", 89, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 90, "EDU-2026-00000000090", 90, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 91, "EDU-2026-00000000091", 91, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 92, "EDU-2026-00000000092", 92, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 93, "EDU-2026-00000000093", 93, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 94, "EDU-2026-00000000094", 94, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 95, "EDU-2026-00000000095", 95, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 96, "EDU-2026-00000000096", 96, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 97, "EDU-2026-00000000097", 97, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 98, "EDU-2026-00000000098", 98, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 99, "EDU-2026-00000000099", 99, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 100, "EDU-2026-00000000100", 100, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 101, "EDU-2026-00000000101", 101, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 102, "EDU-2026-00000000102", 102, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 103, "EDU-2026-00000000103", 103, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 104, "EDU-2026-00000000104", 104, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 105, "EDU-2026-00000000105", 105, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 106, "EDU-2026-00000000106", 106, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 107, "EDU-2026-00000000107", 107, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 108, "EDU-2026-00000000108", 108, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 109, "EDU-2026-00000000109", 109, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 110, "EDU-2026-00000000110", 110, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 111, "EDU-2026-00000000111", 111, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 112, "EDU-2026-00000000112", 112, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 113, "EDU-2026-00000000113", 113, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 114, "EDU-2026-00000000114", 114, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 115, "EDU-2026-00000000115", 115, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 116, "EDU-2026-00000000116", 116, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 117, "EDU-2026-00000000117", 117, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 118, "EDU-2026-00000000118", 118, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 119, "EDU-2026-00000000119", 119, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 120, "EDU-2026-00000000120", 120, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "EducationAccountSweepTargets",
                columns: new[] { "Id", "Action", "Nric", "Reason", "Status", "SweepReportId" },
                values: new object[,]
                {
                    { 1, 0, "S0000001I", "Eligible citizen account created.", 1, 1 },
                    { 2, 1, "S0000002G", "Account reached the closing date.", 1, 1 },
                    { 3, 2, "S0000003E", "Active enrollment requires an extension.", 1, 1 },
                    { 4, 0, "S0000004C", "Manual identity verification is required.", 2, 1 },
                    { 5, 0, "S0000005A", "Eligible citizen account created.", 1, 2 },
                    { 6, 2, "S0000006Z", "Active enrollment requires an extension.", 1, 2 },
                    { 7, 1, "S0000007H", "Outstanding reconciliation requires manual handling.", 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "RefreshToken",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExpiresAt", "IsDeleted", "RevokedAt", "TokenHash", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-001", null, null, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-002", null, null, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-003", null, null, 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "refresh-token-hash-004", null, null, 4 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-005", null, null, 5 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-006", null, null, 6 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-007", null, null, 7 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), false, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), "refresh-token-hash-008", null, null, 8 }
                });

            migrationBuilder.InsertData(
                table: "ScheduleTopUpConditionGroup",
                columns: new[] { "Id", "DisplayOrder", "LogicalOperator", "ParentGroupId", "ScheduleTopUpId" },
                values: new object[,]
                {
                    { 1, 0, 1, null, 1 },
                    { 2, 0, 1, null, 2 },
                    { 3, 0, 1, null, 3 },
                    { 4, 0, 1, null, 4 },
                    { 5, 0, 1, null, 5 },
                    { 6, 0, 1, null, 6 },
                    { 7, 0, 1, null, 7 },
                    { 8, 0, 1, null, 8 },
                    { 9, 0, 1, null, 9 },
                    { 10, 0, 1, null, 10 },
                    { 11, 0, 1, null, 11 },
                    { 12, 0, 1, null, 12 },
                    { 13, 0, 1, null, 13 },
                    { 14, 0, 1, null, 14 },
                    { 15, 0, 1, null, 15 },
                    { 16, 0, 1, null, 16 },
                    { 17, 0, 1, null, 17 },
                    { 18, 0, 1, null, 18 },
                    { 19, 0, 1, null, 19 },
                    { 20, 0, 1, null, 20 }
                });

            migrationBuilder.InsertData(
                table: "SsoIdentity",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Provider", "ProviderUserId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "5fc549a1-ee08-4273-9497-27607842e1f9", null, null, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "f8cdef31-a31e-4b4a-93e4-5f571e91255a", null, null, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "b1e7cdf2-43ef-4a3e-9eb8-4e63b3ae42f4", null, null, 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "f116e09e-1a6f-4847-aa57-442705d242d0", null, null, 4 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "db3c66a7-b7f4-47df-bd7a-3f70fcaaa73d", null, null, 5 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "0446ecca-6483-4129-bd4f-906f970f18d5", null, null, 6 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "1fedf576-1c66-4742-881b-f7a456b2b027", null, null, 7 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "78ce9568-1d38-44aa-a9c5-ea50293934de", null, null, 8 }
                });

            migrationBuilder.InsertData(
                table: "SystemTopupConditionGroup",
                columns: new[] { "Id", "DisplayOrder", "LogicalOperator", "ParentGroupId", "SystemTopupId" },
                values: new object[,]
                {
                    { 1, 0, 1, null, 21 },
                    { 2, 0, 1, null, 22 },
                    { 3, 0, 1, null, 23 },
                    { 4, 0, 1, null, 24 },
                    { 5, 0, 1, null, 25 },
                    { 6, 0, 1, null, 26 },
                    { 7, 0, 1, null, 27 },
                    { 8, 0, 1, null, 28 },
                    { 9, 0, 1, null, 29 },
                    { 10, 0, 1, null, 30 },
                    { 11, 0, 1, null, 31 },
                    { 12, 0, 1, null, 32 },
                    { 13, 0, 1, null, 33 },
                    { 14, 0, 1, null, 34 },
                    { 15, 0, 1, null, 35 },
                    { 16, 0, 1, null, 36 },
                    { 17, 0, 1, null, 37 },
                    { 18, 0, 1, null, 38 },
                    { 19, 0, 1, null, 39 },
                    { 20, 0, 1, null, 40 },
                    { 21, 0, 1, null, 41 },
                    { 22, 0, 1, null, 42 },
                    { 23, 0, 1, null, 43 },
                    { 24, 0, 1, null, 44 },
                    { 25, 0, 1, null, 45 },
                    { 26, 0, 1, null, 46 },
                    { 27, 0, 1, null, 47 },
                    { 28, 0, 1, null, 48 },
                    { 29, 0, 1, null, 49 },
                    { 30, 0, 1, null, 50 }
                });

            migrationBuilder.InsertData(
                table: "TopupExecution",
                columns: new[] { "Id", "ConditionsSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutionCode", "FailedCount", "IdempotencyKey", "IsDeleted", "ManualAmount", "ManualReason", "ScheduleTopUpId", "SourceType", "Status", "SuccessCount", "SystemTopupId", "TopupAmountSnapshot", "TopupNameSnapshot", "TotalExecutedAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 2, "{\"logicalOperator\":\"And\",\"conditions\":[]}", new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "TOPUP-SEED-SYSTEM-001", 0, "seed-system-topup-001", false, null, null, null, 1, 3, 1, 21, 200m, "System Top-up 021", 200m, 1, null, null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CitizenId", "CreatedAt", "CreatedBy", "DeletedAt", "FailedLoginCount", "IsDeleted", "LastLoginAt", "LockedUntil", "Role", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 9, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 4, 1, null, null },
                    { 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 2, null, null },
                    { 11, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 4, 1, null, null },
                    { 12, 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 13, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 4, 1, null, null },
                    { 14, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 15, 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 4, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "UserStatusHistory",
                columns: new[] { "Id", "ChangedAt", "ChangedByUserId", "NewStatus", "PreviousStatus", "Reason", "UserId" },
                values: new object[] { 1, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 1, "Seeded administrative suspension.", 5 });

            migrationBuilder.InsertData(
                table: "AuditLog",
                columns: new[] { "Id", "Action", "ActorUserId", "Category", "IpAddress", "Nric", "OccurredAt" },
                values: new object[,]
                {
                    { 9, "AdminLoginSucceeded", 9, 3, "127.0.0.9", null, new DateTime(2026, 1, 1, 0, 9, 0, 0, DateTimeKind.Utc) },
                    { 10, "CreditTransactionCreated", 10, 5, "127.0.0.10", null, new DateTime(2026, 1, 1, 0, 10, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "EducationAccountStatusHistory",
                columns: new[] { "Id", "ChangedAt", "ChangedByUserId", "EducationAccountId", "NewStatus", "PreviousStatus", "Reason" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, 3, 1, "Education account closed by the scheduled sweep." },
                    { 2, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, 6, 2, 1, "Education account extended because outstanding charges remain." }
                });

            migrationBuilder.InsertData(
                table: "EducationCreditTransaction",
                columns: new[] { "Id", "Amount", "BalanceAfter", "BalanceBefore", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "Direction", "EducationAccountId", "IsDeleted", "TransactionCode", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 100m, 100m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Scheduled top-up transaction 001.", 1, 1, false, new Guid("00000000-0000-0000-0000-000000000001"), 1, null, null },
                    { 2, 100m, 1200m, 1100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Scheduled top-up transaction 002.", 1, 2, false, new Guid("00000000-0000-0000-0000-000000000002"), 1, null, null },
                    { 3, 200m, 1300m, 1100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Scheduled top-up transaction 003.", 1, 3, false, new Guid("00000000-0000-0000-0000-000000000003"), 1, null, null },
                    { 4, 30m, 0m, 30m, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Education account balance expired at age 31.", 2, 1, false, new Guid("00000000-0000-0000-0000-000000000004"), 4, null, null },
                    { 5, 70m, 1130m, 1200m, new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment for Computer Science Fundamentals.", 2, 2, false, new Guid("00000000-0000-0000-0000-000000000005"), 2, null, null },
                    { 6, 140m, 1160m, 1300m, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment for Business Communication.", 2, 3, false, new Guid("00000000-0000-0000-0000-000000000006"), 2, null, null },
                    { 7, 50m, 450m, 500m, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "March outstanding charge auto deduction.", 2, 6, false, new Guid("00000000-0000-0000-0000-000000000007"), 3, null, null },
                    { 8, 70m, 0m, 70m, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "April outstanding charge auto deduction.", 2, 8, false, new Guid("00000000-0000-0000-0000-000000000008"), 3, null, null },
                    { 9, 180m, 1220m, 1400m, new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment for Environmental Science.", 2, 4, false, new Guid("00000000-0000-0000-0000-000000000009"), 2, null, null },
                    { 10, 180m, 1320m, 1500m, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment for Digital Media Design.", 2, 5, false, new Guid("00000000-0000-0000-0000-000000000010"), 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "RefreshToken",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExpiresAt", "IsDeleted", "RevokedAt", "TokenHash", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-009", null, null, 9 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-010", null, null, 10 }
                });

            migrationBuilder.InsertData(
                table: "ScheduleTopUpCondition",
                columns: new[] { "Id", "DisplayOrder", "Field", "GroupId", "Operator", "ValueNumber", "ValueNumberTo", "ValueText" },
                values: new object[,]
                {
                    { 1, 0, 2, 1, 6, 1000m, null, null },
                    { 2, 0, 2, 2, 6, 1000m, null, null },
                    { 3, 0, 2, 3, 6, 1000m, null, null },
                    { 4, 0, 2, 4, 6, 1000m, null, null },
                    { 5, 0, 2, 5, 6, 1000m, null, null },
                    { 6, 0, 2, 6, 6, 1000m, null, null },
                    { 7, 0, 2, 7, 6, 1000m, null, null },
                    { 8, 0, 2, 8, 6, 1000m, null, null },
                    { 9, 0, 2, 9, 6, 1000m, null, null },
                    { 10, 0, 2, 10, 6, 1000m, null, null },
                    { 11, 0, 2, 11, 6, 1000m, null, null },
                    { 12, 0, 2, 12, 6, 1000m, null, null },
                    { 13, 0, 2, 13, 6, 1000m, null, null },
                    { 14, 0, 2, 14, 6, 1000m, null, null },
                    { 15, 0, 2, 15, 6, 1000m, null, null },
                    { 16, 0, 2, 16, 6, 1000m, null, null },
                    { 17, 0, 2, 17, 6, 1000m, null, null },
                    { 18, 0, 2, 18, 6, 1000m, null, null },
                    { 19, 0, 2, 19, 6, 1000m, null, null },
                    { 20, 0, 2, 20, 6, 1000m, null, null }
                });

            migrationBuilder.InsertData(
                table: "SchoolStudent",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "IsDeleted", "SchoolId", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 1, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 2, 1, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, 3, 1, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, 4, 1, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, 5, 1, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, 6, 1, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, 7, 1, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, 8, 1, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, 9, 1, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, 10, 2, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, false, 1, 1, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, false, 1, 1, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, false, 1, 1, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, false, 1, 1, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, false, 1, 1, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, false, 1, 1, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, false, 1, 1, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, false, 1, 1, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, false, 1, 1, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, false, 2, 1, null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, false, 2, 1, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, false, 2, 1, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 23, false, 2, 1, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 24, false, 2, 1, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 25, false, 2, 1, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 26, false, 2, 1, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, false, 2, 1, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 28, false, 2, 1, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 29, false, 3, 1, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 30, false, 3, 1, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 31, false, 3, 1, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 32, false, 3, 1, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 33, false, 3, 1, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 34, false, 3, 1, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 35, false, 3, 1, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 36, false, 3, 1, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 37, false, 3, 1, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 38, false, 4, 1, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 39, false, 4, 1, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 40, false, 4, 1, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 41, false, 4, 1, null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 42, false, 4, 1, null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 43, false, 4, 1, null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 44, false, 4, 1, null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 45, false, 4, 1, null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 46, false, 4, 1, null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 47, false, 5, 1, null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 48, false, 5, 1, null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 49, false, 5, 1, null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 50, false, 5, 1, null, null },
                    { 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 51, false, 5, 1, null, null },
                    { 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 52, false, 5, 1, null, null },
                    { 53, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 53, false, 5, 1, null, null },
                    { 54, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 54, false, 5, 1, null, null },
                    { 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 55, false, 5, 1, null, null },
                    { 56, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 56, false, 6, 1, null, null },
                    { 57, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 57, false, 6, 1, null, null },
                    { 58, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 58, false, 6, 1, null, null },
                    { 59, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 59, false, 6, 1, null, null },
                    { 60, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 60, false, 6, 1, null, null },
                    { 61, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 61, false, 6, 1, null, null },
                    { 62, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 62, false, 6, 1, null, null },
                    { 63, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 63, false, 6, 1, null, null },
                    { 64, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 64, false, 6, 1, null, null },
                    { 65, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 65, false, 7, 1, null, null },
                    { 66, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 66, false, 7, 1, null, null },
                    { 67, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 67, false, 7, 1, null, null },
                    { 68, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 68, false, 7, 1, null, null },
                    { 69, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 69, false, 7, 1, null, null },
                    { 70, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 70, false, 7, 1, null, null },
                    { 71, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 71, false, 7, 1, null, null },
                    { 72, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 72, false, 7, 1, null, null },
                    { 73, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 73, false, 7, 1, null, null },
                    { 74, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 74, false, 8, 1, null, null },
                    { 75, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 75, false, 8, 1, null, null },
                    { 76, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 76, false, 8, 1, null, null },
                    { 77, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 77, false, 8, 1, null, null },
                    { 78, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 78, false, 8, 1, null, null },
                    { 79, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 79, false, 8, 1, null, null },
                    { 80, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 80, false, 8, 1, null, null },
                    { 81, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 81, false, 8, 1, null, null },
                    { 82, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 82, false, 8, 1, null, null },
                    { 83, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 83, false, 9, 1, null, null },
                    { 84, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 84, false, 9, 1, null, null },
                    { 85, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 85, false, 9, 1, null, null },
                    { 86, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 86, false, 9, 1, null, null },
                    { 87, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 87, false, 9, 1, null, null },
                    { 88, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 88, false, 9, 1, null, null },
                    { 89, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 89, false, 9, 1, null, null },
                    { 90, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 90, false, 9, 1, null, null },
                    { 91, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 91, false, 9, 1, null, null },
                    { 92, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 92, false, 10, 1, null, null },
                    { 93, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 93, false, 10, 1, null, null },
                    { 94, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 94, false, 10, 1, null, null },
                    { 95, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 95, false, 10, 1, null, null },
                    { 96, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 96, false, 10, 1, null, null },
                    { 97, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 97, false, 10, 1, null, null },
                    { 98, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 98, false, 10, 1, null, null },
                    { 99, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 99, false, 10, 1, null, null },
                    { 100, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 100, false, 10, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "SsoIdentity",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Provider", "ProviderUserId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-009", null, null, 9 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-010", null, null, 10 },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-011", null, null, 11 },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-012", null, null, 12 },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-013", null, null, 13 },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-014", null, null, 14 },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-015", null, null, 15 }
                });

            migrationBuilder.InsertData(
                table: "SystemTopupCondition",
                columns: new[] { "Id", "DisplayOrder", "Field", "GroupId", "Operator", "ValueNumber", "ValueNumberTo", "ValueText" },
                values: new object[,]
                {
                    { 1, 0, 3, 1, 1, null, null, "Enrolled" },
                    { 2, 1, 1, 1, 7, 16m, 25m, null },
                    { 3, 0, 3, 2, 1, null, null, "Enrolled" },
                    { 4, 1, 1, 2, 7, 16m, 25m, null },
                    { 5, 0, 3, 3, 1, null, null, "Enrolled" },
                    { 6, 1, 1, 3, 7, 16m, 25m, null },
                    { 7, 0, 3, 4, 1, null, null, "Enrolled" },
                    { 8, 1, 1, 4, 7, 16m, 25m, null },
                    { 9, 0, 3, 5, 1, null, null, "Enrolled" },
                    { 10, 1, 1, 5, 7, 16m, 25m, null },
                    { 11, 0, 3, 6, 1, null, null, "Enrolled" },
                    { 12, 1, 1, 6, 7, 16m, 25m, null },
                    { 13, 0, 3, 7, 1, null, null, "Enrolled" },
                    { 14, 1, 1, 7, 7, 16m, 25m, null },
                    { 15, 0, 3, 8, 1, null, null, "Enrolled" },
                    { 16, 1, 1, 8, 7, 16m, 25m, null },
                    { 17, 0, 3, 9, 1, null, null, "Enrolled" },
                    { 18, 1, 1, 9, 7, 16m, 25m, null },
                    { 19, 0, 3, 10, 1, null, null, "Enrolled" },
                    { 20, 1, 1, 10, 7, 16m, 25m, null },
                    { 21, 0, 3, 11, 1, null, null, "Enrolled" },
                    { 22, 1, 1, 11, 7, 16m, 25m, null },
                    { 23, 0, 3, 12, 1, null, null, "Enrolled" },
                    { 24, 1, 1, 12, 7, 16m, 25m, null },
                    { 25, 0, 3, 13, 1, null, null, "Enrolled" },
                    { 26, 1, 1, 13, 7, 16m, 25m, null },
                    { 27, 0, 3, 14, 1, null, null, "Enrolled" },
                    { 28, 1, 1, 14, 7, 16m, 25m, null },
                    { 29, 0, 3, 15, 1, null, null, "Enrolled" },
                    { 30, 1, 1, 15, 7, 16m, 25m, null },
                    { 31, 0, 3, 16, 1, null, null, "Enrolled" },
                    { 32, 1, 1, 16, 7, 16m, 25m, null },
                    { 33, 0, 3, 17, 1, null, null, "Enrolled" },
                    { 34, 1, 1, 17, 7, 16m, 25m, null },
                    { 35, 0, 3, 18, 1, null, null, "Enrolled" },
                    { 36, 1, 1, 18, 7, 16m, 25m, null },
                    { 37, 0, 3, 19, 1, null, null, "Enrolled" },
                    { 38, 1, 1, 19, 7, 16m, 25m, null },
                    { 39, 0, 3, 20, 1, null, null, "Enrolled" },
                    { 40, 1, 1, 20, 7, 16m, 25m, null },
                    { 41, 0, 3, 21, 1, null, null, "Enrolled" },
                    { 42, 1, 1, 21, 7, 16m, 25m, null },
                    { 43, 0, 3, 22, 1, null, null, "Enrolled" },
                    { 44, 1, 1, 22, 7, 16m, 25m, null },
                    { 45, 0, 3, 23, 1, null, null, "Enrolled" },
                    { 46, 1, 1, 23, 7, 16m, 25m, null },
                    { 47, 0, 3, 24, 1, null, null, "Enrolled" },
                    { 48, 1, 1, 24, 7, 16m, 25m, null },
                    { 49, 0, 3, 25, 1, null, null, "Enrolled" },
                    { 50, 1, 1, 25, 7, 16m, 25m, null },
                    { 51, 0, 3, 26, 1, null, null, "Enrolled" },
                    { 52, 1, 1, 26, 7, 16m, 25m, null },
                    { 53, 0, 3, 27, 1, null, null, "Enrolled" },
                    { 54, 1, 1, 27, 7, 16m, 25m, null },
                    { 55, 0, 3, 28, 1, null, null, "Enrolled" },
                    { 56, 1, 1, 28, 7, 16m, 25m, null },
                    { 57, 0, 3, 29, 1, null, null, "Enrolled" },
                    { 58, 1, 1, 29, 7, 16m, 25m, null },
                    { 59, 0, 3, 30, 1, null, null, "Enrolled" },
                    { 60, 1, 1, 30, 7, 16m, 25m, null }
                });

            migrationBuilder.InsertData(
                table: "TopupExecutionTarget",
                columns: new[] { "Id", "AccountNumber", "Amount", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "EducationCreditTransactionId", "FailureReason", "IsDeleted", "MatchedConditionsSnapshot", "Status", "TopupExecutionId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 2, "EDU-2026-00000000002", 100m, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, 2, null, "Seeded failure for manual review.", false, null, 4, 1, null, null });

            migrationBuilder.InsertData(
                table: "Enrollment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenEmailSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CitizenPhoneNumberSnapshot", "CourseDescriptionSnapshot", "CourseId", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "SchoolNameSnapshot", "SchoolStudentId", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-00000000001", "citizen001@example.com", "Citizen 001", "S0000001I", "+6590000001", "Foundation course in applied mathematics.", 1, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 1, 1, null, null },
                    { 2, "EDU-2026-00000000002", "citizen002@example.com", "Citizen 002", "S0000002G", "+6590000002", "Introduction to programming and computing.", 2, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 2, 1, null, null },
                    { 3, "EDU-2026-00000000003", "citizen003@example.com", "Citizen 003", "S0000003E", "+6590000003", "Professional written and verbal communication.", 3, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 3, 1, null, null },
                    { 4, "EDU-2026-00000000004", "citizen004@example.com", "Citizen 004", "S0000004C", "+6590000004", "Environmental systems and sustainability.", 4, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 4, 1, null, null },
                    { 5, "EDU-2026-00000000005", "citizen005@example.com", "Citizen 005", "S0000005A", "+6590000005", "Digital design principles and production.", 5, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 5, 1, null, null },
                    { 6, "EDU-2026-00000000006", "citizen006@example.com", "Citizen 006", "S0000006Z", "+6590000006", "Core hospitality service operations.", 6, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 6, 1, null, null },
                    { 7, "EDU-2026-00000000007", "citizen007@example.com", "Citizen 007", "S0000007H", "+6590000007", "Fundamentals of electrical systems.", 7, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 7, 1, null, null },
                    { 8, "EDU-2026-00000000008", "citizen008@example.com", "Citizen 008", "S0000008F", "+6590000008", "Writing techniques across common genres.", 8, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 8, 1, null, null },
                    { 9, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Data preparation, analysis and reporting.", 9, "Data Analytics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 9, 1, null, null },
                    { 10, "EDU-2026-00000000010", "citizen010@example.com", "Citizen 010", "S0000010H", "+6590000010", "Archived office applications programme.", 10, "Legacy Office Applications", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 10, 1, null, null },
                    { 11, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Foundation course in applied mathematics.", 1, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 9, 1, null, null },
                    { 12, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Environmental systems and sustainability.", 4, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 9, 1, null, null },
                    { 13, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Introduction to programming and computing.", 2, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 9, 1, null, null },
                    { 14, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Professional written and verbal communication.", 3, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 9, 1, null, null },
                    { 15, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Digital design principles and production.", 5, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 9, 1, null, null },
                    { 16, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Core hospitality service operations.", 6, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 9, 1, null, null },
                    { 17, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Fundamentals of electrical systems.", 7, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 9, 1, null, null },
                    { 18, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Writing techniques across common genres.", 8, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 9, 1, null, null },
                    { 19, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Archived office applications programme.", 10, "Legacy Office Applications", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 9, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditTransactionId", "ExternalReference", "IsDeleted", "PaidAt", "PaymentMethod", "Status", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-00000000002", "Citizen 002", "S0000002G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, null, false, new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 70m, null, null },
                    { 2, "EDU-2026-00000000003", "Citizen 003", "S0000003E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, null, false, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 140m, null, null },
                    { 3, "EDU-2026-00000000004", "Citizen 004", "S0000004C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, null, false, new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 180m, null, null },
                    { 4, "EDU-2026-00000000005", "Citizen 005", "S0000005A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, null, false, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 180m, null, null },
                    { 9, "EDU-2026-00000000006", "Citizen 006", "S0000006Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, null, false, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 50m, null, null },
                    { 10, "EDU-2026-00000000008", "Citizen 008", "S0000008F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, null, false, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 70m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupExecutionTarget",
                columns: new[] { "Id", "AccountNumber", "Amount", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "EducationCreditTransactionId", "FailureReason", "IsDeleted", "MatchedConditionsSnapshot", "Status", "TopupExecutionId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-00000000001", 100m, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, 1, 1, null, false, null, 3, 1, null, null },
                    { 3, "EDU-2026-00000000003", 200m, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 3, null, false, null, 3, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Charge",
                columns: new[] { "Id", "BecameOutstandingAt", "CourseCodeSnapshot", "CourseDescriptionSnapshot", "CourseEndDateSnapshot", "CourseFeeAmountSnapshot", "CourseNameSnapshot", "CourseStartDateSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EnrollmentId", "GrossAmount", "GstAmountSnapshot", "IsDeleted", "LastAutoDeductedAt", "MiscFeeAmountSnapshot", "NetAmount", "PaidAmount", "RemainingAmount", "SchoolNameSnapshot", "Status", "SubsidyAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, "CRS-2026-A1B2C3D", "Foundation course in applied mathematics.", new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 100m, "Applied Mathematics", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 120m, 10m, false, null, 10m, 120m, 120m, 0m, "Northview Secondary School", 3, 0m, null, null },
                    { 2, null, "CRS-2026-B2C3D4E", "Introduction to programming and computing.", new DateTime(2026, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 115m, "Computer Science Fundamentals", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 140m, 13m, false, null, 12m, 140m, 70m, 70m, "Eastbridge Secondary School", 2, 0m, null, null },
                    { 3, null, "CRS-2026-C3D4E5F", "Professional written and verbal communication.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 130m, "Business Communication", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 160m, 15m, false, null, 15m, 160m, 140m, 20m, "Westhaven Secondary School", 2, 0m, null, null },
                    { 4, null, "CRS-2026-D4E5F6G", "Environmental systems and sustainability.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 145m, "Environmental Science", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, 180m, 18m, false, null, 17m, 180m, 180m, 0m, "Southpoint Secondary School", 3, 0m, null, null },
                    { 5, null, "CRS-2026-E5F6G7H", "Digital design principles and production.", new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 160m, "Digital Media Design", new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, 200m, 20m, false, null, 20m, 200m, 180m, 20m, "Central Heights School", 2, 0m, null, null },
                    { 6, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), "CRS-2026-F6G7H8J", "Core hospitality service operations.", new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), 175m, "Hospitality Operations", new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 220m, 23m, false, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 22m, 220m, 150m, 70m, "Riverside Learning Institute", 4, 0m, null, null },
                    { 7, null, "CRS-2026-G7H8J9K", "Fundamentals of electrical systems.", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 190m, "Electrical Engineering Basics", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, 240m, 25m, false, null, 25m, 240m, 200m, 40m, "Lakeside Technical School", 2, 0m, null, null },
                    { 8, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), "CRS-2026-H8J9K0L", "Writing techniques across common genres.", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 205m, "Creative Writing", new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 260m, 28m, false, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 27m, 260m, 200m, 60m, "Greenfield Academy", 4, 0m, null, null },
                    { 9, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "CRS-2026-J9K0L1M", "Data preparation, analysis and reporting.", new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), 220m, "Data Analytics", new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, 280m, 30m, false, null, 30m, 280m, 0m, 280m, "Harbourfront School", 4, 0m, null, null },
                    { 10, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), "CRS-2026-K0L1M2N", "Archived office applications programme.", new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 235m, "Legacy Office Applications", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 300m, 33m, false, null, 32m, 300m, 0m, 300m, "Hillcrest Education Centre", 4, 0m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupSystemApplication",
                columns: new[] { "Id", "EducationAccountId", "SystemTopupId", "TopupExecutionTargetId" },
                values: new object[] { 1, 3, 21, 3 });

            migrationBuilder.InsertData(
                table: "OutstandingDeductionTarget",
                columns: new[] { "Id", "BalanceAfter", "BalanceBefore", "ChargeId", "CreatedAt", "CreatedBy", "DeductedAmount", "DeletedAt", "EducationAccountId", "EducationCreditTransactionId", "FailureReason", "IsDeleted", "OutstandingDeductionRunId", "PaymentId", "RemainingAfter", "RemainingBefore", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 0m, 0m, 1, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, 1, null, "Charge was already paid.", false, 1, null, 0m, 0m, 3, null, null },
                    { 2, 100m, 100m, 2, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, 2, null, "Concurrency conflict while updating the balance.", false, 2, null, 70m, 70m, 2, null, null },
                    { 3, 450m, 500m, 6, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 50m, null, 6, 7, null, false, 7, 9, 70m, 120m, 1, null, null },
                    { 4, 0m, 70m, 8, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 70m, null, 8, 8, null, false, 8, 10, 60m, 130m, 1, null, null },
                    { 5, 300m, 300m, 3, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, 3, null, "Charge was already paid.", false, 3, null, 0m, 0m, 3, null, null },
                    { 6, 0m, 0m, 4, new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, 4, null, "No remaining amount was available.", false, 4, null, 0m, 0m, 3, null, null },
                    { 7, 200m, 200m, 5, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, 5, null, "Charge was already paid.", false, 5, null, 0m, 0m, 3, null, null },
                    { 8, 0m, 0m, 7, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, 7, null, "Education account balance was zero.", false, 6, null, 0m, 0m, 3, null, null },
                    { 9, 250m, 250m, 9, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, 9, null, "Run terminated before the target could be committed.", false, 9, null, 250m, 250m, 2, null, null },
                    { 10, 0m, 0m, 10, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, 10, null, "Education account balance was zero.", false, 10, null, 300m, 300m, 3, null, null }
                });

            migrationBuilder.InsertData(
                table: "PaymentAllocation",
                columns: new[] { "Id", "Amount", "ChargeGrossAmountSnapshot", "ChargeId", "ChargeNetAmountSnapshot", "ChargeRemainingAmountSnapshot", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "PaymentId", "SchoolNameSnapshot", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 70m, 140m, 2, 140m, 140m, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Eastbridge Secondary School", null, null },
                    { 2, 140m, 160m, 3, 160m, 160m, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Westhaven Secondary School", null, null },
                    { 3, 180m, 180m, 4, 180m, 180m, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 3, "Southpoint Secondary School", null, null },
                    { 4, 180m, 200m, 5, 200m, 200m, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, "Central Heights School", null, null },
                    { 5, 120m, 120m, 1, 120m, 120m, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 5, "Northview Secondary School", null, null },
                    { 6, 100m, 220m, 6, 220m, 220m, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 6, "Riverside Learning Institute", null, null },
                    { 7, 200m, 240m, 7, 240m, 240m, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 7, "Lakeside Technical School", null, null },
                    { 8, 130m, 260m, 8, 260m, 260m, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 8, "Greenfield Academy", null, null },
                    { 9, 50m, 220m, 6, 220m, 120m, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 9, "Riverside Learning Institute", null, null },
                    { 10, 70m, 260m, 8, 260m, 130m, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 10, "Greenfield Academy", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_Email",
                table: "AdminProfile",
                column: "Email",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_SchoolId",
                table: "AdminProfile",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_StaffCode",
                table: "AdminProfile",
                column: "StaffCode",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"StaffCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_UserId",
                table: "AdminProfile",
                column: "UserId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"UserId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Action",
                table: "AuditLog",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ActorUserId",
                table: "AuditLog",
                column: "ActorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Category",
                table: "AuditLog",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_OccurredAt",
                table: "AuditLog",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_BecameOutstandingAt",
                table: "Charge",
                column: "BecameOutstandingAt");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_EnrollmentId",
                table: "Charge",
                column: "EnrollmentId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"EnrollmentId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_Status",
                table: "Charge",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_CitizenshipStatus",
                table: "Citizen",
                column: "CitizenshipStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_DateOfBirth",
                table: "Citizen",
                column: "DateOfBirth");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_Email",
                table: "Citizen",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_Nric",
                table: "Citizen",
                column: "Nric",
                unique: true,
                filter: "\"Nric\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Course_CourseCode",
                table: "Course",
                column: "CourseCode",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"CourseCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Course_CourseName",
                table: "Course",
                column: "CourseName",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"CourseName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Course_EndDate",
                table: "Course",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Course_FasApplicationDueDate",
                table: "Course",
                column: "FasApplicationDueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Course_SchoolId",
                table: "Course",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_SchoolId_CourseCode",
                table: "Course",
                columns: new[] { "SchoolId", "CourseCode" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"SchoolId\" IS NOT NULL AND \"CourseCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Course_StartDate",
                table: "Course",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Course_Status",
                table: "Course",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccount_AccountNumber",
                table: "EducationAccount",
                column: "AccountNumber",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"AccountNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccount_CitizenId",
                table: "EducationAccount",
                column: "CitizenId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"CitizenId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccount_ClosedAt",
                table: "EducationAccount",
                column: "ClosedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccount_OpenedAt",
                table: "EducationAccount",
                column: "OpenedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccount_Status",
                table: "EducationAccount",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccountStatusHistory_ChangedAt",
                table: "EducationAccountStatusHistory",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccountStatusHistory_ChangedByUserId",
                table: "EducationAccountStatusHistory",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccountStatusHistory_EducationAccountId",
                table: "EducationAccountStatusHistory",
                column: "EducationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccountSweepReports_BatchDate",
                table: "EducationAccountSweepReports",
                column: "BatchDate",
                unique: true,
                filter: "\"BatchDate\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EducationAccountSweepTargets_SweepReportId",
                table: "EducationAccountSweepTargets",
                column: "SweepReportId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationCreditTransaction_CreatedAt",
                table: "EducationCreditTransaction",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EducationCreditTransaction_Direction",
                table: "EducationCreditTransaction",
                column: "Direction");

            migrationBuilder.CreateIndex(
                name: "IX_EducationCreditTransaction_EducationAccountId",
                table: "EducationCreditTransaction",
                column: "EducationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationCreditTransaction_TransactionCode",
                table: "EducationCreditTransaction",
                column: "TransactionCode",
                unique: true,
                filter: "\"TransactionCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EducationCreditTransaction_Type",
                table: "EducationCreditTransaction",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_AccountNumberSnapshot",
                table: "Enrollment",
                column: "AccountNumberSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CitizenNricSnapshot",
                table: "Enrollment",
                column: "CitizenNricSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseId",
                table: "Enrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseId_SchoolStudentId",
                table: "Enrollment",
                columns: new[] { "CourseId", "SchoolStudentId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"CourseId\" IS NOT NULL AND \"SchoolStudentId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_SchoolStudentId",
                table: "Enrollment",
                column: "SchoolStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_OccurredAt",
                table: "OutboxMessage",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_Status",
                table: "OutboxMessage",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_Type",
                table: "OutboxMessage",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionRun_RunDate",
                table: "OutstandingDeductionRun",
                column: "RunDate");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionRun_RunMonth",
                table: "OutstandingDeductionRun",
                column: "RunMonth",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"RunMonth\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionRun_StartedAt",
                table: "OutstandingDeductionRun",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionRun_Status",
                table: "OutstandingDeductionRun",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionTarget_ChargeId",
                table: "OutstandingDeductionTarget",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionTarget_EducationAccountId",
                table: "OutstandingDeductionTarget",
                column: "EducationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionTarget_EducationCreditTransactionId",
                table: "OutstandingDeductionTarget",
                column: "EducationCreditTransactionId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"EducationCreditTransactionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionTarget_OutstandingDeductionRunId",
                table: "OutstandingDeductionTarget",
                column: "OutstandingDeductionRunId");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionTarget_OutstandingDeductionRunId_ChargeId",
                table: "OutstandingDeductionTarget",
                columns: new[] { "OutstandingDeductionRunId", "ChargeId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"OutstandingDeductionRunId\" IS NOT NULL AND \"ChargeId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionTarget_PaymentId",
                table: "OutstandingDeductionTarget",
                column: "PaymentId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PaymentId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OutstandingDeductionTarget_Status",
                table: "OutstandingDeductionTarget",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AccountNumberSnapshot",
                table: "Payment",
                column: "AccountNumberSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CitizenNricSnapshot",
                table: "Payment",
                column: "CitizenNricSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_EducationCreditTransactionId",
                table: "Payment",
                column: "EducationCreditTransactionId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"EducationCreditTransactionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ExternalReference",
                table: "Payment",
                column: "ExternalReference",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"ExternalReference\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaidAt",
                table: "Payment",
                column: "PaidAt");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentMethod",
                table: "Payment",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Status",
                table: "Payment",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocation_ChargeId",
                table: "PaymentAllocation",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocation_PaymentId",
                table: "PaymentAllocation",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId",
                table: "PaymentAllocation",
                columns: new[] { "PaymentId", "ChargeId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PaymentId\" IS NOT NULL AND \"ChargeId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ExpiresAt",
                table: "RefreshToken",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_RevokedAt",
                table: "RefreshToken",
                column: "RevokedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_TokenHash",
                table: "RefreshToken",
                column: "TokenHash",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"TokenHash\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTopUp_Name",
                table: "ScheduleTopUp",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTopUp_NextExecutionAt",
                table: "ScheduleTopUp",
                column: "NextExecutionAt");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTopUp_Status",
                table: "ScheduleTopUp",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTopUpCondition_Field",
                table: "ScheduleTopUpCondition",
                column: "Field");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTopUpCondition_GroupId",
                table: "ScheduleTopUpCondition",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTopUpConditionGroup_ParentGroupId",
                table: "ScheduleTopUpConditionGroup",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTopUpConditionGroup_ScheduleTopUpId",
                table: "ScheduleTopUpConditionGroup",
                column: "ScheduleTopUpId",
                unique: true,
                filter: "[ParentGroupId] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_School_Email",
                table: "School",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_School_SchoolName",
                table: "School",
                column: "SchoolName");

            migrationBuilder.CreateIndex(
                name: "IX_School_Status",
                table: "School",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolStudent_EducationAccountId",
                table: "SchoolStudent",
                column: "EducationAccountId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"EducationAccountId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolStudent_SchoolId",
                table: "SchoolStudent",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolStudent_Status",
                table: "SchoolStudent",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SsoIdentity_Provider",
                table: "SsoIdentity",
                column: "Provider");

            migrationBuilder.CreateIndex(
                name: "IX_SsoIdentity_Provider_ProviderUserId",
                table: "SsoIdentity",
                columns: new[] { "Provider", "ProviderUserId" },
                unique: true,
                filter: "\"Provider\" IS NOT NULL AND \"ProviderUserId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SsoIdentity_ProviderUserId",
                table: "SsoIdentity",
                column: "ProviderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SsoIdentity_UserId",
                table: "SsoIdentity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SsoIdentity_UserId_Provider",
                table: "SsoIdentity",
                columns: new[] { "UserId", "Provider" },
                unique: true,
                filter: "\"UserId\" IS NOT NULL AND \"Provider\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SystemTopup_Name",
                table: "SystemTopup",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SystemTopup_Status",
                table: "SystemTopup",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SystemTopupCondition_Field",
                table: "SystemTopupCondition",
                column: "Field");

            migrationBuilder.CreateIndex(
                name: "IX_SystemTopupCondition_GroupId",
                table: "SystemTopupCondition",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemTopupConditionGroup_ParentGroupId",
                table: "SystemTopupConditionGroup",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemTopupConditionGroup_SystemTopupId",
                table: "SystemTopupConditionGroup",
                column: "SystemTopupId",
                unique: true,
                filter: "[ParentGroupId] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecution_ExecutionCode",
                table: "TopupExecution",
                column: "ExecutionCode",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"ExecutionCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecution_IdempotencyKey",
                table: "TopupExecution",
                column: "IdempotencyKey",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"IdempotencyKey\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecution_ScheduleTopUpId",
                table: "TopupExecution",
                column: "ScheduleTopUpId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecution_SourceType",
                table: "TopupExecution",
                column: "SourceType");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecution_Status",
                table: "TopupExecution",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecution_SystemTopupId",
                table: "TopupExecution",
                column: "SystemTopupId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecutionTarget_EducationAccountId",
                table: "TopupExecutionTarget",
                column: "EducationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecutionTarget_EducationCreditTransactionId",
                table: "TopupExecutionTarget",
                column: "EducationCreditTransactionId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"EducationCreditTransactionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecutionTarget_Status",
                table: "TopupExecutionTarget",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecutionTarget_TopupExecutionId",
                table: "TopupExecutionTarget",
                column: "TopupExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecutionTarget_TopupExecutionId_AccountNumber",
                table: "TopupExecutionTarget",
                columns: new[] { "TopupExecutionId", "AccountNumber" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"TopupExecutionId\" IS NOT NULL AND \"AccountNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupSystemApplication_EducationAccountId",
                table: "TopupSystemApplication",
                column: "EducationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupSystemApplication_SystemTopupId",
                table: "TopupSystemApplication",
                column: "SystemTopupId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupSystemApplication_SystemTopupId_EducationAccountId",
                table: "TopupSystemApplication",
                columns: new[] { "SystemTopupId", "EducationAccountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopupSystemApplication_TopupExecutionTargetId",
                table: "TopupSystemApplication",
                column: "TopupExecutionTargetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CitizenId",
                table: "User",
                column: "CitizenId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"CitizenId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role",
                table: "User",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_User_Status",
                table: "User",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatusHistory_ChangedAt",
                table: "UserStatusHistory",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatusHistory_ChangedByUserId",
                table: "UserStatusHistory",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatusHistory_UserId",
                table: "UserStatusHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminProfile");

            migrationBuilder.DropTable(
                name: "AiAssistantSetting");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "EducationAccountStatusHistory");

            migrationBuilder.DropTable(
                name: "EducationAccountSweepTargets");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "OutstandingDeductionTarget");

            migrationBuilder.DropTable(
                name: "PaymentAllocation");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "ScheduleTopUpCondition");

            migrationBuilder.DropTable(
                name: "SsoIdentity");

            migrationBuilder.DropTable(
                name: "SystemTopupCondition");

            migrationBuilder.DropTable(
                name: "TopupSystemApplication");

            migrationBuilder.DropTable(
                name: "UserStatusHistory");

            migrationBuilder.DropTable(
                name: "EducationAccountSweepReports");

            migrationBuilder.DropTable(
                name: "OutstandingDeductionRun");

            migrationBuilder.DropTable(
                name: "Charge");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "ScheduleTopUpConditionGroup");

            migrationBuilder.DropTable(
                name: "SystemTopupConditionGroup");

            migrationBuilder.DropTable(
                name: "TopupExecutionTarget");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "EducationCreditTransaction");

            migrationBuilder.DropTable(
                name: "TopupExecution");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "SchoolStudent");

            migrationBuilder.DropTable(
                name: "ScheduleTopUp");

            migrationBuilder.DropTable(
                name: "SystemTopup");

            migrationBuilder.DropTable(
                name: "EducationAccount");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropTable(
                name: "Citizen");
        }
    }
}
