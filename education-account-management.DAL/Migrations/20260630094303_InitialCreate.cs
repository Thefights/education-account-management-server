using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAiFeatureEnabled = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_ApplicationSetting", x => x.Id);
                    table.CheckConstraint("CK_ApplicationSetting_Singleton", "[Id] = 1");
                    table.CheckConstraint("CK_ApplicationSetting_TaxRate", "[TaxRate] >= 0");
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
                    table.CheckConstraint("CK_EducationCreditTransaction_BalanceEquation", "([Direction] = 1 AND [BalanceAfter] = [BalanceBefore] + [Amount]) OR ([Direction] = 2 AND [BalanceAfter] = [BalanceBefore] - [Amount]) OR ([Direction] = 3 AND [BalanceAfter] = [BalanceBefore])");
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
                name: "ManagementActionLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    PreviousStatus = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    NewStatus = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ActorUserId = table.Column<int>(type: "int", nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementActionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagementActionLog_User_ActorUserId",
                        column: x => x.ActorUserId,
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
                name: "FasSchemeAdditionalQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FasSchemeId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasSchemeAdditionalQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasSchemeAdditionalQuestion_FasScheme_FasSchemeId",
                        column: x => x.FasSchemeId,
                        principalTable: "FasScheme",
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
                    MaxGrossHouseholdIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.CheckConstraint("CK_FasSchemeTier_Amounts_NonNegative", "([MaxPerCapitaIncome] IS NULL OR [MaxPerCapitaIncome] >= 0) AND ([MaxGrossHouseholdIncome] IS NULL OR [MaxGrossHouseholdIncome] >= 0) AND ([SubsidyValue] IS NULL OR [SubsidyValue] >= 0) AND ([CourseFeeSubsidyValue] IS NULL OR [CourseFeeSubsidyValue] >= 0) AND ([MiscFeeSubsidyValue] IS NULL OR [MiscFeeSubsidyValue] >= 0)");
                    table.CheckConstraint("CK_FasSchemeTier_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
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
                    ExternalRejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InternalRejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                name: "FasApplicationAdditionalQuestionAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FasApplicationId = table.Column<int>(type: "int", nullable: false),
                    FasSchemeAdditionalQuestionId = table.Column<int>(type: "int", nullable: true),
                    QuestionTextSnapshot = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRequiredSnapshot = table.Column<bool>(type: "bit", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasApplicationAdditionalQuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasApplicationAdditionalQuestionAnswer_FasApplication_FasApplicationId",
                        column: x => x.FasApplicationId,
                        principalTable: "FasApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FasApplicationAdditionalQuestionAnswer_FasSchemeAdditionalQuestion_FasSchemeAdditionalQuestionId",
                        column: x => x.FasSchemeAdditionalQuestionId,
                        principalTable: "FasSchemeAdditionalQuestion",
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
                table: "ApplicationSetting",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "InstallmentDueDay", "IsAiFeatureEnabled", "IsDeleted", "TaxRate", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, true, false, 0.09m, null, null });

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
                table: "Payment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditTransactionId", "ExternalReference", "IsDeleted", "PaidAt", "PaymentMethod", "Status", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-M7QVS7X", "Sterling Quach", "S0000001I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 1, 166.77m, null, null },
                    { 2, "EDU-2026-H4W9JYQ", "Amelia Tan", "S0000002G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00002", false, null, 2, 3, 172.22m, null, null },
                    { 4, "EDU-2026-K6CAJET", "Priya Nair", "S0000004C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00004", false, null, 2, 1, 153.12m, null, null },
                    { 5, "EDU-2026-C865DQ8", "Ethan Koh", "S0000005A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 3, 188.57m, null, null },
                    { 6, "EDU-2026-J3PD5GI", "Hannah Lee", "S0000006Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00006", false, new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 194.02m, null, null },
                    { 7, "EDU-2026-RHEGIE7", "Daniel Wong", "S0000007H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 1, 199.47m, null, null },
                    { 8, "EDU-2026-ERZPP9D", "Sofia Chen", "S0000008F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00008", false, null, 2, 3, 174.92m, null, null },
                    { 10, "EDU-2026-5A2BWXE", "Maya Rahman", "S0000010H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00010", false, null, 2, 1, 215.82m, null, null },
                    { 11, "EDU-2026-YKWELYD", "Noah Teo", "S0000011F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 3, 221.27m, null, null },
                    { 12, "EDU-2026-XW95AJ5", "Aisha Fernandez", "S0000012D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00012", false, new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 196.72m, null, null },
                    { 13, "EDU-2026-OOROKGZ", "Ryan Chua", "S0000013B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 1, 232.17m, null, null },
                    { 14, "EDU-2026-EC0POYJ", "Chloe Goh", "S0000014J", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00014", false, null, 2, 3, 237.62m, null, null },
                    { 16, "EDU-2026-A5L21GT", "Natalie Seah", "S0000016G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00016", false, null, 2, 1, 218.52m, null, null },
                    { 17, "EDU-2026-R9VEYR0", "Alina Ang", "S0000017E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 3, 253.97m, null, null },
                    { 18, "EDU-2026-JLH113O", "Benjamin Bala", "S0000018C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00018", false, new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 259.42m, null, null },
                    { 19, "EDU-2026-S7OK05W", "Clara Chew", "S0000019A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 1, 264.87m, null, null },
                    { 20, "EDU-2026-2MYNLK1", "Darius Das", "S0000020E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00020", false, null, 2, 3, 240.32m, null, null },
                    { 22, "EDU-2026-TIXLNIB", "Farhan Foo", "S0000022A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00022", false, null, 2, 1, 281.22m, null, null },
                    { 23, "EDU-2026-T5N58HB", "Grace Gan", "S0000023Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 3, 286.67m, null, null },
                    { 24, "EDU-2026-7MH1FBP", "Haruto Ho", "S0000024H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00024", false, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 262.12m, null, null },
                    { 25, "EDU-2026-GDE8Q6P", "Isabelle Ismail", "S0000025F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 1, 297.57m, null, null },
                    { 26, "EDU-2026-SNYKN82", "Jasper Jeyaratnam", "S0000026D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00026", false, null, 2, 3, 303.02m, null, null },
                    { 28, "EDU-2026-G0N8QXW", "Leon Lim", "S0000028J", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00028", false, null, 2, 1, 283.92m, null, null },
                    { 29, "EDU-2026-A3B8IRY", "Mei Lin Mohamed", "S0000029I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 3, 319.37m, null, null },
                    { 30, "EDU-2026-CYOBPQU", "Nathan Ng", "S0000030B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00030", false, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 324.82m, null, null },
                    { 31, "EDU-2026-20G2H3L", "Olivia Ong", "S0000031J", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 1, 330.27m, null, null },
                    { 32, "EDU-2026-9RY98UG", "Pranav Pillai", "S0000032I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00032", false, null, 2, 3, 305.72m, null, null },
                    { 34, "EDU-2026-CVJPW51", "Rafael Rao", "S0000034E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00034", false, null, 2, 1, 346.62m, null, null },
                    { 35, "EDU-2026-GL31H3K", "Selina Sim", "S0000035C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 3, 352.07m, null, null },
                    { 36, "EDU-2026-L1OX9Y0", "Terence Tan", "S0000036A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00036", false, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 327.52m, null, null },
                    { 37, "EDU-2026-3V9Y2I0", "Umairah Uddin", "S0000037Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 1, 362.97m, null, null },
                    { 38, "EDU-2026-UIX61F4", "Victor Vasquez", "S0000038H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00038", false, null, 2, 3, 368.42m, null, null },
                    { 40, "EDU-2026-2A9X7HT", "Xavier Xu", "S0000040Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00040", false, null, 2, 1, 349.32m, null, null },
                    { 41, "EDU-2026-HIZFYQR", "Yasmin Yeo", "S0000041H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 3, 384.77m, null, null },
                    { 42, "EDU-2026-5FO1ERB", "Zachary Zainal", "S0000042F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00042", false, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 390.22m, null, null },
                    { 43, "EDU-2026-C6GLP5G", "Adeline Ang", "S0000043D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 1, 395.67m, null, null },
                    { 44, "EDU-2026-VBGBHB4", "Brandon Bala", "S0000044B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00044", false, null, 2, 3, 371.12m, null, null },
                    { 46, "EDU-2026-Z2MR7S3", "Damien Das", "S0000046I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00046", false, null, 2, 1, 412.02m, null, null },
                    { 47, "EDU-2026-2OK04RQ", "Evelyn Eng", "S0000047G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 3, 417.47m, null, null },
                    { 48, "EDU-2026-5L2W56M", "Faris Foo", "S0000048E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00048", false, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 392.92m, null, null },
                    { 49, "EDU-2026-7UZZHKN", "Giselle Gan", "S0000049C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, 1, 1, 428.37m, null, null },
                    { 50, "EDU-2026-9DG3ZSZ", "Haziq Ho", "S0000050G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "PAY-EXT-00050", false, null, 2, 3, 433.82m, null, null }
                });

            migrationBuilder.InsertData(
                table: "ScheduleTopUp",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExecuteAtDay", "ExecuteAtMonth", "ExecutionTime", "Frequency", "IsDeleted", "Name", "NextExecutionAt", "OneTimeExecutionAt", "Status", "TopupAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(10, 0, 0), 1, false, "July Intake Welcome Credit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 65m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, null, new TimeOnly(11, 0, 0), 2, false, "Monthly Meal Allowance", new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 80m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, new TimeOnly(12, 0, 0), 3, false, "Annual Materials Grant", null, null, 3, 95m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(13, 0, 0), 1, false, "Orientation Transport Credit", new DateTime(2026, 7, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, 110m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, null, new TimeOnly(14, 0, 0), 2, false, "Monthly Attendance Support", new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 125m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, new TimeOnly(9, 0, 0), 3, false, "Year-End Progress Award", null, null, 3, 140m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(10, 0, 0), 1, false, "One-Time Device Support", new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, 155m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, null, new TimeOnly(11, 0, 0), 2, false, "Monthly Commuter Credit", new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 170m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, new TimeOnly(12, 0, 0), 3, false, "Annual Skills Grant", null, null, 3, 185m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(13, 0, 0), 1, false, "Workshop Participation Credit", new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, 200m, null, null }
                });

            migrationBuilder.InsertData(
                table: "School",
                columns: new[] { "Id", "Address", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "IsDeleted", "PhoneNumber", "SchoolName", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "10 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school01@example.edu.sg", false, "+656000001", "Northview Secondary School", 1, null, null },
                    { 2, "20 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school02@example.edu.sg", false, "+656000002", "Eastbridge Secondary School", 1, null, null },
                    { 3, "30 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school03@example.edu.sg", false, "+656000003", "Westhaven Secondary School", 1, null, null },
                    { 4, "40 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school04@example.edu.sg", false, "+656000004", "Southpoint Secondary School", 1, null, null },
                    { 5, "50 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school05@example.edu.sg", false, "+656000005", "Central Heights School", 1, null, null },
                    { 6, "60 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school06@example.edu.sg", false, "+656000006", "Riverside Learning Institute", 1, null, null },
                    { 7, "70 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school07@example.edu.sg", false, "+656000007", "Lakeside Technical School", 1, null, null },
                    { 8, "80 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school08@example.edu.sg", false, "+656000008", "Greenfield Academy", 1, null, null },
                    { 9, "90 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school09@example.edu.sg", false, "+656000009", "Harbourfront School", 1, null, null },
                    { 10, "100 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school10@example.edu.sg", false, "+656000010", "Hillcrest Education Centre", 2, null, null },
                    { 11, "110 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school11@example.edu.sg", false, "+656000011", "Cedar Valley School", 1, null, null },
                    { 12, "120 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school12@example.edu.sg", false, "+656000012", "Maple Ridge Academy", 1, null, null },
                    { 13, "130 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school13@example.edu.sg", false, "+656000013", "Oceanview Institute", 1, null, null },
                    { 14, "140 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school14@example.edu.sg", false, "+656000014", "Brighton Learning Centre", 1, null, null },
                    { 15, "150 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school15@example.edu.sg", false, "+656000015", "Pioneer Technical College", 1, null, null },
                    { 16, "160 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school16@example.edu.sg", false, "+656000016", "Summit Arts School", 1, null, null },
                    { 17, "170 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school17@example.edu.sg", false, "+656000017", "Meridian Business School", 1, null, null },
                    { 18, "180 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school18@example.edu.sg", false, "+656000018", "Silverstream Polytechnic", 1, null, null },
                    { 19, "190 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school19@example.edu.sg", false, "+656000019", "Redwood Community College", 1, null, null },
                    { 20, "200 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school20@example.edu.sg", false, "+656000020", "Bluewater Skills Institute", 2, null, null },
                    { 21, "210 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school21@example.edu.sg", false, "+656000021", "Golden Grove Academy", 1, null, null },
                    { 22, "220 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school22@example.edu.sg", false, "+656000022", "Sunrise Training Centre", 1, null, null },
                    { 23, "230 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school23@example.edu.sg", false, "+656000023", "Crescent School of Technology", 1, null, null },
                    { 24, "240 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school24@example.edu.sg", false, "+656000024", "Orchid City College", 1, null, null },
                    { 25, "250 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school25@example.edu.sg", false, "+656000025", "Evergreen Education Hub", 1, null, null },
                    { 26, "260 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school26@example.edu.sg", false, "+656000026", "Vista Applied Learning", 1, null, null },
                    { 27, "270 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school27@example.edu.sg", false, "+656000027", "Compass Point School", 1, null, null },
                    { 28, "280 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school28@example.edu.sg", false, "+656000028", "Newbridge Institute", 1, null, null },
                    { 29, "290 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school29@example.edu.sg", false, "+656000029", "Heritage Skills Academy", 1, null, null },
                    { 30, "300 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school30@example.edu.sg", false, "+656000030", "Frontier Science School", 2, null, null },
                    { 31, "310 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school31@example.edu.sg", false, "+656000031", "Meadowbrook College", 1, null, null },
                    { 32, "320 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school32@example.edu.sg", false, "+656000032", "Peakview Education Centre", 1, null, null },
                    { 33, "330 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school33@example.edu.sg", false, "+656000033", "Bayfront Technical School", 1, null, null },
                    { 34, "340 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school34@example.edu.sg", false, "+656000034", "Queensway Learning Academy", 1, null, null },
                    { 35, "350 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school35@example.edu.sg", false, "+656000035", "Unity Continuing Education", 1, null, null },
                    { 36, "360 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school36@example.edu.sg", false, "+656000036", "Elmwood Professional School", 1, null, null },
                    { 37, "370 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school37@example.edu.sg", false, "+656000037", "Innovation Training Campus", 1, null, null },
                    { 38, "380 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school38@example.edu.sg", false, "+656000038", "Riverbend School", 1, null, null },
                    { 39, "390 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school39@example.edu.sg", false, "+656000039", "Stonefield Institute", 1, null, null },
                    { 40, "400 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school40@example.edu.sg", false, "+656000040", "Seaside Skills Centre", 2, null, null },
                    { 41, "410 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school41@example.edu.sg", false, "+656000041", "Northstar Academy", 1, null, null },
                    { 42, "420 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school42@example.edu.sg", false, "+656000042", "Eastgate Technical College", 1, null, null },
                    { 43, "430 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school43@example.edu.sg", false, "+656000043", "Westlake Learning Hub", 1, null, null },
                    { 44, "440 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school44@example.edu.sg", false, "+656000044", "Southridge School", 1, null, null },
                    { 45, "450 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school45@example.edu.sg", false, "+656000045", "Central Park Institute", 1, null, null },
                    { 46, "460 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school46@example.edu.sg", false, "+656000046", "Hillview Polytechnic", 1, null, null },
                    { 47, "470 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school47@example.edu.sg", false, "+656000047", "Greenridge Academy", 1, null, null },
                    { 48, "480 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school48@example.edu.sg", false, "+656000048", "Harbour Bay College", 1, null, null },
                    { 49, "490 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school49@example.edu.sg", false, "+656000049", "Lighthouse Skills School", 1, null, null },
                    { 50, "500 Campus Road, Singapore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "school50@example.edu.sg", false, "+656000050", "Civic Learning Centre", 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "SystemTopup",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Name", "Status", "TopupAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Monthly Learning Credit Boost", 1, 100m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Transport Support Credit", 1, 120m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Digital Access Allowance", 1, 140m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Back-to-School Credit", 2, 160m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Exam Preparation Support", 1, 180m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "SkillsFuture Starter Credit", 1, 200m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "STEM Enrichment Credit", 1, 220m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Community Learning Grant", 2, 240m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Attendance Reward Credit", 1, 260m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Holiday Programme Support", 1, 280m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupExecution",
                columns: new[] { "Id", "ConditionsSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutionCode", "FailedCount", "IdempotencyKey", "IsDeleted", "ManualAmount", "ManualReason", "ScheduleTopUpId", "SourceType", "Status", "SuccessCount", "SystemTopupId", "TopupAmountSnapshot", "TopupNameSnapshot", "TotalExecutedAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 3, "Matched active eligibility conditions", new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-003", 0, "topup-run-003", false, 105m, "Manual support credit adjustment", null, 3, 3, 1, null, 105m, "Manual Account Adjustment", 105m, 1, null, null },
                    { 6, "Matched active eligibility conditions", new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-006", 0, "topup-run-006", false, 135m, "Manual support credit adjustment", null, 3, 3, 1, null, 135m, "Manual Transport Reimbursement", 135m, 1, null, null },
                    { 9, "Matched active eligibility conditions", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-009", 0, "topup-run-009", false, 165m, "Manual support credit adjustment", null, 3, 3, 1, null, 165m, "Manual Materials Reimbursement", 165m, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CitizenId", "CreatedAt", "CreatedBy", "DeletedAt", "FailedLoginCount", "IsDeleted", "LastLoginAt", "LockedUntil", "Role", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, null, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, null, null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 2, null, null },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, null, null },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, null, null },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, null, null },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, null, null },
                    { 12, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, null, null },
                    { 13, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 14, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, null, null },
                    { 15, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, null, null },
                    { 16, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 17, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 18, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 19, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 20, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null },
                    { 21, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "AdminProfile",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "FullName", "IsDeleted", "Nric", "PhoneNumber", "SchoolId", "StaffCode", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin001@example.com", "Aaron Lim", false, "S0000101E", "+6580000001", null, "STAFF-2026-XX2CU83", null, null, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin002@example.com", "Belinda Tan", false, "S0000102C", "+6580000002", null, "STAFF-2026-STL4YQH", null, null, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin003@example.com", "Cheryl Ng", false, "S0000103A", "+6580000003", 1, "STAFF-2026-MUWAHW6", null, null, 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin004@example.com", "Daniel Koh", false, "S0000104Z", "+6580000004", 10, "STAFF-2026-S4FZU83", null, null, 21 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin005@example.com", "Elaine Chua", false, "S0000105H", "+6580000005", null, "STAFF-2026-AOAY3A4", null, null, 5 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin006@example.com", "Farhan Rahman", false, "S0000106F", "+6580000006", null, "STAFF-2026-KRNWMLK", null, null, 6 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin007@example.com", "Grace Lee", false, "S0000107D", "+6580000007", 2, "STAFF-2026-UNJFZW7", null, null, 7 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin008@example.com", "Hannah Wong", false, "S0000108B", "+6580000008", null, "STAFF-2026-SFXEF6F", null, null, 8 },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin009@example.com", "Isaac Teo", false, "S0000109J", "+6580000009", null, "STAFF-2026-O3408F1", null, null, 9 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin010@example.com", "Jasmine Goh", false, "S0000110D", "+6580000010", 3, "STAFF-2026-7F0PDWW", null, null, 10 },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin011@example.com", "Kenneth Low", false, "S0000111B", "+6580000011", null, "STAFF-2026-8X4V13Q", null, null, 11 },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin012@example.com", "Liyana Hassan", false, "S0000112J", "+6580000012", null, "STAFF-2026-U02ABWW", null, null, 12 },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin013@example.com", "Marcus Chen", false, "S0000113I", "+6580000013", 4, "STAFF-2026-YAASVP4", null, null, 13 },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin014@example.com", "Nadia Ismail", false, "S0000114G", "+6580000014", null, "STAFF-2026-MD7QFDN", null, null, 14 },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin015@example.com", "Oliver Tan", false, "S0000115E", "+6580000015", null, "STAFF-2026-YM3YL5X", null, null, 15 },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin016@example.com", "Priya Nair", false, "S0000116C", "+6580000016", 5, "STAFF-2026-KOWBPV1", null, null, 16 },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin017@example.com", "Qiao En Lim", false, "S0000117A", "+6580000017", 6, "STAFF-2026-D7G9W6U", null, null, 17 },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin018@example.com", "Rachel Ong", false, "S0000118Z", "+6580000018", 7, "STAFF-2026-ZAQHZJY", null, null, 18 },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin019@example.com", "Samuel Neo", false, "S0000119H", "+6580000019", 8, "STAFF-2026-NI8TCIU", null, null, 19 },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin020@example.com", "Theresa Yeo", false, "S0000120A", "+6580000020", 9, "STAFF-2026-BM63HUE", null, null, 20 }
                });

            migrationBuilder.InsertData(
                table: "AuditLog",
                columns: new[] { "Id", "Action", "ActorUserId", "Category", "IpAddress", "Nric", "OccurredAt" },
                values: new object[,]
                {
                    { 1, "Created education account", 1, 1, "127.0.0.2", null, new DateTime(2026, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Activated account batch", 2, 2, "127.0.0.3", null, new DateTime(2026, 1, 1, 2, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "Executed system top-up", 3, 3, "127.0.0.4", null, new DateTime(2026, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "Recorded payment allocation", 1, 5, "127.0.0.6", null, new DateTime(2026, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "Generated tuition charge", 2, 6, "127.0.0.7", null, new DateTime(2026, 1, 1, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "Generated support recommendation", 3, 7, "127.0.0.8", null, new DateTime(2026, 1, 1, 7, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, "Deactivated user profile", 1, 2, "127.0.0.10", null, new DateTime(2026, 1, 1, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, "Processed scheduled top-up", 2, 3, "127.0.0.11", null, new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "Reviewed sign-in activity", 3, 4, "127.0.0.12", null, new DateTime(2026, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, "Updated installment schedule", 1, 6, "127.0.0.14", null, new DateTime(2026, 1, 1, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, "Reviewed assistance suggestion", 2, 7, "127.0.0.15", null, new DateTime(2026, 1, 1, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, "Imported education accounts", 3, 1, "127.0.0.16", null, new DateTime(2026, 1, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, "Reviewed top-up target", 1, 3, "127.0.0.18", null, new DateTime(2026, 1, 1, 17, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, "Updated access policy", 2, 4, "127.0.0.19", null, new DateTime(2026, 1, 1, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, "Reconciled payment record", 3, 5, "127.0.0.20", null, new DateTime(2026, 1, 1, 19, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, "Evaluated eligibility summary", 1, 7, "127.0.0.22", null, new DateTime(2026, 1, 1, 21, 0, 0, 0, DateTimeKind.Utc) },
                    { 22, "Created school student record", 2, 1, "127.0.0.23", null, new DateTime(2026, 1, 1, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 23, "Changed course status", 3, 2, "127.0.0.24", null, new DateTime(2026, 1, 1, 23, 0, 0, 0, DateTimeKind.Utc) },
                    { 25, "Revoked expired session", 1, 4, "127.0.0.26", null, new DateTime(2026, 1, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 26, "Updated charge balance", 2, 5, "127.0.0.27", null, new DateTime(2026, 1, 2, 2, 0, 0, 0, DateTimeKind.Utc) },
                    { 27, "Recorded billing adjustment", 3, 6, "127.0.0.28", null, new DateTime(2026, 1, 2, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { 29, "Created education account", 1, 1, "127.0.0.30", null, new DateTime(2026, 1, 2, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 30, "Activated account batch", 2, 2, "127.0.0.31", null, new DateTime(2026, 1, 2, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 31, "Executed system top-up", 3, 3, "127.0.0.32", null, new DateTime(2026, 1, 2, 7, 0, 0, 0, DateTimeKind.Utc) },
                    { 33, "Recorded payment allocation", 1, 5, "127.0.0.34", null, new DateTime(2026, 1, 2, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 34, "Generated tuition charge", 2, 6, "127.0.0.35", null, new DateTime(2026, 1, 2, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 35, "Generated support recommendation", 3, 7, "127.0.0.36", null, new DateTime(2026, 1, 2, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { 37, "Deactivated user profile", 1, 2, "127.0.0.38", null, new DateTime(2026, 1, 2, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { 38, "Processed scheduled top-up", 2, 3, "127.0.0.39", null, new DateTime(2026, 1, 2, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 39, "Reviewed sign-in activity", 3, 4, "127.0.0.40", null, new DateTime(2026, 1, 2, 15, 0, 0, 0, DateTimeKind.Utc) },
                    { 41, "Updated installment schedule", 1, 6, "127.0.0.42", null, new DateTime(2026, 1, 2, 17, 0, 0, 0, DateTimeKind.Utc) },
                    { 42, "Reviewed assistance suggestion", 2, 7, "127.0.0.43", null, new DateTime(2026, 1, 2, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { 43, "Imported education accounts", 3, 1, "127.0.0.44", null, new DateTime(2026, 1, 2, 19, 0, 0, 0, DateTimeKind.Utc) },
                    { 45, "Reviewed top-up target", 1, 3, "127.0.0.46", null, new DateTime(2026, 1, 2, 21, 0, 0, 0, DateTimeKind.Utc) },
                    { 46, "Updated access policy", 2, 4, "127.0.0.47", null, new DateTime(2026, 1, 2, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 47, "Reconciled payment record", 3, 5, "127.0.0.48", null, new DateTime(2026, 1, 2, 23, 0, 0, 0, DateTimeKind.Utc) },
                    { 49, "Evaluated eligibility summary", 1, 7, "127.0.0.50", null, new DateTime(2026, 1, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 50, "Created school student record", 2, 1, "127.0.0.51", null, new DateTime(2026, 1, 3, 2, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "CourseCode", "CourseFeeAmount", "CourseName", "CreatedAt", "CreatedBy", "DeletedAt", "EndDate", "EnrollmentDeadline", "FasApplicationDueDate", "GstAmount", "IsDeleted", "MiscFeeAmount", "SchoolId", "StartDate", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "CRS-2026-SAGH611", 125m, "Academic Writing Cohort 01", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13.32m, false, 23m, 1, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 2, "CRS-2026-A9CFJRZ", 130m, "Business Numeracy Cohort 02", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), 14.04m, false, 26m, 1, new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 3, "CRS-2026-1QOTM9Q", 135m, "Digital Literacy Cohort 03", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), 14.76m, false, 29m, 1, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 4, "CRS-2026-EJ1A67A", 140m, "Career Readiness Cohort 04", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 15.48m, false, 32m, 1, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 5, "CRS-2026-HPS3B9I", 145m, "Applied Science Cohort 05", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 14.85m, false, 20m, 1, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 6, "CRS-2026-UPUZSA3", 150m, "Financial Literacy Cohort 06", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 15.57m, false, 23m, 1, new DateTime(2026, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 7, "CRS-2026-SMDS48L", 155m, "Project Collaboration Cohort 07", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 16.29m, false, 26m, 1, new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 8, "CRS-2026-DTVK2L0", 160m, "Data Skills Cohort 08", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 17.01m, false, 29m, 1, new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 9, "CRS-2026-5HA3NN0", 165m, "Workplace Communication Cohort 09", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 17.73m, false, 32m, 1, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 10, "CRS-2026-PG7MVO8", 170m, "Software Foundations Cohort 10", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 17.10m, false, 20m, 1, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 11, "CRS-2026-7HGGL06", 175m, "Academic Writing Cohort 11", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 17.82m, false, 23m, 1, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 12, "CRS-2026-8QVCFIJ", 180m, "Business Numeracy Cohort 12", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), 18.54m, false, 26m, 1, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 13, "CRS-2026-LTXWODC", 185m, "Digital Literacy Cohort 13", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), 19.26m, false, 29m, 1, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 14, "CRS-2026-9O0PVAP", 190m, "Career Readiness Cohort 14", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 19.98m, false, 32m, 1, new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 15, "CRS-2026-WEJ9TUS", 195m, "Applied Science Cohort 15", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 19.35m, false, 20m, 1, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 16, "CRS-2026-DW55O4J", 200m, "Financial Literacy Cohort 16", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 20.07m, false, 23m, 1, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 17, "CRS-2026-BEOMA7Q", 205m, "Project Collaboration Cohort 17", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 20.79m, false, 26m, 1, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 18, "CRS-2026-E5YXUWF", 210m, "Data Skills Cohort 18", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), 21.51m, false, 29m, 1, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 19, "CRS-2026-5TQ32YK", 215m, "Workplace Communication Cohort 19", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), 22.23m, false, 32m, 1, new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 20, "CRS-2026-QHA65C0", 220m, "Software Foundations Cohort 20", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), 21.60m, false, 20m, 1, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 21, "CRS-2026-IF4LTDN", 225m, "Academic Writing Cohort 21", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), 22.32m, false, 23m, 1, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 22, "CRS-2026-EOWJ7RI", 230m, "Business Numeracy Cohort 22", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), 23.04m, false, 26m, 1, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 23, "CRS-2026-JTOV8NE", 235m, "Digital Literacy Cohort 23", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), 23.76m, false, 29m, 1, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 24, "CRS-2026-D6C5MU1", 240m, "Career Readiness Cohort 24", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), 24.48m, false, 32m, 1, new DateTime(2026, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 25, "CRS-2026-CWWS3U3", 245m, "Applied Science Cohort 25", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), 23.85m, false, 20m, 1, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 26, "CRS-2026-SPJ7E30", 250m, "Financial Literacy Cohort 26", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), 24.57m, false, 23m, 1, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 27, "CRS-2026-HHV04Y9", 255m, "Project Collaboration Cohort 27", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), 25.29m, false, 26m, 1, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 28, "CRS-2026-LUJ9RT4", 260m, "Data Skills Cohort 28", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), 26.01m, false, 29m, 1, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 29, "CRS-2026-MATA5OE", 265m, "Workplace Communication Cohort 29", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 26.73m, false, 32m, 1, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 30, "CRS-2026-TBP3E3W", 270m, "Software Foundations Cohort 30", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), 26.10m, false, 20m, 1, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 31, "CRS-2026-NMXIH0W", 275m, "Academic Writing Cohort 31", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), 26.82m, false, 23m, 1, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 32, "CRS-2026-M2C29EQ", 280m, "Business Numeracy Cohort 32", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 27.54m, false, 26m, 1, new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 33, "CRS-2026-YXZ452K", 285m, "Digital Literacy Cohort 33", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 28.26m, false, 29m, 1, new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 34, "CRS-2026-OJZT05F", 290m, "Career Readiness Cohort 34", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 28.98m, false, 32m, 1, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 35, "CRS-2026-FBV0QY7", 295m, "Applied Science Cohort 35", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 28.35m, false, 20m, 1, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 36, "CRS-2026-R46A00W", 300m, "Financial Literacy Cohort 36", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 29.07m, false, 23m, 1, new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 37, "CRS-2026-JMAO1QA", 305m, "Project Collaboration Cohort 37", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 29.79m, false, 26m, 1, new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 38, "CRS-2026-VA3LGE0", 310m, "Data Skills Cohort 38", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 30.51m, false, 29m, 1, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 39, "CRS-2026-C2L9WG4", 315m, "Workplace Communication Cohort 39", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 31.23m, false, 32m, 1, new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 40, "CRS-2026-L6UUQYK", 320m, "Software Foundations Cohort 40", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), 30.60m, false, 20m, 1, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 41, "CRS-2026-79UQVDL", 325m, "Academic Writing Cohort 41", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), 31.32m, false, 23m, 1, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 42, "CRS-2026-SDLMCO9", 330m, "Business Numeracy Cohort 42", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 32.04m, false, 26m, 1, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 43, "CRS-2026-LUEVJLD", 335m, "Digital Literacy Cohort 43", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 32.76m, false, 29m, 1, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 44, "CRS-2026-GO9AJL4", 340m, "Career Readiness Cohort 44", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 33.48m, false, 32m, 1, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 45, "CRS-2026-B29KV62", 345m, "Applied Science Cohort 45", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 32.85m, false, 20m, 1, new DateTime(2026, 3, 17, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 46, "CRS-2026-2ME55AE", 350m, "Financial Literacy Cohort 46", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), 33.57m, false, 23m, 1, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 47, "CRS-2026-X7JQUEV", 355m, "Project Collaboration Cohort 47", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), 34.29m, false, 26m, 1, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 48, "CRS-2026-Y8VG30C", 360m, "Data Skills Cohort 48", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), 35.01m, false, 29m, 1, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 49, "CRS-2026-8A1B6PI", 365m, "Workplace Communication Cohort 49", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), 35.73m, false, 32m, 1, new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 50, "CRS-2026-2H98HR4", 370m, "Software Foundations Cohort 50", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), 35.10m, false, 20m, 1, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 51, "CRS-2026-W2MTY4B", 150m, "Creative Thinking Cohort 51", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15.30m, false, 20m, 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 52, "CRS-2026-6YV5VO9", 160m, "Critical Analysis Cohort 52", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 16.65m, false, 25m, 1, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 53, "CRS-2026-HR6FMHH", 140m, "Public Speaking Cohort 53", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14.22m, false, 18m, 1, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 54, "CRS-2026-SGK2RR1", 170m, "Data Literacy Cohort 54", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 17.28m, false, 22m, 1, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 55, "CRS-2026-NVVEGXE", 155m, "Problem Solving Cohort 55", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 15.75m, false, 20m, 1, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 56, "CRS-2026-PDY15VJ", 155m, "Future Draft Course A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 15.75m, false, 20m, 1, new DateTime(2027, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 57, "CRS-2026-8Q05JX1", 200m, "Future Draft Course B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), 19.80m, false, 20m, 1, new DateTime(2027, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 58, "CRS-2026-98UAS1T", 300m, "Future Draft Course C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 28.80m, false, 20m, 1, new DateTime(2027, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "EducationAccount",
                columns: new[] { "Id", "AccountNumber", "CitizenId", "ClosedAt", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditBalance", "IsDeleted", "OpenedAt", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-M7QVS7X", 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 815m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 2, "EDU-2026-H4W9JYQ", 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 3, "EDU-2026-5BHC84W", 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 927.33m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 4, "EDU-2026-K6CAJET", 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 5, "EDU-2026-C865DQ8", 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 6, "EDU-2026-J3PD5GI", 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 7, "EDU-2026-RHEGIE7", 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1145m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 8, "EDU-2026-ERZPP9D", 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 9, "EDU-2026-3WTXQL2", 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 789.63m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 10, "EDU-2026-5A2BWXE", 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 11, "EDU-2026-YKWELYD", 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 12, "EDU-2026-XW95AJ5", 12, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 13, "EDU-2026-OOROKGZ", 13, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 14, "EDU-2026-EC0POYJ", 14, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 15, "EDU-2026-1ZHBQ2S", 15, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 756.93m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 16, "EDU-2026-A5L21GT", 16, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 17, "EDU-2026-R9VEYR0", 17, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 18, "EDU-2026-JLH113O", 18, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 19, "EDU-2026-S7OK05W", 19, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 20, "EDU-2026-2MYNLK1", 20, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 21, "EDU-2026-7M9PDGH", 21, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 724.23m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 22, "EDU-2026-TIXLNIB", 22, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 23, "EDU-2026-T5N58HB", 23, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 24, "EDU-2026-7MH1FBP", 24, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 25, "EDU-2026-GDE8Q6P", 25, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 26, "EDU-2026-SNYKN82", 26, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 27, "EDU-2026-QJZM8YL", 27, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 691.53m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 28, "EDU-2026-G0N8QXW", 28, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 29, "EDU-2026-A3B8IRY", 29, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 30, "EDU-2026-CYOBPQU", 30, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 31, "EDU-2026-20G2H3L", 31, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 32, "EDU-2026-9RY98UG", 32, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 33, "EDU-2026-LBUTZEW", 33, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 658.83m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 34, "EDU-2026-CVJPW51", 34, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 35, "EDU-2026-GL31H3K", 35, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 36, "EDU-2026-L1OX9Y0", 36, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 37, "EDU-2026-3V9Y2I0", 37, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 38, "EDU-2026-UIX61F4", 38, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 39, "EDU-2026-NQZL2LT", 39, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 626.13m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 40, "EDU-2026-2A9X7HT", 40, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 41, "EDU-2026-HIZFYQR", 41, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 42, "EDU-2026-5FO1ERB", 42, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 43, "EDU-2026-C6GLP5G", 43, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 44, "EDU-2026-VBGBHB4", 44, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 45, "EDU-2026-PMQT1K5", 45, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 593.43m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 46, "EDU-2026-Z2MR7S3", 46, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 47, "EDU-2026-2OK04RQ", 47, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 48, "EDU-2026-5L2W56M", 48, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 49, "EDU-2026-7UZZHKN", 49, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 50, "EDU-2026-9DG3ZSZ", 50, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1000m, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "EducationAccountSweepTargets",
                columns: new[] { "Id", "Action", "Nric", "Reason", "Status", "SweepReportId" },
                values: new object[,]
                {
                    { 1, 0, "S0000001I", "Pending account creation request.", 0, 1 },
                    { 2, 1, "S0000002G", "Account closure completed successfully.", 1, 1 },
                    { 3, 2, "S0000003E", "Account extension failed validation.", 2, 1 },
                    { 4, 1, "S0000004C", "Account closure completed successfully.", 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "FasScheme",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "DurationInMonths", "IsDeleted", "IsPerComponent", "PublishedAt", "SchemeCode", "SchemeName", "SchoolId", "Status", "SubsidyType", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-99JIVES", "Seed FAS Scheme 01", 1, 2, 2, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-ETESJRO", "Seed FAS Scheme 02", 1, 2, 2, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-BCKWR0F", "Seed FAS Scheme 03", 1, 2, 2, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-XY25JRK", "Seed FAS Scheme 04", 1, 2, 2, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-14PI50K", "Seed FAS Scheme 05", 1, 2, 2, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-94EERHM", "Seed FAS Scheme 06", 1, 2, 2, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-Z2RKIYE", "Seed FAS Scheme 07", 1, 2, 2, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-3ZNQSMN", "Seed FAS Scheme 08", 1, 2, 2, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-T3E6QO3", "Seed FAS Scheme 09", 1, 2, 2, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-5AED0DC", "Seed FAS Scheme 10", 1, 2, 2, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-7F2AA2W", "Seed FAS Scheme 11", 1, 2, 2, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-MDR3M24", "Seed FAS Scheme 12", 1, 2, 2, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-X7LT4SX", "Seed FAS Scheme 13", 1, 2, 2, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-B2B3OO9", "Seed FAS Scheme 14", 1, 2, 2, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-NYLMULZ", "Seed FAS Scheme 15", 1, 2, 2, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, null, "FAS-2026-ICZOJ17", "Seed FAS Scheme 16", 1, 1, 2, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, null, "FAS-2026-L682KDM", "Seed FAS Scheme 17", 1, 1, 2, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, null, "FAS-2026-QC170M4", "Seed FAS Scheme 18", 1, 1, 2, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, null, "FAS-2026-PYAD7M3", "Seed FAS Scheme 19", 1, 1, 2, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, null, "FAS-2026-MLG12JM", "Seed FAS Scheme 20", 1, 1, 2, null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, null, "FAS-2026-NR4ZOWZ", "Seed FAS Scheme 21", 1, 3, 2, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, null, "FAS-2026-M4KYXRX", "Seed FAS Scheme 22", 1, 3, 2, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, null, "FAS-2026-GZ0R7EF", "Seed FAS Scheme 23", 1, 3, 2, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-PYNOHDE", "Seed FAS Scheme 24", 1, 2, 2, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-KND633Z", "Seed FAS Scheme 25", 1, 2, 2, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-07EPV3J", "Seed FAS Scheme 26", 1, 2, 2, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-FD1GSLY", "Seed FAS Scheme 27", 1, 2, 2, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-A6Q5DL4", "Seed FAS Scheme 28", 1, 2, 2, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-6I8DUHA", "Seed FAS Scheme 29", 1, 2, 2, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-ZDFM1CJ", "Seed FAS Scheme 30", 1, 2, 2, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-18WS9SO", "Seed FAS Scheme 31", 1, 2, 2, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-4BU2HMW", "Seed FAS Scheme 32", 1, 2, 2, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-2H4T6CQ", "Seed FAS Scheme 33", 1, 2, 2, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-7LJJC53", "Seed FAS Scheme 34", 1, 2, 2, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-FQ40IYS", "Seed FAS Scheme 35", 1, 2, 2, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-WNMI04D", "Seed FAS Scheme 36", 1, 2, 2, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-8YOA8J5", "Seed FAS Scheme 37", 1, 2, 2, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-F5C5H8Q", "Seed FAS Scheme 38", 1, 2, 2, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-WBI1M2R", "Seed FAS Scheme 39", 1, 2, 2, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-557IYXH", "Seed FAS Scheme 40", 1, 2, 2, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-PG813XQ", "Seed FAS Scheme 41", 1, 2, 2, null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-I6J2I8R", "Seed FAS Scheme 42", 1, 2, 2, null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-K2PZ04U", "Seed FAS Scheme 43", 1, 2, 2, null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-8RLS525", "Seed FAS Scheme 44", 1, 2, 2, null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-549GJC5", "Seed FAS Scheme 45", 1, 2, 2, null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-BA22U8Z", "Seed FAS Scheme 46", 1, 2, 2, null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-3K472GX", "Seed FAS Scheme 47", 1, 2, 2, null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-8NS1G9W", "Seed FAS Scheme 48", 1, 2, 2, null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-2PUZCYD", "Seed FAS Scheme 49", 1, 2, 2, null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-1V9CJ5T", "Seed FAS Scheme 50", 1, 2, 2, null, null },
                    { 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-U33PLCY", "Seed FAS Scheme 51", 1, 2, 2, null, null },
                    { 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-7Q75CVE", "Seed FAS Scheme 52", 1, 2, 2, null, null },
                    { 53, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-QUBAMWD", "Seed FAS Scheme 53", 1, 2, 2, null, null },
                    { 54, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-6CWEG3J", "Seed FAS Scheme 54", 1, 2, 2, null, null },
                    { 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-M3EU0F4", "Seed FAS Scheme 55", 1, 2, 2, null, null },
                    { 56, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-1259CWR", "Seed FAS Scheme 56", 1, 2, 2, null, null },
                    { 57, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-JDAMPQ2", "Seed FAS Scheme 57", 1, 2, 2, null, null },
                    { 58, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-CRDWLCK", "Seed FAS Scheme 58", 1, 2, 2, null, null },
                    { 59, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-2YED0P1", "Seed FAS Scheme 59", 1, 2, 2, null, null },
                    { 60, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed financial assistance scheme.", 12, false, false, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "FAS-2026-Y1RBVLN", "Seed FAS Scheme 60", 1, 2, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "ManagementActionLog",
                columns: new[] { "Id", "Action", "ActorUserId", "BatchId", "EntityId", "EntityType", "IpAddress", "NewStatus", "OccurredAt", "PreviousStatus", "Reason" },
                values: new object[,]
                {
                    { 1, 4, 1, new Guid("11111111-1111-1111-1111-000000000001"), 1, 6, "127.0.0.1", "Active", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Draft", "Manual review passed, scheme published." },
                    { 2, 5, 1, new Guid("11111111-1111-1111-1111-000000000002"), 5, 5, "127.0.0.1", "Closed", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "InProgress", "Course duration finished." }
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
                    { 1, "Matched active eligibility conditions", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-001", 0, "topup-run-001", false, null, null, null, 1, 1, 1, 1, 85m, "Monthly Learning Credit Boost", 85m, 1, null, null },
                    { 2, "Matched active eligibility conditions", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-002", 0, "topup-run-002", false, null, null, 2, 2, 2, 1, null, 95m, "Monthly Meal Allowance", 95m, 1, null, null },
                    { 4, "Matched active eligibility conditions", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-004", 1, "topup-run-004", false, null, null, null, 1, 1, 0, 4, 115m, "Back-to-School Credit", 0m, 1, null, null },
                    { 5, "Matched active eligibility conditions", new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-005", 0, "topup-run-005", false, null, null, 5, 2, 2, 1, null, 125m, "Monthly Attendance Support", 125m, 1, null, null },
                    { 7, "Matched active eligibility conditions", new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-007", 0, "topup-run-007", false, null, null, null, 1, 1, 1, 7, 145m, "STEM Enrichment Credit", 145m, 1, null, null },
                    { 8, "Matched active eligibility conditions", new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-008", 1, "topup-run-008", false, null, null, 8, 2, 2, 0, null, 155m, "Monthly Commuter Credit", 0m, 1, null, null },
                    { 10, "Matched active eligibility conditions", new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "TOPUP-RUN-010", 0, "topup-run-010", false, null, null, null, 1, 1, 1, 10, 175m, "Holiday Programme Support", 175m, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CitizenId", "CreatedAt", "CreatedBy", "DeletedAt", "FailedLoginCount", "IsDeleted", "LastLoginAt", "LockedUntil", "Role", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 4, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 22, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 23, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 24, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 25, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 26, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 27, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 28, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 29, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 30, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 31, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 32, 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 33, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 34, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 35, 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 36, 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 37, 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 38, 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 39, 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 40, 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 41, 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 42, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 43, 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 44, 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 45, 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 46, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 47, 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 48, 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 49, 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null },
                    { 50, 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "AuditLog",
                columns: new[] { "Id", "Action", "ActorUserId", "Category", "IpAddress", "Nric", "OccurredAt" },
                values: new object[,]
                {
                    { 4, "Refreshed authentication token", 4, 4, "127.0.0.5", null, new DateTime(2026, 1, 1, 4, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "Approved manual account request", 4, 1, "127.0.0.9", null, new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, "Posted education credit transaction", 4, 5, "127.0.0.13", null, new DateTime(2026, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, "Updated school status", 4, 2, "127.0.0.17", null, new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, "Marked charge overdue", 4, 6, "127.0.0.21", null, new DateTime(2026, 1, 1, 20, 0, 0, 0, DateTimeKind.Utc) },
                    { 24, "Completed manual top-up", 4, 3, "127.0.0.25", null, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 28, "Queued automation insight", 4, 7, "127.0.0.29", null, new DateTime(2026, 1, 2, 4, 0, 0, 0, DateTimeKind.Utc) },
                    { 32, "Refreshed authentication token", 4, 4, "127.0.0.33", null, new DateTime(2026, 1, 2, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 36, "Approved manual account request", 4, 1, "127.0.0.37", null, new DateTime(2026, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 40, "Posted education credit transaction", 4, 5, "127.0.0.41", null, new DateTime(2026, 1, 2, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 44, "Updated school status", 4, 2, "127.0.0.45", null, new DateTime(2026, 1, 2, 20, 0, 0, 0, DateTimeKind.Utc) },
                    { 48, "Marked charge overdue", 4, 6, "127.0.0.49", null, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "EducationAccountStatusHistory",
                columns: new[] { "Id", "ChangedAt", "ChangedByUserId", "EducationAccountId", "NewStatus", "PreviousStatus", "Reason" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, 3, 1, "Account status updated after lifecycle review" },
                    { 2, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 2, 1, "Account status updated after lifecycle review" },
                    { 3, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3, 3, 1, "Account status updated after lifecycle review" },
                    { 4, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4, 2, 1, "Account status updated after lifecycle review" },
                    { 5, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5, 3, 1, "Account status updated after lifecycle review" },
                    { 6, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, 6, 2, 1, "Account status updated after lifecycle review" }
                });

            migrationBuilder.InsertData(
                table: "EducationCreditTransaction",
                columns: new[] { "Id", "Amount", "BalanceAfter", "BalanceBefore", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "Direction", "EducationAccountId", "IsDeleted", "TransactionCode", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 3, 177.67m, 822.33m, 1000m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment deduction", 2, 3, false, new Guid("00000000-0000-0000-0000-000000000003"), 2, null, null },
                    { 9, 210.37m, 789.63m, 1000m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment deduction", 2, 9, false, new Guid("00000000-0000-0000-0000-000000000009"), 2, null, null },
                    { 15, 243.07m, 756.93m, 1000m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment deduction", 2, 15, false, new Guid("00000000-0000-0000-0000-000000000015"), 2, null, null },
                    { 21, 275.77m, 724.23m, 1000m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment deduction", 2, 21, false, new Guid("00000000-0000-0000-0000-000000000021"), 2, null, null },
                    { 27, 308.47m, 691.53m, 1000m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment deduction", 2, 27, false, new Guid("00000000-0000-0000-0000-000000000027"), 2, null, null },
                    { 33, 341.17m, 658.83m, 1000m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment deduction", 2, 33, false, new Guid("00000000-0000-0000-0000-000000000033"), 2, null, null },
                    { 39, 373.87m, 626.13m, 1000m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment deduction", 2, 39, false, new Guid("00000000-0000-0000-0000-000000000039"), 2, null, null },
                    { 45, 406.57m, 593.43m, 1000m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment deduction", 2, 45, false, new Guid("00000000-0000-0000-0000-000000000045"), 2, null, null },
                    { 51, 185m, 815m, 1000m, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Course fee payment for Creative Thinking Cohort 51", 2, 1, false, new Guid("00000000-0000-0000-0000-000000000051"), 2, null, null },
                    { 101, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 1, false, new Guid("00000000-0000-0000-0000-000000000101"), 1, null, null },
                    { 102, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 2, false, new Guid("00000000-0000-0000-0000-000000000102"), 1, null, null },
                    { 103, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 3, false, new Guid("00000000-0000-0000-0000-000000000103"), 1, null, null },
                    { 104, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 4, false, new Guid("00000000-0000-0000-0000-000000000104"), 1, null, null },
                    { 105, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 5, false, new Guid("00000000-0000-0000-0000-000000000105"), 1, null, null },
                    { 106, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 6, false, new Guid("00000000-0000-0000-0000-000000000106"), 1, null, null },
                    { 107, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 7, false, new Guid("00000000-0000-0000-0000-000000000107"), 1, null, null },
                    { 108, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 8, false, new Guid("00000000-0000-0000-0000-000000000108"), 1, null, null },
                    { 109, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 9, false, new Guid("00000000-0000-0000-0000-000000000109"), 1, null, null },
                    { 110, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 10, false, new Guid("00000000-0000-0000-0000-000000000110"), 1, null, null },
                    { 111, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 11, false, new Guid("00000000-0000-0000-0000-000000000111"), 1, null, null },
                    { 112, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 12, false, new Guid("00000000-0000-0000-0000-000000000112"), 1, null, null },
                    { 113, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 13, false, new Guid("00000000-0000-0000-0000-000000000113"), 1, null, null },
                    { 114, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 14, false, new Guid("00000000-0000-0000-0000-000000000114"), 1, null, null },
                    { 115, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 15, false, new Guid("00000000-0000-0000-0000-000000000115"), 1, null, null },
                    { 116, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 16, false, new Guid("00000000-0000-0000-0000-000000000116"), 1, null, null },
                    { 117, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 17, false, new Guid("00000000-0000-0000-0000-000000000117"), 1, null, null },
                    { 118, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 18, false, new Guid("00000000-0000-0000-0000-000000000118"), 1, null, null },
                    { 119, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 19, false, new Guid("00000000-0000-0000-0000-000000000119"), 1, null, null },
                    { 120, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 20, false, new Guid("00000000-0000-0000-0000-000000000120"), 1, null, null },
                    { 121, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 21, false, new Guid("00000000-0000-0000-0000-000000000121"), 1, null, null },
                    { 122, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 22, false, new Guid("00000000-0000-0000-0000-000000000122"), 1, null, null },
                    { 123, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 23, false, new Guid("00000000-0000-0000-0000-000000000123"), 1, null, null },
                    { 124, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 24, false, new Guid("00000000-0000-0000-0000-000000000124"), 1, null, null },
                    { 125, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 25, false, new Guid("00000000-0000-0000-0000-000000000125"), 1, null, null },
                    { 126, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 26, false, new Guid("00000000-0000-0000-0000-000000000126"), 1, null, null },
                    { 127, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 27, false, new Guid("00000000-0000-0000-0000-000000000127"), 1, null, null },
                    { 128, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 28, false, new Guid("00000000-0000-0000-0000-000000000128"), 1, null, null },
                    { 129, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 29, false, new Guid("00000000-0000-0000-0000-000000000129"), 1, null, null },
                    { 130, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 30, false, new Guid("00000000-0000-0000-0000-000000000130"), 1, null, null },
                    { 131, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 31, false, new Guid("00000000-0000-0000-0000-000000000131"), 1, null, null },
                    { 132, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 32, false, new Guid("00000000-0000-0000-0000-000000000132"), 1, null, null },
                    { 133, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 33, false, new Guid("00000000-0000-0000-0000-000000000133"), 1, null, null },
                    { 134, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 34, false, new Guid("00000000-0000-0000-0000-000000000134"), 1, null, null },
                    { 135, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 35, false, new Guid("00000000-0000-0000-0000-000000000135"), 1, null, null },
                    { 136, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 36, false, new Guid("00000000-0000-0000-0000-000000000136"), 1, null, null },
                    { 137, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 37, false, new Guid("00000000-0000-0000-0000-000000000137"), 1, null, null },
                    { 138, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 38, false, new Guid("00000000-0000-0000-0000-000000000138"), 1, null, null },
                    { 139, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 39, false, new Guid("00000000-0000-0000-0000-000000000139"), 1, null, null },
                    { 140, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 40, false, new Guid("00000000-0000-0000-0000-000000000140"), 1, null, null },
                    { 141, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 41, false, new Guid("00000000-0000-0000-0000-000000000141"), 1, null, null },
                    { 142, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 42, false, new Guid("00000000-0000-0000-0000-000000000142"), 1, null, null },
                    { 143, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 43, false, new Guid("00000000-0000-0000-0000-000000000143"), 1, null, null },
                    { 144, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 44, false, new Guid("00000000-0000-0000-0000-000000000144"), 1, null, null },
                    { 145, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 45, false, new Guid("00000000-0000-0000-0000-000000000145"), 1, null, null },
                    { 146, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 46, false, new Guid("00000000-0000-0000-0000-000000000146"), 1, null, null },
                    { 147, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 47, false, new Guid("00000000-0000-0000-0000-000000000147"), 1, null, null },
                    { 148, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 48, false, new Guid("00000000-0000-0000-0000-000000000148"), 1, null, null },
                    { 149, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 49, false, new Guid("00000000-0000-0000-0000-000000000149"), 1, null, null },
                    { 150, 1000m, 1000m, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Initial system top-up", 1, 50, false, new Guid("00000000-0000-0000-0000-000000000150"), 1, null, null },
                    { 203, 105m, 1105m, 1000m, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Manual Account Adjustment", 1, 3, false, new Guid("00000000-0000-0000-0000-000000000203"), 1, null, null },
                    { 207, 145m, 1145m, 1000m, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "STEM Enrichment Credit", 1, 7, false, new Guid("00000000-0000-0000-0000-000000000207"), 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeAdditionalQuestion",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "FasSchemeId", "IsDeleted", "IsRequired", "QuestionText", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 23, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 23, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 24, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 24, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 25, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 25, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 26, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 26, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 53, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 54, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 28, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 56, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 28, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 57, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 29, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 58, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 29, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 59, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 30, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 60, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 30, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 61, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 31, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 62, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 31, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 63, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 32, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 64, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 32, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 65, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 33, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 66, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 33, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 67, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 34, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 68, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 34, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 69, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 35, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 70, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 35, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 71, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 36, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 72, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 36, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 73, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 37, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 74, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 37, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 75, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 38, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 76, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 38, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 77, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 39, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 78, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 39, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 79, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 40, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 80, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 40, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 81, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 41, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 82, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 41, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 83, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 42, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 84, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 42, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 85, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 43, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 86, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 43, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 87, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 44, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 88, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 44, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 89, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 45, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 90, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 45, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 91, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 46, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 92, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 46, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 93, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 47, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 94, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 47, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 95, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 48, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 96, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 48, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 97, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 49, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 98, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 49, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 99, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 50, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 100, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 50, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 101, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 51, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 102, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 51, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 103, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 52, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 104, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 52, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 105, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 53, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 106, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 53, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 107, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 54, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 108, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 54, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 109, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 55, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 110, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 55, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 111, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 56, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 112, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 56, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 113, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 57, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 114, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 57, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 115, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 58, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 116, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 58, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 117, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 59, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 118, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 59, false, false, "Are there any specific medical conditions in your household?", null, null },
                    { 119, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 60, false, true, "Briefly explain the primary reason for your financial assistance application.", null, null },
                    { 120, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 60, false, false, "Are there any specific medical conditions in your household?", null, null }
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
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 10, false, 1, null, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 11, false, 1, null, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 12, false, 1, null, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 13, false, 1, null, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 14, false, 1, null, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 15, false, 1, null, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 16, false, 1, null, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 17, false, 1, null, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 18, false, 1, null, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 19, false, 1, null, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 20, false, 1, null, null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 21, false, 1, null, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 22, false, 1, null, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 23, false, 1, null, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 24, false, 1, null, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 25, false, 1, null, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 26, false, 1, null, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 27, false, 1, null, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 28, false, 1, null, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 29, false, 1, null, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 30, false, 1, null, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 31, false, 1, null, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 32, false, 1, null, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 33, false, 1, null, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 34, false, 1, null, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 35, false, 1, null, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 36, false, 1, null, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 37, false, 1, null, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 38, false, 1, null, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 39, false, 1, null, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 40, false, 1, null, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 41, false, 1, null, null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 42, false, 1, null, null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 43, false, 1, null, null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 44, false, 1, null, null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 45, false, 1, null, null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 46, false, 1, null, null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 47, false, 1, null, null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 48, false, 1, null, null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 49, false, 1, null, null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 50, false, 1, null, null, null },
                    { 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 51, false, 1, null, null, null },
                    { 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 52, false, 1, null, null, null },
                    { 53, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 53, false, 1, null, null, null },
                    { 54, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 54, false, 1, null, null, null },
                    { 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 55, false, 1, null, null, null },
                    { 56, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 56, false, 1, null, null, null },
                    { 57, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 57, false, 1, null, null, null },
                    { 58, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 58, false, 1, null, null, null },
                    { 59, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 59, false, 1, null, null, null },
                    { 60, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 60, false, 1, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeCourse",
                columns: new[] { "Id", "CourseId", "CreatedAt", "CreatedBy", "DeletedAt", "FasSchemeId", "IsDeleted", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null },
                    { 2, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null },
                    { 3, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, null, null },
                    { 4, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, null, null },
                    { 5, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, null, null },
                    { 6, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, null, null },
                    { 7, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, null, null },
                    { 8, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, null, null },
                    { 9, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, null, null },
                    { 10, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, null, null },
                    { 11, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, false, null, null },
                    { 12, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, false, null, null },
                    { 13, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, false, null, null },
                    { 14, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, false, null, null },
                    { 15, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, false, null, null },
                    { 16, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, false, null, null },
                    { 17, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, false, null, null },
                    { 18, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, false, null, null },
                    { 19, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, false, null, null },
                    { 20, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, false, null, null },
                    { 21, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, false, null, null },
                    { 22, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, false, null, null },
                    { 23, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 23, false, null, null },
                    { 24, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 24, false, null, null },
                    { 25, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 25, false, null, null },
                    { 26, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 26, false, null, null },
                    { 27, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, false, null, null },
                    { 28, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 28, false, null, null },
                    { 29, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 29, false, null, null },
                    { 30, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 30, false, null, null },
                    { 31, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 31, false, null, null },
                    { 32, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 32, false, null, null },
                    { 33, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 33, false, null, null },
                    { 34, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 34, false, null, null },
                    { 35, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 35, false, null, null },
                    { 36, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 36, false, null, null },
                    { 37, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 37, false, null, null },
                    { 38, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 38, false, null, null },
                    { 39, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 39, false, null, null },
                    { 40, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 40, false, null, null },
                    { 41, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 41, false, null, null },
                    { 42, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 42, false, null, null },
                    { 43, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 43, false, null, null },
                    { 44, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 44, false, null, null },
                    { 45, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 45, false, null, null },
                    { 46, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 46, false, null, null },
                    { 47, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 47, false, null, null },
                    { 48, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 48, false, null, null },
                    { 49, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 49, false, null, null },
                    { 50, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 50, false, null, null },
                    { 51, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 51, false, null, null },
                    { 52, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 52, false, null, null },
                    { 53, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 53, false, null, null },
                    { 54, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 54, false, null, null },
                    { 55, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 55, false, null, null },
                    { 56, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 56, false, null, null },
                    { 57, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 57, false, null, null },
                    { 58, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 58, false, null, null },
                    { 59, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 59, false, null, null },
                    { 60, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 60, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeRequiredDocument",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "DocumentName", "FasSchemeId", "IsDeleted", "TemplateFileKey", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 1, false, "fas/templates/document-01.pdf", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 2, false, "fas/templates/document-02.pdf", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 3, false, "fas/templates/document-03.pdf", null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 4, false, "fas/templates/document-04.pdf", null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 5, false, "fas/templates/document-05.pdf", null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 6, false, "fas/templates/document-06.pdf", null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 7, false, "fas/templates/document-07.pdf", null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 8, false, "fas/templates/document-08.pdf", null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 9, false, "fas/templates/document-09.pdf", null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 10, false, "fas/templates/document-10.pdf", null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 11, false, "fas/templates/document-11.pdf", null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 12, false, "fas/templates/document-12.pdf", null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 13, false, "fas/templates/document-13.pdf", null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 14, false, "fas/templates/document-14.pdf", null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 15, false, "fas/templates/document-15.pdf", null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 16, false, "fas/templates/document-16.pdf", null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 17, false, "fas/templates/document-17.pdf", null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 18, false, "fas/templates/document-18.pdf", null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 19, false, "fas/templates/document-19.pdf", null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 20, false, "fas/templates/document-20.pdf", null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 21, false, "fas/templates/document-21.pdf", null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 22, false, "fas/templates/document-22.pdf", null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 23, false, "fas/templates/document-23.pdf", null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 24, false, "fas/templates/document-24.pdf", null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 25, false, "fas/templates/document-25.pdf", null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 26, false, "fas/templates/document-26.pdf", null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 27, false, "fas/templates/document-27.pdf", null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 28, false, "fas/templates/document-28.pdf", null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 29, false, "fas/templates/document-29.pdf", null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 30, false, "fas/templates/document-30.pdf", null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 31, false, "fas/templates/document-31.pdf", null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 32, false, "fas/templates/document-32.pdf", null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 33, false, "fas/templates/document-33.pdf", null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 34, false, "fas/templates/document-34.pdf", null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 35, false, "fas/templates/document-35.pdf", null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 36, false, "fas/templates/document-36.pdf", null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 37, false, "fas/templates/document-37.pdf", null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 38, false, "fas/templates/document-38.pdf", null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 39, false, "fas/templates/document-39.pdf", null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 40, false, "fas/templates/document-40.pdf", null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 41, false, "fas/templates/document-41.pdf", null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 42, false, "fas/templates/document-42.pdf", null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 43, false, "fas/templates/document-43.pdf", null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 44, false, "fas/templates/document-44.pdf", null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 45, false, "fas/templates/document-45.pdf", null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 46, false, "fas/templates/document-46.pdf", null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 47, false, "fas/templates/document-47.pdf", null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 48, false, "fas/templates/document-48.pdf", null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 49, false, "fas/templates/document-49.pdf", null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 50, false, "fas/templates/document-50.pdf", null, null },
                    { 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 51, false, "fas/templates/document-51.pdf", null, null },
                    { 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 52, false, "fas/templates/document-52.pdf", null, null },
                    { 53, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 53, false, "fas/templates/document-53.pdf", null, null },
                    { 54, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 54, false, "fas/templates/document-54.pdf", null, null },
                    { 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 55, false, "fas/templates/document-55.pdf", null, null },
                    { 56, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 56, false, "fas/templates/document-56.pdf", null, null },
                    { 57, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 57, false, "fas/templates/document-57.pdf", null, null },
                    { 58, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 58, false, "fas/templates/document-58.pdf", null, null },
                    { 59, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 59, false, "fas/templates/document-59.pdf", null, null },
                    { 60, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Income Statement", 60, false, "fas/templates/document-60.pdf", null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeTier",
                columns: new[] { "Id", "CourseFeeSubsidyValue", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "FasSchemeId", "IsDeleted", "MaxGrossHouseholdIncome", "MaxPerCapitaIncome", "MiscFeeSubsidyValue", "SubsidyValue", "TierName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 3, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 4, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 5, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 7, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 8, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 9, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 10, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 11, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 12, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 12, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 13, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 13, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 14, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 14, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 15, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 15, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 16, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 16, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 17, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 17, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 18, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 18, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 19, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 19, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 20, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 20, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 21, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 21, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 22, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 22, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 23, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 23, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 24, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 24, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 25, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 25, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 26, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 26, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 27, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 27, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 28, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 28, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 29, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 29, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 30, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 30, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 31, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 31, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 32, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 32, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 33, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 33, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 34, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 34, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 35, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 35, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 36, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 36, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 37, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 37, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 38, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 38, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 39, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 39, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 40, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 40, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 41, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 41, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 42, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 42, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 43, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 43, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 44, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 44, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 45, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 45, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 46, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 46, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 47, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 47, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 48, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 48, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 49, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 49, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 50, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 50, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 51, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 51, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 52, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 52, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 53, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 53, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 54, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 54, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 55, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 55, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 56, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 56, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 57, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 57, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 58, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 58, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 59, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 59, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 60, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 60, false, null, 500m, null, 100m, "Tier 1", null, null },
                    { 61, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 62, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 63, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 3, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 64, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 4, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 65, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 5, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 66, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 6, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 67, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 7, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 68, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 8, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 69, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 9, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 70, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 10, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 71, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 11, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 72, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 12, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 73, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 13, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 74, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 14, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 75, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 15, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 76, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 16, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 77, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 17, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 78, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 18, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 79, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 19, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 80, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 20, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 81, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 21, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 82, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 22, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 83, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 23, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 84, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 24, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 85, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 25, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 86, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 26, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 87, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 27, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 88, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 28, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 89, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 29, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 90, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 30, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 91, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 31, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 92, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 32, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 93, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 33, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 94, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 34, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 95, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 35, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 96, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 36, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 97, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 37, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 98, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 38, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 99, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 39, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 100, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 40, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 101, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 41, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 102, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 42, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 103, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 43, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 104, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 44, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 105, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 45, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 106, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 46, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 107, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 47, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 108, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 48, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 109, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 49, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 110, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 50, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 111, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 51, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 112, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 52, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 113, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 53, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 114, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 54, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 115, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 55, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 116, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 56, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 117, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 57, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 118, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 58, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 119, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 59, false, null, 1000m, null, 70m, "Tier 2", null, null },
                    { 120, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 60, false, null, 1000m, null, 70m, "Tier 2", null, null }
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
                    { 1, "EDU-2026-M7QVS7X", 85m, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 1, null, null, false, "Matched household income condition", 1, 1, null, null },
                    { 2, "EDU-2026-H4W9JYQ", 95m, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 2, null, null, false, "Matched household income condition", 2, 2, null, null },
                    { 4, "EDU-2026-K6CAJET", 115m, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 4, null, "Account failed validation and requires review", false, "Matched household income condition", 4, 4, null, null },
                    { 5, "EDU-2026-C865DQ8", 125m, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 5, null, null, false, "Matched household income condition", 1, 5, null, null },
                    { 6, "EDU-2026-J3PD5GI", 135m, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 6, null, null, false, "Matched household income condition", 2, 6, null, null },
                    { 8, "EDU-2026-ERZPP9D", 155m, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 8, null, "Account failed validation and requires review", false, "Matched household income condition", 4, 8, null, null },
                    { 9, "EDU-2026-3WTXQL2", 165m, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 9, null, null, false, "Matched household income condition", 1, 9, null, null },
                    { 10, "EDU-2026-5A2BWXE", 175m, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 10, null, null, false, "Matched household income condition", 2, 10, null, null }
                });

            migrationBuilder.InsertData(
                table: "Enrollment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenEmailSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CitizenPhoneNumberSnapshot", "CourseDescriptionSnapshot", "CourseId", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "SchoolNameSnapshot", "SchoolStudentId", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-M7QVS7X", "sterling.quach@example.com", "Sterling Quach", "S0000001I", "+6590000001", "Course enrollment created for school-admin review.", 1, "Academic Writing Cohort 01", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 1, 1, null, null },
                    { 2, "EDU-2026-H4W9JYQ", "amelia.tan@example.com", "Amelia Tan", "S0000002G", "+6590000002", "Course enrollment created for school-admin review.", 2, "Business Numeracy Cohort 02", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 2, 1, null, null },
                    { 3, "EDU-2026-5BHC84W", "marcus.lim@example.com", "Marcus Lim", "S0000003E", "+6590000003", "Course enrollment created for school-admin review.", 3, "Digital Literacy Cohort 03", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 3, 1, null, null },
                    { 4, "EDU-2026-K6CAJET", "priya.nair@example.com", "Priya Nair", "S0000004C", "+6590000004", "Course enrollment created for school-admin review.", 4, "Career Readiness Cohort 04", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 4, 1, null, null },
                    { 5, "EDU-2026-C865DQ8", "ethan.koh@example.com", "Ethan Koh", "S0000005A", "+6590000005", "Course enrollment created for school-admin review.", 5, "Applied Science Cohort 05", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 5, 1, null, null },
                    { 6, "EDU-2026-J3PD5GI", "hannah.lee@example.com", "Hannah Lee", "S0000006Z", "+6590000006", "Course enrollment created for school-admin review.", 6, "Financial Literacy Cohort 06", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 6, 1, null, null },
                    { 7, "EDU-2026-RHEGIE7", "daniel.wong@example.com", "Daniel Wong", "S0000007H", "+6590000007", "Course enrollment created for school-admin review.", 7, "Project Collaboration Cohort 07", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 7, 1, null, null },
                    { 8, "EDU-2026-ERZPP9D", "sofia.chen@example.com", "Sofia Chen", "S0000008F", "+6590000008", "Course enrollment created for school-admin review.", 8, "Data Skills Cohort 08", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 8, 2, null, null },
                    { 9, "EDU-2026-3WTXQL2", "lucas.nguyen@example.com", "Lucas Nguyen", "S0000009D", "+6590000009", "Course enrollment created for school-admin review.", 9, "Workplace Communication Cohort 09", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 9, 1, null, null },
                    { 10, "EDU-2026-5A2BWXE", "maya.rahman@example.com", "Maya Rahman", "S0000010H", "+6590000010", "Course enrollment created for school-admin review.", 10, "Software Foundations Cohort 10", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 10, 1, null, null },
                    { 11, "EDU-2026-YKWELYD", "noah.teo@example.com", "Noah Teo", "S0000011F", "+6590000011", "Course enrollment created for school-admin review.", 11, "Academic Writing Cohort 11", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 11, 1, null, null },
                    { 12, "EDU-2026-XW95AJ5", "aisha.fernandez@example.com", "Aisha Fernandez", "S0000012D", "+6590000012", "Course enrollment created for school-admin review.", 12, "Business Numeracy Cohort 12", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 12, 1, null, null },
                    { 13, "EDU-2026-OOROKGZ", "ryan.chua@example.com", "Ryan Chua", "S0000013B", "+6590000013", "Course enrollment created for school-admin review.", 13, "Digital Literacy Cohort 13", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 13, 1, null, null },
                    { 14, "EDU-2026-EC0POYJ", "chloe.goh@example.com", "Chloe Goh", "S0000014J", "+6590000014", "Course enrollment created for school-admin review.", 14, "Career Readiness Cohort 14", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 14, 1, null, null },
                    { 15, "EDU-2026-1ZHBQ2S", "irfan.hassan@example.com", "Irfan Hassan", "S0000015I", "+6590000015", "Course enrollment created for school-admin review.", 15, "Applied Science Cohort 15", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 15, 1, null, null },
                    { 16, "EDU-2026-A5L21GT", "natalie.seah@example.com", "Natalie Seah", "S0000016G", "+6590000016", "Course enrollment created for school-admin review.", 16, "Financial Literacy Cohort 16", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 16, 2, null, null },
                    { 17, "EDU-2026-R9VEYR0", "alina.ang@example.com", "Alina Ang", "S0000017E", "+6590000017", "Course enrollment created for school-admin review.", 17, "Project Collaboration Cohort 17", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 17, 1, null, null },
                    { 18, "EDU-2026-JLH113O", "benjamin.bala@example.com", "Benjamin Bala", "S0000018C", "+6590000018", "Course enrollment created for school-admin review.", 18, "Data Skills Cohort 18", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 18, 1, null, null },
                    { 19, "EDU-2026-S7OK05W", "clara.chew@example.com", "Clara Chew", "S0000019A", "+6590000019", "Course enrollment created for school-admin review.", 19, "Workplace Communication Cohort 19", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 19, 1, null, null },
                    { 20, "EDU-2026-2MYNLK1", "darius.das@example.com", "Darius Das", "S0000020E", "+6590000020", "Course enrollment created for school-admin review.", 20, "Software Foundations Cohort 20", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 20, 1, null, null },
                    { 21, "EDU-2026-7M9PDGH", "elena.eng@example.com", "Elena Eng", "S0000021C", "+6590000021", "Course enrollment created for school-admin review.", 21, "Academic Writing Cohort 21", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 21, 1, null, null },
                    { 22, "EDU-2026-TIXLNIB", "farhan.foo@example.com", "Farhan Foo", "S0000022A", "+6590000022", "Course enrollment created for school-admin review.", 22, "Business Numeracy Cohort 22", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 22, 1, null, null },
                    { 23, "EDU-2026-T5N58HB", "grace.gan@example.com", "Grace Gan", "S0000023Z", "+6590000023", "Course enrollment created for school-admin review.", 23, "Digital Literacy Cohort 23", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 23, 1, null, null },
                    { 24, "EDU-2026-7MH1FBP", "haruto.ho@example.com", "Haruto Ho", "S0000024H", "+6590000024", "Course enrollment created for school-admin review.", 24, "Career Readiness Cohort 24", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 24, 2, null, null },
                    { 25, "EDU-2026-GDE8Q6P", "isabelle.ismail@example.com", "Isabelle Ismail", "S0000025F", "+6590000025", "Course enrollment created for school-admin review.", 25, "Applied Science Cohort 25", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 25, 1, null, null },
                    { 26, "EDU-2026-SNYKN82", "jasper.jeyaratnam@example.com", "Jasper Jeyaratnam", "S0000026D", "+6590000026", "Course enrollment created for school-admin review.", 26, "Financial Literacy Cohort 26", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 26, 1, null, null },
                    { 27, "EDU-2026-QJZM8YL", "keira.kwek@example.com", "Keira Kwek", "S0000027B", "+6590000027", "Course enrollment created for school-admin review.", 27, "Project Collaboration Cohort 27", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 27, 1, null, null },
                    { 28, "EDU-2026-G0N8QXW", "leon.lim@example.com", "Leon Lim", "S0000028J", "+6590000028", "Course enrollment created for school-admin review.", 28, "Data Skills Cohort 28", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 28, 1, null, null },
                    { 29, "EDU-2026-A3B8IRY", "mei.lin.mohamed@example.com", "Mei Lin Mohamed", "S0000029I", "+6590000029", "Course enrollment created for school-admin review.", 29, "Workplace Communication Cohort 29", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 29, 1, null, null },
                    { 30, "EDU-2026-CYOBPQU", "nathan.ng@example.com", "Nathan Ng", "S0000030B", "+6590000030", "Course enrollment created for school-admin review.", 30, "Software Foundations Cohort 30", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 30, 1, null, null },
                    { 31, "EDU-2026-20G2H3L", "olivia.ong@example.com", "Olivia Ong", "S0000031J", "+6590000031", "Course enrollment created for school-admin review.", 31, "Academic Writing Cohort 31", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 31, 1, null, null },
                    { 32, "EDU-2026-9RY98UG", "pranav.pillai@example.com", "Pranav Pillai", "S0000032I", "+6590000032", "Course enrollment created for school-admin review.", 32, "Business Numeracy Cohort 32", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 32, 2, null, null },
                    { 33, "EDU-2026-LBUTZEW", "qistina.quek@example.com", "Qistina Quek", "S0000033G", "+6590000033", "Course enrollment created for school-admin review.", 33, "Digital Literacy Cohort 33", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 33, 1, null, null },
                    { 34, "EDU-2026-CVJPW51", "rafael.rao@example.com", "Rafael Rao", "S0000034E", "+6590000034", "Course enrollment created for school-admin review.", 34, "Career Readiness Cohort 34", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 34, 1, null, null },
                    { 35, "EDU-2026-GL31H3K", "selina.sim@example.com", "Selina Sim", "S0000035C", "+6590000035", "Course enrollment created for school-admin review.", 35, "Applied Science Cohort 35", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 35, 1, null, null },
                    { 36, "EDU-2026-L1OX9Y0", "terence.tan@example.com", "Terence Tan", "S0000036A", "+6590000036", "Course enrollment created for school-admin review.", 36, "Financial Literacy Cohort 36", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 36, 1, null, null },
                    { 37, "EDU-2026-3V9Y2I0", "umairah.uddin@example.com", "Umairah Uddin", "S0000037Z", "+6590000037", "Course enrollment created for school-admin review.", 37, "Project Collaboration Cohort 37", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 37, 1, null, null },
                    { 38, "EDU-2026-UIX61F4", "victor.vasquez@example.com", "Victor Vasquez", "S0000038H", "+6590000038", "Course enrollment created for school-admin review.", 38, "Data Skills Cohort 38", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 38, 1, null, null },
                    { 39, "EDU-2026-NQZL2LT", "wen.jie.wong@example.com", "Wen Jie Wong", "S0000039F", "+6590000039", "Course enrollment created for school-admin review.", 39, "Workplace Communication Cohort 39", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 39, 1, null, null },
                    { 40, "EDU-2026-2A9X7HT", "xavier.xu@example.com", "Xavier Xu", "S0000040Z", "+6590000040", "Course enrollment created for school-admin review.", 40, "Software Foundations Cohort 40", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 40, 2, null, null },
                    { 41, "EDU-2026-HIZFYQR", "yasmin.yeo@example.com", "Yasmin Yeo", "S0000041H", "+6590000041", "Course enrollment created for school-admin review.", 41, "Academic Writing Cohort 41", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 41, 1, null, null },
                    { 42, "EDU-2026-5FO1ERB", "zachary.zainal@example.com", "Zachary Zainal", "S0000042F", "+6590000042", "Course enrollment created for school-admin review.", 42, "Business Numeracy Cohort 42", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 42, 1, null, null },
                    { 43, "EDU-2026-C6GLP5G", "adeline.ang@example.com", "Adeline Ang", "S0000043D", "+6590000043", "Course enrollment created for school-admin review.", 43, "Digital Literacy Cohort 43", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 43, 1, null, null },
                    { 44, "EDU-2026-VBGBHB4", "brandon.bala@example.com", "Brandon Bala", "S0000044B", "+6590000044", "Course enrollment created for school-admin review.", 44, "Career Readiness Cohort 44", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 44, 1, null, null },
                    { 45, "EDU-2026-PMQT1K5", "celeste.chew@example.com", "Celeste Chew", "S0000045J", "+6590000045", "Course enrollment created for school-admin review.", 45, "Applied Science Cohort 45", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 45, 1, null, null },
                    { 46, "EDU-2026-Z2MR7S3", "damien.das@example.com", "Damien Das", "S0000046I", "+6590000046", "Course enrollment created for school-admin review.", 46, "Financial Literacy Cohort 46", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 46, 1, null, null },
                    { 47, "EDU-2026-2OK04RQ", "evelyn.eng@example.com", "Evelyn Eng", "S0000047G", "+6590000047", "Course enrollment created for school-admin review.", 47, "Project Collaboration Cohort 47", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 47, 1, null, null },
                    { 48, "EDU-2026-5L2W56M", "faris.foo@example.com", "Faris Foo", "S0000048E", "+6590000048", "Course enrollment created for school-admin review.", 48, "Data Skills Cohort 48", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 48, 2, null, null },
                    { 49, "EDU-2026-7UZZHKN", "giselle.gan@example.com", "Giselle Gan", "S0000049C", "+6590000049", "Course enrollment created for school-admin review.", 49, "Workplace Communication Cohort 49", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 49, 1, null, null },
                    { 50, "EDU-2026-9DG3ZSZ", "haziq.ho@example.com", "Haziq Ho", "S0000050G", "+6590000050", "Course enrollment created for school-admin review.", 50, "Software Foundations Cohort 50", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 50, 1, null, null },
                    { 51, "EDU-2026-M7QVS7X", "phuckhang1088@gmail.com", "Sterling Quach", "S0000001I", "+6590000001", "Course enrollment created for school-admin review.", 51, "Creative Thinking Cohort 51", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 1, 1, null, null },
                    { 52, "EDU-2026-K6CAJET", "priya.nair@example.com", "Priya Nair", "S0000004C", "+6590000004", "Additional course enrollment with unpaid full charge.", 5, "Applied Science Cohort 52", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 4, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasApplication",
                columns: new[] { "Id", "ApplicationNumber", "ApprovedAt", "ApprovedByUserId", "ApprovedTierId", "CreatedAt", "CreatedBy", "DeletedAt", "DurationInMonthsSnapshot", "ExternalRejectionReason", "FasSchemeId", "GrossHouseholdIncomeSnapshot", "GuardianNationalitySnapshot", "HouseholdMemberCountSnapshot", "InternalRejectionReason", "IsDeleted", "PerCapitaIncomeSnapshot", "RecommendationReason", "RecommendedTierId", "SchoolStudentId", "Status", "StudentAgeSnapshot", "StudentNationalitySnapshot", "UpdatedAt", "UpdatedBy", "ValidityEndDate", "ValidityStartDate", "WithdrawnAt" },
                values: new object[,]
                {
                    { 1, "FASAPP-2026-0001", null, null, null, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, 1, 1800m, 1, 4, null, false, 450m, "PCI <= 500", 1, 1, 1, 18, 1, null, null, null, null, null },
                    { 2, "FASAPP-2026-0002", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, 62, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 2, 3000m, 1, 4, null, false, 750m, "PCI <= 1000", 62, 1, 2, 18, 1, null, null, new DateTime(2027, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 3, "FASAPP-2026-0003", null, null, null, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, 3, 1800m, 1, 4, null, false, 450m, "PCI <= 500", 3, 1, 1, 18, 1, null, null, null, null, null },
                    { 4, "FASAPP-2026-0004", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, 64, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 4, 3000m, 1, 4, null, false, 750m, "PCI <= 1000", 64, 1, 2, 18, 1, null, null, new DateTime(2027, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 5, "FASAPP-2026-0005", null, null, null, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, 5, 1800m, 1, 4, null, false, 450m, "PCI <= 500", 5, 1, 1, 18, 1, null, null, null, null, null },
                    { 6, "FASAPP-2026-0006", new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, 66, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 6, 3000m, 1, 4, null, false, 750m, "PCI <= 1000", 66, 1, 2, 18, 1, null, null, new DateTime(2027, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 7, "FASAPP-2026-0007", null, null, null, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, 7, 1800m, 1, 4, null, false, 450m, "PCI <= 500", 7, 1, 1, 18, 1, null, null, null, null, null },
                    { 8, "FASAPP-2026-0008", new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, 68, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 8, 3000m, 1, 4, null, false, 750m, "PCI <= 1000", 68, 1, 2, 18, 1, null, null, new DateTime(2027, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 9, "FASAPP-2026-0009", null, null, null, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, 9, 1800m, 1, 4, null, false, 450m, "PCI <= 500", 9, 1, 1, 18, 1, null, null, null, null, null },
                    { 10, "FASAPP-2026-0010", new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, 70, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 10, 3000m, 1, 4, null, false, 750m, "PCI <= 1000", 70, 1, 2, 18, 1, null, null, new DateTime(2027, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 11, "FASAPP-2026-0011", new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, 11, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 11, 1800m, 1, 4, null, false, 450m, "PCI <= 500", 11, 1, 6, 18, 1, null, null, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 12, "FASAPP-2026-0012", new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, 72, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 12, 3000m, 1, 4, null, false, 750m, "PCI <= 1000", 72, 1, 6, 18, 1, null, null, new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 13, "FASAPP-2026-0013", new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, 13, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 13, 1800m, 1, 4, null, false, 450m, "PCI <= 500", 13, 1, 6, 18, 1, null, null, new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 14, "FASAPP-2026-0014", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, 74, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 14, 3000m, 1, 4, null, false, 750m, "PCI <= 1000", 74, 1, 6, 18, 1, null, null, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 15, "FASAPP-2026-0015", new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, 15, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, 15, 1800m, 1, 4, null, false, 450m, "PCI <= 500", 15, 1, 6, 18, 1, null, null, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 16, "FASAPP-2026-0016", null, null, null, new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Income documents are incomplete.", 16, 3000m, 1, 4, "Admin note: Missing page 2 of income statement.", false, 750m, "PCI <= 1000", 76, 1, 3, 18, 1, null, null, null, null, null },
                    { 17, "FASAPP-2026-0017", null, null, null, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Income documents are incomplete.", 17, 1800m, 1, 4, "Admin note: Missing page 2 of income statement.", false, 450m, "PCI <= 500", 17, 1, 3, 18, 1, null, null, null, null, null },
                    { 18, "FASAPP-2026-0018", null, null, null, new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Income documents are incomplete.", 18, 3000m, 1, 4, "Admin note: Missing page 2 of income statement.", false, 750m, "PCI <= 1000", 78, 1, 3, 18, 1, null, null, null, null, null },
                    { 19, "FASAPP-2026-0019", null, null, null, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Income documents are incomplete.", 19, 1800m, 1, 4, "Admin note: Missing page 2 of income statement.", false, 450m, "PCI <= 500", 19, 1, 3, 18, 1, null, null, null, null, null },
                    { 20, "FASAPP-2026-0020", null, null, null, new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Income documents are incomplete.", 20, 3000m, 1, 4, "Admin note: Missing page 2 of income statement.", false, 750m, "PCI <= 1000", 80, 1, 3, 18, 1, null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasSchemeCondition",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "Field", "GroupId", "IsDeleted", "Operator", "UpdatedAt", "UpdatedBy", "ValueNumber", "ValueNumberTo", "ValueText" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 1, false, 4, null, null, 5000m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 2, false, 4, null, null, 5000m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 3, false, 4, null, null, 5000m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 4, false, 4, null, null, 5000m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 5, false, 4, null, null, 5000m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 6, false, 4, null, null, 5000m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 7, false, 4, null, null, 5000m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 8, false, 4, null, null, 5000m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 9, false, 4, null, null, 5000m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 10, false, 4, null, null, 5000m, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 11, false, 4, null, null, 5000m, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 12, false, 4, null, null, 5000m, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 13, false, 4, null, null, 5000m, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 14, false, 4, null, null, 5000m, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 15, false, 4, null, null, 5000m, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 16, false, 4, null, null, 5000m, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 17, false, 4, null, null, 5000m, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 18, false, 4, null, null, 5000m, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 19, false, 4, null, null, 5000m, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 20, false, 4, null, null, 5000m, null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 21, false, 4, null, null, 5000m, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 22, false, 4, null, null, 5000m, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 23, false, 4, null, null, 5000m, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 24, false, 4, null, null, 5000m, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 25, false, 4, null, null, 5000m, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 26, false, 4, null, null, 5000m, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 27, false, 4, null, null, 5000m, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 28, false, 4, null, null, 5000m, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 29, false, 4, null, null, 5000m, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 30, false, 4, null, null, 5000m, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 31, false, 4, null, null, 5000m, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 32, false, 4, null, null, 5000m, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 33, false, 4, null, null, 5000m, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 34, false, 4, null, null, 5000m, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 35, false, 4, null, null, 5000m, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 36, false, 4, null, null, 5000m, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 37, false, 4, null, null, 5000m, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 38, false, 4, null, null, 5000m, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 39, false, 4, null, null, 5000m, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 40, false, 4, null, null, 5000m, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 41, false, 4, null, null, 5000m, null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 42, false, 4, null, null, 5000m, null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 43, false, 4, null, null, 5000m, null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 44, false, 4, null, null, 5000m, null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 45, false, 4, null, null, 5000m, null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 46, false, 4, null, null, 5000m, null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 47, false, 4, null, null, 5000m, null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 48, false, 4, null, null, 5000m, null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 49, false, 4, null, null, 5000m, null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 50, false, 4, null, null, 5000m, null, null },
                    { 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 51, false, 4, null, null, 5000m, null, null },
                    { 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 52, false, 4, null, null, 5000m, null, null },
                    { 53, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 53, false, 4, null, null, 5000m, null, null },
                    { 54, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 54, false, 4, null, null, 5000m, null, null },
                    { 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 55, false, 4, null, null, 5000m, null, null },
                    { 56, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 56, false, 4, null, null, 5000m, null, null },
                    { 57, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 57, false, 4, null, null, 5000m, null, null },
                    { 58, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 58, false, 4, null, null, 5000m, null, null },
                    { 59, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 59, false, 4, null, null, 5000m, null, null },
                    { 60, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 6, 60, false, 4, null, null, 5000m, null, null }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditTransactionId", "ExternalReference", "IsDeleted", "PaidAt", "PaymentMethod", "Status", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 3, "EDU-2026-5BHC84W", "Marcus Lim", "S0000003E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, null, false, new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 177.67m, null, null },
                    { 9, "EDU-2026-3WTXQL2", "Lucas Nguyen", "S0000009D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, null, false, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 210.37m, null, null },
                    { 15, "EDU-2026-1ZHBQ2S", "Irfan Hassan", "S0000015I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, null, false, new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 243.07m, null, null },
                    { 21, "EDU-2026-7M9PDGH", "Elena Eng", "S0000021C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, null, false, new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 275.77m, null, null },
                    { 27, "EDU-2026-QJZM8YL", "Keira Kwek", "S0000027B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, null, false, new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 308.47m, null, null },
                    { 33, "EDU-2026-LBUTZEW", "Qistina Quek", "S0000033G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 33, null, false, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 341.17m, null, null },
                    { 39, "EDU-2026-NQZL2LT", "Wen Jie Wong", "S0000039F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 39, null, false, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 373.87m, null, null },
                    { 45, "EDU-2026-PMQT1K5", "Celeste Chew", "S0000045J", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 45, null, false, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 406.57m, null, null },
                    { 51, "EDU-2026-M7QVS7X", "Sterling Quach", "S0000001I", new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 51, null, false, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 185m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupExecutionTarget",
                columns: new[] { "Id", "AccountNumber", "Amount", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "EducationCreditTransactionId", "FailureReason", "IsDeleted", "MatchedConditionsSnapshot", "Status", "TopupExecutionId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 3, "EDU-2026-5BHC84W", 105m, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 3, 203, null, false, "Matched household income condition", 3, 3, null, null },
                    { 7, "EDU-2026-RHEGIE7", 145m, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, 7, 207, null, false, "Matched household income condition", 3, 7, null, null }
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
                    { 1, null, null, false, null, null, null, null, null, "CRS-2026-SAGH611", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 130m, "Academic Writing Cohort 01", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 166.77m, 13.77m, false, 23m, 166.77m, 0m, null, 166.77m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 2, null, null, false, null, null, null, null, null, "CRS-2026-A9CFJRZ", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), 135m, "Business Numeracy Cohort 02", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 172.22m, 14.22m, false, 23m, 172.22m, 0m, null, 172.22m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 3, null, null, false, null, null, null, null, null, "CRS-2026-1QOTM9Q", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 140m, "Digital Literacy Cohort 03", new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 177.67m, 14.67m, false, 23m, 177.67m, 177.67m, null, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 4, 4, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-EJ1A67A", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 145m, "Career Readiness Cohort 04", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, 183.12m, 15.12m, false, 23m, 153.12m, 0m, 6, 153.12m, "Northview Secondary School", 1, 30m, 0.09m, null, null },
                    { 5, null, null, false, null, null, null, null, null, "CRS-2026-HPS3B9I", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 150m, "Applied Science Cohort 05", new DateTime(2026, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, 188.57m, 15.57m, false, 23m, 188.57m, 0m, 3, 188.57m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 6, null, null, false, null, null, null, null, null, "CRS-2026-UPUZSA3", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), 155m, "Financial Literacy Cohort 06", new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 194.02m, 16.02m, false, 23m, 194.02m, 194.02m, null, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 7, null, null, false, null, null, null, null, null, "CRS-2026-SMDS48L", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), 160m, "Project Collaboration Cohort 07", new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, 199.47m, 16.47m, false, 23m, 199.47m, 0m, 6, 199.47m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 8, 8, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-DTVK2L0", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 165m, "Data Skills Cohort 08", new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 204.92m, 16.92m, false, 23m, 174.92m, 0m, null, 174.92m, "Northview Secondary School", 3, 30m, 0.09m, null, null },
                    { 9, null, null, false, null, null, null, null, null, "CRS-2026-5HA3NN0", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), 170m, "Workplace Communication Cohort 09", new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, 210.37m, 17.37m, false, 23m, 210.37m, 210.37m, null, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 10, null, null, false, null, null, null, null, null, "CRS-2026-PG7MVO8", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Utc), 175m, "Software Foundations Cohort 10", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 215.82m, 17.82m, false, 23m, 215.82m, 0m, 3, 215.82m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 11, null, null, false, null, null, null, null, null, "CRS-2026-7HGGL06", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 180m, "Academic Writing Cohort 11", new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, 221.27m, 18.27m, false, 23m, 221.27m, 0m, 9, 221.27m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 12, 2, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-8QVCFIJ", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 185m, "Business Numeracy Cohort 12", new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 226.72m, 18.72m, false, 23m, 196.72m, 196.72m, null, 0.00m, "Northview Secondary School", 2, 30m, 0.09m, null, null },
                    { 13, null, null, false, null, null, null, null, null, "CRS-2026-LTXWODC", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), 190m, "Digital Literacy Cohort 13", new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, 232.17m, 19.17m, false, 23m, 232.17m, 0m, 12, 232.17m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 14, null, null, false, null, null, null, null, null, "CRS-2026-9O0PVAP", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 195m, "Career Readiness Cohort 14", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, 237.62m, 19.62m, false, 23m, 237.62m, 0m, 6, 237.62m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 15, null, null, false, null, null, null, null, null, "CRS-2026-WEJ9TUS", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), 200m, "Applied Science Cohort 15", new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, 243.07m, 20.07m, false, 23m, 243.07m, 243.07m, 3, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 16, 6, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-DW55O4J", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 205m, "Financial Literacy Cohort 16", new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, 248.52m, 20.52m, false, 23m, 218.52m, 0m, null, 218.52m, "Northview Secondary School", 1, 30m, 0.09m, null, null },
                    { 17, null, null, false, null, null, null, null, null, "CRS-2026-BEOMA7Q", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 210m, "Project Collaboration Cohort 17", new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, 253.97m, 20.97m, false, 23m, 253.97m, 0m, null, 253.97m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 18, null, null, false, null, null, null, null, null, "CRS-2026-E5YXUWF", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), 215m, "Data Skills Cohort 18", new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, 259.42m, 21.42m, false, 23m, 259.42m, 259.42m, null, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 19, null, null, false, null, null, null, null, null, "CRS-2026-5TQ32YK", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), 220m, "Workplace Communication Cohort 19", new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, 264.87m, 21.87m, false, 23m, 264.87m, 0m, null, 264.87m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 20, 10, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-QHA65C0", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 225m, "Software Foundations Cohort 20", new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, 270.32m, 22.32m, false, 23m, 240.32m, 0m, 3, 240.32m, "Northview Secondary School", 3, 30m, 0.09m, null, null },
                    { 21, null, null, false, null, null, null, null, null, "CRS-2026-IF4LTDN", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), 230m, "Academic Writing Cohort 21", new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, 275.77m, 22.77m, false, 23m, 275.77m, 275.77m, 6, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 22, null, null, false, null, null, null, null, null, "CRS-2026-EOWJ7RI", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), 235m, "Business Numeracy Cohort 22", new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, 281.22m, 23.22m, false, 23m, 281.22m, 0m, 9, 281.22m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 23, null, null, false, null, null, null, null, null, "CRS-2026-JTOV8NE", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 24, 0, 0, 0, 0, DateTimeKind.Utc), 240m, "Digital Literacy Cohort 23", new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 23, 286.67m, 23.67m, false, 23m, 286.67m, 0m, null, 286.67m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 24, 4, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-D6C5MU1", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Utc), 245m, "Career Readiness Cohort 24", new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 24, 292.12m, 24.12m, false, 23m, 262.12m, 262.12m, null, 0.00m, "Northview Secondary School", 2, 30m, 0.09m, null, null },
                    { 25, null, null, false, null, null, null, null, null, "CRS-2026-CWWS3U3", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 250m, "Applied Science Cohort 25", new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 25, 297.57m, 24.57m, false, 23m, 297.57m, 0m, 3, 297.57m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 26, null, null, false, null, null, null, null, null, "CRS-2026-SPJ7E30", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), 255m, "Financial Literacy Cohort 26", new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 26, 303.02m, 25.02m, false, 23m, 303.02m, 0m, 12, 303.02m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 27, null, null, false, null, null, null, null, null, "CRS-2026-HHV04Y9", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), 260m, "Project Collaboration Cohort 27", new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, 308.47m, 25.47m, false, 23m, 308.47m, 308.47m, null, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 28, 8, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-LUJ9RT4", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 265m, "Data Skills Cohort 28", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 28, 313.92m, 25.92m, false, 23m, 283.92m, 0m, 6, 283.92m, "Northview Secondary School", 1, 30m, 0.09m, null, null },
                    { 29, null, null, false, null, null, null, null, null, "CRS-2026-MATA5OE", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 270m, "Workplace Communication Cohort 29", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 29, 319.37m, 26.37m, false, 23m, 319.37m, 0m, null, 319.37m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 30, null, null, false, null, null, null, null, null, "CRS-2026-TBP3E3W", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), 275m, "Software Foundations Cohort 30", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 30, 324.82m, 26.82m, false, 23m, 324.82m, 324.82m, 3, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 31, null, null, false, null, null, null, null, null, "CRS-2026-NMXIH0W", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 280m, "Academic Writing Cohort 31", new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 31, 330.27m, 27.27m, false, 23m, 330.27m, 0m, null, 330.27m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 32, 2, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-M2C29EQ", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 285m, "Business Numeracy Cohort 32", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 32, 335.72m, 27.72m, false, 23m, 305.72m, 0m, null, 305.72m, "Northview Secondary School", 3, 30m, 0.09m, null, null },
                    { 33, null, null, false, null, null, null, null, null, "CRS-2026-YXZ452K", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 290m, "Digital Literacy Cohort 33", new DateTime(2026, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 33, 341.17m, 28.17m, false, 23m, 341.17m, 341.17m, 9, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 34, null, null, false, null, null, null, null, null, "CRS-2026-OJZT05F", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), 295m, "Career Readiness Cohort 34", new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 34, 346.62m, 28.62m, false, 23m, 346.62m, 0m, null, 346.62m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 35, null, null, false, null, null, null, null, null, "CRS-2026-FBV0QY7", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), 300m, "Applied Science Cohort 35", new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 35, 352.07m, 29.07m, false, 23m, 352.07m, 0m, 3, 352.07m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 36, 6, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-R46A00W", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 305m, "Financial Literacy Cohort 36", new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 36, 357.52m, 29.52m, false, 23m, 327.52m, 327.52m, null, 0.00m, "Northview Secondary School", 2, 30m, 0.09m, null, null },
                    { 37, null, null, false, null, null, null, null, null, "CRS-2026-JMAO1QA", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), 310m, "Project Collaboration Cohort 37", new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 37, 362.97m, 29.97m, false, 23m, 362.97m, 0m, null, 362.97m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 38, null, null, false, null, null, null, null, null, "CRS-2026-VA3LGE0", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Utc), 315m, "Data Skills Cohort 38", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 38, 368.42m, 30.42m, false, 23m, 368.42m, 0m, null, 368.42m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 39, null, null, false, null, null, null, null, null, "CRS-2026-C2L9WG4", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 320m, "Workplace Communication Cohort 39", new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 39, 373.87m, 30.87m, false, 23m, 373.87m, 373.87m, 12, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 40, 10, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-L6UUQYK", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), 325m, "Software Foundations Cohort 40", new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 40, 379.32m, 31.32m, false, 23m, 349.32m, 0m, 3, 349.32m, "Northview Secondary School", 1, 30m, 0.09m, null, null },
                    { 41, null, null, false, null, null, null, null, null, "CRS-2026-79UQVDL", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), 330m, "Academic Writing Cohort 41", new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 41, 384.77m, 31.77m, false, 23m, 384.77m, 0m, null, 384.77m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 42, null, null, false, null, null, null, null, null, "CRS-2026-SDLMCO9", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 335m, "Business Numeracy Cohort 42", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 42, 390.22m, 32.22m, false, 23m, 390.22m, 390.22m, 6, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 43, null, null, false, null, null, null, null, null, "CRS-2026-LUEVJLD", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), 340m, "Digital Literacy Cohort 43", new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 43, 395.67m, 32.67m, false, 23m, 395.67m, 0m, null, 395.67m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 44, 4, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-GO9AJL4", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 345m, "Career Readiness Cohort 44", new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 44, 401.12m, 33.12m, false, 23m, 371.12m, 0m, 9, 371.12m, "Northview Secondary School", 3, 30m, 0.09m, null, null },
                    { 45, null, null, false, null, null, null, null, null, "CRS-2026-B29KV62", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 350m, "Applied Science Cohort 45", new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 45, 406.57m, 33.57m, false, 23m, 406.57m, 406.57m, 3, 0.00m, "Northview Secondary School", 2, 0m, 0.09m, null, null },
                    { 46, null, null, false, null, null, null, null, null, "CRS-2026-2ME55AE", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), 355m, "Financial Literacy Cohort 46", new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 46, 412.02m, 34.02m, false, 23m, 412.02m, 0m, null, 412.02m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 47, null, null, false, null, null, null, null, null, "CRS-2026-X7JQUEV", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), 360m, "Project Collaboration Cohort 47", new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 47, 417.47m, 34.47m, false, 23m, 417.47m, 0m, null, 417.47m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 48, 8, null, false, null, "Low Income Course Fee Support", 2, 30m, "Tier 1", "CRS-2026-Y8VG30C", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), 365m, "Data Skills Cohort 48", new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 48, 422.92m, 34.92m, false, 23m, 392.92m, 392.92m, null, 0.00m, "Northview Secondary School", 2, 30m, 0.09m, null, null },
                    { 49, null, null, false, null, null, null, null, null, "CRS-2026-8A1B6PI", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), 370m, "Workplace Communication Cohort 49", new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 49, 428.37m, 35.37m, false, 23m, 428.37m, 0m, 6, 428.37m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 50, null, null, false, null, null, null, null, null, "CRS-2026-2H98HR4", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), 375m, "Software Foundations Cohort 50", new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 50, 433.82m, 35.82m, false, 23m, 433.82m, 0m, 3, 433.82m, "Northview Secondary School", 3, 0m, 0.09m, null, null },
                    { 51, null, null, false, null, null, null, null, null, "CRS-2026-W2MTY4B", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 900m, "Creative Thinking Cohort 51", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 51, 1110m, 91.65m, false, 118.35m, 1110m, 185m, 6, 925m, "Northview Secondary School", 1, 0m, 0.09m, null, null },
                    { 52, null, null, false, null, null, null, null, null, "CRS-2026-6YV5VO9", "Tuition charge generated from enrollment.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 220m, "Applied Science Cohort 52", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 52, 250m, 22.50m, false, 30m, 250m, 0m, null, 250m, "Northview Secondary School", 1, 0m, 0.09m, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasApplicationAdditionalQuestionAnswer",
                columns: new[] { "Id", "AnswerText", "CreatedAt", "CreatedBy", "DeletedAt", "FasApplicationId", "FasSchemeAdditionalQuestionId", "IsDeleted", "IsRequiredSnapshot", "QuestionTextSnapshot", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, true, "Reason", null, null },
                    { 3, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 3, false, true, "Reason", null, null },
                    { 5, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 5, false, true, "Reason", null, null },
                    { 7, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, 7, false, true, "Reason", null, null },
                    { 9, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, 9, false, true, "Reason", null, null },
                    { 11, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 11, false, true, "Reason", null, null },
                    { 13, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, 13, false, true, "Reason", null, null },
                    { 15, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 15, false, true, "Reason", null, null },
                    { 17, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, 17, false, true, "Reason", null, null },
                    { 19, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 19, false, true, "Reason", null, null },
                    { 21, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, 21, false, true, "Reason", null, null },
                    { 23, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, 23, false, true, "Reason", null, null },
                    { 25, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, 25, false, true, "Reason", null, null },
                    { 27, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, 27, false, true, "Reason", null, null },
                    { 29, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, 29, false, true, "Reason", null, null },
                    { 31, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, 31, false, true, "Reason", null, null },
                    { 33, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, 33, false, true, "Reason", null, null },
                    { 35, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, 35, false, true, "Reason", null, null },
                    { 37, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, 37, false, true, "Reason", null, null },
                    { 39, "Need help.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, 39, false, true, "Reason", null, null }
                });

            migrationBuilder.InsertData(
                table: "FasApplicationDocument",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DocumentNameSnapshot", "FasApplicationId", "FasSchemeRequiredDocumentId", "FileKey", "FileName", "IsDeleted", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 1, 1, "fas/applications/1/document.pdf", "fas-application-01.pdf", false, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 2, 2, "fas/applications/2/document.pdf", "fas-application-02.pdf", false, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 3, 3, "fas/applications/3/document.pdf", "fas-application-03.pdf", false, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 4, 4, "fas/applications/4/document.pdf", "fas-application-04.pdf", false, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 5, 5, "fas/applications/5/document.pdf", "fas-application-05.pdf", false, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 6, 6, "fas/applications/6/document.pdf", "fas-application-06.pdf", false, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 7, 7, "fas/applications/7/document.pdf", "fas-application-07.pdf", false, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 8, 8, "fas/applications/8/document.pdf", "fas-application-08.pdf", false, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 9, 9, "fas/applications/9/document.pdf", "fas-application-09.pdf", false, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 10, 10, "fas/applications/10/document.pdf", "fas-application-10.pdf", false, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 11, 11, "fas/applications/11/document.pdf", "fas-application-11.pdf", false, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 12, 12, "fas/applications/12/document.pdf", "fas-application-12.pdf", false, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 13, 13, "fas/applications/13/document.pdf", "fas-application-13.pdf", false, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 14, 14, "fas/applications/14/document.pdf", "fas-application-14.pdf", false, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 15, 15, "fas/applications/15/document.pdf", "fas-application-15.pdf", false, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 16, 16, "fas/applications/16/document.pdf", "fas-application-16.pdf", false, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 17, 17, "fas/applications/17/document.pdf", "fas-application-17.pdf", false, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 18, 18, "fas/applications/18/document.pdf", "fas-application-18.pdf", false, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 19, 19, "fas/applications/19/document.pdf", "fas-application-19.pdf", false, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Income Statement", 20, 20, "fas/applications/20/document.pdf", "fas-application-20.pdf", false, null, null }
                });

            migrationBuilder.InsertData(
                table: "FasTierOverrideHistory",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "FasApplicationId", "IsDeleted", "ModifiedAt", "ModifiedByUserId", "NewTierId", "OldTierId", "Reason", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 2, "Seed tier review trail.", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, 6, 6, "Seed tier review trail.", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 10, 10, "Seed tier review trail.", null, null }
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
                    { 1, 166.77m, null, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 2, 172.22m, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 3, 177.67m, null, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 4, 25.52m, null, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 5, 62.86m, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 6, 194.02m, null, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 7, 33.24m, null, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 8, 174.92m, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Utc), 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 9, 210.37m, null, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 10, 71.94m, null, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 11, 24.59m, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 12, 196.72m, null, 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 13, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 14, 39.60m, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 15, 81.02m, null, 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 16, 218.52m, null, 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 17, 253.97m, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 18, 259.42m, null, 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 19, 264.87m, null, 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 20, 80.11m, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 21, 45.96m, null, 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 22, 31.25m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 23, 286.67m, new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 24, 262.12m, null, 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 25, 99.19m, null, 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 26, 25.25m, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 27, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 27, 308.47m, null, 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 28, 47.32m, null, 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 29, 319.37m, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Utc), 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 30, 108.27m, null, 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 31, 330.27m, null, 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 32, 305.72m, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 33, 37.91m, null, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 34, 346.62m, null, 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 35, 117.36m, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 36, 327.52m, null, 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 37, 362.97m, null, 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 38, 368.42m, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 39, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 40, 116.44m, null, 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 41, 384.77m, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 42, 65.04m, null, 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 43, 395.67m, null, 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 44, 41.24m, new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 45, 135.52m, null, 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 46, 412.02m, null, 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 47, 417.47m, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 48, 392.92m, null, 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 49, 71.40m, null, 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 50, 144.61m, new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 3, null, null },
                    { 51, 185m, null, 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2, null, null },
                    { 52, 185m, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 3, null, null },
                    { 53, 185m, null, 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 54, 185m, null, 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 55, 185m, null, 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 56, 185m, null, 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 57, 250m, null, 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null },
                    { 1050, 25.52m, null, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1051, 25.52m, null, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1052, 25.52m, null, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1053, 25.52m, null, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1054, 25.52m, null, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1062, 62.86m, null, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1063, 62.85m, null, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1086, 33.24m, null, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1087, 33.24m, null, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1088, 33.24m, null, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1089, 33.24m, null, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1090, 33.27m, null, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1122, 71.94m, null, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1123, 71.94m, null, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1134, 24.59m, null, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1135, 24.59m, null, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1136, 24.59m, null, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1137, 24.59m, null, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 12, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1138, 24.59m, null, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1139, 24.59m, null, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), 7, false, 1, null, null },
                    { 1140, 24.59m, null, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 8, false, 1, null, null },
                    { 1141, 24.55m, null, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, 1, null, null },
                    { 1158, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1159, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1160, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1161, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1162, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1163, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), 7, false, 1, null, null },
                    { 1164, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 8, false, 1, null, null },
                    { 1165, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, 1, null, null },
                    { 1166, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 10, false, 1, null, null },
                    { 1167, 19.35m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), 11, false, 1, null, null },
                    { 1168, 19.32m, null, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), 12, false, 1, null, null },
                    { 1170, 39.60m, null, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1171, 39.60m, null, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1172, 39.60m, null, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1173, 39.60m, null, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1174, 39.62m, null, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1182, 81.02m, null, 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2, null, null },
                    { 1183, 81.03m, null, 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 2, null, null },
                    { 1242, 80.11m, null, 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1243, 80.10m, null, 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1254, 45.96m, null, 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2, null, null },
                    { 1255, 45.96m, null, 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 2, null, null },
                    { 1256, 45.96m, null, 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 2, null, null },
                    { 1257, 45.96m, null, 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 2, null, null },
                    { 1258, 45.97m, null, 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 2, null, null },
                    { 1266, 31.25m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1267, 31.25m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1268, 31.25m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1269, 31.25m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 23, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1270, 31.25m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1271, 31.25m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 7, false, 1, null, null },
                    { 1272, 31.25m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 8, false, 1, null, null },
                    { 1273, 31.22m, null, 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, 1, null, null },
                    { 1302, 99.19m, null, 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1303, 99.19m, null, 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1314, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1315, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1316, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1317, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 27, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1318, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1319, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 7, false, 1, null, null },
                    { 1320, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 8, false, 1, null, null },
                    { 1321, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, 1, null, null },
                    { 1322, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), 10, false, 1, null, null },
                    { 1323, 25.25m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), 11, false, 1, null, null },
                    { 1324, 25.27m, null, 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), 12, false, 1, null, null },
                    { 1338, 47.32m, null, 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1339, 47.32m, null, 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1340, 47.32m, null, 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1341, 47.32m, null, 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1342, 47.32m, null, 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1362, 108.27m, null, 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2, null, null },
                    { 1363, 108.28m, null, 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 2, null, null },
                    { 1398, 37.91m, null, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2, null, null },
                    { 1399, 37.91m, null, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 2, null, null },
                    { 1400, 37.91m, null, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 2, null, null },
                    { 1401, 37.91m, null, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 2, null, null },
                    { 1402, 37.91m, null, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 2, null, null },
                    { 1403, 37.91m, null, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc), 7, false, 2, null, null },
                    { 1404, 37.91m, null, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 8, false, 2, null, null },
                    { 1405, 37.89m, null, 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, 2, null, null },
                    { 1422, 117.36m, null, 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1423, 117.35m, null, 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1470, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2, null, null },
                    { 1471, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 2, null, null },
                    { 1472, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 2, null, null },
                    { 1473, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 12, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 2, null, null },
                    { 1474, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 2, null, null },
                    { 1475, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), 7, false, 2, null, null },
                    { 1476, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 8, false, 2, null, null },
                    { 1477, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, 2, null, null },
                    { 1478, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), 10, false, 2, null, null },
                    { 1479, 31.16m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), 11, false, 2, null, null },
                    { 1480, 31.11m, null, 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), 12, false, 2, null, null },
                    { 1482, 116.44m, null, 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1483, 116.44m, null, 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1506, 65.04m, null, 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2, null, null },
                    { 1507, 65.04m, null, 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 2, null, null },
                    { 1508, 65.04m, null, 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 2, null, null },
                    { 1509, 65.04m, null, 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 2, null, null },
                    { 1510, 65.02m, null, 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 2, null, null },
                    { 1530, 41.24m, null, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1531, 41.24m, null, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1532, 41.24m, null, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1533, 41.24m, null, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1534, 41.24m, null, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1535, 41.24m, null, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 7, false, 1, null, null },
                    { 1536, 41.24m, null, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 8, false, 1, null, null },
                    { 1537, 41.20m, null, 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2027, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, 1, null, null },
                    { 1542, 135.52m, null, 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2, null, null },
                    { 1543, 135.53m, null, 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 2, null, null },
                    { 1590, 71.40m, null, 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1591, 71.40m, null, 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null },
                    { 1592, 71.40m, null, 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 1, null, null },
                    { 1593, 71.40m, null, 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 1, null, null },
                    { 1594, 71.37m, null, 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 1, null, null },
                    { 1602, 144.61m, null, 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 1, null, null },
                    { 1603, 144.60m, null, 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "PaymentAllocation",
                columns: new[] { "Id", "Amount", "ChargeGrossAmountSnapshot", "ChargeId", "ChargeInstallmentId", "ChargeNetAmountSnapshot", "ChargeRemainingAmountSnapshot", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "PaymentId", "SchoolNameSnapshot", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 177.67m, 177.67m, 3, 3, 177.67m, 177.67m, "Digital Literacy Cohort 03", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 3, "Northview Secondary School", null, null },
                    { 2, 194.02m, 194.02m, 6, 6, 194.02m, 194.02m, "Financial Literacy Cohort 06", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 6, "Northview Secondary School", null, null },
                    { 3, 210.37m, 210.37m, 9, 9, 210.37m, 210.37m, "Workplace Communication Cohort 09", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 9, "Northview Secondary School", null, null },
                    { 4, 196.72m, 226.72m, 12, 12, 196.72m, 196.72m, "Business Numeracy Cohort 12", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 12, "Northview Secondary School", null, null },
                    { 5, 243.07m, 243.07m, 15, 15, 243.07m, 243.07m, "Applied Science Cohort 15", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 15, "Northview Secondary School", null, null },
                    { 6, 259.42m, 259.42m, 18, 18, 259.42m, 259.42m, "Data Skills Cohort 18", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 18, "Northview Secondary School", null, null },
                    { 7, 275.77m, 275.77m, 21, 21, 275.77m, 275.77m, "Academic Writing Cohort 21", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 21, "Northview Secondary School", null, null },
                    { 8, 262.12m, 292.12m, 24, 24, 262.12m, 262.12m, "Career Readiness Cohort 24", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 24, "Northview Secondary School", null, null },
                    { 9, 308.47m, 308.47m, 27, 27, 308.47m, 308.47m, "Project Collaboration Cohort 27", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 27, "Northview Secondary School", null, null },
                    { 10, 324.82m, 324.82m, 30, 30, 324.82m, 324.82m, "Software Foundations Cohort 30", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 30, "Northview Secondary School", null, null },
                    { 11, 341.17m, 341.17m, 33, 33, 341.17m, 341.17m, "Digital Literacy Cohort 33", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 33, "Northview Secondary School", null, null },
                    { 12, 327.52m, 357.52m, 36, 36, 327.52m, 327.52m, "Financial Literacy Cohort 36", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 36, "Northview Secondary School", null, null },
                    { 13, 373.87m, 373.87m, 39, 39, 373.87m, 373.87m, "Workplace Communication Cohort 39", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 39, "Northview Secondary School", null, null },
                    { 14, 390.22m, 390.22m, 42, 42, 390.22m, 390.22m, "Business Numeracy Cohort 42", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 42, "Northview Secondary School", null, null },
                    { 15, 406.57m, 406.57m, 45, 45, 406.57m, 406.57m, "Applied Science Cohort 45", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 45, "Northview Secondary School", null, null },
                    { 16, 392.92m, 422.92m, 48, 48, 392.92m, 392.92m, "Data Skills Cohort 48", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 48, "Northview Secondary School", null, null },
                    { 17, 185m, 1110m, 51, 51, 1110m, 1110m, "Creative Thinking Cohort 51", new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 51, "Northview Secondary School", null, null }
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
                name: "IX_FasApplicationAdditionalQuestionAnswer_FasApplicationId",
                table: "FasApplicationAdditionalQuestionAnswer",
                column: "FasApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplicationAdditionalQuestionAnswer_FasSchemeAdditionalQuestionId",
                table: "FasApplicationAdditionalQuestionAnswer",
                column: "FasSchemeAdditionalQuestionId");

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
                name: "IX_FasSchemeAdditionalQuestion_FasSchemeId",
                table: "FasSchemeAdditionalQuestion",
                column: "FasSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeAdditionalQuestion_FasSchemeId_QuestionText",
                table: "FasSchemeAdditionalQuestion",
                columns: new[] { "FasSchemeId", "QuestionText" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"FasSchemeId\" IS NOT NULL AND \"QuestionText\" IS NOT NULL");

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
                name: "IX_ManagementActionLog_Action",
                table: "ManagementActionLog",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_ActorUserId",
                table: "ManagementActionLog",
                column: "ActorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_BatchId",
                table: "ManagementActionLog",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_EntityId",
                table: "ManagementActionLog",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_EntityType",
                table: "ManagementActionLog",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_OccurredAt",
                table: "ManagementActionLog",
                column: "OccurredAt");

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
                name: "IX_PaymentAllocation_PaymentId_ChargeId_ChargeInstallmentId",
                table: "PaymentAllocation",
                columns: new[] { "PaymentId", "ChargeId", "ChargeInstallmentId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PaymentId\" IS NOT NULL AND \"ChargeId\" IS NOT NULL AND \"ChargeInstallmentId\" IS NOT NULL");

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
                name: "ApplicationSetting");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "EducationAccountStatusHistory");

            migrationBuilder.DropTable(
                name: "EducationAccountSweepTargets");

            migrationBuilder.DropTable(
                name: "FasApplicationAdditionalQuestionAnswer");

            migrationBuilder.DropTable(
                name: "FasApplicationDocument");

            migrationBuilder.DropTable(
                name: "FasSchemeCondition");

            migrationBuilder.DropTable(
                name: "FasSchemeCourse");

            migrationBuilder.DropTable(
                name: "FasTierOverrideHistory");

            migrationBuilder.DropTable(
                name: "ManagementActionLog");

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
                name: "FasSchemeAdditionalQuestion");

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
