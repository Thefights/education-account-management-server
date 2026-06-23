using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueRunMonth : Migration
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
                name: "TopupRule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    MatchMode = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TopupRule", x => x.Id);
                    table.CheckConstraint("CK_TopupRule_Amount_By_MatchMode", "([MatchMode] = 1 AND [TopupAmount] > 0) OR ([MatchMode] = 2 AND [TopupAmount] IS NULL)");
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
                    EnrollmentDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.CheckConstraint("CK_Course_Date_Order", "[EnrollmentDueDate] <= [PaymentDueDate] AND [EnrollmentDueDate] <= [StartDate] AND [StartDate] <= [EndDate]");
                    table.ForeignKey(
                        name: "FK_Course_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopupRuleCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Field = table.Column<int>(type: "int", nullable: false),
                    Operator = table.Column<int>(type: "int", nullable: false),
                    ValueText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValueNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConditionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TopupRuleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopupRuleCondition", x => x.Id);
                    table.CheckConstraint("CK_TopupRuleCondition_DisplayOrder_NonNegative", "[DisplayOrder] >= 0");
                    table.CheckConstraint("CK_TopupRuleCondition_Value_By_Field", "([Field] IN (1, 2) AND [ValueNumber] IS NOT NULL AND [ValueText] IS NULL) OR ([Field] = 3 AND [ValueText] IS NOT NULL AND [ValueNumber] IS NULL)");
                    table.ForeignKey(
                        name: "FK_TopupRuleCondition_TopupRule_TopupRuleId",
                        column: x => x.TopupRuleId,
                        principalTable: "TopupRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopupSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopupRuleId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TopupSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopupSchedule_TopupRule_TopupRuleId",
                        column: x => x.TopupRuleId,
                        principalTable: "TopupRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "TopupExecution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    TopupRuleId = table.Column<int>(type: "int", nullable: true),
                    TopupScheduleId = table.Column<int>(type: "int", nullable: true),
                    IdempotencyKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ManualAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ManualReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalTargetCount = table.Column<int>(type: "int", nullable: false),
                    SuccessCount = table.Column<int>(type: "int", nullable: false),
                    FailedCount = table.Column<int>(type: "int", nullable: false),
                    TotalExecutedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RuleNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RuleTypeSnapshot = table.Column<int>(type: "int", nullable: true),
                    MatchModeSnapshot = table.Column<int>(type: "int", nullable: true),
                    TopupAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RuleConditionsSnapshot = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.CheckConstraint("CK_TopupExecution_Source_Fields", "([SourceType] = 1 AND [TopupRuleId] IS NOT NULL AND [TopupScheduleId] IS NULL AND [ManualAmount] IS NULL AND [ManualReason] IS NULL) OR ([SourceType] = 2 AND [TopupRuleId] IS NOT NULL AND [TopupScheduleId] IS NOT NULL AND [ManualAmount] IS NULL AND [ManualReason] IS NULL) OR ([SourceType] = 3 AND [TopupRuleId] IS NULL AND [TopupScheduleId] IS NULL AND [ManualAmount] > 0 AND [ManualReason] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_TopupExecution_TopupRule_TopupRuleId",
                        column: x => x.TopupRuleId,
                        principalTable: "TopupRule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopupExecution_TopupSchedule_TopupScheduleId",
                        column: x => x.TopupScheduleId,
                        principalTable: "TopupSchedule",
                        principalColumn: "Id");
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
                name: "Enrollment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SchoolStudentId = table.Column<int>(type: "int", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollment_SchoolStudent_SchoolStudentId",
                        column: x => x.SchoolStudentId,
                        principalTable: "SchoolStudent",
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
                name: "Charge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrollmentId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CourseFeeAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MiscFeeAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GstAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubsidyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BecameOutstandingAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAutoDeductedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                name: "TopupSystemApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopupRuleId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_TopupSystemApplication_TopupExecutionTarget_TopupExecutionTargetId",
                        column: x => x.TopupExecutionTargetId,
                        principalTable: "TopupExecutionTarget",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopupSystemApplication_TopupRule_TopupRuleId",
                        column: x => x.TopupRuleId,
                        principalTable: "TopupRule",
                        principalColumn: "Id");
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
                    { 17, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 1, 15), null, "unlinked.citizen017@example.com", "Unlinked Test Citizen 017", false, "17 Test Avenue, Singapore", "S0000017E", "+6590000017", "17 Test Avenue, Singapore", "Not Enrolled", null, null },
                    { 18, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 2, 15), null, "unlinked.citizen018@example.com", "Unlinked Test Citizen 018", false, "18 Test Avenue, Singapore", "S0000018C", "+6590000018", "18 Test Avenue, Singapore", "Enrolled", null, null },
                    { 19, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 3, 15), null, "unlinked.citizen019@example.com", "Unlinked Test Citizen 019", false, "19 Test Avenue, Singapore", "S0000019A", "+6590000019", "19 Test Avenue, Singapore", "Not Enrolled", null, null },
                    { 20, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 4, 15), null, "unlinked.citizen020@example.com", "Unlinked Test Citizen 020", false, "20 Test Avenue, Singapore", "S0000020E", "+6590000020", "20 Test Avenue, Singapore", "Enrolled", null, null },
                    { 21, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 5, 15), null, "unlinked.citizen021@example.com", "Unlinked Test Citizen 021", false, "21 Test Avenue, Singapore", "S0000021C", "+6590000021", "21 Test Avenue, Singapore", "Not Enrolled", null, null },
                    { 22, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 15), null, "unlinked.citizen022@example.com", "Unlinked Test Citizen 022", false, "22 Test Avenue, Singapore", "S0000022A", "+6590000022", "22 Test Avenue, Singapore", "Enrolled", null, null },
                    { 23, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 15), null, "unlinked.citizen023@example.com", "Unlinked Test Citizen 023", false, "23 Test Avenue, Singapore", "S0000023Z", "+6590000023", "23 Test Avenue, Singapore", "Not Enrolled", null, null },
                    { 24, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 15), null, "unlinked.citizen024@example.com", "Unlinked Test Citizen 024", false, "24 Test Avenue, Singapore", "S0000024H", "+6590000024", "24 Test Avenue, Singapore", "Enrolled", null, null },
                    { 25, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 15), null, "unlinked.citizen025@example.com", "Unlinked Test Citizen 025", false, "25 Test Avenue, Singapore", "S0000025F", "+6590000025", "25 Test Avenue, Singapore", "Not Enrolled", null, null },
                    { 26, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 15), null, "unlinked.citizen026@example.com", "Unlinked Test Citizen 026", false, "26 Test Avenue, Singapore", "S0000026D", "+6590000026", "26 Test Avenue, Singapore", "Enrolled", null, null }
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
                table: "TopupExecution",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutionCode", "FailedCount", "IdempotencyKey", "IsDeleted", "ManualAmount", "ManualReason", "MatchModeSnapshot", "RuleConditionsSnapshot", "RuleNameSnapshot", "RuleTypeSnapshot", "SourceType", "Status", "SuccessCount", "TopupAmountSnapshot", "TopupRuleId", "TopupScheduleId", "TotalExecutedAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, "TOPUP-SEED-MANUAL-001", 1, "seed-manual-topup-001", false, 100m, "Seeded manual top-up execution.", null, null, null, null, 3, 3, 1, null, null, null, 100m, 2, null, null });

            migrationBuilder.InsertData(
                table: "TopupRule",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "MatchMode", "RuleName", "Status", "TopupAmount", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 001", 2, 70m, 2, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 002", 1, 770m, 2, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 003", 1, 790m, 2, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 004", 1, null, 2, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 005", 1, 730m, 2, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 006", 1, 510m, 2, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 007", 1, 390m, 2, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 008", 2, null, 2, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 009", 1, 140m, 2, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 010", 1, 200m, 2, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 011", 1, 860m, 2, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 012", 1, null, 2, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 013", 1, 530m, 2, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 014", 1, 710m, 2, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 015", 1, 620m, 2, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 016", 1, null, 2, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 017", 1, 400m, 2, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 018", 1, 680m, 2, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 019", 1, 520m, 2, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 020", 1, null, 2, null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 021", 2, 770m, 1, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 022", 1, 480m, 1, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 023", 1, 490m, 1, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 024", 1, null, 1, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 025", 1, 270m, 1, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 026", 1, 630m, 1, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 027", 1, 370m, 1, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 028", 1, null, 1, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 029", 1, 850m, 1, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 030", 1, 610m, 1, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 031", 2, 500m, 1, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 032", 1, null, 1, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 033", 2, 710m, 1, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 034", 1, 930m, 1, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 035", 1, 100m, 1, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 036", 1, null, 1, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 037", 1, 20m, 1, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 038", 1, 20m, 1, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 039", 1, 730m, 1, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 040", 1, null, 1, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 041", 1, 740m, 1, null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 042", 1, 440m, 1, null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 043", 1, 650m, 1, null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 044", 1, null, 1, null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 045", 2, 80m, 1, null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 046", 1, 290m, 1, null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 047", 1, 880m, 1, null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Random Top-up Rule 048", 1, null, 1, null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 049", 2, 600m, 1, null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Random Top-up Rule 050", 2, 770m, 1, null, null }
                });

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
                columns: new[] { "Id", "CourseCode", "CourseFeeAmount", "CourseName", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "EndDate", "EnrollmentDueDate", "GstAmount", "IsDeleted", "MiscFeeAmount", "PaymentDueDate", "SchoolId", "StartDate", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "CRS-2026-A1B2C3D", 100m, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation course in applied mathematics.", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 10m, false, 10m, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 2, "CRS-2026-B2C3D4E", 115m, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Introduction to programming and computing.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), 13m, false, 12m, new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 3, "CRS-2026-C3D4E5F", 130m, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Professional written and verbal communication.", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), 15m, false, 15m, new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 4, "CRS-2026-D4E5F6G", 145m, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Environmental systems and sustainability.", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 18m, false, 17m, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 5, "CRS-2026-E5F6G7H", 160m, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Digital design principles and production.", new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 20m, false, 20m, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 5, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 6, "CRS-2026-F6G7H8J", 175m, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Core hospitality service operations.", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 23m, false, 22m, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null },
                    { 7, "CRS-2026-G7H8J9K", 190m, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Fundamentals of electrical systems.", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 25m, false, 25m, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 7, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 8, "CRS-2026-H8J9K0L", 205m, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Writing techniques across common genres.", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 28m, false, 27m, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 8, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 9, "CRS-2026-J9K0L1M", 220m, "Data Analytics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Data preparation, analysis and reporting.", new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 30m, false, 30m, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 9, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null },
                    { 10, "CRS-2026-K0L1M2N", 235m, "Legacy Office Applications", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Archived office applications programme.", new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 33m, false, 32m, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 10, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null }
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
                    { 10, "EDU-2026-00000000010", 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2000m, false, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null }
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
                table: "TopupExecution",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutionCode", "FailedCount", "IdempotencyKey", "IsDeleted", "ManualAmount", "ManualReason", "MatchModeSnapshot", "RuleConditionsSnapshot", "RuleNameSnapshot", "RuleTypeSnapshot", "SourceType", "Status", "SuccessCount", "TopupAmountSnapshot", "TopupRuleId", "TopupScheduleId", "TotalExecutedAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 2, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "TOPUP-SEED-SYSTEM-001", 0, "seed-system-topup-001", false, null, null, 1, null, "Random Top-up Rule 021", 1, 1, 3, 1, 200m, 21, null, 200m, 1, null, null });

            migrationBuilder.InsertData(
                table: "TopupRuleCondition",
                columns: new[] { "Id", "ConditionAmount", "CreatedAt", "CreatedBy", "DeletedAt", "DisplayOrder", "Field", "IsDeleted", "Operator", "TopupRuleId", "UpdatedAt", "UpdatedBy", "ValueNumber", "ValueText" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 1, null, null, 12m, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 1, null, null, 22m, null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 2, null, null, 700m, null },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 2, null, null, 1500m, null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 3, null, null, 16m, null },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 3, null, null, 19m, null },
                    { 7, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 4, null, null, 200m, null },
                    { 8, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 4, null, null, 600m, null },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 5, null, null, 12m, null },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 5, null, null, 14m, null },
                    { 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 6, null, null, 100m, null },
                    { 12, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 6, null, null, 900m, null },
                    { 13, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 7, null, null, 300m, null },
                    { 14, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 7, null, null, 400m, null },
                    { 15, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 8, null, null, 300m, null },
                    { 16, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 8, null, null, 1000m, null },
                    { 17, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 9, null, null, 600m, null },
                    { 18, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 9, null, null, 1300m, null },
                    { 19, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 10, null, null, 300m, null },
                    { 20, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 10, null, null, 600m, null },
                    { 21, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 11, null, null, 200m, null },
                    { 22, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 11, null, null, 700m, null },
                    { 23, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 12, null, null, 13m, null },
                    { 24, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 12, null, null, 22m, null },
                    { 25, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 13, null, null, 14m, null },
                    { 26, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 13, null, null, 19m, null },
                    { 27, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 14, null, null, 16m, null },
                    { 28, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 14, null, null, 20m, null },
                    { 29, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 15, null, null, 13m, null },
                    { 30, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 15, null, null, 20m, null },
                    { 31, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 16, null, null, 14m, null },
                    { 32, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 16, null, null, 23m, null },
                    { 33, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 17, null, null, 17m, null },
                    { 34, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 17, null, null, 22m, null },
                    { 35, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 18, null, null, 400m, null },
                    { 36, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 18, null, null, 900m, null },
                    { 37, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 19, null, null, 15m, null },
                    { 38, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 19, null, null, 22m, null },
                    { 39, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 20, null, null, 17m, null },
                    { 40, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 20, null, null, 19m, null },
                    { 41, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 21, null, null, 14m, null },
                    { 42, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 21, null, null, 18m, null },
                    { 43, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 22, null, null, 16m, null },
                    { 44, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 22, null, null, 17m, null },
                    { 45, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 23, null, null, 700m, null },
                    { 46, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 23, null, null, 1400m, null },
                    { 47, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 24, null, null, 16m, null },
                    { 48, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 24, null, null, 22m, null },
                    { 49, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 25, null, null, 14m, null },
                    { 50, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 25, null, null, 21m, null },
                    { 51, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 26, null, null, 200m, null },
                    { 52, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 26, null, null, 300m, null },
                    { 53, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 27, null, null, 13m, null },
                    { 54, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 27, null, null, 16m, null },
                    { 55, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 28, null, null, 500m, null },
                    { 56, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 28, null, null, 1000m, null },
                    { 57, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 29, null, null, 0m, null },
                    { 58, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 29, null, null, 700m, null },
                    { 59, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 30, null, null, 14m, null },
                    { 60, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 30, null, null, 22m, null },
                    { 61, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 31, null, null, 17m, null },
                    { 62, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 31, null, null, 21m, null },
                    { 63, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 32, null, null, 14m, null },
                    { 64, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 32, null, null, 17m, null },
                    { 65, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 33, null, null, 13m, null },
                    { 66, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 33, null, null, 19m, null },
                    { 67, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 34, null, null, 400m, null },
                    { 68, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 34, null, null, 1000m, null },
                    { 69, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 35, null, null, 16m, null },
                    { 70, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 35, null, null, 17m, null },
                    { 71, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 36, null, null, 100m, null },
                    { 72, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 36, null, null, 200m, null },
                    { 73, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 37, null, null, 300m, null },
                    { 74, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 37, null, null, 600m, null },
                    { 75, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 38, null, null, 900m, null },
                    { 76, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 38, null, null, 1500m, null },
                    { 77, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 39, null, null, 200m, null },
                    { 78, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 39, null, null, 1000m, null },
                    { 79, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 40, null, null, 400m, null },
                    { 80, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 40, null, null, 1100m, null },
                    { 81, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 41, null, null, 17m, null },
                    { 82, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 41, null, null, 19m, null },
                    { 83, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 42, null, null, 13m, null },
                    { 84, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 42, null, null, 22m, null },
                    { 85, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 43, null, null, 13m, null },
                    { 86, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 43, null, null, 24m, null },
                    { 87, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 44, null, null, 15m, null },
                    { 88, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 44, null, null, 16m, null },
                    { 89, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 45, null, null, 400m, null },
                    { 90, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 45, null, null, 1200m, null },
                    { 91, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 46, null, null, 700m, null },
                    { 92, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 46, null, null, 1400m, null },
                    { 93, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 47, null, null, 16m, null },
                    { 94, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 47, null, null, 24m, null },
                    { 95, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 48, null, null, 100m, null },
                    { 96, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 48, null, null, 200m, null },
                    { 97, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 1, false, 4, 49, null, null, 14m, null },
                    { 98, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 1, false, 6, 49, null, null, 24m, null },
                    { 99, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 2, false, 4, 50, null, null, 300m, null },
                    { 100, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 2, false, 6, 50, null, null, 500m, null }
                });

            migrationBuilder.InsertData(
                table: "TopupSchedule",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExecuteAtDay", "ExecuteAtMonth", "ExecutionTime", "Frequency", "IsDeleted", "NextExecutionAt", "OneTimeExecutionAt", "Status", "TopupRuleId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(1, 46, 0), 1, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 7, new TimeOnly(19, 9, 0), 3, false, new DateTime(2027, 7, 8, 19, 9, 0, 0, DateTimeKind.Unspecified), null, 1, 2, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(9, 12, 0), 1, false, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(17, 11, 0), 1, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, 9, new TimeOnly(8, 6, 0), 3, false, new DateTime(2027, 9, 11, 8, 6, 0, 0, DateTimeKind.Unspecified), null, 1, 5, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, null, new TimeOnly(14, 43, 0), 2, false, new DateTime(2027, 3, 12, 14, 43, 0, 0, DateTimeKind.Unspecified), null, 1, 6, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 10, new TimeOnly(5, 31, 0), 3, false, new DateTime(2027, 10, 8, 5, 31, 0, 0, DateTimeKind.Unspecified), null, 1, 7, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(11, 26, 0), 1, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, 4, new TimeOnly(10, 16, 0), 3, false, new DateTime(2027, 4, 18, 10, 16, 0, 0, DateTimeKind.Unspecified), null, 1, 9, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, null, new TimeOnly(20, 10, 0), 2, false, new DateTime(2027, 8, 19, 20, 10, 0, 0, DateTimeKind.Unspecified), null, 1, 10, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, null, new TimeOnly(11, 1, 0), 2, false, new DateTime(2027, 1, 20, 11, 1, 0, 0, DateTimeKind.Unspecified), null, 1, 11, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 1, new TimeOnly(4, 5, 0), 3, false, new DateTime(2027, 1, 10, 4, 5, 0, 0, DateTimeKind.Unspecified), null, 1, 12, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, 5, new TimeOnly(0, 52, 0), 3, false, new DateTime(2027, 5, 22, 0, 52, 0, 0, DateTimeKind.Unspecified), null, 1, 13, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, 8, new TimeOnly(17, 26, 0), 3, false, new DateTime(2027, 8, 19, 17, 26, 0, 0, DateTimeKind.Unspecified), null, 1, 14, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(1, 4, 0), 1, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 15, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(21, 34, 0), 1, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 16, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, null, new TimeOnly(1, 46, 0), 2, false, null, null, 2, 17, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, new TimeOnly(22, 33, 0), 1, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 18, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, null, new TimeOnly(7, 6, 0), 2, false, new DateTime(2027, 8, 13, 7, 6, 0, 0, DateTimeKind.Unspecified), null, 1, 19, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, null, new TimeOnly(15, 5, 0), 2, false, new DateTime(2027, 12, 4, 15, 5, 0, 0, DateTimeKind.Unspecified), null, 1, 20, null, null }
                });

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
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, 10, 2, null, null }
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
                table: "TopupExecutionTarget",
                columns: new[] { "Id", "AccountNumber", "Amount", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "EducationCreditTransactionId", "FailureReason", "IsDeleted", "MatchedConditionsSnapshot", "Status", "TopupExecutionId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 2, "EDU-2026-00000000002", 100m, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, 2, null, "Seeded failure for manual review.", false, null, 4, 1, null, null });

            migrationBuilder.InsertData(
                table: "Enrollment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenEmailSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CitizenPhoneNumberSnapshot", "CourseDescriptionSnapshot", "CourseId", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "SchoolNameSnapshot", "SchoolStudentId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EDU-2026-00000000001", "citizen001@example.com", "Citizen 001", "S0000001I", "+6590000001", "Foundation course in applied mathematics.", 1, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 1, null, null },
                    { 2, "EDU-2026-00000000002", "citizen002@example.com", "Citizen 002", "S0000002G", "+6590000002", "Introduction to programming and computing.", 2, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 2, null, null },
                    { 3, "EDU-2026-00000000003", "citizen003@example.com", "Citizen 003", "S0000003E", "+6590000003", "Professional written and verbal communication.", 3, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 3, null, null },
                    { 4, "EDU-2026-00000000004", "citizen004@example.com", "Citizen 004", "S0000004C", "+6590000004", "Environmental systems and sustainability.", 4, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 4, null, null },
                    { 5, "EDU-2026-00000000005", "citizen005@example.com", "Citizen 005", "S0000005A", "+6590000005", "Digital design principles and production.", 5, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 5, null, null },
                    { 6, "EDU-2026-00000000006", "citizen006@example.com", "Citizen 006", "S0000006Z", "+6590000006", "Core hospitality service operations.", 6, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 6, null, null },
                    { 7, "EDU-2026-00000000007", "citizen007@example.com", "Citizen 007", "S0000007H", "+6590000007", "Fundamentals of electrical systems.", 7, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 7, null, null },
                    { 8, "EDU-2026-00000000008", "citizen008@example.com", "Citizen 008", "S0000008F", "+6590000008", "Writing techniques across common genres.", 8, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 8, null, null },
                    { 9, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Data preparation, analysis and reporting.", 9, "Data Analytics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Harbourfront School", 9, null, null },
                    { 10, "EDU-2026-00000000010", "citizen010@example.com", "Citizen 010", "S0000010H", "+6590000010", "Archived office applications programme.", 10, "Legacy Office Applications", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 10, null, null },
                    { 11, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Foundation course in applied mathematics.", 1, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Northview Secondary School", 9, null, null },
                    { 12, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Environmental systems and sustainability.", 4, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Southpoint Secondary School", 9, null, null },
                    { 13, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Introduction to programming and computing.", 2, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Eastbridge Secondary School", 9, null, null },
                    { 14, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Professional written and verbal communication.", 3, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Westhaven Secondary School", 9, null, null },
                    { 15, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Digital design principles and production.", 5, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Central Heights School", 9, null, null },
                    { 16, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Core hospitality service operations.", 6, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Riverside Learning Institute", 9, null, null },
                    { 17, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Fundamentals of electrical systems.", 7, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Lakeside Technical School", 9, null, null },
                    { 18, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Writing techniques across common genres.", 8, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Greenfield Academy", 9, null, null },
                    { 19, "EDU-2026-00000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", "Archived office applications programme.", 10, "Legacy Office Applications", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Hillcrest Education Centre", 9, null, null }
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
                columns: new[] { "Id", "BecameOutstandingAt", "CourseFeeAmountSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EnrollmentId", "GrossAmount", "GstAmountSnapshot", "IsDeleted", "LastAutoDeductedAt", "MiscFeeAmountSnapshot", "NetAmount", "PaidAmount", "PaymentDueDate", "RemainingAmount", "Status", "SubsidyAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 120m, 10m, false, null, 10m, 120m, 120m, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0m, 3, 0m, null, null },
                    { 2, null, 115m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 140m, 13m, false, null, 12m, 140m, 70m, new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 70m, 2, 0m, null, null },
                    { 3, null, 130m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 160m, 15m, false, null, 15m, 140m, 140m, new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 0m, 3, 20m, null, null },
                    { 4, null, 145m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, 180m, 18m, false, null, 17m, 180m, 180m, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 0m, 3, 0m, null, null },
                    { 5, null, 160m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, 200m, 20m, false, null, 20m, 180m, 180m, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 0m, 3, 20m, null, null },
                    { 6, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 175m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 220m, 23m, false, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 22m, 220m, 150m, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 70m, 4, 0m, null, null },
                    { 7, null, 190m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, 240m, 25m, false, null, 25m, 200m, 200m, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 0m, 3, 40m, null, null },
                    { 8, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 205m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 260m, 28m, false, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 27m, 260m, 200m, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 60m, 4, 0m, null, null },
                    { 9, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 220m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, 280m, 30m, false, null, 30m, 250m, 0m, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 250m, 4, 30m, null, null },
                    { 10, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 235m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 300m, 33m, false, null, 32m, 300m, 0m, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 300m, 4, 0m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupSystemApplication",
                columns: new[] { "Id", "EducationAccountId", "TopupExecutionTargetId", "TopupRuleId" },
                values: new object[] { 1, 3, 3, 21 });

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
                    { 2, 140m, 160m, 3, 140m, 140m, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Westhaven Secondary School", null, null },
                    { 3, 180m, 180m, 4, 180m, 180m, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 3, "Southpoint Secondary School", null, null },
                    { 4, 180m, 200m, 5, 180m, 180m, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, "Central Heights School", null, null },
                    { 5, 120m, 120m, 1, 120m, 120m, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 5, "Northview Secondary School", null, null },
                    { 6, 100m, 220m, 6, 220m, 220m, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 6, "Riverside Learning Institute", null, null },
                    { 7, 200m, 240m, 7, 200m, 200m, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 7, "Lakeside Technical School", null, null },
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
                name: "IX_Charge_PaymentDueDate",
                table: "Charge",
                column: "PaymentDueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_Status",
                table: "Charge",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_Status_PaymentDueDate",
                table: "Charge",
                columns: new[] { "Status", "PaymentDueDate" });

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
                name: "IX_Course_CourseName",
                table: "Course",
                column: "CourseName");

            migrationBuilder.CreateIndex(
                name: "IX_Course_EndDate",
                table: "Course",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Course_EnrollmentDueDate",
                table: "Course",
                column: "EnrollmentDueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Course_PaymentDueDate",
                table: "Course",
                column: "PaymentDueDate");

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
                name: "IX_TopupExecution_SourceType",
                table: "TopupExecution",
                column: "SourceType");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecution_Status",
                table: "TopupExecution",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecution_TopupRuleId",
                table: "TopupExecution",
                column: "TopupRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupExecution_TopupScheduleId",
                table: "TopupExecution",
                column: "TopupScheduleId");

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
                name: "IX_TopupRule_RuleName",
                table: "TopupRule",
                column: "RuleName",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"RuleName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupRule_Status",
                table: "TopupRule",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TopupRuleCondition_Field",
                table: "TopupRuleCondition",
                column: "Field");

            migrationBuilder.CreateIndex(
                name: "IX_TopupRuleCondition_Operator",
                table: "TopupRuleCondition",
                column: "Operator");

            migrationBuilder.CreateIndex(
                name: "IX_TopupRuleCondition_TopupRuleId",
                table: "TopupRuleCondition",
                column: "TopupRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupSchedule_NextExecutionAt",
                table: "TopupSchedule",
                column: "NextExecutionAt");

            migrationBuilder.CreateIndex(
                name: "IX_TopupSchedule_Status",
                table: "TopupSchedule",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TopupSchedule_TopupRuleId",
                table: "TopupSchedule",
                column: "TopupRuleId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"TopupRuleId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupSystemApplication_EducationAccountId",
                table: "TopupSystemApplication",
                column: "EducationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupSystemApplication_TopupExecutionTargetId",
                table: "TopupSystemApplication",
                column: "TopupExecutionTargetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopupSystemApplication_TopupRuleId",
                table: "TopupSystemApplication",
                column: "TopupRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupSystemApplication_TopupRuleId_EducationAccountId",
                table: "TopupSystemApplication",
                columns: new[] { "TopupRuleId", "EducationAccountId" },
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
                name: "SsoIdentity");

            migrationBuilder.DropTable(
                name: "TopupRuleCondition");

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
                name: "TopupSchedule");

            migrationBuilder.DropTable(
                name: "EducationAccount");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropTable(
                name: "TopupRule");

            migrationBuilder.DropTable(
                name: "Citizen");
        }
    }
}
