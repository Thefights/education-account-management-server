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
                    StudentNationalitySnapshot = table.Column<int>(type: "int", nullable: false),
                    GuardianNationalitySnapshot = table.Column<int>(type: "int", nullable: false),
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
                    PaymentPlanMonths = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    SchoolNameSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCodeSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseNameSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseDescriptionSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseStartDateSnapshot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseEndDateSnapshot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseFeeAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MiscFeeAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GstAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRateSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AppliedFasSchemeNameSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppliedFasTierNameSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.CheckConstraint("CK_Charge_PaymentPlanMonths", "[PaymentPlanMonths] IS NULL OR [PaymentPlanMonths] IN (3, 6, 9, 12)");
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
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateOfBirth", "DeletedAt", "Email", "FullName", "IsDeleted", "IsSingaporean", "MailingAddress", "Nric", "PhoneNumber", "ResidentialAddress", "SchoolingStatus", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1995, 1, 2), null, "phuckhang1088@gmail.com", "Sterling Quach", false, true, "1 Learning Grove, Singapore", "S0000001I", "+6590000001", "1 Learning Grove, Singapore", "Enrolled", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1996, 2, 3), null, "amelia.tan@example.com", "Amelia Tan", false, true, "2 Learning Grove, Singapore", "S0000002G", "+6590000002", "2 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1997, 3, 4), null, "marcus.lim@example.com", "Marcus Lim", false, true, "3 Learning Grove, Singapore", "S0000003E", "+6590000003", "3 Learning Grove, Singapore", "Graduated", null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1998, 4, 5), null, "priya.nair@example.com", "Priya Nair", false, true, "4 Learning Grove, Singapore", "S0000004C", "+6590000004", "4 Learning Grove, Singapore", "Suspended", null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1999, 5, 6), null, "ethan.koh@example.com", "Ethan Koh", false, true, "5 Learning Grove, Singapore", "S0000005A", "+6590000005", "5 Learning Grove, Singapore", "Withdrawn", null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 7), null, "hannah.lee@example.com", "Hannah Lee", false, true, "6 Learning Grove, Singapore", "S0000006Z", "+6590000006", "6 Learning Grove, Singapore", "Enrolled", null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 8), null, "daniel.wong@example.com", "Daniel Wong", false, true, "7 Learning Grove, Singapore", "S0000007H", "+6590000007", "7 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 9), null, "sofia.chen@example.com", "Sofia Chen", false, true, "8 Learning Grove, Singapore", "S0000008F", "+6590000008", "8 Learning Grove, Singapore", "Graduated", null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 10), null, "lucas.nguyen@example.com", "Lucas Nguyen", false, true, "9 Learning Grove, Singapore", "S0000009D", "+6590000009", "9 Learning Grove, Singapore", "Suspended", null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 11), null, "maya.rahman@example.com", "Maya Rahman", false, true, "10 Learning Grove, Singapore", "S0000010H", "+6590000010", "10 Learning Grove, Singapore", "Withdrawn", null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2005, 11, 12), null, "noah.teo@example.com", "Noah Teo", false, true, "11 Learning Grove, Singapore", "S0000011F", "+6590000011", "11 Learning Grove, Singapore", "Enrolled", null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1994, 12, 13), null, "aisha.fernandez@example.com", "Aisha Fernandez", false, true, "12 Learning Grove, Singapore", "S0000012D", "+6590000012", "12 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1995, 1, 14), null, "ryan.chua@example.com", "Ryan Chua", false, true, "13 Learning Grove, Singapore", "S0000013B", "+6590000013", "13 Learning Grove, Singapore", "Graduated", null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1996, 2, 15), null, "chloe.goh@example.com", "Chloe Goh", false, true, "14 Learning Grove, Singapore", "S0000014J", "+6590000014", "14 Learning Grove, Singapore", "Suspended", null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1997, 3, 16), null, "irfan.hassan@example.com", "Irfan Hassan", false, true, "15 Learning Grove, Singapore", "S0000015I", "+6590000015", "15 Learning Grove, Singapore", "Withdrawn", null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1998, 4, 17), null, "natalie.seah@example.com", "Natalie Seah", false, true, "16 Learning Grove, Singapore", "S0000016G", "+6590000016", "16 Learning Grove, Singapore", "Enrolled", null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1999, 5, 18), null, "alina.ang@example.com", "Alina Ang", false, true, "17 Learning Grove, Singapore", "S0000017E", "+6590000017", "17 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 19), null, "benjamin.bala@example.com", "Benjamin Bala", false, true, "18 Learning Grove, Singapore", "S0000018C", "+6590000018", "18 Learning Grove, Singapore", "Graduated", null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 20), null, "clara.chew@example.com", "Clara Chew", false, true, "19 Learning Grove, Singapore", "S0000019A", "+6590000019", "19 Learning Grove, Singapore", "Suspended", null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 21), null, "darius.das@example.com", "Darius Das", false, true, "20 Learning Grove, Singapore", "S0000020E", "+6590000020", "20 Learning Grove, Singapore", "Withdrawn", null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 22), null, "elena.eng@example.com", "Elena Eng", false, true, "21 Learning Grove, Singapore", "S0000021C", "+6590000021", "21 Learning Grove, Singapore", "Enrolled", null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 23), null, "farhan.foo@example.com", "Farhan Foo", false, true, "22 Learning Grove, Singapore", "S0000022A", "+6590000022", "22 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2005, 11, 24), null, "grace.gan@example.com", "Grace Gan", false, true, "23 Learning Grove, Singapore", "S0000023Z", "+6590000023", "23 Learning Grove, Singapore", "Graduated", null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1994, 12, 25), null, "haruto.ho@example.com", "Haruto Ho", false, true, "24 Learning Grove, Singapore", "S0000024H", "+6590000024", "24 Learning Grove, Singapore", "Suspended", null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1995, 1, 26), null, "isabelle.ismail@example.com", "Isabelle Ismail", false, true, "25 Learning Grove, Singapore", "S0000025F", "+6590000025", "25 Learning Grove, Singapore", "Withdrawn", null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1996, 2, 27), null, "jasper.jeyaratnam@example.com", "Jasper Jeyaratnam", false, true, "26 Learning Grove, Singapore", "S0000026D", "+6590000026", "26 Learning Grove, Singapore", "Enrolled", null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1997, 3, 1), null, "keira.kwek@example.com", "Keira Kwek", false, true, "27 Learning Grove, Singapore", "S0000027B", "+6590000027", "27 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1998, 4, 2), null, "leon.lim@example.com", "Leon Lim", false, true, "28 Learning Grove, Singapore", "S0000028J", "+6590000028", "28 Learning Grove, Singapore", "Graduated", null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1999, 5, 3), null, "mei.lin.mohamed@example.com", "Mei Lin Mohamed", false, true, "29 Learning Grove, Singapore", "S0000029I", "+6590000029", "29 Learning Grove, Singapore", "Suspended", null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 4), null, "nathan.ng@example.com", "Nathan Ng", false, true, "30 Learning Grove, Singapore", "S0000030B", "+6590000030", "30 Learning Grove, Singapore", "Withdrawn", null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 5), null, "olivia.ong@example.com", "Olivia Ong", false, true, "31 Learning Grove, Singapore", "S0000031J", "+6590000031", "31 Learning Grove, Singapore", "Enrolled", null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 6), null, "pranav.pillai@example.com", "Pranav Pillai", false, true, "32 Learning Grove, Singapore", "S0000032I", "+6590000032", "32 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 7), null, "qistina.quek@example.com", "Qistina Quek", false, true, "33 Learning Grove, Singapore", "S0000033G", "+6590000033", "33 Learning Grove, Singapore", "Graduated", null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 8), null, "rafael.rao@example.com", "Rafael Rao", false, true, "34 Learning Grove, Singapore", "S0000034E", "+6590000034", "34 Learning Grove, Singapore", "Suspended", null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2005, 11, 9), null, "selina.sim@example.com", "Selina Sim", false, true, "35 Learning Grove, Singapore", "S0000035C", "+6590000035", "35 Learning Grove, Singapore", "Withdrawn", null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1994, 12, 10), null, "terence.tan@example.com", "Terence Tan", false, true, "36 Learning Grove, Singapore", "S0000036A", "+6590000036", "36 Learning Grove, Singapore", "Enrolled", null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1995, 1, 11), null, "umairah.uddin@example.com", "Umairah Uddin", false, true, "37 Learning Grove, Singapore", "S0000037Z", "+6590000037", "37 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1996, 2, 12), null, "victor.vasquez@example.com", "Victor Vasquez", false, true, "38 Learning Grove, Singapore", "S0000038H", "+6590000038", "38 Learning Grove, Singapore", "Graduated", null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1997, 3, 13), null, "wen.jie.wong@example.com", "Wen Jie Wong", false, true, "39 Learning Grove, Singapore", "S0000039F", "+6590000039", "39 Learning Grove, Singapore", "Suspended", null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1998, 4, 14), null, "xavier.xu@example.com", "Xavier Xu", false, true, "40 Learning Grove, Singapore", "S0000040Z", "+6590000040", "40 Learning Grove, Singapore", "Withdrawn", null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1999, 5, 15), null, "yasmin.yeo@example.com", "Yasmin Yeo", false, true, "41 Learning Grove, Singapore", "S0000041H", "+6590000041", "41 Learning Grove, Singapore", "Enrolled", null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 16), null, "zachary.zainal@example.com", "Zachary Zainal", false, true, "42 Learning Grove, Singapore", "S0000042F", "+6590000042", "42 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 17), null, "adeline.ang@example.com", "Adeline Ang", false, true, "43 Learning Grove, Singapore", "S0000043D", "+6590000043", "43 Learning Grove, Singapore", "Graduated", null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 18), null, "brandon.bala@example.com", "Brandon Bala", false, true, "44 Learning Grove, Singapore", "S0000044B", "+6590000044", "44 Learning Grove, Singapore", "Suspended", null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 19), null, "celeste.chew@example.com", "Celeste Chew", false, true, "45 Learning Grove, Singapore", "S0000045J", "+6590000045", "45 Learning Grove, Singapore", "Withdrawn", null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 20), null, "damien.das@example.com", "Damien Das", false, true, "46 Learning Grove, Singapore", "S0000046I", "+6590000046", "46 Learning Grove, Singapore", "Enrolled", null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2005, 11, 21), null, "evelyn.eng@example.com", "Evelyn Eng", false, true, "47 Learning Grove, Singapore", "S0000047G", "+6590000047", "47 Learning Grove, Singapore", "Not Enrolled", null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1994, 12, 22), null, "faris.foo@example.com", "Faris Foo", false, true, "48 Learning Grove, Singapore", "S0000048E", "+6590000048", "48 Learning Grove, Singapore", "Graduated", null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1995, 1, 23), null, "giselle.gan@example.com", "Giselle Gan", false, true, "49 Learning Grove, Singapore", "S0000049C", "+6590000049", "49 Learning Grove, Singapore", "Suspended", null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1996, 2, 24), null, "haziq.ho@example.com", "Haziq Ho", false, true, "50 Learning Grove, Singapore", "S0000050G", "+6590000050", "50 Learning Grove, Singapore", "Withdrawn", null, null }
                });

            migrationBuilder.InsertData(
                table: "EducationAccountSweepReports",
                columns: new[] { "Id", "AccountsClosedCount", "AccountsCreatedCount", "AccountsExtendedCount", "BatchDate", "CompletedAt", "StartedAt" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, new DateOnly(2026, 6, 1), new DateTime(2026, 6, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 1, 0, 0, new DateOnly(2026, 6, 2), new DateTime(2026, 6, 2, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "ScheduleTopUp",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExecuteAtDay", "ExecuteAtMonth", "ExecutionTime", "Frequency", "IsDeleted", "Name", "NextExecutionAt", "OneTimeExecutionAt", "Status", "TopupAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(10, 0, 0), 1, false, "Seed Schedule Topup 01", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 65m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, null, new TimeOnly(11, 0, 0), 2, false, "Seed Schedule Topup 02", new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 80m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, new TimeOnly(12, 0, 0), 3, false, "Seed Schedule Topup 03", null, null, 3, 95m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(13, 0, 0), 1, false, "Seed Schedule Topup 04", new DateTime(2026, 7, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, 110m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, null, new TimeOnly(14, 0, 0), 2, false, "Seed Schedule Topup 05", new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 125m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, new TimeOnly(9, 0, 0), 3, false, "Seed Schedule Topup 06", null, null, 3, 140m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(10, 0, 0), 1, false, "Seed Schedule Topup 07", new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, 155m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, null, new TimeOnly(11, 0, 0), 2, false, "Seed Schedule Topup 08", new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 170m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, new TimeOnly(12, 0, 0), 3, false, "Seed Schedule Topup 09", null, null, 3, 185m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(13, 0, 0), 1, false, "Seed Schedule Topup 10", new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, 200m, null, null }
                });

            migrationBuilder.InsertData(
                table: "School",
                columns: new[] { "Id", "Address", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "IsDeleted", "PhoneNumber", "SchoolName", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "10 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school01@example.edu.sg", false, "+656000001", "Northview Secondary School", 1, null, null },
                    { 2, "20 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school02@example.edu.sg", false, "+656000002", "Eastbridge Secondary School", 1, null, null },
                    { 3, "30 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school03@example.edu.sg", false, "+656000003", "Westhaven Secondary School", 1, null, null },
                    { 4, "40 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school04@example.edu.sg", false, "+656000004", "Southpoint Secondary School", 1, null, null },
                    { 5, "50 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school05@example.edu.sg", false, "+656000005", "Central Heights School", 1, null, null },
                    { 6, "60 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school06@example.edu.sg", false, "+656000006", "Riverside Learning Institute", 1, null, null },
                    { 7, "70 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school07@example.edu.sg", false, "+656000007", "Lakeside Technical School", 1, null, null },
                    { 8, "80 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school08@example.edu.sg", false, "+656000008", "Greenfield Academy", 1, null, null },
                    { 9, "90 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school09@example.edu.sg", false, "+656000009", "Harbourfront School", 1, null, null },
                    { 10, "100 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school10@example.edu.sg", false, "+656000010", "Hillcrest Education Centre", 2, null, null },
                    { 11, "110 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school11@example.edu.sg", false, "+656000011", "Cedar Valley School", 1, null, null },
                    { 12, "120 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school12@example.edu.sg", false, "+656000012", "Maple Ridge Academy", 1, null, null },
                    { 13, "130 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school13@example.edu.sg", false, "+656000013", "Oceanview Institute", 1, null, null },
                    { 14, "140 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school14@example.edu.sg", false, "+656000014", "Brighton Learning Centre", 1, null, null },
                    { 15, "150 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school15@example.edu.sg", false, "+656000015", "Pioneer Technical College", 1, null, null },
                    { 16, "160 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school16@example.edu.sg", false, "+656000016", "Summit Arts School", 1, null, null },
                    { 17, "170 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school17@example.edu.sg", false, "+656000017", "Meridian Business School", 1, null, null },
                    { 18, "180 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school18@example.edu.sg", false, "+656000018", "Silverstream Polytechnic", 1, null, null },
                    { 19, "190 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school19@example.edu.sg", false, "+656000019", "Redwood Community College", 1, null, null },
                    { 20, "200 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school20@example.edu.sg", false, "+656000020", "Bluewater Skills Institute", 2, null, null },
                    { 21, "210 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school21@example.edu.sg", false, "+656000021", "Golden Grove Academy", 1, null, null },
                    { 22, "220 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school22@example.edu.sg", false, "+656000022", "Sunrise Training Centre", 1, null, null },
                    { 23, "230 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school23@example.edu.sg", false, "+656000023", "Crescent School of Technology", 1, null, null },
                    { 24, "240 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school24@example.edu.sg", false, "+656000024", "Orchid City College", 1, null, null },
                    { 25, "250 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school25@example.edu.sg", false, "+656000025", "Evergreen Education Hub", 1, null, null },
                    { 26, "260 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school26@example.edu.sg", false, "+656000026", "Vista Applied Learning", 1, null, null },
                    { 27, "270 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school27@example.edu.sg", false, "+656000027", "Compass Point School", 1, null, null },
                    { 28, "280 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school28@example.edu.sg", false, "+656000028", "Newbridge Institute", 1, null, null },
                    { 29, "290 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school29@example.edu.sg", false, "+656000029", "Heritage Skills Academy", 1, null, null },
                    { 30, "300 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school30@example.edu.sg", false, "+656000030", "Frontier Science School", 2, null, null },
                    { 31, "310 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school31@example.edu.sg", false, "+656000031", "Meadowbrook College", 1, null, null },
                    { 32, "320 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school32@example.edu.sg", false, "+656000032", "Peakview Education Centre", 1, null, null },
                    { 33, "330 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school33@example.edu.sg", false, "+656000033", "Bayfront Technical School", 1, null, null },
                    { 34, "340 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school34@example.edu.sg", false, "+656000034", "Queensway Learning Academy", 1, null, null },
                    { 35, "350 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school35@example.edu.sg", false, "+656000035", "Unity Continuing Education", 1, null, null },
                    { 36, "360 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school36@example.edu.sg", false, "+656000036", "Elmwood Professional School", 1, null, null },
                    { 37, "370 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school37@example.edu.sg", false, "+656000037", "Innovation Training Campus", 1, null, null },
                    { 38, "380 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school38@example.edu.sg", false, "+656000038", "Riverbend School", 1, null, null },
                    { 39, "390 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school39@example.edu.sg", false, "+656000039", "Stonefield Institute", 1, null, null },
                    { 40, "400 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school40@example.edu.sg", false, "+656000040", "Seaside Skills Centre", 2, null, null },
                    { 41, "410 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school41@example.edu.sg", false, "+656000041", "Northstar Academy", 1, null, null },
                    { 42, "420 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school42@example.edu.sg", false, "+656000042", "Eastgate Technical College", 1, null, null },
                    { 43, "430 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school43@example.edu.sg", false, "+656000043", "Westlake Learning Hub", 1, null, null },
                    { 44, "440 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school44@example.edu.sg", false, "+656000044", "Southridge School", 1, null, null },
                    { 45, "450 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school45@example.edu.sg", false, "+656000045", "Central Park Institute", 1, null, null },
                    { 46, "460 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school46@example.edu.sg", false, "+656000046", "Hillview Polytechnic", 1, null, null },
                    { 47, "470 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school47@example.edu.sg", false, "+656000047", "Greenridge Academy", 1, null, null },
                    { 48, "480 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school48@example.edu.sg", false, "+656000048", "Harbour Bay College", 1, null, null },
                    { 49, "490 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school49@example.edu.sg", false, "+656000049", "Lighthouse Skills School", 1, null, null },
                    { 50, "500 Seed Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school50@example.edu.sg", false, "+656000050", "Civic Learning Centre", 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "SystemTopup",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Name", "Status", "TopupAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 01", 1, 100m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 02", 1, 120m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 03", 1, 140m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 04", 2, 160m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 05", 1, 180m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 06", 1, 200m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 07", 1, 220m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 08", 2, 240m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 09", 1, 260m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Seed System Topup 10", 1, 280m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupExecution",
                columns: new[] { "Id", "ConditionsSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutionCode", "FailedCount", "IdempotencyKey", "IsDeleted", "ManualAmount", "ManualReason", "ScheduleTopUpId", "SourceType", "Status", "SuccessCount", "SystemTopupId", "TopupAmountSnapshot", "TopupNameSnapshot", "TotalExecutedAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 3, "Seed condition snapshot", new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-003", 0, "topup-seed-003", false, 105m, "Manual seed topup", null, 3, 3, 1, null, 105m, "Seed Topup 03", 105m, 1, null, null },
                    { 6, "Seed condition snapshot", new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-006", 0, "topup-seed-006", false, 135m, "Manual seed topup", null, 3, 3, 1, null, 135m, "Seed Topup 06", 135m, 1, null, null },
                    { 9, "Seed condition snapshot", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-009", 0, "topup-seed-009", false, 165m, "Manual seed topup", null, 3, 3, 1, null, 165m, "Seed Topup 09", 165m, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CitizenId", "CreatedAt", "CreatedBy", "DeletedAt", "FailedLoginCount", "IsDeleted", "LastLoginAt", "LockedUntil", "Role", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, null, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, null, null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 2, null, null },
                    { 12, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 13, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 14, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 15, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 16, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 17, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 18, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 19, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 20, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 21, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 22, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 2, null, null },
                    { 23, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 24, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 25, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 26, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 27, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 28, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 29, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 30, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 31, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 32, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 33, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 2, null, null },
                    { 34, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 35, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 36, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 37, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 38, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 39, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 40, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 41, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 42, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 43, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 44, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 2, null, null },
                    { 45, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 46, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 47, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 48, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null },
                    { 49, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 3, 1, null, null },
                    { 50, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, 1, null, null },
                    { 51, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "AdminProfile",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "FullName", "IsDeleted", "Nric", "PhoneNumber", "SchoolId", "StaffCode", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin001@example.com", "System Administrator", false, "S0000101E", "+6580000001", null, "STAFF-2026-XX2CU83", null, null, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin002@example.com", "Finance Administrator", false, "S0000102C", "+6580000002", null, "STAFF-2026-STL4YQH", null, null, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin003@example.com", "School Administrator", false, "S0000103A", "+6580000003", 1, "STAFF-2026-MUWAHW6", null, null, 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin004@example.com", "Demo Administrator 004", false, "S0000104Z", "+6580000004", null, "STAFF-2026-S4FZU83", null, null, 5 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin005@example.com", "Demo Administrator 005", false, "S0000105H", "+6580000005", null, "STAFF-2026-AOAY3A4", null, null, 6 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin006@example.com", "Demo Administrator 006", false, "S0000106F", "+6580000006", 7, "STAFF-2026-KRNWMLK", null, null, 7 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin007@example.com", "Demo Administrator 007", false, "S0000107D", "+6580000007", null, "STAFF-2026-UNJFZW7", null, null, 8 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin008@example.com", "Demo Administrator 008", false, "S0000108B", "+6580000008", null, "STAFF-2026-SFXEF6F", null, null, 9 },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin009@example.com", "Demo Administrator 009", false, "S0000109J", "+6580000009", 10, "STAFF-2026-O3408F1", null, null, 10 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin010@example.com", "Demo Administrator 010", false, "S0000110D", "+6580000010", null, "STAFF-2026-7F0PDWW", null, null, 11 },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin011@example.com", "Demo Administrator 011", false, "S0000111B", "+6580000011", null, "STAFF-2026-8X4V13Q", null, null, 12 },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin012@example.com", "Demo Administrator 012", false, "S0000112J", "+6580000012", 13, "STAFF-2026-U02ABWW", null, null, 13 },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin013@example.com", "Demo Administrator 013", false, "S0000113I", "+6580000013", null, "STAFF-2026-YAASVP4", null, null, 14 },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin014@example.com", "Demo Administrator 014", false, "S0000114G", "+6580000014", null, "STAFF-2026-MD7QFDN", null, null, 15 },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin015@example.com", "Demo Administrator 015", false, "S0000115E", "+6580000015", 16, "STAFF-2026-YM3YL5X", null, null, 16 },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin016@example.com", "Demo Administrator 016", false, "S0000116C", "+6580000016", null, "STAFF-2026-KOWBPV1", null, null, 17 },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin017@example.com", "Demo Administrator 017", false, "S0000117A", "+6580000017", null, "STAFF-2026-D7G9W6U", null, null, 18 },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin018@example.com", "Demo Administrator 018", false, "S0000118Z", "+6580000018", 19, "STAFF-2026-ZAQHZJY", null, null, 19 },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin019@example.com", "Demo Administrator 019", false, "S0000119H", "+6580000019", null, "STAFF-2026-NI8TCIU", null, null, 20 },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin020@example.com", "Demo Administrator 020", false, "S0000120A", "+6580000020", null, "STAFF-2026-BM63HUE", null, null, 21 },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin021@example.com", "Demo Administrator 021", false, "S0000121Z", "+6580000021", 22, "STAFF-2026-OE318VW", null, null, 22 },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin022@example.com", "Demo Administrator 022", false, "S0000122H", "+6580000022", null, "STAFF-2026-LC8GF2Z", null, null, 23 },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin023@example.com", "Demo Administrator 023", false, "S0000123F", "+6580000023", null, "STAFF-2026-5TBCP3X", null, null, 24 },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin024@example.com", "Demo Administrator 024", false, "S0000124D", "+6580000024", 25, "STAFF-2026-AJHDSIO", null, null, 25 },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin025@example.com", "Demo Administrator 025", false, "S0000125B", "+6580000025", null, "STAFF-2026-VV1BIL1", null, null, 26 },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin026@example.com", "Demo Administrator 026", false, "S0000126J", "+6580000026", null, "STAFF-2026-W7UIFU4", null, null, 27 },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin027@example.com", "Demo Administrator 027", false, "S0000127I", "+6580000027", 28, "STAFF-2026-PZ60RTL", null, null, 28 },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin028@example.com", "Demo Administrator 028", false, "S0000128G", "+6580000028", null, "STAFF-2026-CIIW02U", null, null, 29 },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin029@example.com", "Demo Administrator 029", false, "S0000129E", "+6580000029", null, "STAFF-2026-Z53N6V0", null, null, 30 },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin030@example.com", "Demo Administrator 030", false, "S0000130I", "+6580000030", 31, "STAFF-2026-53DUZ0B", null, null, 31 },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin031@example.com", "Demo Administrator 031", false, "S0000131G", "+6580000031", null, "STAFF-2026-UF5TOFH", null, null, 32 },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin032@example.com", "Demo Administrator 032", false, "S0000132E", "+6580000032", null, "STAFF-2026-BI0W84O", null, null, 33 },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin033@example.com", "Demo Administrator 033", false, "S0000133C", "+6580000033", 34, "STAFF-2026-NQNU96E", null, null, 34 },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin034@example.com", "Demo Administrator 034", false, "S0000134A", "+6580000034", null, "STAFF-2026-IXYYHWY", null, null, 35 },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin035@example.com", "Demo Administrator 035", false, "S0000135Z", "+6580000035", null, "STAFF-2026-6F5BSF7", null, null, 36 },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin036@example.com", "Demo Administrator 036", false, "S0000136H", "+6580000036", 37, "STAFF-2026-76F87EB", null, null, 37 },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin037@example.com", "Demo Administrator 037", false, "S0000137F", "+6580000037", null, "STAFF-2026-7QE6AWN", null, null, 38 },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin038@example.com", "Demo Administrator 038", false, "S0000138D", "+6580000038", null, "STAFF-2026-F5AF0MK", null, null, 39 },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin039@example.com", "Demo Administrator 039", false, "S0000139B", "+6580000039", 40, "STAFF-2026-VN6QRJT", null, null, 40 },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin040@example.com", "Demo Administrator 040", false, "S0000140F", "+6580000040", null, "STAFF-2026-1702LWO", null, null, 41 },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin041@example.com", "Demo Administrator 041", false, "S0000141D", "+6580000041", null, "STAFF-2026-4NCICBA", null, null, 42 },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin042@example.com", "Demo Administrator 042", false, "S0000142B", "+6580000042", 43, "STAFF-2026-IBPQ8VK", null, null, 43 },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin043@example.com", "Demo Administrator 043", false, "S0000143J", "+6580000043", null, "STAFF-2026-JTA16SN", null, null, 44 },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin044@example.com", "Demo Administrator 044", false, "S0000144I", "+6580000044", null, "STAFF-2026-1GN95ZX", null, null, 45 },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin045@example.com", "Demo Administrator 045", false, "S0000145G", "+6580000045", 46, "STAFF-2026-I2D4PEW", null, null, 46 },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin046@example.com", "Demo Administrator 046", false, "S0000146E", "+6580000046", null, "STAFF-2026-Z6LSG0N", null, null, 47 },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin047@example.com", "Demo Administrator 047", false, "S0000147C", "+6580000047", null, "STAFF-2026-TNPXJ1D", null, null, 48 },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin048@example.com", "Demo Administrator 048", false, "S0000148A", "+6580000048", 49, "STAFF-2026-MJO4T2S", null, null, 49 },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin049@example.com", "Demo Administrator 049", false, "S0000149Z", "+6580000049", null, "STAFF-2026-D65QQRF", null, null, 50 },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin050@example.com", "Demo Administrator 050", false, "S0000150C", "+6580000050", null, "STAFF-2026-KT2E8MC", null, null, 51 }
                });

            migrationBuilder.InsertData(
                table: "AuditLog",
                columns: new[] { "Id", "Action", "ActorUserId", "Category", "IpAddress", "Nric", "OccurredAt" },
                values: new object[,]
                {
                    { 1, "SeedAction01", 1, 1, "127.0.0.2", null, new DateTime(2026, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "SeedAction02", 2, 2, "127.0.0.3", null, new DateTime(2026, 1, 1, 2, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "SeedAction03", 3, 3, "127.0.0.4", null, new DateTime(2026, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "SeedAction05", 1, 5, "127.0.0.6", null, new DateTime(2026, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "SeedAction06", 2, 6, "127.0.0.7", null, new DateTime(2026, 1, 1, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "SeedAction07", 3, 7, "127.0.0.8", null, new DateTime(2026, 1, 1, 7, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, "SeedAction09", 1, 2, "127.0.0.10", null, new DateTime(2026, 1, 1, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, "SeedAction10", 2, 3, "127.0.0.11", null, new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "SeedAction11", 3, 4, "127.0.0.12", null, new DateTime(2026, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, "SeedAction13", 1, 6, "127.0.0.14", null, new DateTime(2026, 1, 1, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, "SeedAction14", 2, 7, "127.0.0.15", null, new DateTime(2026, 1, 1, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, "SeedAction15", 3, 1, "127.0.0.16", null, new DateTime(2026, 1, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, "SeedAction17", 1, 3, "127.0.0.18", null, new DateTime(2026, 1, 1, 17, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, "SeedAction18", 2, 4, "127.0.0.19", null, new DateTime(2026, 1, 1, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, "SeedAction19", 3, 5, "127.0.0.20", null, new DateTime(2026, 1, 1, 19, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, "SeedAction21", 1, 7, "127.0.0.22", null, new DateTime(2026, 1, 1, 21, 0, 0, 0, DateTimeKind.Utc) },
                    { 22, "SeedAction22", 2, 1, "127.0.0.23", null, new DateTime(2026, 1, 1, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 23, "SeedAction23", 3, 2, "127.0.0.24", null, new DateTime(2026, 1, 1, 23, 0, 0, 0, DateTimeKind.Utc) },
                    { 25, "SeedAction25", 1, 4, "127.0.0.26", null, new DateTime(2026, 1, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 26, "SeedAction26", 2, 5, "127.0.0.27", null, new DateTime(2026, 1, 2, 2, 0, 0, 0, DateTimeKind.Utc) },
                    { 27, "SeedAction27", 3, 6, "127.0.0.28", null, new DateTime(2026, 1, 2, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { 29, "SeedAction29", 1, 1, "127.0.0.30", null, new DateTime(2026, 1, 2, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 30, "SeedAction30", 2, 2, "127.0.0.31", null, new DateTime(2026, 1, 2, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 31, "SeedAction31", 3, 3, "127.0.0.32", null, new DateTime(2026, 1, 2, 7, 0, 0, 0, DateTimeKind.Utc) },
                    { 33, "SeedAction33", 1, 5, "127.0.0.34", null, new DateTime(2026, 1, 2, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 34, "SeedAction34", 2, 6, "127.0.0.35", null, new DateTime(2026, 1, 2, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 35, "SeedAction35", 3, 7, "127.0.0.36", null, new DateTime(2026, 1, 2, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { 37, "SeedAction37", 1, 2, "127.0.0.38", null, new DateTime(2026, 1, 2, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { 38, "SeedAction38", 2, 3, "127.0.0.39", null, new DateTime(2026, 1, 2, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 39, "SeedAction39", 3, 4, "127.0.0.40", null, new DateTime(2026, 1, 2, 15, 0, 0, 0, DateTimeKind.Utc) },
                    { 41, "SeedAction41", 1, 6, "127.0.0.42", null, new DateTime(2026, 1, 2, 17, 0, 0, 0, DateTimeKind.Utc) },
                    { 42, "SeedAction42", 2, 7, "127.0.0.43", null, new DateTime(2026, 1, 2, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { 43, "SeedAction43", 3, 1, "127.0.0.44", null, new DateTime(2026, 1, 2, 19, 0, 0, 0, DateTimeKind.Utc) },
                    { 45, "SeedAction45", 1, 3, "127.0.0.46", null, new DateTime(2026, 1, 2, 21, 0, 0, 0, DateTimeKind.Utc) },
                    { 46, "SeedAction46", 2, 4, "127.0.0.47", null, new DateTime(2026, 1, 2, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 47, "SeedAction47", 3, 5, "127.0.0.48", null, new DateTime(2026, 1, 2, 23, 0, 0, 0, DateTimeKind.Utc) },
                    { 49, "SeedAction49", 1, 7, "127.0.0.50", null, new DateTime(2026, 1, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 50, "SeedAction50", 2, 1, "127.0.0.51", null, new DateTime(2026, 1, 3, 2, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "CourseCode", "CourseFeeAmount", "CourseName", "CreatedAt", "CreatedBy", "DeletedAt", "EndDate", "EnrollmentDeadline", "FasApplicationDueDate", "GstAmount", "IsDeleted", "MiscFeeAmount", "SchoolId", "StartDate", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "CRS-2026-SAGH611", 125m, "Academic Writing Cohort 01", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13.32m, false, 23m, 1, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 2, "CRS-2026-A9CFJRZ", 130m, "Business Numeracy Cohort 02", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), 14.04m, false, 26m, 1, new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 3, "CRS-2026-1QOTM9Q", 135m, "Digital Literacy Cohort 03", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), 14.76m, false, 29m, 1, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 4, "CRS-2026-EJ1A67A", 140m, "Career Readiness Cohort 04", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 15.48m, false, 32m, 1, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 5, "CRS-2026-HPS3B9I", 145m, "Applied Science Cohort 05", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 14.85m, false, 20m, 1, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 6, "CRS-2026-UPUZSA3", 150m, "Financial Literacy Cohort 06", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 15.57m, false, 23m, 1, new DateTime(2026, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 7, "CRS-2026-SMDS48L", 155m, "Project Collaboration Cohort 07", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 16.29m, false, 26m, 1, new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 8, "CRS-2026-DTVK2L0", 160m, "Data Skills Cohort 08", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 17.01m, false, 29m, 1, new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 9, "CRS-2026-5HA3NN0", 165m, "Workplace Communication Cohort 09", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 17.73m, false, 32m, 1, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 10, "CRS-2026-PG7MVO8", 170m, "Software Foundations Cohort 10", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 17.10m, false, 20m, 1, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 11, "CRS-2026-7HGGL06", 175m, "Academic Writing Cohort 11", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 17.82m, false, 23m, 1, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 12, "CRS-2026-8QVCFIJ", 180m, "Business Numeracy Cohort 12", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), 18.54m, false, 26m, 1, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 13, "CRS-2026-LTXWODC", 185m, "Digital Literacy Cohort 13", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), 19.26m, false, 29m, 1, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 14, "CRS-2026-9O0PVAP", 190m, "Career Readiness Cohort 14", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 19.98m, false, 32m, 1, new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 15, "CRS-2026-WEJ9TUS", 195m, "Applied Science Cohort 15", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 19.35m, false, 20m, 1, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 16, "CRS-2026-DW55O4J", 200m, "Financial Literacy Cohort 16", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 20.07m, false, 23m, 1, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 17, "CRS-2026-BEOMA7Q", 205m, "Project Collaboration Cohort 17", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 20.79m, false, 26m, 1, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 18, "CRS-2026-E5YXUWF", 210m, "Data Skills Cohort 18", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), 21.51m, false, 29m, 1, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 19, "CRS-2026-5TQ32YK", 215m, "Workplace Communication Cohort 19", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), 22.23m, false, 32m, 1, new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 20, "CRS-2026-QHA65C0", 220m, "Software Foundations Cohort 20", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), 21.60m, false, 20m, 1, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 21, "CRS-2026-IF4LTDN", 225m, "Academic Writing Cohort 21", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), 22.32m, false, 23m, 1, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 22, "CRS-2026-EOWJ7RI", 230m, "Business Numeracy Cohort 22", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), 23.04m, false, 26m, 1, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 23, "CRS-2026-JTOV8NE", 235m, "Digital Literacy Cohort 23", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), 23.76m, false, 29m, 1, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 24, "CRS-2026-D6C5MU1", 240m, "Career Readiness Cohort 24", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), 24.48m, false, 32m, 1, new DateTime(2026, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 25, "CRS-2026-CWWS3U3", 245m, "Applied Science Cohort 25", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), 23.85m, false, 20m, 1, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 26, "CRS-2026-SPJ7E30", 250m, "Financial Literacy Cohort 26", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), 24.57m, false, 23m, 1, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 27, "CRS-2026-HHV04Y9", 255m, "Project Collaboration Cohort 27", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), 25.29m, false, 26m, 1, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 28, "CRS-2026-LUJ9RT4", 260m, "Data Skills Cohort 28", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), 26.01m, false, 29m, 1, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 29, "CRS-2026-MATA5OE", 265m, "Workplace Communication Cohort 29", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 26.73m, false, 32m, 1, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 30, "CRS-2026-TBP3E3W", 270m, "Software Foundations Cohort 30", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), 26.10m, false, 20m, 1, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 31, "CRS-2026-NMXIH0W", 275m, "Academic Writing Cohort 31", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), 26.82m, false, 23m, 1, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 32, "CRS-2026-M2C29EQ", 280m, "Business Numeracy Cohort 32", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 27.54m, false, 26m, 1, new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 33, "CRS-2026-YXZ452K", 285m, "Digital Literacy Cohort 33", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 28.26m, false, 29m, 1, new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 34, "CRS-2026-OJZT05F", 290m, "Career Readiness Cohort 34", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 28.98m, false, 32m, 1, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 35, "CRS-2026-FBV0QY7", 295m, "Applied Science Cohort 35", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 28.35m, false, 20m, 1, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 36, "CRS-2026-R46A00W", 300m, "Financial Literacy Cohort 36", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 29.07m, false, 23m, 1, new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 37, "CRS-2026-JMAO1QA", 305m, "Project Collaboration Cohort 37", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 29.79m, false, 26m, 1, new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 38, "CRS-2026-VA3LGE0", 310m, "Data Skills Cohort 38", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 30.51m, false, 29m, 1, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 39, "CRS-2026-C2L9WG4", 315m, "Workplace Communication Cohort 39", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 31.23m, false, 32m, 1, new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 40, "CRS-2026-L6UUQYK", 320m, "Software Foundations Cohort 40", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), 30.60m, false, 20m, 1, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 41, "CRS-2026-79UQVDL", 325m, "Academic Writing Cohort 41", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), 31.32m, false, 23m, 1, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 42, "CRS-2026-SDLMCO9", 330m, "Business Numeracy Cohort 42", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 32.04m, false, 26m, 1, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 43, "CRS-2026-LUEVJLD", 335m, "Digital Literacy Cohort 43", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 32.76m, false, 29m, 1, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 44, "CRS-2026-GO9AJL4", 340m, "Career Readiness Cohort 44", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 33.48m, false, 32m, 1, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 45, "CRS-2026-B29KV62", 345m, "Applied Science Cohort 45", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 32.85m, false, 20m, 1, new DateTime(2026, 3, 17, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 46, "CRS-2026-2ME55AE", 350m, "Financial Literacy Cohort 46", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), 33.57m, false, 23m, 1, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 47, "CRS-2026-X7JQUEV", 355m, "Project Collaboration Cohort 47", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), 34.29m, false, 26m, 1, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 48, "CRS-2026-Y8VG30C", 360m, "Data Skills Cohort 48", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), 35.01m, false, 29m, 1, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 49, "CRS-2026-8A1B6PI", 365m, "Workplace Communication Cohort 49", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), 35.73m, false, 32m, 1, new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 50, "CRS-2026-2H98HR4", 370m, "Software Foundations Cohort 50", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), 35.10m, false, 20m, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null }
                });

            migrationBuilder.InsertData(
                table: "EducationAccount",
                columns: new[] { "Id", "AccountNumber", "CitizenId", "ClosedAt", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditBalance", "IsDeleted", "OpenedAt", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-M7QVS7X", 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1025m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 2, "EDU-2026-H4W9JYQ", 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1050m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 3, "EDU-2026-5BHC84W", 3, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1075m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 4, "EDU-2026-K6CAJET", 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1100m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 5, "EDU-2026-C865DQ8", 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1125m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 6, "EDU-2026-J3PD5GI", 6, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1150m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 7, "EDU-2026-RHEGIE7", 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1175m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 8, "EDU-2026-ERZPP9D", 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1200m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 9, "EDU-2026-3WTXQL2", 9, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1225m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 10, "EDU-2026-5A2BWXE", 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1250m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 11, "EDU-2026-YKWELYD", 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1275m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 12, "EDU-2026-XW95AJ5", 12, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1300m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 13, "EDU-2026-OOROKGZ", 13, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1325m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 14, "EDU-2026-EC0POYJ", 14, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1350m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 15, "EDU-2026-1ZHBQ2S", 15, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1375m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 16, "EDU-2026-A5L21GT", 16, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1400m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 17, "EDU-2026-R9VEYR0", 17, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1425m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 18, "EDU-2026-JLH113O", 18, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1450m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 19, "EDU-2026-S7OK05W", 19, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1475m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 20, "EDU-2026-2MYNLK1", 20, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1500m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 21, "EDU-2026-7M9PDGH", 21, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1525m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 22, "EDU-2026-TIXLNIB", 22, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1550m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 23, "EDU-2026-T5N58HB", 23, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1575m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 24, "EDU-2026-7MH1FBP", 24, new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1600m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 25, "EDU-2026-GDE8Q6P", 25, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1625m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 26, "EDU-2026-SNYKN82", 26, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1650m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 27, "EDU-2026-QJZM8YL", 27, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1675m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 28, "EDU-2026-G0N8QXW", 28, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1700m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 29, "EDU-2026-A3B8IRY", 29, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1725m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 30, "EDU-2026-CYOBPQU", 30, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1750m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 31, "EDU-2026-20G2H3L", 31, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1775m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 32, "EDU-2026-9RY98UG", 32, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1800m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 33, "EDU-2026-LBUTZEW", 33, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1825m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 34, "EDU-2026-CVJPW51", 34, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1850m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 35, "EDU-2026-GL31H3K", 35, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1875m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 36, "EDU-2026-L1OX9Y0", 36, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1900m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 37, "EDU-2026-3V9Y2I0", 37, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1925m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 38, "EDU-2026-UIX61F4", 38, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1950m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 39, "EDU-2026-NQZL2LT", 39, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1975m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 40, "EDU-2026-2A9X7HT", 40, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 41, "EDU-2026-HIZFYQR", 41, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2025m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 42, "EDU-2026-5FO1ERB", 42, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2050m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 43, "EDU-2026-C6GLP5G", 43, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2075m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 44, "EDU-2026-VBGBHB4", 44, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2100m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 45, "EDU-2026-PMQT1K5", 45, new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2125m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 46, "EDU-2026-Z2MR7S3", 46, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2150m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 47, "EDU-2026-2OK04RQ", 47, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2175m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 48, "EDU-2026-5L2W56M", 48, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2200m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 49, "EDU-2026-7UZZHKN", 49, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2225m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 50, "EDU-2026-9DG3ZSZ", 50, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2250m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "EducationAccountSweepTargets",
                columns: new[] { "Id", "Action", "Nric", "Reason", "Status", "SweepReportId" },
                values: new object[,]
                {
                    { 1, 0, "S0000001I", "Seed pending create.", 0, 1 },
                    { 2, 1, "S0000002G", "Seed close success.", 1, 1 },
                    { 3, 2, "S0000003E", "Seed extend failed.", 2, 1 },
                    { 4, 1, "S0000004C", "Seed close success.", 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "FasScheme",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "DurationInMonths", "IsDeleted", "IsPerComponent", "PublishedAt", "SchemeCode", "SchemeName", "SchoolId", "Status", "SubsidyType", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 3, false, true, null, "FAS-2026-99JIVES", "Seed FAS Scheme 01", 1, 1, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 6, false, false, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-ETESJRO", "Seed FAS Scheme 02", 1, 2, 2, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 9, false, true, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-BCKWR0F", "Seed FAS Scheme 03", 1, 3, 1, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 12, false, false, null, "FAS-2026-XY25JRK", "Seed FAS Scheme 04", 1, 1, 2, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 3, false, true, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-14PI50K", "Seed FAS Scheme 05", 1, 2, 1, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 6, false, false, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-94EERHM", "Seed FAS Scheme 06", 1, 3, 2, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 9, false, true, null, "FAS-2026-Z2RKIYE", "Seed FAS Scheme 07", 1, 1, 1, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 12, false, false, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-3ZNQSMN", "Seed FAS Scheme 08", 1, 2, 2, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 3, false, true, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-T3E6QO3", "Seed FAS Scheme 09", 1, 3, 1, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme for school-admin scope.", 6, false, false, null, "FAS-2026-5AED0DC", "Seed FAS Scheme 10", 1, 1, 2, null, null }
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
                    { 10, 0, 1, null, 10 }
                });

            migrationBuilder.InsertData(
                table: "SsoIdentity",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Provider", "ProviderUserId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "5fc549a1-ee08-4273-9497-27607842e1f9", null, null, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "1fedf576-1c66-4742-881b-f7a456b2b027", null, null, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "78ce9568-1d38-44aa-a9c5-ea50293934de", null, null, 3 }
                });

            migrationBuilder.InsertData(
                table: "SystemTopupConditionGroup",
                columns: new[] { "Id", "DisplayOrder", "LogicalOperator", "ParentGroupId", "SystemTopupId" },
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
                    { 10, 0, 1, null, 10 }
                });

            migrationBuilder.InsertData(
                table: "TopupExecution",
                columns: new[] { "Id", "ConditionsSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutionCode", "FailedCount", "IdempotencyKey", "IsDeleted", "ManualAmount", "ManualReason", "ScheduleTopUpId", "SourceType", "Status", "SuccessCount", "SystemTopupId", "TopupAmountSnapshot", "TopupNameSnapshot", "TotalExecutedAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "Seed condition snapshot", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-001", 0, "topup-seed-001", false, null, null, null, 1, 1, 1, 1, 85m, "Seed Topup 01", 85m, 1, null, null },
                    { 2, "Seed condition snapshot", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-002", 0, "topup-seed-002", false, null, null, 2, 2, 2, 1, null, 95m, "Seed Topup 02", 95m, 1, null, null },
                    { 4, "Seed condition snapshot", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-004", 1, "topup-seed-004", false, null, null, null, 1, 1, 0, 4, 115m, "Seed Topup 04", 0m, 1, null, null },
                    { 5, "Seed condition snapshot", new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-005", 0, "topup-seed-005", false, null, null, 5, 2, 2, 1, null, 125m, "Seed Topup 05", 125m, 1, null, null },
                    { 7, "Seed condition snapshot", new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-007", 0, "topup-seed-007", false, null, null, null, 1, 1, 1, 7, 145m, "Seed Topup 07", 145m, 1, null, null },
                    { 8, "Seed condition snapshot", new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-008", 1, "topup-seed-008", false, null, null, 8, 2, 2, 0, null, 155m, "Seed Topup 08", 0m, 1, null, null },
                    { 10, "Seed condition snapshot", new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-SEED-010", 0, "topup-seed-010", false, null, null, null, 1, 1, 1, 10, 175m, "Seed Topup 10", 175m, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CitizenId", "CreatedAt", "CreatedBy", "DeletedAt", "FailedLoginCount", "IsDeleted", "LastLoginAt", "LockedUntil", "Role", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 4, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null });

            migrationBuilder.InsertData(
                table: "AuditLog",
                columns: new[] { "Id", "Action", "ActorUserId", "Category", "IpAddress", "Nric", "OccurredAt" },
                values: new object[,]
                {
                    { 4, "SeedAction04", 4, 4, "127.0.0.5", null, new DateTime(2026, 1, 1, 4, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "SeedAction08", 4, 1, "127.0.0.9", null, new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, "SeedAction12", 4, 5, "127.0.0.13", null, new DateTime(2026, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, "SeedAction16", 4, 2, "127.0.0.17", null, new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, "SeedAction20", 4, 6, "127.0.0.21", null, new DateTime(2026, 1, 1, 20, 0, 0, 0, DateTimeKind.Utc) },
                    { 24, "SeedAction24", 4, 3, "127.0.0.25", null, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 28, "SeedAction28", 4, 7, "127.0.0.29", null, new DateTime(2026, 1, 2, 4, 0, 0, 0, DateTimeKind.Utc) },
                    { 32, "SeedAction32", 4, 4, "127.0.0.33", null, new DateTime(2026, 1, 2, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 36, "SeedAction36", 4, 1, "127.0.0.37", null, new DateTime(2026, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 40, "SeedAction40", 4, 5, "127.0.0.41", null, new DateTime(2026, 1, 2, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 44, "SeedAction44", 4, 2, "127.0.0.45", null, new DateTime(2026, 1, 2, 20, 0, 0, 0, DateTimeKind.Utc) },
                    { 48, "SeedAction48", 4, 6, "127.0.0.49", null, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "EducationAccountStatusHistory",
                columns: new[] { "Id", "ChangedAt", "ChangedByUserId", "EducationAccountId", "NewStatus", "PreviousStatus", "Reason" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, 3, 1, "Seed account lifecycle history" },
                    { 2, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 2, 1, "Seed account lifecycle history" },
                    { 3, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3, 3, 1, "Seed account lifecycle history" },
                    { 4, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4, 2, 1, "Seed account lifecycle history" },
                    { 5, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5, 3, 1, "Seed account lifecycle history" },
                    { 6, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, 6, 2, 1, "Seed account lifecycle history" }
                });

            migrationBuilder.InsertData(
                table: "EducationCreditTransaction",
                columns: new[] { "Id", "Amount", "BalanceAfter", "BalanceBefore", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "Direction", "EducationAccountId", "IsDeleted", "TransactionCode", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 60m, 1080m, 1020m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 01", 1, 1, false, new Guid("49594d61-4376-40b8-ac80-939fdcc75d03"), 1, null, null },
                    { 2, 70m, 970m, 1040m, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 02", 2, 2, false, new Guid("e0318f15-fdda-4abf-9c6b-419497da2df5"), 2, null, null },
                    { 3, 80m, 980m, 1060m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 03", 2, 3, false, new Guid("3cc72b88-c9fe-4854-9f18-20216358e732"), 2, null, null },
                    { 4, 90m, 990m, 1080m, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 04", 2, 4, false, new Guid("20a47861-e67b-4d7d-a411-fa77f197d86b"), 4, null, null },
                    { 5, 100m, 1200m, 1100m, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 05", 1, 5, false, new Guid("13a7d8f6-33b2-457f-83c9-013639995db0"), 1, null, null },
                    { 6, 110m, 1010m, 1120m, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 06", 2, 6, false, new Guid("b173acbb-c707-4e20-8d42-f73c61af31eb"), 2, null, null },
                    { 7, 120m, 1020m, 1140m, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 07", 2, 7, false, new Guid("b9b22a3a-e1ed-445e-830d-3f7900433702"), 2, null, null },
                    { 8, 130m, 1030m, 1160m, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 08", 2, 8, false, new Guid("9c034e05-2ad7-48f6-9d0e-1d75f3a9dc11"), 4, null, null },
                    { 9, 140m, 1320m, 1180m, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 09", 1, 9, false, new Guid("11b9710a-dc40-4940-8346-3426a0b08cc1"), 1, null, null },
                    { 10, 50m, 1150m, 1200m, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 10", 2, 10, false, new Guid("534f9ac0-3749-48bf-9830-9c3a494e0357"), 2, null, null },
                    { 11, 60m, 1160m, 1220m, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 11", 2, 11, false, new Guid("5a7565fa-724d-4ed7-a659-c55f2b82b148"), 2, null, null },
                    { 12, 70m, 1170m, 1240m, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 12", 2, 12, false, new Guid("dce18490-c9c0-47d1-8888-bbc96fbeaa8f"), 4, null, null },
                    { 13, 80m, 1340m, 1260m, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 13", 1, 13, false, new Guid("26410860-8d9c-4653-b8dc-737d522a260c"), 1, null, null },
                    { 14, 90m, 1190m, 1280m, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 14", 2, 14, false, new Guid("56c40714-1195-4170-ab78-fde7334aac85"), 2, null, null },
                    { 15, 100m, 1200m, 1300m, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 15", 2, 15, false, new Guid("60b17549-097b-4586-bcc9-b7adcdd28ab0"), 2, null, null },
                    { 16, 110m, 1210m, 1320m, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 16", 2, 16, false, new Guid("411d1141-302f-4164-a767-0427a915ca08"), 4, null, null },
                    { 17, 120m, 1460m, 1340m, new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 17", 1, 17, false, new Guid("0bc2a282-c6de-47b7-98b0-fdac93855e55"), 1, null, null },
                    { 18, 130m, 1230m, 1360m, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 18", 2, 18, false, new Guid("3a8d08c2-da97-4c08-9195-9c6fd5486e4f"), 2, null, null },
                    { 19, 140m, 1240m, 1380m, new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 19", 2, 19, false, new Guid("8f433ea5-cfc6-4449-9915-76fba85d7474"), 2, null, null },
                    { 20, 50m, 1350m, 1400m, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 20", 2, 20, false, new Guid("5a3b8e16-fa7f-4ae2-b395-4dd8e27c3c25"), 4, null, null },
                    { 21, 60m, 1480m, 1420m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 21", 1, 21, false, new Guid("af161551-4185-4376-a70e-b0ad42e7d980"), 1, null, null },
                    { 22, 70m, 1370m, 1440m, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 22", 2, 22, false, new Guid("206fc099-fd74-4264-b317-04c0426e1be8"), 2, null, null },
                    { 23, 80m, 1380m, 1460m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 23", 2, 23, false, new Guid("56b65cbf-d647-4447-90df-1b52a3f11fb6"), 2, null, null },
                    { 24, 90m, 1390m, 1480m, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 24", 2, 24, false, new Guid("7867f9b5-1d38-4781-b786-4daeb46ad46f"), 4, null, null },
                    { 25, 100m, 1600m, 1500m, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 25", 1, 25, false, new Guid("641eb577-86a7-48aa-855c-66b19f1bb194"), 1, null, null },
                    { 26, 110m, 1410m, 1520m, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 26", 2, 26, false, new Guid("16b8a137-6519-48ba-95f8-00561fc14186"), 2, null, null },
                    { 27, 120m, 1420m, 1540m, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 27", 2, 27, false, new Guid("302e2943-b197-498a-92a2-3b56ee17c5c8"), 2, null, null },
                    { 28, 130m, 1430m, 1560m, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 28", 2, 28, false, new Guid("0a68c2bf-fa68-482d-97df-5c7e8fe02e12"), 4, null, null },
                    { 29, 140m, 1720m, 1580m, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 29", 1, 29, false, new Guid("03ed2f50-9240-41d8-8ed5-d4d029e1f377"), 1, null, null },
                    { 30, 50m, 1550m, 1600m, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 30", 2, 30, false, new Guid("947ff9d0-5119-4ae2-91b8-3c2ed6763e0a"), 2, null, null },
                    { 31, 60m, 1560m, 1620m, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 31", 2, 31, false, new Guid("41c77e92-62c7-4e1b-9228-2a440f58347b"), 2, null, null },
                    { 32, 70m, 1570m, 1640m, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 32", 2, 32, false, new Guid("19e0145e-509e-495b-a780-2fbca86bb7f0"), 4, null, null },
                    { 33, 80m, 1740m, 1660m, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 33", 1, 33, false, new Guid("4818a12d-3778-476f-818f-93513e797f82"), 1, null, null },
                    { 34, 90m, 1590m, 1680m, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 34", 2, 34, false, new Guid("b866b87d-d793-495b-ba11-a8e4adb05302"), 2, null, null },
                    { 35, 100m, 1600m, 1700m, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 35", 2, 35, false, new Guid("84a02f3a-4080-4a44-bee4-4c6c57238abd"), 2, null, null },
                    { 36, 110m, 1610m, 1720m, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 36", 2, 36, false, new Guid("ef705496-e433-4e7d-a27b-2f283fd15f2d"), 4, null, null },
                    { 37, 120m, 1860m, 1740m, new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 37", 1, 37, false, new Guid("a3a314a6-1a3c-4939-af36-3d1b4edcb340"), 1, null, null },
                    { 38, 130m, 1630m, 1760m, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 38", 2, 38, false, new Guid("f8033fe6-a6f8-4ee1-a7d6-d9d92638dd47"), 2, null, null },
                    { 39, 140m, 1640m, 1780m, new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 39", 2, 39, false, new Guid("e83276cf-8a65-4d36-8f34-56b2aceed589"), 2, null, null },
                    { 40, 50m, 1750m, 1800m, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 40", 2, 40, false, new Guid("66360922-c48e-4a2e-a027-a250c090621b"), 4, null, null },
                    { 41, 60m, 1880m, 1820m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 41", 1, 41, false, new Guid("b24cc55a-d9d6-4496-af44-3a66f7554f54"), 1, null, null },
                    { 42, 70m, 1770m, 1840m, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 42", 2, 42, false, new Guid("1ff96513-da7e-42f4-a069-3a2ca5d8683e"), 2, null, null },
                    { 43, 80m, 1780m, 1860m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 43", 2, 43, false, new Guid("d63ba3cb-e4e2-43c9-ac55-44c8471a39ca"), 2, null, null },
                    { 44, 90m, 1790m, 1880m, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 44", 2, 44, false, new Guid("a4b0db24-dd9d-46ad-bd0e-06338a7f223d"), 4, null, null },
                    { 45, 100m, 2000m, 1900m, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 45", 1, 45, false, new Guid("fc22cb9e-b54e-4be1-8da5-4f6bc19f5872"), 1, null, null },
                    { 46, 110m, 1810m, 1920m, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 46", 2, 46, false, new Guid("b8281119-e152-4b4b-a305-12a6cdac0443"), 2, null, null },
                    { 47, 120m, 1820m, 1940m, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 47", 2, 47, false, new Guid("aa3b2e4c-4168-41e1-9e2d-e42027d45872"), 2, null, null },
                    { 48, 130m, 1830m, 1960m, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 48", 2, 48, false, new Guid("f2568b2f-b965-4d82-8bb2-0d344af9df0c"), 4, null, null },
                    { 49, 140m, 2120m, 1980m, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 49", 1, 49, false, new Guid("20489516-1121-469e-8d21-e46a93a2fffc"), 1, null, null },
                    { 50, 50m, 1950m, 2000m, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 50", 2, 50, false, new Guid("ed9be019-597a-4217-9b0e-d0c47009c4ec"), 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeConditionGroup",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "FasSchemeId", "IsDeleted", "LogicalOperator", "ParentGroupId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 1, null, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 1, null, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 3, false, 1, null, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 4, false, 1, null, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 5, false, 1, null, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, false, 1, null, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 7, false, 1, null, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 8, false, 1, null, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 9, false, 1, null, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 10, false, 1, null, null, null }
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
                    { 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeRequiredDocument",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "DocumentName", "FasSchemeId", "IsDeleted", "TemplateFileKey", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 1, false, "fas/templates/document-01.pdf", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 2, false, "fas/templates/document-02.pdf", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 3, false, "fas/templates/document-03.pdf", null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 4, false, "fas/templates/document-04.pdf", null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 5, false, "fas/templates/document-05.pdf", null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 6, false, "fas/templates/document-06.pdf", null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 7, false, "fas/templates/document-07.pdf", null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 8, false, "fas/templates/document-08.pdf", null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Recent Payslip", 9, false, "fas/templates/document-09.pdf", null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 10, false, "fas/templates/document-10.pdf", null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeTier",
                columns: new[] { "Id", "CourseFeeSubsidyValue", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "FasSchemeId", "IsDeleted", "MaxPerCapitaIncome", "MiscFeeSubsidyValue", "SubsidyValue", "TierName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 11m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 750m, 6m, 21m, "Tier 1", null, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 800m, null, 70m, "Tier 2", null, null },
                    { 3, 13m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 3, false, 850m, 8m, 23m, "Tier 3", null, null },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 4, false, 900m, null, 90m, "Tier 1", null, null },
                    { 5, 15m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 5, false, 950m, 10m, 25m, "Tier 2", null, null },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, false, 1000m, null, 110m, "Tier 3", null, null },
                    { 7, 17m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 7, false, 1050m, 12m, 27m, "Tier 1", null, null },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 8, false, 1100m, null, 130m, "Tier 2", null, null },
                    { 9, 19m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 9, false, 1150m, 14m, 29m, "Tier 3", null, null },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 10, false, 1200m, null, 150m, "Tier 1", null, null }
                });

            migrationBuilder.InsertData(
                table: "ScheduleTopUpCondition",
                columns: new[] { "Id", "DisplayOrder", "Field", "GroupId", "Operator", "ValueNumber", "ValueNumberTo", "ValueText" },
                values: new object[,]
                {
                    { 1, 0, 2, 1, 6, 550m, null, null },
                    { 2, 0, 2, 2, 6, 600m, null, null },
                    { 3, 0, 2, 3, 6, 650m, null, null },
                    { 4, 0, 2, 4, 6, 700m, null, null },
                    { 5, 0, 2, 5, 6, 750m, null, null },
                    { 6, 0, 2, 6, 6, 800m, null, null },
                    { 7, 0, 2, 7, 6, 850m, null, null },
                    { 8, 0, 2, 8, 6, 900m, null, null },
                    { 9, 0, 2, 9, 6, 950m, null, null },
                    { 10, 0, 2, 10, 6, 1000m, null, null }
                });

            migrationBuilder.InsertData(
                table: "SchoolStudent",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "IsDeleted", "SchoolId", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 1, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 1, 1, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, 1, 1, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, 1, 1, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, 1, 1, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, 1, 1, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, 1, 1, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, 1, 1, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, 1, 1, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, 1, 2, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, false, 1, 1, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, false, 1, 1, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, false, 1, 1, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, false, 1, 1, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, false, 1, 1, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, false, 1, 1, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, false, 1, 1, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, false, 1, 1, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, false, 1, 1, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, false, 1, 2, null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, false, 1, 1, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, false, 1, 1, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 23, false, 1, 1, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 24, false, 1, 1, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 25, false, 1, 1, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 26, false, 1, 1, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, false, 1, 1, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 28, false, 1, 1, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 29, false, 1, 1, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 30, false, 1, 2, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 31, false, 1, 1, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 32, false, 1, 1, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 33, false, 1, 1, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 34, false, 1, 1, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 35, false, 1, 1, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 36, false, 1, 1, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 37, false, 1, 1, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 38, false, 1, 1, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 39, false, 1, 1, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 40, false, 1, 2, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 41, false, 1, 1, null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 42, false, 1, 1, null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 43, false, 1, 1, null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 44, false, 1, 1, null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 45, false, 1, 1, null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 46, false, 1, 1, null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 47, false, 1, 1, null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 48, false, 1, 1, null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 49, false, 1, 1, null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 50, false, 1, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "SsoIdentity",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Provider", "ProviderUserId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[] { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-004", null, null, 4 });

            migrationBuilder.InsertData(
                table: "SystemTopupCondition",
                columns: new[] { "Id", "DisplayOrder", "Field", "GroupId", "Operator", "ValueNumber", "ValueNumberTo", "ValueText" },
                values: new object[,]
                {
                    { 1, 0, 3, 1, 1, null, null, "Enrolled" },
                    { 2, 0, 3, 2, 1, null, null, "Enrolled" },
                    { 3, 0, 3, 3, 1, null, null, "Enrolled" },
                    { 4, 0, 3, 4, 1, null, null, "Enrolled" },
                    { 5, 0, 3, 5, 1, null, null, "Enrolled" },
                    { 6, 0, 3, 6, 1, null, null, "Enrolled" },
                    { 7, 0, 3, 7, 1, null, null, "Enrolled" },
                    { 8, 0, 3, 8, 1, null, null, "Enrolled" },
                    { 9, 0, 3, 9, 1, null, null, "Enrolled" },
                    { 10, 0, 3, 10, 1, null, null, "Enrolled" }
                });

            migrationBuilder.InsertData(
                table: "TopupExecutionTarget",
                columns: new[] { "Id", "AccountNumber", "Amount", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "EducationCreditTransactionId", "FailureReason", "IsDeleted", "MatchedConditionsSnapshot", "Status", "TopupExecutionId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-M7QVS7X", 85m, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 1, null, null, false, "Seed matched condition", 1, 1, null, null },
                    { 2, "EDU-2026-H4W9JYQ", 95m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 2, null, null, false, "Seed matched condition", 2, 2, null, null },
                    { 4, "EDU-2026-K6CAJET", 115m, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 4, null, "Seed failure for review", false, "Seed matched condition", 4, 4, null, null },
                    { 5, "EDU-2026-C865DQ8", 125m, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 5, null, null, false, "Seed matched condition", 1, 5, null, null },
                    { 6, "EDU-2026-J3PD5GI", 135m, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 6, null, null, false, "Seed matched condition", 2, 6, null, null },
                    { 8, "EDU-2026-ERZPP9D", 155m, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 8, null, "Seed failure for review", false, "Seed matched condition", 4, 8, null, null },
                    { 9, "EDU-2026-3WTXQL2", 165m, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 9, null, null, false, "Seed matched condition", 1, 9, null, null },
                    { 10, "EDU-2026-5A2BWXE", 175m, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 10, null, null, false, "Seed matched condition", 2, 10, null, null }
                });

            migrationBuilder.InsertData(
                table: "Enrollment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenEmailSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CitizenPhoneNumberSnapshot", "CourseDescriptionSnapshot", "CourseId", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "SchoolNameSnapshot", "SchoolStudentId", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-M7QVS7X", "sterling.quach@example.com", "Sterling Quach", "S0000001I", "+6590000001", "Seeded course enrollment for scoped school-admin data.", 1, "Academic Writing Cohort 01", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 1, 1, null, null },
                    { 2, "EDU-2026-H4W9JYQ", "amelia.tan@example.com", "Amelia Tan", "S0000002G", "+6590000002", "Seeded course enrollment for scoped school-admin data.", 2, "Business Numeracy Cohort 02", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 2, 1, null, null },
                    { 3, "EDU-2026-5BHC84W", "marcus.lim@example.com", "Marcus Lim", "S0000003E", "+6590000003", "Seeded course enrollment for scoped school-admin data.", 3, "Digital Literacy Cohort 03", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 3, 1, null, null },
                    { 4, "EDU-2026-K6CAJET", "priya.nair@example.com", "Priya Nair", "S0000004C", "+6590000004", "Seeded course enrollment for scoped school-admin data.", 4, "Career Readiness Cohort 04", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 4, 1, null, null },
                    { 5, "EDU-2026-C865DQ8", "ethan.koh@example.com", "Ethan Koh", "S0000005A", "+6590000005", "Seeded course enrollment for scoped school-admin data.", 5, "Applied Science Cohort 05", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 5, 1, null, null },
                    { 6, "EDU-2026-J3PD5GI", "hannah.lee@example.com", "Hannah Lee", "S0000006Z", "+6590000006", "Seeded course enrollment for scoped school-admin data.", 6, "Financial Literacy Cohort 06", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 6, 1, null, null },
                    { 7, "EDU-2026-RHEGIE7", "daniel.wong@example.com", "Daniel Wong", "S0000007H", "+6590000007", "Seeded course enrollment for scoped school-admin data.", 7, "Project Collaboration Cohort 07", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 7, 1, null, null },
                    { 8, "EDU-2026-ERZPP9D", "sofia.chen@example.com", "Sofia Chen", "S0000008F", "+6590000008", "Seeded course enrollment for scoped school-admin data.", 8, "Data Skills Cohort 08", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 8, 2, null, null },
                    { 9, "EDU-2026-3WTXQL2", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Seeded course enrollment for scoped school-admin data.", 9, "Workplace Communication Cohort 09", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 9, 1, null, null },
                    { 10, "EDU-2026-5A2BWXE", "maya.rahman@example.com", "Maya Rahman", "S0000010H", "+6590000010", "Seeded course enrollment for scoped school-admin data.", 10, "Software Foundations Cohort 10", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 10, 1, null, null },
                    { 11, "EDU-2026-YKWELYD", "noah.teo@example.com", "Noah Teo", "S0000011F", "+6590000011", "Seeded course enrollment for scoped school-admin data.", 11, "Academic Writing Cohort 11", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 11, 1, null, null },
                    { 12, "EDU-2026-XW95AJ5", "aisha.fernandez@example.com", "Aisha Fernandez", "S0000012D", "+6590000012", "Seeded course enrollment for scoped school-admin data.", 12, "Business Numeracy Cohort 12", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 12, 1, null, null },
                    { 13, "EDU-2026-OOROKGZ", "ryan.chua@example.com", "Ryan Chua", "S0000013B", "+6590000013", "Seeded course enrollment for scoped school-admin data.", 13, "Digital Literacy Cohort 13", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 13, 1, null, null },
                    { 14, "EDU-2026-EC0POYJ", "chloe.goh@example.com", "Chloe Goh", "S0000014J", "+6590000014", "Seeded course enrollment for scoped school-admin data.", 14, "Career Readiness Cohort 14", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 14, 1, null, null },
                    { 15, "EDU-2026-1ZHBQ2S", "irfan.hassan@example.com", "Irfan Hassan", "S0000015I", "+6590000015", "Seeded course enrollment for scoped school-admin data.", 15, "Applied Science Cohort 15", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 15, 1, null, null },
                    { 16, "EDU-2026-A5L21GT", "natalie.seah@example.com", "Natalie Seah", "S0000016G", "+6590000016", "Seeded course enrollment for scoped school-admin data.", 16, "Financial Literacy Cohort 16", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 16, 2, null, null },
                    { 17, "EDU-2026-R9VEYR0", "alina.ang@example.com", "Alina Ang", "S0000017E", "+6590000017", "Seeded course enrollment for scoped school-admin data.", 17, "Project Collaboration Cohort 17", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 17, 1, null, null },
                    { 18, "EDU-2026-JLH113O", "benjamin.bala@example.com", "Benjamin Bala", "S0000018C", "+6590000018", "Seeded course enrollment for scoped school-admin data.", 18, "Data Skills Cohort 18", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 18, 1, null, null },
                    { 19, "EDU-2026-S7OK05W", "clara.chew@example.com", "Clara Chew", "S0000019A", "+6590000019", "Seeded course enrollment for scoped school-admin data.", 19, "Workplace Communication Cohort 19", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 19, 1, null, null },
                    { 20, "EDU-2026-2MYNLK1", "darius.das@example.com", "Darius Das", "S0000020E", "+6590000020", "Seeded course enrollment for scoped school-admin data.", 20, "Software Foundations Cohort 20", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 20, 1, null, null },
                    { 21, "EDU-2026-7M9PDGH", "elena.eng@example.com", "Elena Eng", "S0000021C", "+6590000021", "Seeded course enrollment for scoped school-admin data.", 21, "Academic Writing Cohort 21", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 21, 1, null, null },
                    { 22, "EDU-2026-TIXLNIB", "farhan.foo@example.com", "Farhan Foo", "S0000022A", "+6590000022", "Seeded course enrollment for scoped school-admin data.", 22, "Business Numeracy Cohort 22", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 22, 1, null, null },
                    { 23, "EDU-2026-T5N58HB", "grace.gan@example.com", "Grace Gan", "S0000023Z", "+6590000023", "Seeded course enrollment for scoped school-admin data.", 23, "Digital Literacy Cohort 23", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 23, 1, null, null },
                    { 24, "EDU-2026-7MH1FBP", "haruto.ho@example.com", "Haruto Ho", "S0000024H", "+6590000024", "Seeded course enrollment for scoped school-admin data.", 24, "Career Readiness Cohort 24", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 24, 2, null, null },
                    { 25, "EDU-2026-GDE8Q6P", "isabelle.ismail@example.com", "Isabelle Ismail", "S0000025F", "+6590000025", "Seeded course enrollment for scoped school-admin data.", 25, "Applied Science Cohort 25", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 25, 1, null, null },
                    { 26, "EDU-2026-SNYKN82", "jasper.jeyaratnam@example.com", "Jasper Jeyaratnam", "S0000026D", "+6590000026", "Seeded course enrollment for scoped school-admin data.", 26, "Financial Literacy Cohort 26", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 26, 1, null, null },
                    { 27, "EDU-2026-QJZM8YL", "keira.kwek@example.com", "Keira Kwek", "S0000027B", "+6590000027", "Seeded course enrollment for scoped school-admin data.", 27, "Project Collaboration Cohort 27", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 27, 1, null, null },
                    { 28, "EDU-2026-G0N8QXW", "leon.lim@example.com", "Leon Lim", "S0000028J", "+6590000028", "Seeded course enrollment for scoped school-admin data.", 28, "Data Skills Cohort 28", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 28, 1, null, null },
                    { 29, "EDU-2026-A3B8IRY", "mei.lin.mohamed@example.com", "Mei Lin Mohamed", "S0000029I", "+6590000029", "Seeded course enrollment for scoped school-admin data.", 29, "Workplace Communication Cohort 29", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 29, 1, null, null },
                    { 30, "EDU-2026-CYOBPQU", "nathan.ng@example.com", "Nathan Ng", "S0000030B", "+6590000030", "Seeded course enrollment for scoped school-admin data.", 30, "Software Foundations Cohort 30", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 30, 1, null, null },
                    { 31, "EDU-2026-20G2H3L", "olivia.ong@example.com", "Olivia Ong", "S0000031J", "+6590000031", "Seeded course enrollment for scoped school-admin data.", 31, "Academic Writing Cohort 31", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 31, 1, null, null },
                    { 32, "EDU-2026-9RY98UG", "pranav.pillai@example.com", "Pranav Pillai", "S0000032I", "+6590000032", "Seeded course enrollment for scoped school-admin data.", 32, "Business Numeracy Cohort 32", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 32, 2, null, null },
                    { 33, "EDU-2026-LBUTZEW", "qistina.quek@example.com", "Qistina Quek", "S0000033G", "+6590000033", "Seeded course enrollment for scoped school-admin data.", 33, "Digital Literacy Cohort 33", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 33, 1, null, null },
                    { 34, "EDU-2026-CVJPW51", "rafael.rao@example.com", "Rafael Rao", "S0000034E", "+6590000034", "Seeded course enrollment for scoped school-admin data.", 34, "Career Readiness Cohort 34", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 34, 1, null, null },
                    { 35, "EDU-2026-GL31H3K", "selina.sim@example.com", "Selina Sim", "S0000035C", "+6590000035", "Seeded course enrollment for scoped school-admin data.", 35, "Applied Science Cohort 35", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 35, 1, null, null },
                    { 36, "EDU-2026-L1OX9Y0", "terence.tan@example.com", "Terence Tan", "S0000036A", "+6590000036", "Seeded course enrollment for scoped school-admin data.", 36, "Financial Literacy Cohort 36", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 36, 1, null, null },
                    { 37, "EDU-2026-3V9Y2I0", "umairah.uddin@example.com", "Umairah Uddin", "S0000037Z", "+6590000037", "Seeded course enrollment for scoped school-admin data.", 37, "Project Collaboration Cohort 37", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 37, 1, null, null },
                    { 38, "EDU-2026-UIX61F4", "victor.vasquez@example.com", "Victor Vasquez", "S0000038H", "+6590000038", "Seeded course enrollment for scoped school-admin data.", 38, "Data Skills Cohort 38", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 38, 1, null, null },
                    { 39, "EDU-2026-NQZL2LT", "wen.jie.wong@example.com", "Wen Jie Wong", "S0000039F", "+6590000039", "Seeded course enrollment for scoped school-admin data.", 39, "Workplace Communication Cohort 39", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 39, 1, null, null },
                    { 40, "EDU-2026-2A9X7HT", "xavier.xu@example.com", "Xavier Xu", "S0000040Z", "+6590000040", "Seeded course enrollment for scoped school-admin data.", 40, "Software Foundations Cohort 40", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 40, 2, null, null },
                    { 41, "EDU-2026-HIZFYQR", "yasmin.yeo@example.com", "Yasmin Yeo", "S0000041H", "+6590000041", "Seeded course enrollment for scoped school-admin data.", 41, "Academic Writing Cohort 41", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 41, 1, null, null },
                    { 42, "EDU-2026-5FO1ERB", "zachary.zainal@example.com", "Zachary Zainal", "S0000042F", "+6590000042", "Seeded course enrollment for scoped school-admin data.", 42, "Business Numeracy Cohort 42", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 42, 1, null, null },
                    { 43, "EDU-2026-C6GLP5G", "adeline.ang@example.com", "Adeline Ang", "S0000043D", "+6590000043", "Seeded course enrollment for scoped school-admin data.", 43, "Digital Literacy Cohort 43", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 43, 1, null, null },
                    { 44, "EDU-2026-VBGBHB4", "brandon.bala@example.com", "Brandon Bala", "S0000044B", "+6590000044", "Seeded course enrollment for scoped school-admin data.", 44, "Career Readiness Cohort 44", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 44, 1, null, null },
                    { 45, "EDU-2026-PMQT1K5", "celeste.chew@example.com", "Celeste Chew", "S0000045J", "+6590000045", "Seeded course enrollment for scoped school-admin data.", 45, "Applied Science Cohort 45", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 45, 1, null, null },
                    { 46, "EDU-2026-Z2MR7S3", "damien.das@example.com", "Damien Das", "S0000046I", "+6590000046", "Seeded course enrollment for scoped school-admin data.", 46, "Financial Literacy Cohort 46", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 46, 1, null, null },
                    { 47, "EDU-2026-2OK04RQ", "evelyn.eng@example.com", "Evelyn Eng", "S0000047G", "+6590000047", "Seeded course enrollment for scoped school-admin data.", 47, "Project Collaboration Cohort 47", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 47, 1, null, null },
                    { 48, "EDU-2026-5L2W56M", "faris.foo@example.com", "Faris Foo", "S0000048E", "+6590000048", "Seeded course enrollment for scoped school-admin data.", 48, "Data Skills Cohort 48", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 48, 2, null, null },
                    { 49, "EDU-2026-7UZZHKN", "giselle.gan@example.com", "Giselle Gan", "S0000049C", "+6590000049", "Seeded course enrollment for scoped school-admin data.", 49, "Workplace Communication Cohort 49", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 49, 1, null, null },
                    { 50, "EDU-2026-9DG3ZSZ", "haziq.ho@example.com", "Haziq Ho", "S0000050G", "+6590000050", "Seeded course enrollment for scoped school-admin data.", 50, "Software Foundations Cohort 50", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 50, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasApplication",
                columns: new[] { "Id", "ApplicationNumber", "ApprovedAt", "ApprovedByUserId", "ApprovedTierId", "CreatedAt", "CreatedBy", "DeletedAt", "DurationInMonthsSnapshot", "FasSchemeId", "GrossHouseholdIncomeSnapshot", "GuardianNationalitySnapshot", "HouseholdMemberCountSnapshot", "IsDeleted", "PerCapitaIncomeSnapshot", "RecommendationReason", "RecommendedTierId", "RejectionReason", "SchoolStudentId", "Status", "StudentAgeSnapshot", "StudentNationalitySnapshot", "UpdatedAt", "UpdatedBy", "ValidityEndDate", "ValidityStartDate", "WithdrawnAt" },
                values: new object[,]
                {
                    { 1, "FASAPP-20260101-A1B2C3D", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 1, 2500m, 1, 4, false, 625m, "PCI <= 750", 1, null, 1, 2, 18, 1, null, null, new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 2, "FASAPP-20260102-E4F5G6H", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 2, 3000m, 2, 4, false, 750m, "GHI <= 3500", 2, null, 2, 2, 17, 1, null, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 3, "FASAPP-20260103-I7J8K9L", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 3, 2200m, 1, 5, false, 440m, "Singapore citizen and PCI <= 690", 3, null, 3, 2, 19, 1, null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 4, "FASAPP-20260104-M1N2P3Q", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 4, 3600m, 2, 4, false, 900m, "Draft state", 4, null, 4, 5, 20, 1, null, null, null, null, null },
                    { 5, "FASAPP-20260105-R4S5T6U", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 5, 7000m, 2, 4, false, 1750m, "No tier matched", 5, "Income exceeds supported threshold.", 5, 3, 21, 1, null, null, null, null, null },
                    { 6, "FASAPP-20260106-V7W8X9Y", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, 2800m, 1, 5, false, 560m, "Student withdrew before review", 6, null, 6, 4, 18, 1, null, null, null, null, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "FASAPP-20260107-Z1A2B3C", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 7, 4800m, 1, 4, false, 1200m, "Special needs support threshold matched", 7, null, 7, 2, 16, 1, null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 8, "FASAPP-20260108-D4E5F6G", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 8, 3200m, 2, 5, false, 640m, "PCI <= 850", 8, null, 8, 6, 22, 1, null, null, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 9, "FASAPP-20260109-H7J8K9L", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 9, 2600m, 2, 3, false, 866.67m, "Emergency aid review required", 9, null, 9, 1, 17, 1, null, null, null, null, null },
                    { 10, "FASAPP-20260110-M1N2P3Q", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 10, 3900m, 1, 5, false, 780m, "PCI <= 1000", 10, null, 10, 2, 18, 1, null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 11, "FASAPP-GEN-0000", null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 1, 2000m, 1, 3, false, 666.67m, "Auto generated reason 0", 1, null, 1, 1, 16, 2, null, null, null, null, null },
                    { 12, "FASAPP-GEN-0001", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 2, 2100m, 1, 4, false, 525m, "Auto generated reason 1", 2, null, 1, 2, 17, 1, null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 13, "FASAPP-GEN-0002", null, null, null, new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 3, 2200m, 1, 5, false, 440m, "Auto generated reason 2", 3, "Does not meet requirements.", 1, 3, 18, 1, null, null, null, null, null },
                    { 14, "FASAPP-GEN-0003", null, null, null, new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 4, 2300m, 1, 6, false, 383.33m, "Auto generated reason 3", 4, null, 1, 4, 19, 2, null, null, null, null, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, "FASAPP-GEN-0004", null, null, null, new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 5, 2400m, 1, 3, false, 800m, "Auto generated reason 4", 5, null, 1, 5, 20, 1, null, null, null, null, null },
                    { 16, "FASAPP-GEN-0005", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 6, new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 6, 2500m, 1, 4, false, 625m, "Auto generated reason 5", 6, null, 1, 6, 21, 1, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 17, "FASAPP-GEN-0006", null, null, null, new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 7, 2600m, 1, 5, false, 520m, "Auto generated reason 6", 7, null, 1, 1, 16, 2, null, null, null, null, null },
                    { 18, "FASAPP-GEN-0007", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 8, new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 8, 2700m, 1, 6, false, 450m, "Auto generated reason 7", 8, null, 1, 2, 17, 1, null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 19, "FASAPP-GEN-0008", null, null, null, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 9, 2800m, 1, 3, false, 933.33m, "Auto generated reason 8", 9, "Does not meet requirements.", 1, 3, 18, 1, null, null, null, null, null },
                    { 20, "FASAPP-GEN-0009", null, null, null, new DateTime(2025, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 10, 2900m, 1, 4, false, 725m, "Auto generated reason 9", 10, null, 1, 4, 19, 2, null, null, null, null, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, "FASAPP-GEN-0010", null, null, null, new DateTime(2025, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 1, 3000m, 1, 5, false, 600m, "Auto generated reason 10", 1, null, 1, 5, 20, 1, null, null, null, null, null },
                    { 22, "FASAPP-GEN-0011", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 2, 3100m, 1, 6, false, 516.67m, "Auto generated reason 11", 2, null, 1, 6, 21, 1, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 23, "FASAPP-GEN-0012", null, null, null, new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 3, 3200m, 1, 3, false, 1066.67m, "Auto generated reason 12", 3, null, 1, 1, 16, 2, null, null, null, null, null },
                    { 24, "FASAPP-GEN-0013", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4, new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 4, 3300m, 1, 4, false, 825m, "Auto generated reason 13", 4, null, 1, 2, 17, 1, null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 25, "FASAPP-GEN-0014", null, null, null, new DateTime(2025, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 5, 3400m, 1, 5, false, 680m, "Auto generated reason 14", 5, "Does not meet requirements.", 1, 3, 18, 1, null, null, null, null, null },
                    { 26, "FASAPP-GEN-0015", null, null, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, 3500m, 1, 6, false, 583.33m, "Auto generated reason 15", 6, null, 1, 4, 19, 2, null, null, null, null, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 27, "FASAPP-GEN-0016", null, null, null, new DateTime(2025, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 7, 3600m, 1, 3, false, 1200m, "Auto generated reason 16", 7, null, 1, 5, 20, 1, null, null, null, null, null },
                    { 28, "FASAPP-GEN-0017", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 8, new DateTime(2024, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 8, 3700m, 1, 4, false, 925m, "Auto generated reason 17", 8, null, 1, 6, 21, 1, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 29, "FASAPP-GEN-0018", null, null, null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 9, 3800m, 1, 5, false, 760m, "Auto generated reason 18", 9, null, 1, 1, 16, 2, null, null, null, null, null },
                    { 30, "FASAPP-GEN-0019", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 10, new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 10, 3900m, 1, 6, false, 650m, "Auto generated reason 19", 10, null, 1, 2, 17, 1, null, null, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 31, "FASAPP-GEN-0020", null, null, null, new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 1, 4000m, 1, 3, false, 1333.33m, "Auto generated reason 20", 1, "Does not meet requirements.", 1, 3, 18, 1, null, null, null, null, null },
                    { 32, "FASAPP-GEN-0021", null, null, null, new DateTime(2025, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 2, 4100m, 1, 4, false, 1025m, "Auto generated reason 21", 2, null, 1, 4, 19, 2, null, null, null, null, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 33, "FASAPP-GEN-0022", null, null, null, new DateTime(2025, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 3, 4200m, 1, 5, false, 840m, "Auto generated reason 22", 3, null, 1, 5, 20, 1, null, null, null, null, null },
                    { 34, "FASAPP-GEN-0023", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4, new DateTime(2024, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 4, 4300m, 1, 6, false, 716.67m, "Auto generated reason 23", 4, null, 1, 6, 21, 1, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 35, "FASAPP-GEN-0024", null, null, null, new DateTime(2025, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 5, 4400m, 1, 3, false, 1466.67m, "Auto generated reason 24", 5, null, 1, 1, 16, 2, null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeCondition",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "Field", "GroupId", "IsDeleted", "Operator", "UpdatedAt", "UpdatedBy", "ValueNumber", "ValueNumberTo", "ValueText" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 1, false, 4, null, null, 750m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 2, false, 4, null, null, 800m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 3, false, 4, null, null, 850m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 4, false, 4, null, null, 900m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 5, false, 4, null, null, 950m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 6, false, 4, null, null, 1000m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 7, false, 4, null, null, 1050m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 8, false, 4, null, null, 1100m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 9, false, 4, null, null, 1150m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 10, false, 4, null, null, 1200m, null, null }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditTransactionId", "ExternalReference", "IsDeleted", "PaidAt", "PaymentMethod", "Status", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-M7QVS7X", "Sterling Quach", "S0000001I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, null, false, null, 1, 1, 124m, null, null },
                    { 2, "EDU-2026-H4W9JYQ", "Amelia Tan", "S0000002G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, "PAY-EXT-00002", false, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 128m, null, null },
                    { 3, "EDU-2026-5BHC84W", "Marcus Lim", "S0000003E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, null, false, null, 1, 3, 132m, null, null },
                    { 4, "EDU-2026-K6CAJET", "Priya Nair", "S0000004C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, "PAY-EXT-00004", false, null, 2, 1, 136m, null, null },
                    { 5, "EDU-2026-C865DQ8", "Ethan Koh", "S0000005A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, null, false, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 140m, null, null },
                    { 6, "EDU-2026-J3PD5GI", "Hannah Lee", "S0000006Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, "PAY-EXT-00006", false, null, 2, 3, 144m, null, null },
                    { 7, "EDU-2026-RHEGIE7", "Daniel Wong", "S0000007H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, null, false, null, 1, 1, 148m, null, null },
                    { 8, "EDU-2026-ERZPP9D", "Sofia Chen", "S0000008F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, "PAY-EXT-00008", false, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 152m, null, null },
                    { 9, "EDU-2026-3WTXQL2", "Lucas Nguyen", "S0000009D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, null, false, null, 1, 3, 156m, null, null },
                    { 10, "EDU-2026-5A2BWXE", "Maya Rahman", "S0000010H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, "PAY-EXT-00010", false, null, 2, 1, 160m, null, null },
                    { 11, "EDU-2026-YKWELYD", "Noah Teo", "S0000011F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, null, false, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 164m, null, null },
                    { 12, "EDU-2026-XW95AJ5", "Aisha Fernandez", "S0000012D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, "PAY-EXT-00012", false, null, 2, 3, 168m, null, null },
                    { 13, "EDU-2026-OOROKGZ", "Ryan Chua", "S0000013B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, null, false, null, 1, 1, 172m, null, null },
                    { 14, "EDU-2026-EC0POYJ", "Chloe Goh", "S0000014J", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, "PAY-EXT-00014", false, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 176m, null, null },
                    { 15, "EDU-2026-1ZHBQ2S", "Irfan Hassan", "S0000015I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, null, false, null, 1, 3, 180m, null, null },
                    { 16, "EDU-2026-A5L21GT", "Natalie Seah", "S0000016G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, "PAY-EXT-00016", false, null, 2, 1, 184m, null, null },
                    { 17, "EDU-2026-R9VEYR0", "Alina Ang", "S0000017E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, null, false, new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 188m, null, null },
                    { 18, "EDU-2026-JLH113O", "Benjamin Bala", "S0000018C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, "PAY-EXT-00018", false, null, 2, 3, 192m, null, null },
                    { 19, "EDU-2026-S7OK05W", "Clara Chew", "S0000019A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, null, false, null, 1, 1, 196m, null, null },
                    { 20, "EDU-2026-2MYNLK1", "Darius Das", "S0000020E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, "PAY-EXT-00020", false, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 200m, null, null },
                    { 21, "EDU-2026-7M9PDGH", "Elena Eng", "S0000021C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, null, false, null, 1, 3, 204m, null, null },
                    { 22, "EDU-2026-TIXLNIB", "Farhan Foo", "S0000022A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, "PAY-EXT-00022", false, null, 2, 1, 208m, null, null },
                    { 23, "EDU-2026-T5N58HB", "Grace Gan", "S0000023Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 23, null, false, new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 212m, null, null },
                    { 24, "EDU-2026-7MH1FBP", "Haruto Ho", "S0000024H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 24, "PAY-EXT-00024", false, null, 2, 3, 216m, null, null },
                    { 25, "EDU-2026-GDE8Q6P", "Isabelle Ismail", "S0000025F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 25, null, false, null, 1, 1, 220m, null, null },
                    { 26, "EDU-2026-SNYKN82", "Jasper Jeyaratnam", "S0000026D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 26, "PAY-EXT-00026", false, new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 224m, null, null },
                    { 27, "EDU-2026-QJZM8YL", "Keira Kwek", "S0000027B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, null, false, null, 1, 3, 228m, null, null },
                    { 28, "EDU-2026-G0N8QXW", "Leon Lim", "S0000028J", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 28, "PAY-EXT-00028", false, null, 2, 1, 232m, null, null },
                    { 29, "EDU-2026-A3B8IRY", "Mei Lin Mohamed", "S0000029I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 29, null, false, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 236m, null, null },
                    { 30, "EDU-2026-CYOBPQU", "Nathan Ng", "S0000030B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 30, "PAY-EXT-00030", false, null, 2, 3, 240m, null, null },
                    { 31, "EDU-2026-20G2H3L", "Olivia Ong", "S0000031J", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 31, null, false, null, 1, 1, 244m, null, null },
                    { 32, "EDU-2026-9RY98UG", "Pranav Pillai", "S0000032I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 32, "PAY-EXT-00032", false, new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 248m, null, null },
                    { 33, "EDU-2026-LBUTZEW", "Qistina Quek", "S0000033G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 33, null, false, null, 1, 3, 252m, null, null },
                    { 34, "EDU-2026-CVJPW51", "Rafael Rao", "S0000034E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 34, "PAY-EXT-00034", false, null, 2, 1, 256m, null, null },
                    { 35, "EDU-2026-GL31H3K", "Selina Sim", "S0000035C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 35, null, false, new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 260m, null, null },
                    { 36, "EDU-2026-L1OX9Y0", "Terence Tan", "S0000036A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 36, "PAY-EXT-00036", false, null, 2, 3, 264m, null, null },
                    { 37, "EDU-2026-3V9Y2I0", "Umairah Uddin", "S0000037Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 37, null, false, null, 1, 1, 268m, null, null },
                    { 38, "EDU-2026-UIX61F4", "Victor Vasquez", "S0000038H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 38, "PAY-EXT-00038", false, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 272m, null, null },
                    { 39, "EDU-2026-NQZL2LT", "Wen Jie Wong", "S0000039F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 39, null, false, null, 1, 3, 276m, null, null },
                    { 40, "EDU-2026-2A9X7HT", "Xavier Xu", "S0000040Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 40, "PAY-EXT-00040", false, null, 2, 1, 280m, null, null },
                    { 41, "EDU-2026-HIZFYQR", "Yasmin Yeo", "S0000041H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 41, null, false, new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 284m, null, null },
                    { 42, "EDU-2026-5FO1ERB", "Zachary Zainal", "S0000042F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 42, "PAY-EXT-00042", false, null, 2, 3, 288m, null, null },
                    { 43, "EDU-2026-C6GLP5G", "Adeline Ang", "S0000043D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 43, null, false, null, 1, 1, 292m, null, null },
                    { 44, "EDU-2026-VBGBHB4", "Brandon Bala", "S0000044B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 44, "PAY-EXT-00044", false, new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 296m, null, null },
                    { 45, "EDU-2026-PMQT1K5", "Celeste Chew", "S0000045J", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 45, null, false, null, 1, 3, 300m, null, null },
                    { 46, "EDU-2026-Z2MR7S3", "Damien Das", "S0000046I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 46, "PAY-EXT-00046", false, null, 2, 1, 304m, null, null },
                    { 47, "EDU-2026-2OK04RQ", "Evelyn Eng", "S0000047G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 47, null, false, new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 308m, null, null },
                    { 48, "EDU-2026-5L2W56M", "Faris Foo", "S0000048E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 48, "PAY-EXT-00048", false, null, 2, 3, 312m, null, null },
                    { 49, "EDU-2026-7UZZHKN", "Giselle Gan", "S0000049C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 49, null, false, null, 1, 1, 316m, null, null },
                    { 50, "EDU-2026-9DG3ZSZ", "Haziq Ho", "S0000050G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 50, "PAY-EXT-00050", false, new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 320m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupExecutionTarget",
                columns: new[] { "Id", "AccountNumber", "Amount", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "EducationCreditTransactionId", "FailureReason", "IsDeleted", "MatchedConditionsSnapshot", "Status", "TopupExecutionId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 3, "EDU-2026-5BHC84W", 105m, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 3, 3, null, false, "Seed matched condition", 3, 3, null, null },
                    { 7, "EDU-2026-RHEGIE7", 145m, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 7, 7, null, false, "Seed matched condition", 3, 7, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupSystemApplication",
                columns: new[] { "Id", "EducationAccountId", "SystemTopupId", "TopupExecutionTargetId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 2, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Charge",
                columns: new[] { "Id", "AppliedFasApplicationId", "AppliedFasCourseFeeSubsidyValueSnapshot", "AppliedFasIsPerComponentSnapshot", "AppliedFasMiscFeeSubsidyValueSnapshot", "AppliedFasSchemeNameSnapshot", "AppliedFasSubsidyTypeSnapshot", "AppliedFasSubsidyValueSnapshot", "AppliedFasTierNameSnapshot", "CourseCodeSnapshot", "CourseDescriptionSnapshot", "CourseEndDateSnapshot", "CourseFeeAmountSnapshot", "CourseNameSnapshot", "CourseStartDateSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EnrollmentId", "GrossAmount", "GstAmountSnapshot", "IsDeleted", "MiscFeeAmountSnapshot", "NetAmount", "PaidAmount", "PaymentPlanMonths", "RemainingAmount", "SchoolNameSnapshot", "Status", "SubsidyAmount", "TaxRateSnapshot", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, null, false, null, null, null, null, null, "CRS-2026-SAGH611", "Seeded tuition charge.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 125m, "Academic Writing Cohort 01", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 156m, 13.32m, false, 23m, 156m, 0m, null, 156m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 2, null, null, false, null, null, null, null, null, "CRS-2026-A9CFJRZ", "Seeded tuition charge.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 130m, "Business Numeracy Cohort 02", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 162m, 14.04m, false, 26m, 162m, 162m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 3, null, null, false, null, null, null, null, null, "CRS-2026-1QOTM9Q", "Seeded tuition charge.", new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), 135m, "Digital Literacy Cohort 03", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 168m, 14.76m, false, 29m, 168m, 0m, null, 168m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 4, 4, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-EJ1A67A", "Seeded tuition charge.", new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 140m, "Career Readiness Cohort 04", new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, 174m, 15.48m, false, 32m, 144m, 0m, null, 144m, "Northview Secondary School", 1, 30m, 0.09m, null, null },
                    { 5, null, null, false, null, null, null, null, null, "CRS-2026-HPS3B9I", "Seeded tuition charge.", new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 145m, "Applied Science Cohort 05", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, 180m, 14.85m, false, 20m, 180m, 180m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 6, null, null, false, null, null, null, null, null, "CRS-2026-UPUZSA3", "Seeded tuition charge.", new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 150m, "Financial Literacy Cohort 06", new DateTime(2026, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 186m, 15.57m, false, 23m, 186m, 0m, null, 186m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 7, null, null, false, null, null, null, null, null, "CRS-2026-SMDS48L", "Seeded tuition charge.", new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), 155m, "Project Collaboration Cohort 07", new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, 192m, 16.29m, false, 26m, 192m, 0m, null, 192m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 8, 8, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-DTVK2L0", "Seeded tuition charge.", new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), 160m, "Data Skills Cohort 08", new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 198m, 17.01m, false, 29m, 168m, 168m, null, 0m, "Northview Secondary School", 2, 30m, 0.09m, null, null },
                    { 9, null, null, false, null, null, null, null, null, "CRS-2026-5HA3NN0", "Seeded tuition charge.", new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 165m, "Workplace Communication Cohort 09", new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, 204m, 17.73m, false, 32m, 204m, 0m, null, 204m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 10, null, null, false, null, null, null, null, null, "CRS-2026-PG7MVO8", "Seeded tuition charge.", new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), 170m, "Software Foundations Cohort 10", new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 210m, 17.10m, false, 20m, 210m, 0m, null, 210m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 11, null, null, false, null, null, null, null, null, "CRS-2026-7HGGL06", "Seeded tuition charge.", new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Utc), 175m, "Academic Writing Cohort 11", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, 216m, 17.82m, false, 23m, 216m, 216m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 12, 2, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-8QVCFIJ", "Seeded tuition charge.", new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 180m, "Business Numeracy Cohort 12", new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 222m, 18.54m, false, 26m, 192m, 0m, null, 192m, "Northview Secondary School", 3, 30m, 0.09m, null, null },
                    { 13, null, null, false, null, null, null, null, null, "CRS-2026-LTXWODC", "Seeded tuition charge.", new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 185m, "Digital Literacy Cohort 13", new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, 228m, 19.26m, false, 29m, 228m, 0m, null, 228m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 14, null, null, false, null, null, null, null, null, "CRS-2026-9O0PVAP", "Seeded tuition charge.", new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), 190m, "Career Readiness Cohort 14", new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, 234m, 19.98m, false, 32m, 234m, 234m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 15, null, null, false, null, null, null, null, null, "CRS-2026-WEJ9TUS", "Seeded tuition charge.", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 195m, "Applied Science Cohort 15", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, 240m, 19.35m, false, 20m, 240m, 0m, null, 240m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 16, 6, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-DW55O4J", "Seeded tuition charge.", new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), 200m, "Financial Literacy Cohort 16", new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, 246m, 20.07m, false, 23m, 216m, 0m, null, 216m, "Northview Secondary School", 1, 30m, 0.09m, null, null },
                    { 17, null, null, false, null, null, null, null, null, "CRS-2026-BEOMA7Q", "Seeded tuition charge.", new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 205m, "Project Collaboration Cohort 17", new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, 252m, 20.79m, false, 26m, 252m, 252m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 18, null, null, false, null, null, null, null, null, "CRS-2026-E5YXUWF", "Seeded tuition charge.", new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 210m, "Data Skills Cohort 18", new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, 258m, 21.51m, false, 29m, 258m, 0m, null, 258m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 19, null, null, false, null, null, null, null, null, "CRS-2026-5TQ32YK", "Seeded tuition charge.", new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), 215m, "Workplace Communication Cohort 19", new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, 264m, 22.23m, false, 32m, 264m, 0m, null, 264m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 20, 10, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-QHA65C0", "Seeded tuition charge.", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), 220m, "Software Foundations Cohort 20", new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, 270m, 21.60m, false, 20m, 240m, 240m, null, 0m, "Northview Secondary School", 2, 30m, 0.09m, null, null },
                    { 21, null, null, false, null, null, null, null, null, "CRS-2026-IF4LTDN", "Seeded tuition charge.", new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 225m, "Academic Writing Cohort 21", new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, 276m, 22.32m, false, 23m, 276m, 0m, null, 276m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 22, null, null, false, null, null, null, null, null, "CRS-2026-EOWJ7RI", "Seeded tuition charge.", new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), 230m, "Business Numeracy Cohort 22", new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, 282m, 23.04m, false, 26m, 282m, 0m, null, 282m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 23, null, null, false, null, null, null, null, null, "CRS-2026-JTOV8NE", "Seeded tuition charge.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), 235m, "Digital Literacy Cohort 23", new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 23, 288m, 23.76m, false, 29m, 288m, 288m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 24, 4, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-D6C5MU1", "Seeded tuition charge.", new DateTime(2026, 10, 24, 0, 0, 0, 0, DateTimeKind.Utc), 240m, "Career Readiness Cohort 24", new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 24, 294m, 24.48m, false, 32m, 264m, 0m, null, 264m, "Northview Secondary School", 3, 30m, 0.09m, null, null },
                    { 25, null, null, false, null, null, null, null, null, "CRS-2026-CWWS3U3", "Seeded tuition charge.", new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Utc), 245m, "Applied Science Cohort 25", new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 25, 300m, 23.85m, false, 20m, 300m, 0m, null, 300m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 26, null, null, false, null, null, null, null, null, "CRS-2026-SPJ7E30", "Seeded tuition charge.", new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 250m, "Financial Literacy Cohort 26", new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 26, 306m, 24.57m, false, 23m, 306m, 306m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 27, null, null, false, null, null, null, null, null, "CRS-2026-HHV04Y9", "Seeded tuition charge.", new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), 255m, "Project Collaboration Cohort 27", new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, 312m, 25.29m, false, 26m, 312m, 0m, null, 312m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 28, 8, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-LUJ9RT4", "Seeded tuition charge.", new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), 260m, "Data Skills Cohort 28", new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 28, 318m, 26.01m, false, 29m, 288m, 0m, null, 288m, "Northview Secondary School", 1, 30m, 0.09m, null, null },
                    { 29, null, null, false, null, null, null, null, null, "CRS-2026-MATA5OE", "Seeded tuition charge.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 265m, "Workplace Communication Cohort 29", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 29, 324m, 26.73m, false, 32m, 324m, 324m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 30, null, null, false, null, null, null, null, null, "CRS-2026-TBP3E3W", "Seeded tuition charge.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 270m, "Software Foundations Cohort 30", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 30, 330m, 26.10m, false, 20m, 330m, 0m, null, 330m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 31, null, null, false, null, null, null, null, null, "CRS-2026-NMXIH0W", "Seeded tuition charge.", new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), 275m, "Academic Writing Cohort 31", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 31, 336m, 26.82m, false, 23m, 336m, 0m, null, 336m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 32, 2, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-M2C29EQ", "Seeded tuition charge.", new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 280m, "Business Numeracy Cohort 32", new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 32, 342m, 27.54m, false, 26m, 312m, 312m, null, 0m, "Northview Secondary School", 2, 30m, 0.09m, null, null },
                    { 33, null, null, false, null, null, null, null, null, "CRS-2026-YXZ452K", "Seeded tuition charge.", new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 285m, "Digital Literacy Cohort 33", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 33, 348m, 28.26m, false, 29m, 348m, 0m, null, 348m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 34, null, null, false, null, null, null, null, null, "CRS-2026-OJZT05F", "Seeded tuition charge.", new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 290m, "Career Readiness Cohort 34", new DateTime(2026, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 34, 354m, 28.98m, false, 32m, 354m, 0m, null, 354m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 35, null, null, false, null, null, null, null, null, "CRS-2026-FBV0QY7", "Seeded tuition charge.", new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), 295m, "Applied Science Cohort 35", new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 35, 360m, 28.35m, false, 20m, 360m, 360m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 36, 6, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-R46A00W", "Seeded tuition charge.", new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), 300m, "Financial Literacy Cohort 36", new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 36, 366m, 29.07m, false, 23m, 336m, 0m, null, 336m, "Northview Secondary School", 3, 30m, 0.09m, null, null },
                    { 37, null, null, false, null, null, null, null, null, "CRS-2026-JMAO1QA", "Seeded tuition charge.", new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 305m, "Project Collaboration Cohort 37", new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 37, 372m, 29.79m, false, 26m, 372m, 0m, null, 372m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 38, null, null, false, null, null, null, null, null, "CRS-2026-VA3LGE0", "Seeded tuition charge.", new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), 310m, "Data Skills Cohort 38", new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 38, 378m, 30.51m, false, 29m, 378m, 378m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 39, null, null, false, null, null, null, null, null, "CRS-2026-C2L9WG4", "Seeded tuition charge.", new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Utc), 315m, "Workplace Communication Cohort 39", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 39, 384m, 31.23m, false, 32m, 384m, 0m, null, 384m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 40, 10, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-L6UUQYK", "Seeded tuition charge.", new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 320m, "Software Foundations Cohort 40", new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 40, 390m, 30.60m, false, 20m, 360m, 0m, null, 360m, "Northview Secondary School", 1, 30m, 0.09m, null, null },
                    { 41, null, null, false, null, null, null, null, null, "CRS-2026-79UQVDL", "Seeded tuition charge.", new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 325m, "Academic Writing Cohort 41", new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 41, 396m, 31.32m, false, 23m, 396m, 396m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 42, null, null, false, null, null, null, null, null, "CRS-2026-SDLMCO9", "Seeded tuition charge.", new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), 330m, "Business Numeracy Cohort 42", new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 42, 402m, 32.04m, false, 26m, 402m, 0m, null, 402m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 43, null, null, false, null, null, null, null, null, "CRS-2026-LUEVJLD", "Seeded tuition charge.", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 335m, "Digital Literacy Cohort 43", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 43, 408m, 32.76m, false, 29m, 408m, 0m, null, 408m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 44, 4, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-GO9AJL4", "Seeded tuition charge.", new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), 340m, "Career Readiness Cohort 44", new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 44, 414m, 33.48m, false, 32m, 384m, 384m, null, 0m, "Northview Secondary School", 2, 30m, 0.09m, null, null },
                    { 45, null, null, false, null, null, null, null, null, "CRS-2026-B29KV62", "Seeded tuition charge.", new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 345m, "Applied Science Cohort 45", new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 45, 420m, 32.85m, false, 20m, 420m, 0m, null, 420m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 46, null, null, false, null, null, null, null, null, "CRS-2026-2ME55AE", "Seeded tuition charge.", new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 350m, "Financial Literacy Cohort 46", new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 46, 426m, 33.57m, false, 23m, 426m, 0m, null, 426m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 47, null, null, false, null, null, null, null, null, "CRS-2026-X7JQUEV", "Seeded tuition charge.", new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), 355m, "Project Collaboration Cohort 47", new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 47, 432m, 34.29m, false, 26m, 432m, 432m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 48, 8, null, false, null, "School 1 FAS Demo", 2, 30m, "Tier 1", "CRS-2026-Y8VG30C", "Seeded tuition charge.", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), 360m, "Data Skills Cohort 48", new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 48, 438m, 35.01m, false, 29m, 408m, 0m, null, 408m, "Northview Secondary School", 3, 30m, 0.09m, null, null },
                    { 49, null, null, false, null, null, null, null, null, "CRS-2026-8A1B6PI", "Seeded tuition charge.", new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 365m, "Workplace Communication Cohort 49", new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 49, 444m, 35.73m, false, 32m, 444m, 0m, null, 444m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 50, null, null, false, null, null, null, null, null, "CRS-2026-2H98HR4", "Seeded tuition charge.", new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), 370m, "Software Foundations Cohort 50", new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 50, 450m, 35.10m, false, 20m, 450m, 450m, null, 0m, "Northview Secondary School", 2, 0m, 0.09m, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasApplicationDocument",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DocumentNameSnapshot", "FasApplicationId", "FasSchemeRequiredDocumentId", "FileKey", "FileName", "IsDeleted", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 1, 1, "fas/applications/1/document.pdf", "fas-application-01.pdf", false, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 2, 2, "fas/applications/2/document.pdf", "fas-application-02.pdf", false, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 3, 3, "fas/applications/3/document.pdf", "fas-application-03.pdf", false, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 4, 4, "fas/applications/4/document.pdf", "fas-application-04.pdf", false, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 5, 5, "fas/applications/5/document.pdf", "fas-application-05.pdf", false, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 6, 6, "fas/applications/6/document.pdf", "fas-application-06.pdf", false, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 7, 7, "fas/applications/7/document.pdf", "fas-application-07.pdf", false, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 8, 8, "fas/applications/8/document.pdf", "fas-application-08.pdf", false, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 9, 9, "fas/applications/9/document.pdf", "fas-application-09.pdf", false, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 10, 10, "fas/applications/10/document.pdf", "fas-application-10.pdf", false, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 11, 1, "fas/applications/11/document.pdf", "fas-application-11.pdf", false, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 12, 2, "fas/applications/12/document.pdf", "fas-application-12.pdf", false, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 13, 3, "fas/applications/13/document.pdf", "fas-application-13.pdf", false, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 14, 4, "fas/applications/14/document.pdf", "fas-application-14.pdf", false, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 15, 5, "fas/applications/15/document.pdf", "fas-application-15.pdf", false, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 16, 6, "fas/applications/16/document.pdf", "fas-application-16.pdf", false, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 17, 7, "fas/applications/17/document.pdf", "fas-application-17.pdf", false, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 18, 8, "fas/applications/18/document.pdf", "fas-application-18.pdf", false, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 19, 9, "fas/applications/19/document.pdf", "fas-application-19.pdf", false, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 20, 10, "fas/applications/20/document.pdf", "fas-application-20.pdf", false, null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 21, 1, "fas/applications/21/document.pdf", "fas-application-21.pdf", false, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 22, 2, "fas/applications/22/document.pdf", "fas-application-22.pdf", false, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 23, 3, "fas/applications/23/document.pdf", "fas-application-23.pdf", false, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 24, 4, "fas/applications/24/document.pdf", "fas-application-24.pdf", false, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 25, 5, "fas/applications/25/document.pdf", "fas-application-25.pdf", false, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 26, 6, "fas/applications/26/document.pdf", "fas-application-26.pdf", false, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 27, 7, "fas/applications/27/document.pdf", "fas-application-27.pdf", false, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 28, 8, "fas/applications/28/document.pdf", "fas-application-28.pdf", false, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 29, 9, "fas/applications/29/document.pdf", "fas-application-29.pdf", false, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 30, 10, "fas/applications/30/document.pdf", "fas-application-30.pdf", false, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 31, 1, "fas/applications/31/document.pdf", "fas-application-31.pdf", false, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 32, 2, "fas/applications/32/document.pdf", "fas-application-32.pdf", false, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 33, 3, "fas/applications/33/document.pdf", "fas-application-33.pdf", false, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 34, 4, "fas/applications/34/document.pdf", "fas-application-34.pdf", false, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Recent Payslip", 35, 5, "fas/applications/35/document.pdf", "fas-application-35.pdf", false, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasTierOverrideHistory",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "FasApplicationId", "IsDeleted", "ModifiedAt", "ModifiedByUserId", "NewTierId", "OldTierId", "Reason", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 2, "Seed tier review trail.", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, 6, 6, "Seed tier review trail.", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 10, 10, "Seed tier review trail.", null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupSystemApplication",
                columns: new[] { "Id", "EducationAccountId", "SystemTopupId", "TopupExecutionTargetId" },
                values: new object[] { 3, 3, 3, 3 });

            migrationBuilder.InsertData(
                table: "ChargeInstallment",
                columns: new[] { "Id", "Amount", "BecameOverdueAt", "ChargeId", "CreatedAt", "CreatedBy", "DeletedAt", "DueDate", "InstallmentNumber", "IsDeleted", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 156m, null, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 2, 162m, null, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 3, 168m, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 4, 174m, null, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 5, 180m, null, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 6, 186m, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 7, 192m, null, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 8, 198m, null, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 9, 204m, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Utc), 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 10, 210m, null, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 11, 216m, null, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 12, 222m, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 13, 228m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 14, 234m, null, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 15, 240m, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 16, 246m, null, 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 17, 252m, null, 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 18, 258m, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 19, 264m, null, 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 20, 270m, null, 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 21, 276m, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 22, 282m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 23, 288m, null, 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 24, 294m, new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 25, 300m, null, 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 26, 306m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 27, 312m, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 27, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 28, 318m, null, 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 29, 324m, null, 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 30, 330m, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Utc), 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 31, 336m, null, 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 32, 342m, null, 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 33, 348m, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 34, 354m, null, 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 35, 360m, null, 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 36, 366m, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 37, 372m, null, 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 38, 378m, null, 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 39, 384m, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 40, 390m, null, 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 41, 396m, null, 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 42, 402m, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 43, 408m, null, 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 44, 414m, null, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 45, 420m, new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 46, 426m, null, 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 47, 432m, null, 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 48, 438m, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 49, 444m, null, 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 50, 450m, null, 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "PaymentAllocation",
                columns: new[] { "Id", "Amount", "ChargeGrossAmountSnapshot", "ChargeId", "ChargeInstallmentId", "ChargeNetAmountSnapshot", "ChargeRemainingAmountSnapshot", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "PaymentId", "SchoolNameSnapshot", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 128m, 162m, 2, 2, 162m, 162m, "Academic Writing Cohort 02", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Northview Secondary School", null, null },
                    { 2, 140m, 180m, 5, 5, 180m, 180m, "Business Numeracy Cohort 05", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 5, "Northview Secondary School", null, null },
                    { 3, 152m, 198m, 8, 8, 198m, 198m, "Digital Literacy Cohort 08", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 8, "Northview Secondary School", null, null },
                    { 4, 164m, 216m, 11, 11, 216m, 216m, "Career Readiness Cohort 11", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 11, "Northview Secondary School", null, null },
                    { 5, 176m, 234m, 14, 14, 234m, 234m, "Applied Science Cohort 14", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 14, "Northview Secondary School", null, null },
                    { 6, 188m, 252m, 17, 17, 252m, 252m, "Financial Literacy Cohort 17", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 17, "Northview Secondary School", null, null },
                    { 7, 200m, 270m, 20, 20, 270m, 270m, "Project Collaboration Cohort 20", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 20, "Northview Secondary School", null, null },
                    { 8, 212m, 288m, 23, 23, 288m, 288m, "Data Skills Cohort 23", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 23, "Northview Secondary School", null, null },
                    { 9, 224m, 306m, 26, 26, 306m, 306m, "Workplace Communication Cohort 26", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 26, "Northview Secondary School", null, null },
                    { 10, 236m, 324m, 29, 29, 324m, 324m, "Software Foundations Cohort 29", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 29, "Northview Secondary School", null, null },
                    { 11, 248m, 342m, 32, 32, 342m, 342m, "Academic Writing Cohort 32", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 32, "Northview Secondary School", null, null },
                    { 12, 260m, 360m, 35, 35, 360m, 360m, "Business Numeracy Cohort 35", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 35, "Northview Secondary School", null, null },
                    { 13, 272m, 378m, 38, 38, 378m, 378m, "Digital Literacy Cohort 38", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 38, "Northview Secondary School", null, null },
                    { 14, 284m, 396m, 41, 41, 396m, 396m, "Career Readiness Cohort 41", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 41, "Northview Secondary School", null, null },
                    { 15, 296m, 414m, 44, 44, 414m, 414m, "Applied Science Cohort 44", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 44, "Northview Secondary School", null, null },
                    { 16, 308m, 432m, 47, 47, 432m, 432m, "Financial Literacy Cohort 47", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 47, "Northview Secondary School", null, null },
                    { 17, 320m, 450m, 50, 50, 450m, 450m, "Project Collaboration Cohort 50", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 50, "Northview Secondary School", null, null }
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
                name: "IX_AdminProfile_PhoneNumber",
                table: "AdminProfile",
                column: "PhoneNumber",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PhoneNumber\" IS NOT NULL");

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
                column: "Email",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_School_PhoneNumber",
                table: "School",
                column: "PhoneNumber",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PhoneNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_School_SchoolName",
                table: "School",
                column: "SchoolName",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"SchoolName\" IS NOT NULL");

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
                unique: true,
                filter: "\"TopupExecutionTargetId\" IS NOT NULL");

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
