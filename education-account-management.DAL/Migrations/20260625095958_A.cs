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
                    TaxRate = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    InstallmentDueDay = table.Column<int>(type: "int", nullable: false),
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
                    table.CheckConstraint("CK_AiAssistantSetting_TaxRate", "[TaxRate] >= 0");
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
                    IsSingaporean = table.Column<bool>(type: "bit", nullable: false),
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
                    EnrollmentDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.CheckConstraint("CK_Course_Date_Order", "[EnrollmentDeadline] <= [StartDate] AND [StartDate] <= [EndDate]");
                    table.ForeignKey(
                        name: "FK_Course_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FasScheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SchemeCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchemeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DurationInMonths = table.Column<int>(type: "int", nullable: false),
                    SubsidyType = table.Column<int>(type: "int", nullable: false),
                    IsPerComponent = table.Column<bool>(type: "bit", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasScheme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasScheme_School_SchoolId",
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
                name: "FasSchemeConditionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FasSchemeId = table.Column<int>(type: "int", nullable: false),
                    ParentGroupId = table.Column<int>(type: "int", nullable: true),
                    LogicalOperator = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasSchemeConditionGroup", x => x.Id);
                    table.CheckConstraint("CK_FasSchemeConditionGroup_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
                    table.ForeignKey(
                        name: "FK_FasSchemeConditionGroup_FasSchemeConditionGroup_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalTable: "FasSchemeConditionGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FasSchemeConditionGroup_FasScheme_FasSchemeId",
                        column: x => x.FasSchemeId,
                        principalTable: "FasScheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FasSchemeCourse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FasSchemeId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasSchemeCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasSchemeCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FasSchemeCourse_FasScheme_FasSchemeId",
                        column: x => x.FasSchemeId,
                        principalTable: "FasScheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FasSchemeRequiredDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FasSchemeId = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TemplateFileKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasSchemeRequiredDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasSchemeRequiredDocument_FasScheme_FasSchemeId",
                        column: x => x.FasSchemeId,
                        principalTable: "FasScheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FasSchemeTier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FasSchemeId = table.Column<int>(type: "int", nullable: false),
                    TierName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaxPerCapitaIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SubsidyValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CourseFeeSubsidyValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MiscFeeSubsidyValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasSchemeTier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasSchemeTier_FasScheme_FasSchemeId",
                        column: x => x.FasSchemeId,
                        principalTable: "FasScheme",
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
                    Status = table.Column<int>(type: "int", nullable: false),
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
                name: "FasSchemeCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Field = table.Column<int>(type: "int", nullable: false),
                    Operator = table.Column<int>(type: "int", nullable: false),
                    ValueNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValueNumberTo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValueText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasSchemeCondition", x => x.Id);
                    table.CheckConstraint("CK_FasSchemeCondition_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
                    table.CheckConstraint("CK_FasSchemeCondition_Value_By_Field", "([Field] IN (1, 5, 6) AND [ValueNumber] IS NOT NULL AND [ValueText] IS NULL AND (([Operator] = 7 AND [ValueNumberTo] IS NOT NULL AND [ValueNumberTo] >= [ValueNumber]) OR ([Operator] <> 7 AND [ValueNumberTo] IS NULL))) OR ([Field] IN (2, 3, 4) AND [ValueText] IS NOT NULL AND [Operator] IN (1, 2) AND [ValueNumber] IS NULL AND [ValueNumberTo] IS NULL)");
                    table.ForeignKey(
                        name: "FK_FasSchemeCondition_FasSchemeConditionGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "FasSchemeConditionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FasApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FasSchemeId = table.Column<int>(type: "int", nullable: false),
                    SchoolStudentId = table.Column<int>(type: "int", nullable: false),
                    RecommendedTierId = table.Column<int>(type: "int", nullable: true),
                    ApprovedTierId = table.Column<int>(type: "int", nullable: true),
                    ApplicationNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StudentAgeSnapshot = table.Column<int>(type: "int", nullable: false),
                    StudentNationalitySnapshot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FatherNationalitySnapshot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MotherNationalitySnapshot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GrossHouseholdIncomeSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HouseholdMemberCountSnapshot = table.Column<int>(type: "int", nullable: false),
                    PerCapitaIncomeSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecommendationReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: true),
                    DurationInMonthsSnapshot = table.Column<int>(type: "int", nullable: true),
                    ValidityStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidityEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WithdrawnAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasApplication_FasSchemeTier_ApprovedTierId",
                        column: x => x.ApprovedTierId,
                        principalTable: "FasSchemeTier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FasApplication_FasSchemeTier_RecommendedTierId",
                        column: x => x.RecommendedTierId,
                        principalTable: "FasSchemeTier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FasApplication_FasScheme_FasSchemeId",
                        column: x => x.FasSchemeId,
                        principalTable: "FasScheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FasApplication_SchoolStudent_SchoolStudentId",
                        column: x => x.SchoolStudentId,
                        principalTable: "SchoolStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FasApplication_User_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "User",
                        principalColumn: "Id");
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
                    Status = table.Column<int>(type: "int", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubsidyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    SchoolNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseCodeSnapshot = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CourseNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseDescriptionSnapshot = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CourseStartDateSnapshot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseEndDateSnapshot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseFeeAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MiscFeeAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GstAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRateSnapshot = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    AppliedFasSchemeNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AppliedFasTierNameSnapshot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AppliedFasSubsidyTypeSnapshot = table.Column<int>(type: "int", nullable: true),
                    AppliedFasIsPerComponentSnapshot = table.Column<bool>(type: "bit", nullable: false),
                    AppliedFasSubsidyValueSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AppliedFasCourseFeeSubsidyValueSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AppliedFasMiscFeeSubsidyValueSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EnrollmentId = table.Column<int>(type: "int", nullable: false),
                    AppliedFasApplicationId = table.Column<int>(type: "int", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_Charge_FasApplication_AppliedFasApplicationId",
                        column: x => x.AppliedFasApplicationId,
                        principalTable: "FasApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FasApplicationDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FasApplicationId = table.Column<int>(type: "int", nullable: false),
                    FasSchemeRequiredDocumentId = table.Column<int>(type: "int", nullable: true),
                    DocumentNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FileKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasApplicationDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasApplicationDocument_FasApplication_FasApplicationId",
                        column: x => x.FasApplicationId,
                        principalTable: "FasApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FasApplicationDocument_FasSchemeRequiredDocument_FasSchemeRequiredDocumentId",
                        column: x => x.FasSchemeRequiredDocumentId,
                        principalTable: "FasSchemeRequiredDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FasTierOverrideHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FasApplicationId = table.Column<int>(type: "int", nullable: false),
                    OldTierId = table.Column<int>(type: "int", nullable: true),
                    NewTierId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasTierOverrideHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasTierOverrideHistory_FasApplication_FasApplicationId",
                        column: x => x.FasApplicationId,
                        principalTable: "FasApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FasTierOverrideHistory_FasSchemeTier_NewTierId",
                        column: x => x.NewTierId,
                        principalTable: "FasSchemeTier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FasTierOverrideHistory_FasSchemeTier_OldTierId",
                        column: x => x.OldTierId,
                        principalTable: "FasSchemeTier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FasTierOverrideHistory_User_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChargeInstallment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeId = table.Column<int>(type: "int", nullable: false),
                    InstallmentNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BecameOverdueAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeInstallment", x => x.Id);
                    table.CheckConstraint("CK_ChargeInstallment_Amounts", "[Amount] > 0");
                    table.CheckConstraint("CK_ChargeInstallment_Number_Positive", "[InstallmentNumber] > 0");
                    table.ForeignKey(
                        name: "FK_ChargeInstallment_Charge_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "Charge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ChargeInstallmentId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_PaymentAllocation_ChargeInstallment_ChargeInstallmentId",
                        column: x => x.ChargeInstallmentId,
                        principalTable: "ChargeInstallment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "InstallmentDueDay", "IsDeleted", "IsEnabled", "TaxRate", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, true, 0.09m, null, null });

            migrationBuilder.InsertData(
                table: "Citizen",
                columns: new[] { "Id", "CitizenshipStatus", "CreatedAt", "CreatedBy", "DateOfBirth", "DeletedAt", "Email", "FullName", "IsDeleted", "IsSingaporean", "MailingAddress", "Nric", "PhoneNumber", "ResidentialAddress", "SchoolingStatus", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1991, 1, 2), null, "sterling.quach@example.com", "Sterling Quach", false, true, "Mailing block 1, Singapore", "S0000001I", "+6590000001", "Residential block 1, Singapore", "Enrolled", null, null },
                    { 2, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1992, 2, 3), null, "amelia.tan@example.com", "Amelia Tan", false, true, "Mailing block 2, Singapore", "S0000002G", "+6590000002", "Residential block 2, Singapore", "Not Enrolled", null, null },
                    { 3, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1993, 3, 4), null, "marcus.lim@example.com", "Marcus Lim", false, true, "Mailing block 3, Singapore", "S0000003E", "+6590000003", "Residential block 3, Singapore", "Graduated", null, null },
                    { 4, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1994, 4, 5), null, "priya.nair@example.com", "Priya Nair", false, true, "Mailing block 4, Singapore", "S0000004C", "+6590000004", "Residential block 4, Singapore", "Suspended", null, null },
                    { 5, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1995, 5, 6), null, "ethan.koh@example.com", "Ethan Koh", false, true, "Mailing block 5, Singapore", "S0000005A", "+6590000005", "Residential block 5, Singapore", "Withdrawn", null, null },
                    { 6, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1996, 6, 7), null, "hannah.lee@example.com", "Hannah Lee", false, true, "Mailing block 6, Singapore", "S0000006Z", "+6590000006", "Residential block 6, Singapore", "Enrolled", null, null },
                    { 7, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1997, 7, 8), null, "daniel.wong@example.com", "Daniel Wong", false, true, "Mailing block 7, Singapore", "S0000007H", "+6590000007", "Residential block 7, Singapore", "Not Enrolled", null, null },
                    { 8, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1998, 8, 9), null, "sofia.chen@example.com", "Sofia Chen", false, true, "Mailing block 8, Singapore", "S0000008F", "+6590000008", "Residential block 8, Singapore", "Graduated", null, null },
                    { 9, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1999, 9, 10), null, "lucas.nguyen@example.com", "Lucas Nguyen", false, true, "Mailing block 9, Singapore", "S0000009D", "+6590000009", "Residential block 9, Singapore", "Suspended", null, null },
                    { 10, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 10, 11), null, "maya.rahman@example.com", "Maya Rahman", false, true, "Mailing block 10, Singapore", "S0000010H", "+6590000010", "Residential block 10, Singapore", "Withdrawn", null, null },
                    { 11, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 11, 12), null, "noah.teo@example.com", "Noah Teo", false, true, "Mailing block 11, Singapore", "S0000011F", "+6590000011", "Residential block 11, Singapore", "Enrolled", null, null },
                    { 12, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 12, 13), null, "aisha.fernandez@example.com", "Aisha Fernandez", false, true, "Mailing block 12, Singapore", "S0000012D", "+6590000012", "Residential block 12, Singapore", "Not Enrolled", null, null },
                    { 13, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 1, 14), null, "ryan.chua@example.com", "Ryan Chua", false, true, "Mailing block 13, Singapore", "S0000013B", "+6590000013", "Residential block 13, Singapore", "Graduated", null, null },
                    { 14, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 2, 15), null, "chloe.goh@example.com", "Chloe Goh", false, true, "Mailing block 14, Singapore", "S0000014J", "+6590000014", "Residential block 14, Singapore", "Suspended", null, null },
                    { 15, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2005, 3, 16), null, "irfan.hassan@example.com", "Irfan Hassan", false, true, "Mailing block 15, Singapore", "S0000015I", "+6590000015", "Residential block 15, Singapore", "Withdrawn", null, null },
                    { 16, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 1, 16), null, "natalie.seah@example.com", "Natalie Seah", false, false, "16 Orchard Link, Singapore", "S0000016G", "+6590000016", "16 Orchard Link, Singapore", "Not Enrolled", null, null },
                    { 17, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 5, 15), null, "qistina.rao.17@example.com", "Qistina Rao", false, false, "17 Learning Grove, Singapore", "S0000017E", "+6590000017", "17 Learning Grove, Singapore", "Enrolled", null, null },
                    { 18, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 6, 15), null, "rafael.sim.18@example.com", "Rafael Sim", false, false, "18 Learning Grove, Singapore", "S0000018C", "+6590000018", "18 Learning Grove, Singapore", "Enrolled", null, null },
                    { 19, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 7, 15), null, "selina.tan.19@example.com", "Selina Tan", false, false, "19 Learning Grove, Singapore", "S0000019A", "+6590000019", "19 Learning Grove, Singapore", "Enrolled", null, null },
                    { 20, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 8, 15), null, "terence.uddin.20@example.com", "Terence Uddin", false, false, "20 Learning Grove, Singapore", "S0000020E", "+6590000020", "20 Learning Grove, Singapore", "Enrolled", null, null },
                    { 21, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 9, 15), null, "umairah.vasquez.21@example.com", "Umairah Vasquez", false, false, "21 Learning Grove, Singapore", "S0000021C", "+6590000021", "21 Learning Grove, Singapore", "Enrolled", null, null },
                    { 22, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 10, 15), null, "victor.wong.22@example.com", "Victor Wong", false, false, "22 Learning Grove, Singapore", "S0000022A", "+6590000022", "22 Learning Grove, Singapore", "Enrolled", null, null },
                    { 23, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 11, 15), null, "wen.jie.xu.23@example.com", "Wen Jie Xu", false, false, "23 Learning Grove, Singapore", "S0000023Z", "+6590000023", "23 Learning Grove, Singapore", "Enrolled", null, null },
                    { 24, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 12, 15), null, "xavier.yeo.24@example.com", "Xavier Yeo", false, false, "24 Learning Grove, Singapore", "S0000024H", "+6590000024", "24 Learning Grove, Singapore", "Enrolled", null, null },
                    { 25, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 1, 15), null, "yasmin.zainal.25@example.com", "Yasmin Zainal", false, false, "25 Learning Grove, Singapore", "S0000025F", "+6590000025", "25 Learning Grove, Singapore", "Enrolled", null, null },
                    { 26, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 2, 15), null, "zachary.ang.26@example.com", "Zachary Ang", false, false, "26 Learning Grove, Singapore", "S0000026D", "+6590000026", "26 Learning Grove, Singapore", "Enrolled", null, null },
                    { 27, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 3, 15), null, "adeline.bala.27@example.com", "Adeline Bala", false, false, "27 Learning Grove, Singapore", "S0000027B", "+6590000027", "27 Learning Grove, Singapore", "Enrolled", null, null },
                    { 28, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 4, 15), null, "brandon.chew.28@example.com", "Brandon Chew", false, false, "28 Learning Grove, Singapore", "S0000028J", "+6590000028", "28 Learning Grove, Singapore", "Enrolled", null, null },
                    { 29, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 5, 15), null, "celeste.das.29@example.com", "Celeste Das", false, false, "29 Learning Grove, Singapore", "S0000029I", "+6590000029", "29 Learning Grove, Singapore", "Enrolled", null, null },
                    { 30, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 15), null, "damien.eng.30@example.com", "Damien Eng", false, false, "30 Learning Grove, Singapore", "S0000030B", "+6590000030", "30 Learning Grove, Singapore", "Enrolled", null, null },
                    { 31, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 15), null, "evelyn.foo.31@example.com", "Evelyn Foo", false, false, "31 Learning Grove, Singapore", "S0000031J", "+6590000031", "31 Learning Grove, Singapore", "Enrolled", null, null },
                    { 32, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 15), null, "faris.gan.32@example.com", "Faris Gan", false, false, "32 Learning Grove, Singapore", "S0000032I", "+6590000032", "32 Learning Grove, Singapore", "Enrolled", null, null },
                    { 33, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 15), null, "giselle.ho.33@example.com", "Giselle Ho", false, false, "33 Learning Grove, Singapore", "S0000033G", "+6590000033", "33 Learning Grove, Singapore", "Enrolled", null, null },
                    { 34, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 15), null, "haziq.ismail.34@example.com", "Haziq Ismail", false, false, "34 Learning Grove, Singapore", "S0000034E", "+6590000034", "34 Learning Grove, Singapore", "Enrolled", null, null },
                    { 35, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 11, 15), null, "irene.jeyaratnam.35@example.com", "Irene Jeyaratnam", false, false, "35 Learning Grove, Singapore", "S0000035C", "+6590000035", "35 Learning Grove, Singapore", "Enrolled", null, null },
                    { 36, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 12, 15), null, "jonas.kwek.36@example.com", "Jonas Kwek", false, false, "36 Learning Grove, Singapore", "S0000036A", "+6590000036", "36 Learning Grove, Singapore", "Enrolled", null, null },
                    { 37, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 1, 15), null, "kavya.lim.37@example.com", "Kavya Lim", false, false, "37 Learning Grove, Singapore", "S0000037Z", "+6590000037", "37 Learning Grove, Singapore", "Enrolled", null, null },
                    { 38, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 2, 15), null, "lydia.mohamed.38@example.com", "Lydia Mohamed", false, false, "38 Learning Grove, Singapore", "S0000038H", "+6590000038", "38 Learning Grove, Singapore", "Enrolled", null, null },
                    { 39, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 3, 15), null, "malcolm.ng.39@example.com", "Malcolm Ng", false, false, "39 Learning Grove, Singapore", "S0000039F", "+6590000039", "39 Learning Grove, Singapore", "Enrolled", null, null },
                    { 40, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 4, 15), null, "nadia.ong.40@example.com", "Nadia Ong", false, false, "40 Learning Grove, Singapore", "S0000040Z", "+6590000040", "40 Learning Grove, Singapore", "Enrolled", null, null },
                    { 41, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 5, 15), null, "alina.quek.41@example.com", "Alina Quek", false, false, "41 Learning Grove, Singapore", "S0000041H", "+6590000041", "41 Learning Grove, Singapore", "Enrolled", null, null },
                    { 42, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 6, 15), null, "benjamin.rao.42@example.com", "Benjamin Rao", false, false, "42 Learning Grove, Singapore", "S0000042F", "+6590000042", "42 Learning Grove, Singapore", "Enrolled", null, null },
                    { 43, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 7, 15), null, "clara.sim.43@example.com", "Clara Sim", false, false, "43 Learning Grove, Singapore", "S0000043D", "+6590000043", "43 Learning Grove, Singapore", "Enrolled", null, null },
                    { 44, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 8, 15), null, "darius.tan.44@example.com", "Darius Tan", false, false, "44 Learning Grove, Singapore", "S0000044B", "+6590000044", "44 Learning Grove, Singapore", "Enrolled", null, null },
                    { 45, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 9, 15), null, "elena.uddin.45@example.com", "Elena Uddin", false, false, "45 Learning Grove, Singapore", "S0000045J", "+6590000045", "45 Learning Grove, Singapore", "Enrolled", null, null },
                    { 46, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 10, 15), null, "farhan.vasquez.46@example.com", "Farhan Vasquez", false, false, "46 Learning Grove, Singapore", "S0000046I", "+6590000046", "46 Learning Grove, Singapore", "Enrolled", null, null },
                    { 47, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 11, 15), null, "grace.wong.47@example.com", "Grace Wong", false, false, "47 Learning Grove, Singapore", "S0000047G", "+6590000047", "47 Learning Grove, Singapore", "Enrolled", null, null },
                    { 48, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 12, 15), null, "haruto.xu.48@example.com", "Haruto Xu", false, false, "48 Learning Grove, Singapore", "S0000048E", "+6590000048", "48 Learning Grove, Singapore", "Enrolled", null, null },
                    { 49, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 1, 15), null, "isabelle.yeo.49@example.com", "Isabelle Yeo", false, false, "49 Learning Grove, Singapore", "S0000049C", "+6590000049", "49 Learning Grove, Singapore", "Enrolled", null, null },
                    { 50, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 2, 15), null, "jasper.zainal.50@example.com", "Jasper Zainal", false, false, "50 Learning Grove, Singapore", "S0000050G", "+6590000050", "50 Learning Grove, Singapore", "Enrolled", null, null },
                    { 51, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 3, 15), null, "keira.ang.51@example.com", "Keira Ang", false, false, "51 Learning Grove, Singapore", "S0000051E", "+6590000051", "51 Learning Grove, Singapore", "Enrolled", null, null },
                    { 52, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 4, 15), null, "leon.bala.52@example.com", "Leon Bala", false, false, "52 Learning Grove, Singapore", "S0000052C", "+6590000052", "52 Learning Grove, Singapore", "Enrolled", null, null },
                    { 53, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 5, 15), null, "mei.lin.chew.53@example.com", "Mei Lin Chew", false, false, "53 Learning Grove, Singapore", "S0000053A", "+6590000053", "53 Learning Grove, Singapore", "Enrolled", null, null },
                    { 54, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 6, 15), null, "nathan.das.54@example.com", "Nathan Das", false, false, "54 Learning Grove, Singapore", "S0000054Z", "+6590000054", "54 Learning Grove, Singapore", "Enrolled", null, null },
                    { 55, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 7, 15), null, "olivia.eng.55@example.com", "Olivia Eng", false, false, "55 Learning Grove, Singapore", "S0000055H", "+6590000055", "55 Learning Grove, Singapore", "Enrolled", null, null },
                    { 56, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 8, 15), null, "pranav.foo.56@example.com", "Pranav Foo", false, false, "56 Learning Grove, Singapore", "S0000056F", "+6590000056", "56 Learning Grove, Singapore", "Enrolled", null, null },
                    { 57, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 9, 15), null, "qistina.gan.57@example.com", "Qistina Gan", false, false, "57 Learning Grove, Singapore", "S0000057D", "+6590000057", "57 Learning Grove, Singapore", "Enrolled", null, null },
                    { 58, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 10, 15), null, "rafael.ho.58@example.com", "Rafael Ho", false, false, "58 Learning Grove, Singapore", "S0000058B", "+6590000058", "58 Learning Grove, Singapore", "Enrolled", null, null },
                    { 59, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 11, 15), null, "selina.ismail.59@example.com", "Selina Ismail", false, false, "59 Learning Grove, Singapore", "S0000059J", "+6590000059", "59 Learning Grove, Singapore", "Enrolled", null, null },
                    { 60, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 12, 15), null, "terence.jeyaratnam.60@example.com", "Terence Jeyaratnam", false, false, "60 Learning Grove, Singapore", "S0000060D", "+6590000060", "60 Learning Grove, Singapore", "Enrolled", null, null },
                    { 61, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 1, 15), null, "umairah.kwek.61@example.com", "Umairah Kwek", false, false, "61 Learning Grove, Singapore", "S0000061B", "+6590000061", "61 Learning Grove, Singapore", "Enrolled", null, null },
                    { 62, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 2, 15), null, "victor.lim.62@example.com", "Victor Lim", false, false, "62 Learning Grove, Singapore", "S0000062J", "+6590000062", "62 Learning Grove, Singapore", "Enrolled", null, null },
                    { 63, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 3, 15), null, "wen.jie.mohamed.63@example.com", "Wen Jie Mohamed", false, false, "63 Learning Grove, Singapore", "S0000063I", "+6590000063", "63 Learning Grove, Singapore", "Enrolled", null, null },
                    { 64, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 4, 15), null, "xavier.ng.64@example.com", "Xavier Ng", false, false, "64 Learning Grove, Singapore", "S0000064G", "+6590000064", "64 Learning Grove, Singapore", "Enrolled", null, null },
                    { 65, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 5, 15), null, "yasmin.ong.65@example.com", "Yasmin Ong", false, false, "65 Learning Grove, Singapore", "S0000065E", "+6590000065", "65 Learning Grove, Singapore", "Enrolled", null, null },
                    { 66, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 6, 15), null, "zachary.pillai.66@example.com", "Zachary Pillai", false, false, "66 Learning Grove, Singapore", "S0000066C", "+6590000066", "66 Learning Grove, Singapore", "Enrolled", null, null },
                    { 67, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 7, 15), null, "adeline.quek.67@example.com", "Adeline Quek", false, false, "67 Learning Grove, Singapore", "S0000067A", "+6590000067", "67 Learning Grove, Singapore", "Enrolled", null, null },
                    { 68, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 8, 15), null, "brandon.rao.68@example.com", "Brandon Rao", false, false, "68 Learning Grove, Singapore", "S0000068Z", "+6590000068", "68 Learning Grove, Singapore", "Enrolled", null, null },
                    { 69, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 9, 15), null, "celeste.sim.69@example.com", "Celeste Sim", false, false, "69 Learning Grove, Singapore", "S0000069H", "+6590000069", "69 Learning Grove, Singapore", "Enrolled", null, null },
                    { 70, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 10, 15), null, "damien.tan.70@example.com", "Damien Tan", false, false, "70 Learning Grove, Singapore", "S0000070A", "+6590000070", "70 Learning Grove, Singapore", "Enrolled", null, null },
                    { 71, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 11, 15), null, "evelyn.uddin.71@example.com", "Evelyn Uddin", false, false, "71 Learning Grove, Singapore", "S0000071Z", "+6590000071", "71 Learning Grove, Singapore", "Enrolled", null, null },
                    { 72, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 12, 15), null, "faris.vasquez.72@example.com", "Faris Vasquez", false, false, "72 Learning Grove, Singapore", "S0000072H", "+6590000072", "72 Learning Grove, Singapore", "Enrolled", null, null },
                    { 73, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 1, 15), null, "giselle.wong.73@example.com", "Giselle Wong", false, false, "73 Learning Grove, Singapore", "S0000073F", "+6590000073", "73 Learning Grove, Singapore", "Enrolled", null, null },
                    { 74, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 2, 15), null, "haziq.xu.74@example.com", "Haziq Xu", false, false, "74 Learning Grove, Singapore", "S0000074D", "+6590000074", "74 Learning Grove, Singapore", "Enrolled", null, null },
                    { 75, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 3, 15), null, "irene.yeo.75@example.com", "Irene Yeo", false, false, "75 Learning Grove, Singapore", "S0000075B", "+6590000075", "75 Learning Grove, Singapore", "Enrolled", null, null },
                    { 76, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 4, 15), null, "jonas.zainal.76@example.com", "Jonas Zainal", false, false, "76 Learning Grove, Singapore", "S0000076J", "+6590000076", "76 Learning Grove, Singapore", "Enrolled", null, null },
                    { 77, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 5, 15), null, "kavya.ang.77@example.com", "Kavya Ang", false, false, "77 Learning Grove, Singapore", "S0000077I", "+6590000077", "77 Learning Grove, Singapore", "Enrolled", null, null },
                    { 78, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 6, 15), null, "lydia.bala.78@example.com", "Lydia Bala", false, false, "78 Learning Grove, Singapore", "S0000078G", "+6590000078", "78 Learning Grove, Singapore", "Enrolled", null, null },
                    { 79, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 7, 15), null, "malcolm.chew.79@example.com", "Malcolm Chew", false, false, "79 Learning Grove, Singapore", "S0000079E", "+6590000079", "79 Learning Grove, Singapore", "Enrolled", null, null },
                    { 80, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 8, 15), null, "nadia.das.80@example.com", "Nadia Das", false, false, "80 Learning Grove, Singapore", "S0000080I", "+6590000080", "80 Learning Grove, Singapore", "Enrolled", null, null },
                    { 81, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 9, 15), null, "alina.foo.81@example.com", "Alina Foo", false, false, "81 Learning Grove, Singapore", "S0000081G", "+6590000081", "81 Learning Grove, Singapore", "Enrolled", null, null },
                    { 82, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 10, 15), null, "benjamin.gan.82@example.com", "Benjamin Gan", false, false, "82 Learning Grove, Singapore", "S0000082E", "+6590000082", "82 Learning Grove, Singapore", "Enrolled", null, null },
                    { 83, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 11, 15), null, "clara.ho.83@example.com", "Clara Ho", false, false, "83 Learning Grove, Singapore", "S0000083C", "+6590000083", "83 Learning Grove, Singapore", "Enrolled", null, null },
                    { 84, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 12, 15), null, "darius.ismail.84@example.com", "Darius Ismail", false, false, "84 Learning Grove, Singapore", "S0000084A", "+6590000084", "84 Learning Grove, Singapore", "Enrolled", null, null },
                    { 85, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 1, 15), null, "elena.jeyaratnam.85@example.com", "Elena Jeyaratnam", false, false, "85 Learning Grove, Singapore", "S0000085Z", "+6590000085", "85 Learning Grove, Singapore", "Enrolled", null, null },
                    { 86, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 2, 15), null, "farhan.kwek.86@example.com", "Farhan Kwek", false, false, "86 Learning Grove, Singapore", "S0000086H", "+6590000086", "86 Learning Grove, Singapore", "Enrolled", null, null },
                    { 87, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 3, 15), null, "grace.lim.87@example.com", "Grace Lim", false, false, "87 Learning Grove, Singapore", "S0000087F", "+6590000087", "87 Learning Grove, Singapore", "Enrolled", null, null },
                    { 88, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 4, 15), null, "haruto.mohamed.88@example.com", "Haruto Mohamed", false, false, "88 Learning Grove, Singapore", "S0000088D", "+6590000088", "88 Learning Grove, Singapore", "Enrolled", null, null },
                    { 89, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 5, 15), null, "isabelle.ng.89@example.com", "Isabelle Ng", false, false, "89 Learning Grove, Singapore", "S0000089B", "+6590000089", "89 Learning Grove, Singapore", "Enrolled", null, null },
                    { 90, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 15), null, "jasper.ong.90@example.com", "Jasper Ong", false, false, "90 Learning Grove, Singapore", "S0000090F", "+6590000090", "90 Learning Grove, Singapore", "Enrolled", null, null },
                    { 91, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 15), null, "keira.pillai.91@example.com", "Keira Pillai", false, false, "91 Learning Grove, Singapore", "S0000091D", "+6590000091", "91 Learning Grove, Singapore", "Enrolled", null, null },
                    { 92, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 15), null, "leon.quek.92@example.com", "Leon Quek", false, false, "92 Learning Grove, Singapore", "S0000092B", "+6590000092", "92 Learning Grove, Singapore", "Enrolled", null, null },
                    { 93, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 15), null, "mei.lin.rao.93@example.com", "Mei Lin Rao", false, false, "93 Learning Grove, Singapore", "S0000093J", "+6590000093", "93 Learning Grove, Singapore", "Enrolled", null, null },
                    { 94, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 15), null, "nathan.sim.94@example.com", "Nathan Sim", false, false, "94 Learning Grove, Singapore", "S0000094I", "+6590000094", "94 Learning Grove, Singapore", "Enrolled", null, null },
                    { 95, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 11, 15), null, "olivia.tan.95@example.com", "Olivia Tan", false, false, "95 Learning Grove, Singapore", "S0000095G", "+6590000095", "95 Learning Grove, Singapore", "Enrolled", null, null },
                    { 96, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 12, 15), null, "pranav.uddin.96@example.com", "Pranav Uddin", false, false, "96 Learning Grove, Singapore", "S0000096E", "+6590000096", "96 Learning Grove, Singapore", "Enrolled", null, null },
                    { 97, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 1, 15), null, "qistina.vasquez.97@example.com", "Qistina Vasquez", false, false, "97 Learning Grove, Singapore", "S0000097C", "+6590000097", "97 Learning Grove, Singapore", "Enrolled", null, null },
                    { 98, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 2, 15), null, "rafael.wong.98@example.com", "Rafael Wong", false, false, "98 Learning Grove, Singapore", "S0000098A", "+6590000098", "98 Learning Grove, Singapore", "Enrolled", null, null },
                    { 99, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 3, 15), null, "selina.xu.99@example.com", "Selina Xu", false, false, "99 Learning Grove, Singapore", "S0000099Z", "+6590000099", "99 Learning Grove, Singapore", "Enrolled", null, null },
                    { 100, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 4, 15), null, "terence.yeo.100@example.com", "Terence Yeo", false, false, "100 Learning Grove, Singapore", "S0000100G", "+6590000100", "100 Learning Grove, Singapore", "Enrolled", null, null },
                    { 101, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 5, 15), null, "umairah.zainal.101@example.com", "Umairah Zainal", false, false, "101 Community Crescent, Singapore", "S0000101E", "+6590000101", "101 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 102, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 6, 15), null, "victor.ang.102@example.com", "Victor Ang", false, false, "102 Community Crescent, Singapore", "S0000102C", "+6590000102", "102 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 103, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 7, 15), null, "wen.jie.bala.103@example.com", "Wen Jie Bala", false, false, "103 Community Crescent, Singapore", "S0000103A", "+6590000103", "103 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 104, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 8, 15), null, "xavier.chew.104@example.com", "Xavier Chew", false, false, "104 Community Crescent, Singapore", "S0000104Z", "+6590000104", "104 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 105, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 9, 15), null, "yasmin.das.105@example.com", "Yasmin Das", false, false, "105 Community Crescent, Singapore", "S0000105H", "+6590000105", "105 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 106, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 10, 15), null, "zachary.eng.106@example.com", "Zachary Eng", false, false, "106 Community Crescent, Singapore", "S0000106F", "+6590000106", "106 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 107, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 11, 15), null, "adeline.foo.107@example.com", "Adeline Foo", false, false, "107 Community Crescent, Singapore", "S0000107D", "+6590000107", "107 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 108, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 12, 15), null, "brandon.gan.108@example.com", "Brandon Gan", false, false, "108 Community Crescent, Singapore", "S0000108B", "+6590000108", "108 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 109, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 1, 15), null, "celeste.ho.109@example.com", "Celeste Ho", false, false, "109 Community Crescent, Singapore", "S0000109J", "+6590000109", "109 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 110, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 2, 15), null, "damien.ismail.110@example.com", "Damien Ismail", false, false, "110 Community Crescent, Singapore", "S0000110D", "+6590000110", "110 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 111, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 3, 15), null, "evelyn.jeyaratnam.111@example.com", "Evelyn Jeyaratnam", false, false, "111 Community Crescent, Singapore", "S0000111B", "+6590000111", "111 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 112, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 4, 15), null, "faris.kwek.112@example.com", "Faris Kwek", false, false, "112 Community Crescent, Singapore", "S0000112J", "+6590000112", "112 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 113, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 5, 15), null, "giselle.lim.113@example.com", "Giselle Lim", false, false, "113 Community Crescent, Singapore", "S0000113I", "+6590000113", "113 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 114, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 6, 15), null, "haziq.mohamed.114@example.com", "Haziq Mohamed", false, false, "114 Community Crescent, Singapore", "S0000114G", "+6590000114", "114 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 115, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 7, 15), null, "irene.ng.115@example.com", "Irene Ng", false, false, "115 Community Crescent, Singapore", "S0000115E", "+6590000115", "115 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 116, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 8, 15), null, "jonas.ong.116@example.com", "Jonas Ong", false, false, "116 Community Crescent, Singapore", "S0000116C", "+6590000116", "116 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 117, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 9, 15), null, "kavya.pillai.117@example.com", "Kavya Pillai", false, false, "117 Community Crescent, Singapore", "S0000117A", "+6590000117", "117 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 118, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 10, 15), null, "lydia.quek.118@example.com", "Lydia Quek", false, false, "118 Community Crescent, Singapore", "S0000118Z", "+6590000118", "118 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 119, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 11, 15), null, "malcolm.rao.119@example.com", "Malcolm Rao", false, false, "119 Community Crescent, Singapore", "S0000119H", "+6590000119", "119 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 120, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 12, 15), null, "nadia.sim.120@example.com", "Nadia Sim", false, false, "120 Community Crescent, Singapore", "S0000120A", "+6590000120", "120 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 121, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 1, 15), null, "alina.uddin.121@example.com", "Alina Uddin", false, false, "121 Community Crescent, Singapore", "S0000121Z", "+6590000121", "121 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 122, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 2, 15), null, "benjamin.vasquez.122@example.com", "Benjamin Vasquez", false, false, "122 Community Crescent, Singapore", "S0000122H", "+6590000122", "122 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 123, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 3, 15), null, "clara.wong.123@example.com", "Clara Wong", false, false, "123 Community Crescent, Singapore", "S0000123F", "+6590000123", "123 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 124, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 4, 15), null, "darius.xu.124@example.com", "Darius Xu", false, false, "124 Community Crescent, Singapore", "S0000124D", "+6590000124", "124 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 125, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 5, 15), null, "elena.yeo.125@example.com", "Elena Yeo", false, false, "125 Community Crescent, Singapore", "S0000125B", "+6590000125", "125 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 126, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 6, 15), null, "farhan.zainal.126@example.com", "Farhan Zainal", false, false, "126 Community Crescent, Singapore", "S0000126J", "+6590000126", "126 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 127, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 7, 15), null, "grace.ang.127@example.com", "Grace Ang", false, false, "127 Community Crescent, Singapore", "S0000127I", "+6590000127", "127 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 128, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 8, 15), null, "haruto.bala.128@example.com", "Haruto Bala", false, false, "128 Community Crescent, Singapore", "S0000128G", "+6590000128", "128 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 129, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 9, 15), null, "isabelle.chew.129@example.com", "Isabelle Chew", false, false, "129 Community Crescent, Singapore", "S0000129E", "+6590000129", "129 Community Crescent, Singapore", "Not Enrolled", null, null },
                    { 130, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 10, 15), null, "jasper.das.130@example.com", "Jasper Das", false, false, "130 Community Crescent, Singapore", "S0000130I", "+6590000130", "130 Community Crescent, Singapore", "Not Enrolled", null, null }
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
                    { 5, "EDU-2026-00000000001", "Sterling Quach", "S0000001I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-ONLINE-0005", false, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 120m, null, null },
                    { 6, "EDU-2026-00000000006", "Hannah Lee", "S0000006Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-ONLINE-0006", false, new DateTime(2026, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 100m, null, null },
                    { 7, "EDU-2026-00000000007", "Daniel Wong", "S0000007H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-ONLINE-0007", false, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 200m, null, null },
                    { 8, "EDU-2026-00000000008", "Sofia Chen", "S0000008F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-ONLINE-0008", false, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 130m, null, null }
                });

            migrationBuilder.InsertData(
                table: "ScheduleTopUp",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExecuteAtDay", "ExecuteAtMonth", "ExecutionTime", "Frequency", "IsDeleted", "Name", "NextExecutionAt", "OneTimeExecutionAt", "Status", "TopupAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, null, new TimeOnly(8, 0, 0), 2, false, "Monthly Transport Allowance", new DateTime(2027, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 60m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, null, new TimeOnly(8, 30, 0), 2, false, "Monthly Meal Support Credit", new DateTime(2027, 1, 3, 8, 30, 0, 0, DateTimeKind.Unspecified), null, 1, 90m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(9, 0, 0), 1, false, "Semester Materials Allowance", new DateTime(2027, 1, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), 1, 180m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 1, new TimeOnly(9, 30, 0), 3, false, "Annual Study Resource Credit", new DateTime(2027, 1, 10, 9, 30, 0, 0, DateTimeKind.Unspecified), null, 1, 240m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, null, new TimeOnly(10, 0, 0), 2, false, "Monthly Low-Balance Booster", new DateTime(2027, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 75m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(10, 30, 0), 1, false, "Course Start Support Credit", new DateTime(2027, 1, 12, 10, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 12, 10, 30, 0, 0, DateTimeKind.Unspecified), 1, 150m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, 2, new TimeOnly(11, 0, 0), 3, false, "Annual Technology Allowance", new DateTime(2027, 2, 15, 11, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 300m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, null, new TimeOnly(11, 30, 0), 2, false, "Monthly Campus Essentials Credit", new DateTime(2027, 1, 8, 11, 30, 0, 0, DateTimeKind.Unspecified), null, 1, 50m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(12, 0, 0), 1, false, "Exam Period Support Credit", new DateTime(2027, 2, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 2, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, 120m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, 3, new TimeOnly(12, 30, 0), 3, false, "Annual Enrichment Credit", new DateTime(2027, 3, 20, 12, 30, 0, 0, DateTimeKind.Unspecified), null, 1, 220m, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, null, new TimeOnly(13, 0, 0), 2, false, "Monthly Attendance Support", new DateTime(2027, 1, 11, 13, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 65m, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(13, 30, 0), 1, false, "One-Time Emergency Education Credit", new DateTime(2027, 2, 8, 13, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 2, 8, 13, 30, 0, 0, DateTimeKind.Unspecified), 1, 250m, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, 1, new TimeOnly(14, 0, 0), 3, false, "Annual Back-to-School Credit", new DateTime(2027, 1, 5, 14, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 200m, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, null, new TimeOnly(14, 30, 0), 2, false, "Monthly Skills Practice Credit", new DateTime(2027, 1, 14, 14, 30, 0, 0, DateTimeKind.Unspecified), null, 1, 85m, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(15, 0, 0), 1, false, "Workshop Materials Credit", new DateTime(2027, 2, 15, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 2, 15, 15, 0, 0, 0, DateTimeKind.Unspecified), 1, 110m, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, 6, new TimeOnly(15, 30, 0), 3, false, "Annual Progression Support", new DateTime(2027, 6, 18, 15, 30, 0, 0, DateTimeKind.Unspecified), null, 1, 275m, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, null, new TimeOnly(16, 0, 0), 2, false, "Monthly Participation Credit", new DateTime(2027, 1, 17, 16, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 55m, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(16, 30, 0), 1, false, "Graduation Preparation Credit", new DateTime(2027, 3, 1, 16, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 3, 1, 16, 30, 0, 0, DateTimeKind.Unspecified), 1, 160m, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, 4, new TimeOnly(17, 0, 0), 3, false, "Annual Household Relief Credit", new DateTime(2027, 4, 22, 17, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 320m, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, null, new TimeOnly(17, 30, 0), 2, false, "Monthly Learning Continuity Credit", new DateTime(2027, 1, 20, 17, 30, 0, 0, DateTimeKind.Unspecified), null, 1, 95m, null, null }
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
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Post-Secondary Study Support", 1, 200m, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Youth Skills Development Grant", 1, 180m, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Low-Balance Learning Credit", 1, 120m, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Transport Support Credit", 1, 80m, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Learning Materials Allowance", 1, 150m, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Digital Learning Access Grant", 1, 250m, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Exam Readiness Support", 1, 100m, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Training Pathway Credit", 1, 220m, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Student Welfare Credit", 1, 130m, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Household Support Education Credit", 1, 300m, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Continuing Education Credit", 1, 160m, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Campus Essentials Support", 1, 90m, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Apprenticeship Preparation Grant", 1, 240m, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Technical Skills Support", 1, 210m, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Academic Recovery Credit", 1, 140m, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Independent Learning Grant", 1, 170m, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Inclusive Education Support", 1, 280m, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Course Materials Top-up", 1, 110m, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Youth Employability Credit", 1, 230m, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Family Assistance Education Credit", 1, 260m, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Bridge-to-Work Learning Grant", 1, 190m, null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "School Participation Support", 1, 70m, null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Community Learning Credit", 1, 125m, null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Practical Training Allowance", 1, 215m, null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Academic Milestone Credit", 1, 175m, null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Student Resilience Grant", 2, 145m, null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Applied Learning Credit", 1, 205m, null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Education Access Credit", 1, 95m, null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Holiday Learning Support", 2, 115m, null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Progression Support Grant", 1, 275m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupExecution",
                columns: new[] { "Id", "ConditionsSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutionCode", "FailedCount", "IdempotencyKey", "IsDeleted", "ManualAmount", "ManualReason", "ScheduleTopUpId", "SourceType", "Status", "SuccessCount", "SystemTopupId", "TopupAmountSnapshot", "TopupNameSnapshot", "TotalExecutedAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, null, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, "TOPUP-SEED-MANUAL-001", 1, "manual-emergency-education-credit-2026-01", false, 100m, "Emergency education credit approved by finance team.", null, 3, 3, 1, null, null, null, 100m, 2, null, null });

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
                columns: new[] { "Id", "CourseCode", "CourseFeeAmount", "CourseName", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "EndDate", "EnrollmentDeadline", "FasApplicationDueDate", "GstAmount", "IsDeleted", "MiscFeeAmount", "SchoolId", "StartDate", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "CRS-2026-A1B2C3D", 100m, "Quantitative Problem Solving", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied numeracy and structured problem-solving for academic pathways.", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9.90m, false, 10m, 1, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 2, "CRS-2026-B2C3D4E", 115m, "Software Foundations with C#", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Core programming concepts, debugging, and application structure.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 11.43m, false, 12m, 2, new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 3, "CRS-2026-C3D4E5F", 130m, "Professional Communication Lab", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical writing, presentation, and workplace communication skills.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 13.05m, false, 15m, 3, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 4, "CRS-2026-D4E5F6G", 145m, "Sustainability Science Workshop", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Environmental systems, resource planning, and sustainability practice.", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 14.58m, false, 17m, 4, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 5, "CRS-2026-E5F6G7H", 160m, "Digital Media Production", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Digital storytelling, layout, and media production workflows.", new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 16.20m, false, 20m, 5, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 6, "CRS-2026-F6G7H8J", 175m, "Service Operations Practicum", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Customer operations, service standards, and scenario-based practice.", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 17.73m, false, 22m, 6, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 7, "CRS-2026-G7H8J9K", 190m, "Electrical Systems Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundational electrical theory, components, and safety practices.", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 19.35m, false, 25m, 7, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 8, "CRS-2026-H8J9K0L", 205m, "Creative Writing Studio", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Narrative craft, editing practice, and guided writing critique.", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 20.88m, false, 27m, 8, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 9, "CRS-2026-J9K0L1M", 220m, "Data Analytics Essentials", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Data preparation, analysis, visualization, and reporting fundamentals.", new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 22.50m, false, 30m, 9, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 10, "CRS-2026-K0L1M2N", 235m, "Office Productivity for Business", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Document, spreadsheet, and presentation workflows for business users.", new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 24.03m, false, 32m, 10, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 11, "CRS-2026-S0101XA", 130m, "Academic Writing - School 1 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 13.05m, false, 15m, 1, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 12, "CRS-2026-S0102XB", 145m, "Business Numeracy - School 1 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), 14.58m, false, 17m, 1, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 13, "CRS-2026-S0103XC", 160m, "Digital Literacy - School 1 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Utc), 16.11m, false, 19m, 1, new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 14, "CRS-2026-S0104XD", 175m, "Career Readiness - School 1 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 17.64m, false, 21m, 1, new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 15, "CRS-2026-S0105XE", 190m, "Applied Science - School 1 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 19.17m, false, 23m, 1, new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 16, "CRS-2026-S0106XF", 205m, "Financial Literacy - School 1 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 20.70m, false, 25m, 1, new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 17, "CRS-2026-S0107XG", 220m, "Project Collaboration - School 1 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 22.23m, false, 27m, 1, new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 18, "CRS-2026-S0108XH", 235m, "Data Skills - School 1 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), 23.76m, false, 29m, 1, new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 19, "CRS-2026-S0109XI", 250m, "Workplace Communication - School 1 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 25.29m, false, 31m, 1, new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 20, "CRS-2026-S0201XA", 140m, "Academic Writing - School 2 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 13.95m, false, 15m, 2, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 21, "CRS-2026-S0202XB", 155m, "Business Numeracy - School 2 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Utc), 15.48m, false, 17m, 2, new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 22, "CRS-2026-S0203XC", 170m, "Digital Literacy - School 2 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), 17.01m, false, 19m, 2, new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 23, "CRS-2026-S0204XD", 185m, "Career Readiness - School 2 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), 18.54m, false, 21m, 2, new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 24, "CRS-2026-S0205XE", 200m, "Applied Science - School 2 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), 20.07m, false, 23m, 2, new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 25, "CRS-2026-S0206XF", 215m, "Financial Literacy - School 2 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), 21.60m, false, 25m, 2, new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 26, "CRS-2026-S0207XG", 230m, "Project Collaboration - School 2 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), 23.13m, false, 27m, 2, new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 27, "CRS-2026-S0208XH", 245m, "Data Skills - School 2 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 24.66m, false, 29m, 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 28, "CRS-2026-S0209XI", 260m, "Workplace Communication - School 2 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 26.19m, false, 31m, 2, new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 29, "CRS-2026-S0301XA", 150m, "Academic Writing - School 3 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), 14.85m, false, 15m, 3, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 30, "CRS-2026-S0302XB", 165m, "Business Numeracy - School 3 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Utc), 16.38m, false, 17m, 3, new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 31, "CRS-2026-S0303XC", 180m, "Digital Literacy - School 3 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), 17.91m, false, 19m, 3, new DateTime(2026, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 32, "CRS-2026-S0304XD", 195m, "Career Readiness - School 3 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Utc), 19.44m, false, 21m, 3, new DateTime(2026, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 33, "CRS-2026-S0305XE", 210m, "Applied Science - School 3 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), 20.97m, false, 23m, 3, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 34, "CRS-2026-S0306XF", 225m, "Financial Literacy - School 3 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), 22.50m, false, 25m, 3, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 35, "CRS-2026-S0307XG", 240m, "Project Collaboration - School 3 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc), 24.03m, false, 27m, 3, new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 36, "CRS-2026-S0308XH", 255m, "Data Skills - School 3 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 25.56m, false, 29m, 3, new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 37, "CRS-2026-S0309XI", 270m, "Workplace Communication - School 3 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2027, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 27.09m, false, 31m, 3, new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 38, "CRS-2026-S0401XA", 160m, "Academic Writing - School 4 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), 15.75m, false, 15m, 4, new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 39, "CRS-2026-S0402XB", 175m, "Business Numeracy - School 4 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), 17.28m, false, 17m, 4, new DateTime(2026, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 40, "CRS-2026-S0403XC", 190m, "Digital Literacy - School 4 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc), 18.81m, false, 19m, 4, new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 41, "CRS-2026-S0404XD", 205m, "Career Readiness - School 4 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Utc), 20.34m, false, 21m, 4, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 42, "CRS-2026-S0405XE", 220m, "Applied Science - School 4 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), 21.87m, false, 23m, 4, new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 43, "CRS-2026-S0406XF", 235m, "Financial Literacy - School 4 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), 23.40m, false, 25m, 4, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 44, "CRS-2026-S0407XG", 250m, "Project Collaboration - School 4 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24.93m, false, 27m, 4, new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 45, "CRS-2026-S0408XH", 265m, "Data Skills - School 4 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), 26.46m, false, 29m, 4, new DateTime(2026, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 46, "CRS-2026-S0409XI", 280m, "Workplace Communication - School 4 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2027, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 27.99m, false, 31m, 4, new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 47, "CRS-2026-S0501XA", 170m, "Academic Writing - School 5 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), 16.65m, false, 15m, 5, new DateTime(2026, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 48, "CRS-2026-S0502XB", 185m, "Business Numeracy - School 5 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Utc), 18.18m, false, 17m, 5, new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 49, "CRS-2026-S0503XC", 200m, "Digital Literacy - School 5 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Utc), 19.71m, false, 19m, 5, new DateTime(2026, 9, 27, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 50, "CRS-2026-S0504XD", 215m, "Career Readiness - School 5 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), 21.24m, false, 21m, 5, new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 51, "CRS-2026-S0505XE", 230m, "Applied Science - School 5 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), 22.77m, false, 23m, 5, new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 52, "CRS-2026-S0506XF", 245m, "Financial Literacy - School 5 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 27, 0, 0, 0, 0, DateTimeKind.Utc), 24.30m, false, 25m, 5, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 53, "CRS-2026-S0507XG", 260m, "Project Collaboration - School 5 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 25.83m, false, 27m, 5, new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 54, "CRS-2026-S0508XH", 275m, "Data Skills - School 5 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Utc), 27.36m, false, 29m, 5, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 55, "CRS-2026-S0509XI", 290m, "Workplace Communication - School 5 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2027, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 28.89m, false, 31m, 5, new DateTime(2026, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 56, "CRS-2026-S0601XA", 180m, "Academic Writing - School 6 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), 17.55m, false, 15m, 6, new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 57, "CRS-2026-S0602XB", 195m, "Business Numeracy - School 6 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Utc), 19.08m, false, 17m, 6, new DateTime(2026, 9, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 58, "CRS-2026-S0603XC", 210m, "Digital Literacy - School 6 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), 20.61m, false, 19m, 6, new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 59, "CRS-2026-S0604XD", 225m, "Career Readiness - School 6 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), 22.14m, false, 21m, 6, new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 60, "CRS-2026-S0605XE", 240m, "Applied Science - School 6 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 23, 0, 0, 0, 0, DateTimeKind.Utc), 23.67m, false, 23m, 6, new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 61, "CRS-2026-S0606XF", 255m, "Financial Literacy - School 6 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), 25.20m, false, 25m, 6, new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 62, "CRS-2026-S0607XG", 270m, "Project Collaboration - School 6 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), 26.73m, false, 27m, 6, new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 63, "CRS-2026-S0608XH", 285m, "Data Skills - School 6 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2027, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), 28.26m, false, 29m, 6, new DateTime(2026, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 64, "CRS-2026-S0609XI", 300m, "Workplace Communication - School 6 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2027, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 29.79m, false, 31m, 6, new DateTime(2026, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 65, "CRS-2026-S0701XA", 190m, "Academic Writing - School 7 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Utc), 18.45m, false, 15m, 7, new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 66, "CRS-2026-S0702XB", 205m, "Business Numeracy - School 7 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 19.98m, false, 17m, 7, new DateTime(2026, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 67, "CRS-2026-S0703XC", 220m, "Digital Literacy - School 7 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 12, 0, 0, 0, 0, DateTimeKind.Utc), 21.51m, false, 19m, 7, new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 68, "CRS-2026-S0704XD", 235m, "Career Readiness - School 7 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Utc), 23.04m, false, 21m, 7, new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 69, "CRS-2026-S0705XE", 250m, "Applied Science - School 7 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 26, 0, 0, 0, 0, DateTimeKind.Utc), 24.57m, false, 23m, 7, new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 70, "CRS-2026-S0706XF", 265m, "Financial Literacy - School 7 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), 26.10m, false, 25m, 7, new DateTime(2026, 10, 24, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 71, "CRS-2026-S0707XG", 280m, "Project Collaboration - School 7 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), 27.63m, false, 27m, 7, new DateTime(2026, 10, 31, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 72, "CRS-2026-S0708XH", 295m, "Data Skills - School 7 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2027, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 29.16m, false, 29m, 7, new DateTime(2026, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 73, "CRS-2026-S0709XI", 310m, "Workplace Communication - School 7 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2027, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 24, 0, 0, 0, 0, DateTimeKind.Utc), 30.69m, false, 31m, 7, new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 74, "CRS-2026-S0801XA", 200m, "Academic Writing - School 8 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 19.35m, false, 15m, 8, new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 75, "CRS-2026-S0802XB", 215m, "Business Numeracy - School 8 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 20.88m, false, 17m, 8, new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 76, "CRS-2026-S0803XC", 230m, "Digital Literacy - School 8 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 22.41m, false, 19m, 8, new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 77, "CRS-2026-S0804XD", 245m, "Career Readiness - School 8 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 23.94m, false, 21m, 8, new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 78, "CRS-2026-S0805XE", 260m, "Applied Science - School 8 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), 25.47m, false, 23m, 8, new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 79, "CRS-2026-S0806XF", 275m, "Financial Literacy - School 8 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 27.00m, false, 25m, 8, new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 80, "CRS-2026-S0807XG", 290m, "Project Collaboration - School 8 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2027, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 28.53m, false, 27m, 8, new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 81, "CRS-2026-S0808XH", 305m, "Data Skills - School 8 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2027, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), 30.06m, false, 29m, 8, new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 82, "CRS-2026-S0809XI", 320m, "Workplace Communication - School 8 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2027, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), 31.59m, false, 31m, 8, new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 83, "CRS-2026-S0901XA", 210m, "Academic Writing - School 9 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), 20.25m, false, 15m, 9, new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 84, "CRS-2026-S0902XB", 225m, "Business Numeracy - School 9 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), 21.78m, false, 17m, 9, new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 85, "CRS-2026-S0903XC", 240m, "Digital Literacy - School 9 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), 23.31m, false, 19m, 9, new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 86, "CRS-2026-S0904XD", 255m, "Career Readiness - School 9 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), 24.84m, false, 21m, 9, new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 87, "CRS-2026-S0905XE", 270m, "Applied Science - School 9 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 26.37m, false, 23m, 9, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 88, "CRS-2026-S0906XF", 285m, "Financial Literacy - School 9 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 27.90m, false, 25m, 9, new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 89, "CRS-2026-S0907XG", 300m, "Project Collaboration - School 9 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2027, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), 29.43m, false, 27m, 9, new DateTime(2026, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 90, "CRS-2026-S0908XH", 315m, "Data Skills - School 9 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2027, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), 30.96m, false, 29m, 9, new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 91, "CRS-2026-S0909XI", 330m, "Workplace Communication - School 9 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2027, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), 32.49m, false, 31m, 9, new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 92, "CRS-2026-S1001XA", 220m, "Academic Writing - School 10 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2026, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Utc), 21.15m, false, 15m, 10, new DateTime(2026, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 93, "CRS-2026-S1002XB", 235m, "Business Numeracy - School 10 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), 22.68m, false, 17m, 10, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 94, "CRS-2026-S1003XC", 250m, "Digital Literacy - School 10 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), 24.21m, false, 19m, 10, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 95, "CRS-2026-S1004XD", 265m, "Career Readiness - School 10 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2026, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc), 25.74m, false, 21m, 10, new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 96, "CRS-2026-S1005XE", 280m, "Applied Science - School 10 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation training for students preparing for advanced modules.", new DateTime(2026, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 27.27m, false, 23m, 10, new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 97, "CRS-2026-S1006XF", 295m, "Financial Literacy - School 10 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Structured lessons with guided practice and applied assessments.", new DateTime(2027, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 28.80m, false, 25m, 10, new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 98, "CRS-2026-S1007XG", 310m, "Project Collaboration - School 10 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Practical workshops designed for school-based learning pathways.", new DateTime(2027, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), 30.33m, false, 27m, 10, new DateTime(2026, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 99, "CRS-2026-S1008XH", 325m, "Data Skills - School 10 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hands-on sessions focused on confidence, fluency, and real-world use.", new DateTime(2027, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 31.86m, false, 29m, 10, new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 100, "CRS-2026-S1009XI", 340m, "Workplace Communication - School 10 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Applied learning activities with clear progression checkpoints.", new DateTime(2027, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), 33.39m, false, 31m, 10, new DateTime(2026, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null }
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
                    { 7, 1, "S0000007H", "Overdue charge reconciliation requires manual handling.", 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "FasScheme",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "DurationInMonths", "IsDeleted", "IsPerComponent", "PublishedAt", "SchemeCode", "SchemeName", "SchoolId", "Status", "SubsidyType", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Support for students from lower-income households.", 6, false, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-001", "Household Income Subsidy", 1, 2, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Transport support for eligible students.", 3, false, false, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-002", "Transport Assistance", 2, 2, 2, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course and misc fee assistance.", 12, false, true, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-003", "Study Grant", 3, 2, 1, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Draft meal support programme.", 6, false, false, null, "FAS-004", "Meal Subsidy", 4, 1, 1, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Support for digital learning devices.", 9, false, false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-005", "Digital Device Grant", 5, 2, 2, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Inactive uniform support programme.", 3, false, false, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-006", "Uniform Grant", 6, 3, 2, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Support for students with special learning needs.", 12, false, true, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-007", "Special Needs Support", 7, 2, 1, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Support for textbooks and materials.", 6, false, false, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-008", "Learning Materials Grant", 8, 2, 1, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Short term emergency support.", 3, false, false, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-009", "Emergency Financial Aid", 9, 2, 2, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Support for STEM related courses.", 12, false, false, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-010", "STEM Programme Grant", 10, 2, 1, null, null }
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
                values: new object[] { 2, "{\"logicalOperator\":\"And\",\"conditions\":[{\"field\":\"SchoolingStatus\",\"operator\":\"Equals\",\"valueText\":\"Enrolled\"},{\"field\":\"Age\",\"operator\":\"Between\",\"valueNumber\":16,\"valueNumberTo\":25}]}", new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "TOPUP-SEED-SYSTEM-001", 0, "post-secondary-study-support-2026-01", false, null, null, null, 1, 3, 1, 21, 200m, "Post-Secondary Study Support", 200m, 1, null, null });

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
                values: new object[] { 1, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 1, "Account paused after repeated failed sign-in verification.", 5 });

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
                    { 2, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, 6, 2, 1, "Education account extended because unpaid charges remain." }
                });

            migrationBuilder.InsertData(
                table: "EducationCreditTransaction",
                columns: new[] { "Id", "Amount", "BalanceAfter", "BalanceBefore", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "Direction", "EducationAccountId", "IsDeleted", "TransactionCode", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 100m, 100m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Monthly Transport Allowance credited to the account.", 1, 1, false, new Guid("00000000-0000-0000-0000-000000000001"), 1, null, null },
                    { 2, 100m, 1200m, 1100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Monthly Meal Support Credit credited to the account.", 1, 2, false, new Guid("00000000-0000-0000-0000-000000000002"), 1, null, null },
                    { 3, 200m, 1300m, 1100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Post-Secondary Study Support credited to the account.", 1, 3, false, new Guid("00000000-0000-0000-0000-000000000003"), 1, null, null },
                    { 4, 30m, 0m, 30m, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Education account balance expired at age 31.", 2, 1, false, new Guid("00000000-0000-0000-0000-000000000004"), 4, null, null },
                    { 5, 70m, 1130m, 1200m, new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment for Software Foundations with C#.", 2, 2, false, new Guid("00000000-0000-0000-0000-000000000005"), 2, null, null },
                    { 6, 140m, 1160m, 1300m, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment for Professional Communication Lab.", 2, 3, false, new Guid("00000000-0000-0000-0000-000000000006"), 2, null, null },
                    { 7, 50m, 450m, 500m, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "March overdue charge adjustment.", 2, 6, false, new Guid("00000000-0000-0000-0000-000000000007"), 3, null, null },
                    { 8, 70m, 0m, 70m, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "April overdue charge adjustment.", 2, 8, false, new Guid("00000000-0000-0000-0000-000000000008"), 3, null, null },
                    { 9, 180m, 1220m, 1400m, new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment for Sustainability Science Workshop.", 2, 4, false, new Guid("00000000-0000-0000-0000-000000000009"), 2, null, null },
                    { 10, 180m, 1320m, 1500m, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment for Digital Media Production.", 2, 5, false, new Guid("00000000-0000-0000-0000-000000000010"), 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeConditionGroup",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "FasSchemeId", "IsDeleted", "LogicalOperator", "ParentGroupId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 1, null, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 1, null, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 3, false, 1, null, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, 4, false, 1, null, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, 5, false, 1, null, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 6, false, 1, null, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, 7, false, 1, null, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 8, false, 1, null, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, 9, false, 1, null, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 10, false, 1, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeCourse",
                columns: new[] { "Id", "CourseId", "CreatedAt", "CreatedBy", "DeletedAt", "FasSchemeId", "IsDeleted", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 5, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 6, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 7, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 8, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 9, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 11, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 12, 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 13, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 14, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 15, 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 16, 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 17, 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 18, 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 19, 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 20, 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 21, 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 22, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 23, 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 24, 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 25, 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 26, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 27, 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 28, 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 29, 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 30, 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 31, 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 32, 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 33, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 34, 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 35, 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 36, 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 37, 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 38, 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 39, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 40, 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 41, 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 42, 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 43, 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 44, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 45, 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 46, 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 47, 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 48, 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 49, 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 50, 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 51, 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 52, 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 53, 53, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 54, 54, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 55, 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 56, 56, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 57, 57, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 58, 58, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 59, 59, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 60, 60, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 61, 61, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 62, 62, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 63, 63, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 64, 64, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 65, 65, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 66, 66, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 67, 67, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 68, 68, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 69, 69, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 70, 70, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 71, 71, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 72, 72, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 73, 73, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 74, 74, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 75, 75, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 76, 76, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 77, 77, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 78, 78, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 79, 79, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 80, 80, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 81, 81, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 82, 82, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 83, 83, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 84, 84, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 85, 85, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 86, 86, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 87, 87, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 88, 88, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 89, 89, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 90, 90, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 91, 91, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 92, 92, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 93, 93, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 94, 94, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 95, 95, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 96, 96, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 97, 97, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 98, 98, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 99, 99, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 100, 100, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeRequiredDocument",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "DocumentName", "FasSchemeId", "IsDeleted", "TemplateFileKey", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 1, false, "fas/templates/document-1.pdf", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 2, false, "fas/templates/document-2.pdf", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 3, false, "fas/templates/document-3.pdf", null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 4, false, "fas/templates/document-4.pdf", null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 5, false, "fas/templates/document-5.pdf", null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 6, false, "fas/templates/document-6.pdf", null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 7, false, "fas/templates/document-7.pdf", null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 8, false, "fas/templates/document-8.pdf", null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 9, false, "fas/templates/document-9.pdf", null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 10, false, "fas/templates/document-10.pdf", null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeTier",
                columns: new[] { "Id", "CourseFeeSubsidyValue", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "FasSchemeId", "IsDeleted", "MaxPerCapitaIncome", "MiscFeeSubsidyValue", "SubsidyValue", "TierName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 750m, null, 75m, "Tier 1", null, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 900m, null, 300m, "Tier 1", null, null },
                    { 3, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 3, false, 690m, 50m, null, "Tier 1", null, null },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 4, false, 800m, null, 50m, "Tier 1", null, null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 5, false, 1000m, null, 500m, "Tier 1", null, null },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, false, 750m, null, 200m, "Tier 1", null, null },
                    { 7, 75m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 7, false, 1500m, 75m, null, "Tier 1", null, null },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 8, false, 850m, null, 40m, "Tier 1", null, null },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 9, false, 1200m, null, 250m, "Tier 1", null, null },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 10, false, 1000m, null, 60m, "Tier 1", null, null }
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
                values: new object[] { 2, "EDU-2026-00000000002", 100m, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, 2, null, "Account holder verification was pending.", false, null, 4, 1, null, null });

            migrationBuilder.InsertData(
                table: "Enrollment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenEmailSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CitizenPhoneNumberSnapshot", "CourseDescriptionSnapshot", "CourseId", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "SchoolNameSnapshot", "SchoolStudentId", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-00000000001", "sterling.quach@example.com", "Sterling Quach", "S0000001I", "+6590000001", "Applied numeracy and structured problem-solving for academic pathways.", 1, "Quantitative Problem Solving", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 1, 1, null, null },
                    { 2, "EDU-2026-00000000002", "amelia.tan@example.com", "Amelia Tan", "S0000002G", "+6590000002", "Core programming concepts, debugging, and application structure.", 2, "Software Foundations with C#", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 2, 1, null, null },
                    { 3, "EDU-2026-00000000003", "marcus.lim@example.com", "Marcus Lim", "S0000003E", "+6590000003", "Practical writing, presentation, and workplace communication skills.", 3, "Professional Communication Lab", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 3, 1, null, null },
                    { 4, "EDU-2026-00000000004", "priya.nair@example.com", "Priya Nair", "S0000004C", "+6590000004", "Environmental systems, resource planning, and sustainability practice.", 4, "Sustainability Science Workshop", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 4, 1, null, null },
                    { 5, "EDU-2026-00000000005", "ethan.koh@example.com", "Ethan Koh", "S0000005A", "+6590000005", "Digital storytelling, layout, and media production workflows.", 5, "Digital Media Production", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 5, 1, null, null },
                    { 6, "EDU-2026-00000000006", "hannah.lee@example.com", "Hannah Lee", "S0000006Z", "+6590000006", "Customer operations, service standards, and scenario-based practice.", 6, "Service Operations Practicum", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 6, 1, null, null },
                    { 7, "EDU-2026-00000000007", "daniel.wong@example.com", "Daniel Wong", "S0000007H", "+6590000007", "Foundational electrical theory, components, and safety practices.", 7, "Electrical Systems Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 7, 1, null, null },
                    { 8, "EDU-2026-00000000008", "sofia.chen@example.com", "Sofia Chen", "S0000008F", "+6590000008", "Narrative craft, editing practice, and guided writing critique.", 8, "Creative Writing Studio", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 8, 1, null, null },
                    { 9, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Data preparation, analysis, visualization, and reporting fundamentals.", 9, "Data Analytics Essentials", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 9, 1, null, null },
                    { 10, "EDU-2026-00000000010", "maya.rahman@example.com", "Maya Rahman", "S0000010H", "+6590000010", "Document, spreadsheet, and presentation workflows for business users.", 10, "Office Productivity for Business", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 10, 1, null, null },
                    { 11, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Applied numeracy and structured problem-solving for academic pathways.", 1, "Quantitative Problem Solving", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 9, 1, null, null },
                    { 12, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Environmental systems, resource planning, and sustainability practice.", 4, "Sustainability Science Workshop", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 9, 1, null, null },
                    { 13, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Core programming concepts, debugging, and application structure.", 2, "Software Foundations with C#", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 9, 1, null, null },
                    { 14, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Practical writing, presentation, and workplace communication skills.", 3, "Professional Communication Lab", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 9, 1, null, null },
                    { 15, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Digital storytelling, layout, and media production workflows.", 5, "Digital Media Production", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 9, 1, null, null },
                    { 16, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Customer operations, service standards, and scenario-based practice.", 6, "Service Operations Practicum", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 9, 1, null, null },
                    { 17, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Foundational electrical theory, components, and safety practices.", 7, "Electrical Systems Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 9, 1, null, null },
                    { 18, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Narrative craft, editing practice, and guided writing critique.", 8, "Creative Writing Studio", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 9, 1, null, null },
                    { 19, "EDU-2026-00000000009", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Document, spreadsheet, and presentation workflows for business users.", 10, "Office Productivity for Business", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 9, 1, null, null },
                    { 20, "EDU-2026-00000000011", "noah.teo.11@example.com", "Noah Teo", "S0000011F", "+6590000011", "Structured lessons with guided practice and applied assessments.", 11, "Academic Writing - School 1 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 11, 1, null, null },
                    { 21, "EDU-2026-00000000012", "aisha.fernandez.12@example.com", "Aisha Fernandez", "S0000012D", "+6590000012", "Structured lessons with guided practice and applied assessments.", 12, "Business Numeracy - School 1 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 12, 1, null, null },
                    { 22, "EDU-2026-00000000013", "ryan.chua.13@example.com", "Ryan Chua", "S0000013B", "+6590000013", "Structured lessons with guided practice and applied assessments.", 13, "Digital Literacy - School 1 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 13, 1, null, null },
                    { 23, "EDU-2026-00000000014", "chloe.goh.14@example.com", "Chloe Goh", "S0000014J", "+6590000014", "Structured lessons with guided practice and applied assessments.", 14, "Career Readiness - School 1 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 14, 1, null, null },
                    { 24, "EDU-2026-00000000015", "irfan.hassan.15@example.com", "Irfan Hassan", "S0000015I", "+6590000015", "Structured lessons with guided practice and applied assessments.", 15, "Applied Science - School 1 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 15, 1, null, null },
                    { 25, "EDU-2026-00000000016", "natalie.seah.16@example.com", "Natalie Seah", "S0000016G", "+6590000016", "Structured lessons with guided practice and applied assessments.", 16, "Financial Literacy - School 1 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 16, 1, null, null },
                    { 26, "EDU-2026-00000000017", "qistina.rao.17@example.com", "Qistina Rao", "S0000017E", "+6590000017", "Structured lessons with guided practice and applied assessments.", 17, "Project Collaboration - School 1 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 17, 1, null, null },
                    { 27, "EDU-2026-00000000018", "rafael.sim.18@example.com", "Rafael Sim", "S0000018C", "+6590000018", "Structured lessons with guided practice and applied assessments.", 18, "Data Skills - School 1 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 18, 1, null, null },
                    { 28, "EDU-2026-00000000019", "selina.tan.19@example.com", "Selina Tan", "S0000019A", "+6590000019", "Structured lessons with guided practice and applied assessments.", 19, "Workplace Communication - School 1 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 19, 1, null, null },
                    { 29, "EDU-2026-00000000020", "terence.uddin.20@example.com", "Terence Uddin", "S0000020E", "+6590000020", "Structured lessons with guided practice and applied assessments.", 20, "Academic Writing - School 2 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 20, 1, null, null },
                    { 30, "EDU-2026-00000000021", "umairah.vasquez.21@example.com", "Umairah Vasquez", "S0000021C", "+6590000021", "Structured lessons with guided practice and applied assessments.", 21, "Business Numeracy - School 2 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 21, 1, null, null },
                    { 31, "EDU-2026-00000000022", "victor.wong.22@example.com", "Victor Wong", "S0000022A", "+6590000022", "Structured lessons with guided practice and applied assessments.", 22, "Digital Literacy - School 2 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 22, 1, null, null },
                    { 32, "EDU-2026-00000000023", "wen.jie.xu.23@example.com", "Wen Jie Xu", "S0000023Z", "+6590000023", "Structured lessons with guided practice and applied assessments.", 23, "Career Readiness - School 2 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 23, 1, null, null },
                    { 33, "EDU-2026-00000000024", "xavier.yeo.24@example.com", "Xavier Yeo", "S0000024H", "+6590000024", "Structured lessons with guided practice and applied assessments.", 24, "Applied Science - School 2 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 24, 1, null, null },
                    { 34, "EDU-2026-00000000025", "yasmin.zainal.25@example.com", "Yasmin Zainal", "S0000025F", "+6590000025", "Structured lessons with guided practice and applied assessments.", 25, "Financial Literacy - School 2 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 25, 1, null, null },
                    { 35, "EDU-2026-00000000026", "zachary.ang.26@example.com", "Zachary Ang", "S0000026D", "+6590000026", "Structured lessons with guided practice and applied assessments.", 26, "Project Collaboration - School 2 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 26, 1, null, null },
                    { 36, "EDU-2026-00000000027", "adeline.bala.27@example.com", "Adeline Bala", "S0000027B", "+6590000027", "Structured lessons with guided practice and applied assessments.", 27, "Data Skills - School 2 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 27, 1, null, null },
                    { 37, "EDU-2026-00000000028", "brandon.chew.28@example.com", "Brandon Chew", "S0000028J", "+6590000028", "Structured lessons with guided practice and applied assessments.", 28, "Workplace Communication - School 2 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 28, 1, null, null },
                    { 38, "EDU-2026-00000000029", "celeste.das.29@example.com", "Celeste Das", "S0000029I", "+6590000029", "Structured lessons with guided practice and applied assessments.", 29, "Academic Writing - School 3 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 29, 1, null, null },
                    { 39, "EDU-2026-00000000030", "damien.eng.30@example.com", "Damien Eng", "S0000030B", "+6590000030", "Structured lessons with guided practice and applied assessments.", 30, "Business Numeracy - School 3 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 30, 1, null, null },
                    { 40, "EDU-2026-00000000031", "evelyn.foo.31@example.com", "Evelyn Foo", "S0000031J", "+6590000031", "Structured lessons with guided practice and applied assessments.", 31, "Digital Literacy - School 3 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 31, 1, null, null },
                    { 41, "EDU-2026-00000000032", "faris.gan.32@example.com", "Faris Gan", "S0000032I", "+6590000032", "Structured lessons with guided practice and applied assessments.", 32, "Career Readiness - School 3 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 32, 1, null, null },
                    { 42, "EDU-2026-00000000033", "giselle.ho.33@example.com", "Giselle Ho", "S0000033G", "+6590000033", "Structured lessons with guided practice and applied assessments.", 33, "Applied Science - School 3 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 33, 1, null, null },
                    { 43, "EDU-2026-00000000034", "haziq.ismail.34@example.com", "Haziq Ismail", "S0000034E", "+6590000034", "Structured lessons with guided practice and applied assessments.", 34, "Financial Literacy - School 3 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 34, 1, null, null },
                    { 44, "EDU-2026-00000000035", "irene.jeyaratnam.35@example.com", "Irene Jeyaratnam", "S0000035C", "+6590000035", "Structured lessons with guided practice and applied assessments.", 35, "Project Collaboration - School 3 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 35, 1, null, null },
                    { 45, "EDU-2026-00000000036", "jonas.kwek.36@example.com", "Jonas Kwek", "S0000036A", "+6590000036", "Structured lessons with guided practice and applied assessments.", 36, "Data Skills - School 3 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 36, 1, null, null },
                    { 46, "EDU-2026-00000000037", "kavya.lim.37@example.com", "Kavya Lim", "S0000037Z", "+6590000037", "Structured lessons with guided practice and applied assessments.", 37, "Workplace Communication - School 3 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 37, 1, null, null },
                    { 47, "EDU-2026-00000000038", "lydia.mohamed.38@example.com", "Lydia Mohamed", "S0000038H", "+6590000038", "Structured lessons with guided practice and applied assessments.", 38, "Academic Writing - School 4 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 38, 1, null, null },
                    { 48, "EDU-2026-00000000039", "malcolm.ng.39@example.com", "Malcolm Ng", "S0000039F", "+6590000039", "Structured lessons with guided practice and applied assessments.", 39, "Business Numeracy - School 4 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 39, 1, null, null },
                    { 49, "EDU-2026-00000000040", "nadia.ong.40@example.com", "Nadia Ong", "S0000040Z", "+6590000040", "Structured lessons with guided practice and applied assessments.", 40, "Digital Literacy - School 4 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 40, 1, null, null },
                    { 50, "EDU-2026-00000000041", "alina.quek.41@example.com", "Alina Quek", "S0000041H", "+6590000041", "Structured lessons with guided practice and applied assessments.", 41, "Career Readiness - School 4 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 41, 1, null, null },
                    { 51, "EDU-2026-00000000042", "benjamin.rao.42@example.com", "Benjamin Rao", "S0000042F", "+6590000042", "Structured lessons with guided practice and applied assessments.", 42, "Applied Science - School 4 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 42, 1, null, null },
                    { 52, "EDU-2026-00000000043", "clara.sim.43@example.com", "Clara Sim", "S0000043D", "+6590000043", "Structured lessons with guided practice and applied assessments.", 43, "Financial Literacy - School 4 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 43, 1, null, null },
                    { 53, "EDU-2026-00000000044", "darius.tan.44@example.com", "Darius Tan", "S0000044B", "+6590000044", "Structured lessons with guided practice and applied assessments.", 44, "Project Collaboration - School 4 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 44, 1, null, null },
                    { 54, "EDU-2026-00000000045", "elena.uddin.45@example.com", "Elena Uddin", "S0000045J", "+6590000045", "Structured lessons with guided practice and applied assessments.", 45, "Data Skills - School 4 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 45, 1, null, null },
                    { 55, "EDU-2026-00000000046", "farhan.vasquez.46@example.com", "Farhan Vasquez", "S0000046I", "+6590000046", "Structured lessons with guided practice and applied assessments.", 46, "Workplace Communication - School 4 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 46, 1, null, null },
                    { 56, "EDU-2026-00000000047", "grace.wong.47@example.com", "Grace Wong", "S0000047G", "+6590000047", "Structured lessons with guided practice and applied assessments.", 47, "Academic Writing - School 5 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 47, 1, null, null },
                    { 57, "EDU-2026-00000000048", "haruto.xu.48@example.com", "Haruto Xu", "S0000048E", "+6590000048", "Structured lessons with guided practice and applied assessments.", 48, "Business Numeracy - School 5 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 48, 1, null, null },
                    { 58, "EDU-2026-00000000049", "isabelle.yeo.49@example.com", "Isabelle Yeo", "S0000049C", "+6590000049", "Structured lessons with guided practice and applied assessments.", 49, "Digital Literacy - School 5 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 49, 1, null, null },
                    { 59, "EDU-2026-00000000050", "jasper.zainal.50@example.com", "Jasper Zainal", "S0000050G", "+6590000050", "Structured lessons with guided practice and applied assessments.", 50, "Career Readiness - School 5 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 50, 1, null, null },
                    { 60, "EDU-2026-00000000051", "keira.ang.51@example.com", "Keira Ang", "S0000051E", "+6590000051", "Structured lessons with guided practice and applied assessments.", 51, "Applied Science - School 5 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 51, 1, null, null },
                    { 61, "EDU-2026-00000000052", "leon.bala.52@example.com", "Leon Bala", "S0000052C", "+6590000052", "Structured lessons with guided practice and applied assessments.", 52, "Financial Literacy - School 5 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 52, 1, null, null },
                    { 62, "EDU-2026-00000000053", "mei.lin.chew.53@example.com", "Mei Lin Chew", "S0000053A", "+6590000053", "Structured lessons with guided practice and applied assessments.", 53, "Project Collaboration - School 5 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 53, 1, null, null },
                    { 63, "EDU-2026-00000000054", "nathan.das.54@example.com", "Nathan Das", "S0000054Z", "+6590000054", "Structured lessons with guided practice and applied assessments.", 54, "Data Skills - School 5 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 54, 1, null, null },
                    { 64, "EDU-2026-00000000055", "olivia.eng.55@example.com", "Olivia Eng", "S0000055H", "+6590000055", "Structured lessons with guided practice and applied assessments.", 55, "Workplace Communication - School 5 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 55, 1, null, null },
                    { 65, "EDU-2026-00000000056", "pranav.foo.56@example.com", "Pranav Foo", "S0000056F", "+6590000056", "Structured lessons with guided practice and applied assessments.", 56, "Academic Writing - School 6 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 56, 1, null, null },
                    { 66, "EDU-2026-00000000057", "qistina.gan.57@example.com", "Qistina Gan", "S0000057D", "+6590000057", "Structured lessons with guided practice and applied assessments.", 57, "Business Numeracy - School 6 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 57, 1, null, null },
                    { 67, "EDU-2026-00000000058", "rafael.ho.58@example.com", "Rafael Ho", "S0000058B", "+6590000058", "Structured lessons with guided practice and applied assessments.", 58, "Digital Literacy - School 6 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 58, 1, null, null },
                    { 68, "EDU-2026-00000000059", "selina.ismail.59@example.com", "Selina Ismail", "S0000059J", "+6590000059", "Structured lessons with guided practice and applied assessments.", 59, "Career Readiness - School 6 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 59, 1, null, null },
                    { 69, "EDU-2026-00000000060", "terence.jeyaratnam.60@example.com", "Terence Jeyaratnam", "S0000060D", "+6590000060", "Structured lessons with guided practice and applied assessments.", 60, "Applied Science - School 6 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 60, 1, null, null },
                    { 70, "EDU-2026-00000000061", "umairah.kwek.61@example.com", "Umairah Kwek", "S0000061B", "+6590000061", "Structured lessons with guided practice and applied assessments.", 61, "Financial Literacy - School 6 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 61, 1, null, null },
                    { 71, "EDU-2026-00000000062", "victor.lim.62@example.com", "Victor Lim", "S0000062J", "+6590000062", "Structured lessons with guided practice and applied assessments.", 62, "Project Collaboration - School 6 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 62, 1, null, null },
                    { 72, "EDU-2026-00000000063", "wen.jie.mohamed.63@example.com", "Wen Jie Mohamed", "S0000063I", "+6590000063", "Structured lessons with guided practice and applied assessments.", 63, "Data Skills - School 6 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 63, 1, null, null },
                    { 73, "EDU-2026-00000000064", "xavier.ng.64@example.com", "Xavier Ng", "S0000064G", "+6590000064", "Structured lessons with guided practice and applied assessments.", 64, "Workplace Communication - School 6 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 64, 1, null, null },
                    { 74, "EDU-2026-00000000065", "yasmin.ong.65@example.com", "Yasmin Ong", "S0000065E", "+6590000065", "Structured lessons with guided practice and applied assessments.", 65, "Academic Writing - School 7 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 65, 1, null, null },
                    { 75, "EDU-2026-00000000066", "zachary.pillai.66@example.com", "Zachary Pillai", "S0000066C", "+6590000066", "Structured lessons with guided practice and applied assessments.", 66, "Business Numeracy - School 7 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 66, 1, null, null },
                    { 76, "EDU-2026-00000000067", "adeline.quek.67@example.com", "Adeline Quek", "S0000067A", "+6590000067", "Structured lessons with guided practice and applied assessments.", 67, "Digital Literacy - School 7 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 67, 1, null, null },
                    { 77, "EDU-2026-00000000068", "brandon.rao.68@example.com", "Brandon Rao", "S0000068Z", "+6590000068", "Structured lessons with guided practice and applied assessments.", 68, "Career Readiness - School 7 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 68, 1, null, null },
                    { 78, "EDU-2026-00000000069", "celeste.sim.69@example.com", "Celeste Sim", "S0000069H", "+6590000069", "Structured lessons with guided practice and applied assessments.", 69, "Applied Science - School 7 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 69, 1, null, null },
                    { 79, "EDU-2026-00000000070", "damien.tan.70@example.com", "Damien Tan", "S0000070A", "+6590000070", "Structured lessons with guided practice and applied assessments.", 70, "Financial Literacy - School 7 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 70, 1, null, null },
                    { 80, "EDU-2026-00000000071", "evelyn.uddin.71@example.com", "Evelyn Uddin", "S0000071Z", "+6590000071", "Structured lessons with guided practice and applied assessments.", 71, "Project Collaboration - School 7 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 71, 1, null, null },
                    { 81, "EDU-2026-00000000072", "faris.vasquez.72@example.com", "Faris Vasquez", "S0000072H", "+6590000072", "Structured lessons with guided practice and applied assessments.", 72, "Data Skills - School 7 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 72, 1, null, null },
                    { 82, "EDU-2026-00000000073", "giselle.wong.73@example.com", "Giselle Wong", "S0000073F", "+6590000073", "Structured lessons with guided practice and applied assessments.", 73, "Workplace Communication - School 7 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 73, 1, null, null },
                    { 83, "EDU-2026-00000000074", "haziq.xu.74@example.com", "Haziq Xu", "S0000074D", "+6590000074", "Structured lessons with guided practice and applied assessments.", 74, "Academic Writing - School 8 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 74, 1, null, null },
                    { 84, "EDU-2026-00000000075", "irene.yeo.75@example.com", "Irene Yeo", "S0000075B", "+6590000075", "Structured lessons with guided practice and applied assessments.", 75, "Business Numeracy - School 8 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 75, 1, null, null },
                    { 85, "EDU-2026-00000000076", "jonas.zainal.76@example.com", "Jonas Zainal", "S0000076J", "+6590000076", "Structured lessons with guided practice and applied assessments.", 76, "Digital Literacy - School 8 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 76, 1, null, null },
                    { 86, "EDU-2026-00000000077", "kavya.ang.77@example.com", "Kavya Ang", "S0000077I", "+6590000077", "Structured lessons with guided practice and applied assessments.", 77, "Career Readiness - School 8 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 77, 1, null, null },
                    { 87, "EDU-2026-00000000078", "lydia.bala.78@example.com", "Lydia Bala", "S0000078G", "+6590000078", "Structured lessons with guided practice and applied assessments.", 78, "Applied Science - School 8 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 78, 1, null, null },
                    { 88, "EDU-2026-00000000079", "malcolm.chew.79@example.com", "Malcolm Chew", "S0000079E", "+6590000079", "Structured lessons with guided practice and applied assessments.", 79, "Financial Literacy - School 8 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 79, 1, null, null },
                    { 89, "EDU-2026-00000000080", "nadia.das.80@example.com", "Nadia Das", "S0000080I", "+6590000080", "Structured lessons with guided practice and applied assessments.", 80, "Project Collaboration - School 8 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 80, 1, null, null },
                    { 90, "EDU-2026-00000000081", "alina.foo.81@example.com", "Alina Foo", "S0000081G", "+6590000081", "Structured lessons with guided practice and applied assessments.", 81, "Data Skills - School 8 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 81, 1, null, null },
                    { 91, "EDU-2026-00000000082", "benjamin.gan.82@example.com", "Benjamin Gan", "S0000082E", "+6590000082", "Structured lessons with guided practice and applied assessments.", 82, "Workplace Communication - School 8 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 82, 1, null, null },
                    { 92, "EDU-2026-00000000083", "clara.ho.83@example.com", "Clara Ho", "S0000083C", "+6590000083", "Structured lessons with guided practice and applied assessments.", 83, "Academic Writing - School 9 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 83, 1, null, null },
                    { 93, "EDU-2026-00000000084", "darius.ismail.84@example.com", "Darius Ismail", "S0000084A", "+6590000084", "Structured lessons with guided practice and applied assessments.", 84, "Business Numeracy - School 9 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 84, 1, null, null },
                    { 94, "EDU-2026-00000000085", "elena.jeyaratnam.85@example.com", "Elena Jeyaratnam", "S0000085Z", "+6590000085", "Structured lessons with guided practice and applied assessments.", 85, "Digital Literacy - School 9 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 85, 1, null, null },
                    { 95, "EDU-2026-00000000086", "farhan.kwek.86@example.com", "Farhan Kwek", "S0000086H", "+6590000086", "Structured lessons with guided practice and applied assessments.", 86, "Career Readiness - School 9 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 86, 1, null, null },
                    { 96, "EDU-2026-00000000087", "grace.lim.87@example.com", "Grace Lim", "S0000087F", "+6590000087", "Structured lessons with guided practice and applied assessments.", 87, "Applied Science - School 9 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 87, 1, null, null },
                    { 97, "EDU-2026-00000000088", "haruto.mohamed.88@example.com", "Haruto Mohamed", "S0000088D", "+6590000088", "Structured lessons with guided practice and applied assessments.", 88, "Financial Literacy - School 9 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 88, 1, null, null },
                    { 98, "EDU-2026-00000000089", "isabelle.ng.89@example.com", "Isabelle Ng", "S0000089B", "+6590000089", "Structured lessons with guided practice and applied assessments.", 89, "Project Collaboration - School 9 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 89, 1, null, null },
                    { 99, "EDU-2026-00000000090", "jasper.ong.90@example.com", "Jasper Ong", "S0000090F", "+6590000090", "Structured lessons with guided practice and applied assessments.", 90, "Data Skills - School 9 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 90, 1, null, null },
                    { 100, "EDU-2026-00000000091", "keira.pillai.91@example.com", "Keira Pillai", "S0000091D", "+6590000091", "Structured lessons with guided practice and applied assessments.", 91, "Workplace Communication - School 9 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 91, 1, null, null },
                    { 101, "EDU-2026-00000000092", "leon.quek.92@example.com", "Leon Quek", "S0000092B", "+6590000092", "Structured lessons with guided practice and applied assessments.", 92, "Academic Writing - School 10 Cohort 1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 92, 1, null, null },
                    { 102, "EDU-2026-00000000093", "mei.lin.rao.93@example.com", "Mei Lin Rao", "S0000093J", "+6590000093", "Structured lessons with guided practice and applied assessments.", 93, "Business Numeracy - School 10 Cohort 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 93, 1, null, null },
                    { 103, "EDU-2026-00000000094", "nathan.sim.94@example.com", "Nathan Sim", "S0000094I", "+6590000094", "Structured lessons with guided practice and applied assessments.", 94, "Digital Literacy - School 10 Cohort 3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 94, 1, null, null },
                    { 104, "EDU-2026-00000000095", "olivia.tan.95@example.com", "Olivia Tan", "S0000095G", "+6590000095", "Structured lessons with guided practice and applied assessments.", 95, "Career Readiness - School 10 Cohort 4", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 95, 1, null, null },
                    { 105, "EDU-2026-00000000096", "pranav.uddin.96@example.com", "Pranav Uddin", "S0000096E", "+6590000096", "Structured lessons with guided practice and applied assessments.", 96, "Applied Science - School 10 Cohort 5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 96, 1, null, null },
                    { 106, "EDU-2026-00000000097", "qistina.vasquez.97@example.com", "Qistina Vasquez", "S0000097C", "+6590000097", "Structured lessons with guided practice and applied assessments.", 97, "Financial Literacy - School 10 Cohort 6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 97, 1, null, null },
                    { 107, "EDU-2026-00000000098", "rafael.wong.98@example.com", "Rafael Wong", "S0000098A", "+6590000098", "Structured lessons with guided practice and applied assessments.", 98, "Project Collaboration - School 10 Cohort 7", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 98, 1, null, null },
                    { 108, "EDU-2026-00000000099", "selina.xu.99@example.com", "Selina Xu", "S0000099Z", "+6590000099", "Structured lessons with guided practice and applied assessments.", 99, "Data Skills - School 10 Cohort 8", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 99, 1, null, null },
                    { 109, "EDU-2026-00000000100", "terence.yeo.100@example.com", "Terence Yeo", "S0000100G", "+6590000100", "Structured lessons with guided practice and applied assessments.", 100, "Workplace Communication - School 10 Cohort 9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 100, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasApplication",
                columns: new[] { "Id", "ApplicationNumber", "ApprovedAt", "ApprovedByUserId", "ApprovedTierId", "CreatedAt", "CreatedBy", "DeletedAt", "DurationInMonthsSnapshot", "FasSchemeId", "FatherNationalitySnapshot", "GrossHouseholdIncomeSnapshot", "HouseholdMemberCountSnapshot", "IsDeleted", "MotherNationalitySnapshot", "PerCapitaIncomeSnapshot", "RecommendationReason", "RecommendedTierId", "RejectionReason", "SchoolStudentId", "Status", "StudentAgeSnapshot", "StudentNationalitySnapshot", "UpdatedAt", "UpdatedBy", "ValidityEndDate", "ValidityStartDate", "WithdrawnAt" },
                values: new object[,]
                {
                    { 1, "FASAPP-0001", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 1, "Singapore", 2500m, 4, false, "Singapore", 625m, "PCI <= 750", 1, null, 1, 2, 18, "Singapore", null, null, new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 2, "FASAPP-0002", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 2, "Malaysia", 3000m, 4, false, "Malaysia", 750m, "GHI <= 3500", 2, null, 2, 2, 17, "Singapore", null, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 3, "FASAPP-0003", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 3, "Singapore", 2200m, 5, false, "Singapore", 440m, "Singapore citizen and PCI <= 690", 3, null, 3, 2, 19, "Singapore", null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 4, "FASAPP-0004", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 4, "Indonesia", 3600m, 4, false, "Indonesia", 900m, "Pending admin review", 4, null, 4, 1, 20, "Singapore", null, null, null, null, null },
                    { 5, "FASAPP-0005", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 5, "Vietnam", 7000m, 4, false, "Vietnam", 1750m, "No tier matched", 5, "Income exceeds supported threshold.", 5, 3, 21, "Singapore", null, null, null, null, null },
                    { 6, "FASAPP-0006", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, "Singapore", 2800m, 5, false, "Singapore", 560m, "Student withdrew before review", 6, null, 6, 4, 18, "Singapore", null, null, null, null, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "FASAPP-0007", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 7, "Singapore", 4800m, 4, false, "Singapore", 1200m, "Special needs support threshold matched", 7, null, 7, 2, 16, "Singapore", null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 8, "FASAPP-0008", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 8, "Thailand", 3200m, 5, false, "Thailand", 640m, "PCI <= 850", 8, null, 8, 2, 22, "Singapore", null, null, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 9, "FASAPP-0009", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 9, "Philippines", 2600m, 3, false, "Philippines", 866.67m, "Emergency aid review required", 9, null, 9, 1, 17, "Singapore", null, null, null, null, null },
                    { 10, "FASAPP-0010", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 10, "Singapore", 3900m, 5, false, "Singapore", 780m, "PCI <= 1000", 10, null, 10, 2, 18, "Singapore", null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeCondition",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "Field", "GroupId", "IsDeleted", "Operator", "UpdatedAt", "UpdatedBy", "ValueNumber", "ValueNumberTo", "ValueText" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 1, false, 4, null, null, 750m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 5, 2, false, 4, null, null, 3500m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, 3, false, 1, null, null, null, null, "Singapore" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, 4, false, 7, null, null, 16m, 30m, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 3, 5, false, 1, null, null, null, null, "Singapore" },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 6, false, 4, null, null, 1000m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 5, 7, false, 4, null, null, 5000m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, 8, false, 1, null, null, null, null, "Singapore" },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 9, false, 4, null, null, 1200m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, 10, false, 4, null, null, 25m, null, null }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditTransactionId", "ExternalReference", "IsDeleted", "PaidAt", "PaymentMethod", "Status", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-00000000002", "Amelia Tan", "S0000002G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, null, false, new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 70m, null, null },
                    { 2, "EDU-2026-00000000003", "Marcus Lim", "S0000003E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, null, false, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 140m, null, null },
                    { 3, "EDU-2026-00000000004", "Priya Nair", "S0000004C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, null, false, new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 180m, null, null },
                    { 4, "EDU-2026-00000000005", "Ethan Koh", "S0000005A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, null, false, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 180m, null, null },
                    { 9, "EDU-2026-00000000006", "Hannah Lee", "S0000006Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, null, false, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 50m, null, null },
                    { 10, "EDU-2026-00000000008", "Sofia Chen", "S0000008F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, null, false, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 70m, null, null }
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
                columns: new[] { "Id", "AppliedFasApplicationId", "AppliedFasCourseFeeSubsidyValueSnapshot", "AppliedFasIsPerComponentSnapshot", "AppliedFasMiscFeeSubsidyValueSnapshot", "AppliedFasSchemeNameSnapshot", "AppliedFasSubsidyTypeSnapshot", "AppliedFasSubsidyValueSnapshot", "AppliedFasTierNameSnapshot", "CourseCodeSnapshot", "CourseDescriptionSnapshot", "CourseEndDateSnapshot", "CourseFeeAmountSnapshot", "CourseNameSnapshot", "CourseStartDateSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EnrollmentId", "GrossAmount", "GstAmountSnapshot", "IsDeleted", "MiscFeeAmountSnapshot", "NetAmount", "PaidAmount", "RemainingAmount", "SchoolNameSnapshot", "Status", "SubsidyAmount", "TaxRateSnapshot", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, null, false, null, null, null, null, null, "CRS-2026-A1B2C3D", "Applied numeracy and structured problem-solving for academic pathways.", new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 100m, "Quantitative Problem Solving", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 120m, 10m, false, 10m, 120m, 120m, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 2, null, null, false, null, null, null, null, null, "CRS-2026-B2C3D4E", "Core programming concepts, debugging, and application structure.", new DateTime(2026, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 115m, "Software Foundations with C#", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 140m, 13m, false, 12m, 140m, 70m, 70m, "Eastbridge Secondary School", 1, 0m, 0.09m, null, null },
                    { 3, null, null, false, null, null, null, null, null, "CRS-2026-C3D4E5F", "Practical writing, presentation, and workplace communication skills.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 130m, "Professional Communication Lab", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 160m, 15m, false, 15m, 160m, 140m, 20m, "Westhaven Secondary School", 1, 0m, 0.09m, null, null },
                    { 4, null, null, false, null, null, null, null, null, "CRS-2026-D4E5F6G", "Environmental systems, resource planning, and sustainability practice.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 145m, "Sustainability Science Workshop", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, 180m, 18m, false, 17m, 180m, 180m, 0m, "Southpoint Secondary School", 2, 0m, 0.09m, null, null },
                    { 5, null, null, false, null, null, null, null, null, "CRS-2026-E5F6G7H", "Digital storytelling, layout, and media production workflows.", new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 160m, "Digital Media Production", new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, 200m, 20m, false, 20m, 200m, 180m, 20m, "Central Heights School", 1, 0m, 0.09m, null, null },
                    { 6, null, null, false, null, null, null, null, null, "CRS-2026-F6G7H8J", "Customer operations, service standards, and scenario-based practice.", new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), 175m, "Service Operations Practicum", new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 220m, 23m, false, 22m, 220m, 150m, 70m, "Riverside Learning Institute", 3, 0m, 0.09m, null, null },
                    { 7, null, null, false, null, null, null, null, null, "CRS-2026-G7H8J9K", "Foundational electrical theory, components, and safety practices.", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 190m, "Electrical Systems Fundamentals", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, 240m, 25m, false, 25m, 240m, 200m, 40m, "Lakeside Technical School", 1, 0m, 0.09m, null, null },
                    { 8, null, null, false, null, null, null, null, null, "CRS-2026-H8J9K0L", "Narrative craft, editing practice, and guided writing critique.", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 205m, "Creative Writing Studio", new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 260m, 28m, false, 27m, 260m, 200m, 60m, "Greenfield Academy", 3, 0m, 0.09m, null, null },
                    { 9, null, null, false, null, null, null, null, null, "CRS-2026-J9K0L1M", "Data preparation, analysis, visualization, and reporting fundamentals.", new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), 220m, "Data Analytics Essentials", new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, 280m, 30m, false, 30m, 280m, 0m, 280m, "Harbourfront School", 3, 0m, 0.09m, null, null },
                    { 10, null, null, false, null, null, null, null, null, "CRS-2026-K0L1M2N", "Document, spreadsheet, and presentation workflows for business users.", new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 235m, "Office Productivity for Business", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 300m, 33m, false, 32m, 300m, 0m, 300m, "Hillcrest Education Centre", 3, 0m, 0.09m, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasApplicationDocument",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DocumentNameSnapshot", "FasApplicationId", "FasSchemeRequiredDocumentId", "FileKey", "FileName", "IsDeleted", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 1, 1, "fas/applications/1/document.pdf", "fas-application-1.pdf", false, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 2, 2, "fas/applications/2/document.pdf", "fas-application-2.pdf", false, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 3, 3, "fas/applications/3/document.pdf", "fas-application-3.pdf", false, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 4, 4, "fas/applications/4/document.pdf", "fas-application-4.pdf", false, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 5, 5, "fas/applications/5/document.pdf", "fas-application-5.pdf", false, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 6, 6, "fas/applications/6/document.pdf", "fas-application-6.pdf", false, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 7, 7, "fas/applications/7/document.pdf", "fas-application-7.pdf", false, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 8, 8, "fas/applications/8/document.pdf", "fas-application-8.pdf", false, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 9, 9, "fas/applications/9/document.pdf", "fas-application-9.pdf", false, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 10, 10, "fas/applications/10/document.pdf", "fas-application-10.pdf", false, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasTierOverrideHistory",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "FasApplicationId", "IsDeleted", "ModifiedAt", "ModifiedByUserId", "NewTierId", "OldTierId", "Reason", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, 1, "Seed audit trail for tier review.", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 2, "Seed audit trail for tier review.", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3, 3, "Seed audit trail for tier review.", null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4, 4, "Seed audit trail for tier review.", null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5, 5, "Seed audit trail for tier review.", null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, 6, 6, "Seed audit trail for tier review.", null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, 7, 7, "Seed audit trail for tier review.", null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, 8, 8, "Seed audit trail for tier review.", null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Utc), 1, 9, 9, "Seed audit trail for tier review.", null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, 10, 10, "Seed audit trail for tier review.", null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupSystemApplication",
                columns: new[] { "Id", "EducationAccountId", "SystemTopupId", "TopupExecutionTargetId" },
                values: new object[] { 1, 3, 21, 3 });

            migrationBuilder.InsertData(
                table: "ChargeInstallment",
                columns: new[] { "Id", "Amount", "BecameOverdueAt", "ChargeId", "CreatedAt", "CreatedBy", "DeletedAt", "DueDate", "InstallmentNumber", "IsDeleted", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 120m, null, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 2, 70m, null, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 3, 80m, null, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 4, 180m, null, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 5, 100m, null, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 6, 110m, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 7, 120m, null, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 8, 130m, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 9, 140m, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 10, 150m, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null }
                });

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
                columns: new[] { "Id", "Amount", "ChargeGrossAmountSnapshot", "ChargeId", "ChargeInstallmentId", "ChargeNetAmountSnapshot", "ChargeRemainingAmountSnapshot", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "PaymentId", "SchoolNameSnapshot", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 70m, 140m, 2, null, 140m, 140m, "Software Foundations with C#", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Eastbridge Secondary School", null, null },
                    { 2, 140m, 160m, 3, null, 160m, 160m, "Professional Communication Lab", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Westhaven Secondary School", null, null },
                    { 3, 180m, 180m, 4, null, 180m, 180m, "Sustainability Science Workshop", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 3, "Southpoint Secondary School", null, null },
                    { 4, 180m, 200m, 5, null, 200m, 200m, "Digital Media Production", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, "Central Heights School", null, null },
                    { 5, 120m, 120m, 1, null, 120m, 120m, "Quantitative Problem Solving", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 5, "Northview Secondary School", null, null },
                    { 6, 100m, 220m, 6, null, 220m, 220m, "Service Operations Practicum", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 6, "Riverside Learning Institute", null, null },
                    { 7, 200m, 240m, 7, null, 240m, 240m, "Electrical Systems Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 7, "Lakeside Technical School", null, null },
                    { 8, 130m, 260m, 8, null, 260m, 260m, "Creative Writing Studio", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 8, "Greenfield Academy", null, null },
                    { 9, 50m, 220m, 6, null, 220m, 120m, "Service Operations Practicum", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 9, "Riverside Learning Institute", null, null },
                    { 10, 70m, 260m, 8, null, 260m, 130m, "Creative Writing Studio", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 10, "Greenfield Academy", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_Email",
                table: "AdminProfile",
                column: "Email",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_Nric",
                table: "AdminProfile",
                column: "Nric",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Nric\" IS NOT NULL");

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
                name: "IX_Charge_AppliedFasApplicationId",
                table: "Charge",
                column: "AppliedFasApplicationId");

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
                name: "IX_ChargeInstallment_ChargeId",
                table: "ChargeInstallment",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeInstallment_ChargeId_InstallmentNumber",
                table: "ChargeInstallment",
                columns: new[] { "ChargeId", "InstallmentNumber" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"ChargeId\" IS NOT NULL AND \"InstallmentNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeInstallment_DueDate",
                table: "ChargeInstallment",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeInstallment_Status",
                table: "ChargeInstallment",
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
                name: "IX_Course_EnrollmentDeadline",
                table: "Course",
                column: "EnrollmentDeadline");

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
                name: "IX_FasApplication_ApplicationNumber",
                table: "FasApplication",
                column: "ApplicationNumber",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"ApplicationNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplication_ApprovedByUserId",
                table: "FasApplication",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplication_ApprovedTierId",
                table: "FasApplication",
                column: "ApprovedTierId");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplication_FasSchemeId",
                table: "FasApplication",
                column: "FasSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplication_RecommendedTierId",
                table: "FasApplication",
                column: "RecommendedTierId");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplication_SchoolStudentId",
                table: "FasApplication",
                column: "SchoolStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplication_Status",
                table: "FasApplication",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplication_ValidityEndDate",
                table: "FasApplication",
                column: "ValidityEndDate");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplicationDocument_FasApplicationId",
                table: "FasApplicationDocument",
                column: "FasApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplicationDocument_FasSchemeRequiredDocumentId",
                table: "FasApplicationDocument",
                column: "FasSchemeRequiredDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_FasScheme_SchemeCode",
                table: "FasScheme",
                column: "SchemeCode",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"SchemeCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FasScheme_SchemeName",
                table: "FasScheme",
                column: "SchemeName");

            migrationBuilder.CreateIndex(
                name: "IX_FasScheme_SchoolId",
                table: "FasScheme",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_FasScheme_Status",
                table: "FasScheme",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeCondition_Field",
                table: "FasSchemeCondition",
                column: "Field");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeCondition_GroupId",
                table: "FasSchemeCondition",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeConditionGroup_FasSchemeId",
                table: "FasSchemeConditionGroup",
                column: "FasSchemeId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"FasSchemeId\" IS NOT NULL AND \"ParentGroupId\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeConditionGroup_ParentGroupId",
                table: "FasSchemeConditionGroup",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeCourse_CourseId",
                table: "FasSchemeCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeCourse_FasSchemeId",
                table: "FasSchemeCourse",
                column: "FasSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeCourse_FasSchemeId_CourseId",
                table: "FasSchemeCourse",
                columns: new[] { "FasSchemeId", "CourseId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"FasSchemeId\" IS NOT NULL AND \"CourseId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeRequiredDocument_FasSchemeId",
                table: "FasSchemeRequiredDocument",
                column: "FasSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeRequiredDocument_FasSchemeId_DocumentName",
                table: "FasSchemeRequiredDocument",
                columns: new[] { "FasSchemeId", "DocumentName" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"FasSchemeId\" IS NOT NULL AND \"DocumentName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeTier_FasSchemeId",
                table: "FasSchemeTier",
                column: "FasSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeTier_FasSchemeId_TierName",
                table: "FasSchemeTier",
                columns: new[] { "FasSchemeId", "TierName" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"FasSchemeId\" IS NOT NULL AND \"TierName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FasTierOverrideHistory_FasApplicationId",
                table: "FasTierOverrideHistory",
                column: "FasApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_FasTierOverrideHistory_ModifiedAt",
                table: "FasTierOverrideHistory",
                column: "ModifiedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FasTierOverrideHistory_ModifiedByUserId",
                table: "FasTierOverrideHistory",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FasTierOverrideHistory_NewTierId",
                table: "FasTierOverrideHistory",
                column: "NewTierId");

            migrationBuilder.CreateIndex(
                name: "IX_FasTierOverrideHistory_OldTierId",
                table: "FasTierOverrideHistory",
                column: "OldTierId");

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
                name: "IX_PaymentAllocation_ChargeInstallmentId",
                table: "PaymentAllocation",
                column: "ChargeInstallmentId");

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
                name: "FasApplicationDocument");

            migrationBuilder.DropTable(
                name: "FasSchemeCondition");

            migrationBuilder.DropTable(
                name: "FasSchemeCourse");

            migrationBuilder.DropTable(
                name: "FasTierOverrideHistory");

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
                name: "FasSchemeRequiredDocument");

            migrationBuilder.DropTable(
                name: "FasSchemeConditionGroup");

            migrationBuilder.DropTable(
                name: "OutstandingDeductionRun");

            migrationBuilder.DropTable(
                name: "ChargeInstallment");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "ScheduleTopUpConditionGroup");

            migrationBuilder.DropTable(
                name: "SystemTopupConditionGroup");

            migrationBuilder.DropTable(
                name: "TopupExecutionTarget");

            migrationBuilder.DropTable(
                name: "Charge");

            migrationBuilder.DropTable(
                name: "EducationCreditTransaction");

            migrationBuilder.DropTable(
                name: "TopupExecution");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "FasApplication");

            migrationBuilder.DropTable(
                name: "ScheduleTopUp");

            migrationBuilder.DropTable(
                name: "SystemTopup");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "FasSchemeTier");

            migrationBuilder.DropTable(
                name: "SchoolStudent");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "FasScheme");

            migrationBuilder.DropTable(
                name: "EducationAccount");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropTable(
                name: "Citizen");
        }
    }
}
