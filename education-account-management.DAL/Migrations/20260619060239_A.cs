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
                name: "AdhocTopupBatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalTargetCount = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExecutedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdhocTopupBatch", x => x.Id);
                    table.CheckConstraint("CK_AdhocTopupBatch_TotalAmount_NonNegative", "[TotalAmount] >= 0 AND [TotalTargetCount] >= 0");
                });

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
                name: "AuthAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false),
                    LockedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Citizen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nric = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    SingpassSubjectId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
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
                    TopupAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.CheckConstraint("CK_TopupRule_Amount_NonNegative", "[TopupAmount] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "TopupSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ScheduleType = table.Column<int>(type: "int", nullable: false),
                    OneTimeExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExecuteAtDay = table.Column<int>(type: "int", nullable: true),
                    ExecuteAtMonth = table.Column<int>(type: "int", nullable: true),
                    SpecificExecutionTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastExecutedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextExecutionAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    AuthAccountId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_RefreshToken_AuthAccount_AuthAccountId",
                        column: x => x.AuthAccountId,
                        principalTable: "AuthAccount",
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
                    AuthAccountId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_SsoIdentity_AuthAccount_AuthAccountId",
                        column: x => x.AuthAccountId,
                        principalTable: "AuthAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    AuthAccountId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_User_AuthAccount_AuthAccountId",
                        column: x => x.AuthAccountId,
                        principalTable: "AuthAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CourseFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MiscFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GstAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Course_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopupBatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalTargetCount = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExecutedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_TopupBatch", x => x.Id);
                    table.CheckConstraint("CK_TopupBatch_TotalAmount_NonNegative", "[TotalAmount] >= 0 AND [TotalTargetCount] >= 0");
                    table.ForeignKey(
                        name: "FK_TopupBatch_TopupRule_TopupRuleId",
                        column: x => x.TopupRuleId,
                        principalTable: "TopupRule",
                        principalColumn: "Id");
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
                    table.ForeignKey(
                        name: "FK_TopupRuleCondition_TopupRule_TopupRuleId",
                        column: x => x.TopupRuleId,
                        principalTable: "TopupRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopupScheduleRule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopupScheduleId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TopupScheduleRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopupScheduleRule_TopupRule_TopupRuleId",
                        column: x => x.TopupRuleId,
                        principalTable: "TopupRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopupScheduleRule_TopupSchedule_TopupScheduleId",
                        column: x => x.TopupScheduleId,
                        principalTable: "TopupSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdhocTopupBatchTarget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdhocTopupBatchId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_AdhocTopupBatchTarget", x => x.Id);
                    table.CheckConstraint("CK_AdhocTopupBatchTarget_Amount_NonNegative", "[Amount] >= 0");
                    table.ForeignKey(
                        name: "FK_AdhocTopupBatchTarget_AdhocTopupBatch_AdhocTopupBatchId",
                        column: x => x.AdhocTopupBatchId,
                        principalTable: "AdhocTopupBatch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdhocTopupBatchTarget_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
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
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.CheckConstraint("CK_EducationCreditTransaction_Amounts_NonNegative", "[Amount] >= 0 AND [BalanceBefore] >= 0 AND [BalanceAfter] >= 0");
                    table.CheckConstraint("CK_EducationCreditTransaction_BalanceEquation", "([Direction] = 1 AND [BalanceAfter] = [BalanceBefore] + [Amount]) OR ([Direction] = 2 AND [BalanceAfter] = [BalanceBefore] - [Amount])");
                    table.ForeignKey(
                        name: "FK_EducationCreditTransaction_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
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
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Enrollment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    EducationAccountId = table.Column<int>(type: "int", nullable: false),
                    SchoolNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseDescriptionSnapshot = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CitizenNricSnapshot = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    CitizenFullNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CitizenEmailSnapshot = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    CitizenPhoneNumberSnapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AccountNumberSnapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_Enrollment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollment_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollment_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopupBatchTarget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TopupBatchId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TopupBatchTarget", x => x.Id);
                    table.CheckConstraint("CK_TopupBatchTarget_Amount_NonNegative", "[Amount] >= 0");
                    table.ForeignKey(
                        name: "FK_TopupBatchTarget_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopupBatchTarget_TopupBatch_TopupBatchId",
                        column: x => x.TopupBatchId,
                        principalTable: "TopupBatch",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdhocTopupBatchTargetTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdhocTopupBatchTargetId = table.Column<int>(type: "int", nullable: false),
                    EducationCreditTransactionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdhocTopupBatchTargetTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdhocTopupBatchTargetTransaction_AdhocTopupBatchTarget_AdhocTopupBatchTargetId",
                        column: x => x.AdhocTopupBatchTargetId,
                        principalTable: "AdhocTopupBatchTarget",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdhocTopupBatchTargetTransaction_EducationCreditTransaction_EducationCreditTransactionId",
                        column: x => x.EducationCreditTransactionId,
                        principalTable: "EducationCreditTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    AccountNumberSnapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CitizenNricSnapshot = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    CitizenFullNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalReference = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                    table.CheckConstraint("CK_Payment_TotalAmount_NonNegative", "[TotalAmount] >= 0");
                    table.ForeignKey(
                        name: "FK_Payment_EducationCreditTransaction_EducationCreditTransactionId",
                        column: x => x.EducationCreditTransactionId,
                        principalTable: "EducationCreditTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    table.CheckConstraint("CK_Charge_Amounts_NonNegative", "[GrossAmount] >= 0 AND [SubsidyAmount] >= 0 AND [NetAmount] >= 0 AND [PaidAmount] >= 0 AND [RemainingAmount] >= 0");
                    table.ForeignKey(
                        name: "FK_Charge_Enrollment_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopupBatchTargetTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopupBatchTargetId = table.Column<int>(type: "int", nullable: false),
                    EducationCreditTransactionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopupBatchTargetTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopupBatchTargetTransaction_EducationCreditTransaction_EducationCreditTransactionId",
                        column: x => x.EducationCreditTransactionId,
                        principalTable: "EducationCreditTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopupBatchTargetTransaction_TopupBatchTarget_TopupBatchTargetId",
                        column: x => x.TopupBatchTargetId,
                        principalTable: "TopupBatchTarget",
                        principalColumn: "Id");
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
                    table.CheckConstraint("CK_PaymentAllocation_Amount_NonNegative", "[Amount] >= 0");
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
                table: "AdhocTopupBatch",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutedAt", "IsDeleted", "Reason", "Status", "TotalAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, "Adhoc adjustment reason 001", 1, 55m, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), false, "Adhoc adjustment reason 002", 2, 60m, 1, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, "Adhoc adjustment reason 003", 1, 65m, 1, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, "Adhoc adjustment reason 004", 2, 70m, 1, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, "Adhoc adjustment reason 005", 1, 75m, 1, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), false, "Adhoc adjustment reason 006", 2, 80m, 1, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, "Adhoc adjustment reason 007", 1, 85m, 1, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Adhoc adjustment reason 008", 2, 90m, 1, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, "Adhoc adjustment reason 009", 1, 95m, 1, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), false, "Adhoc adjustment reason 010", 2, 100m, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "AiAssistantSetting",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "IsEnabled", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, true, null, null });

            migrationBuilder.InsertData(
                table: "AuthAccount",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "FailedLoginCount", "IsDeleted", "LastLoginAt", "LockedUntil", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 1, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, new DateTime(2026, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 1, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 1, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, null, null, 1, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, null, null, 1, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, null, null, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Citizen",
                columns: new[] { "Id", "CitizenshipStatus", "CreatedAt", "CreatedBy", "DateOfBirth", "DeletedAt", "Email", "FullName", "IsDeleted", "MailingAddress", "Nric", "PhoneNumber", "ResidentialAddress", "SchoolingStatus", "SingpassSubjectId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1991, 1, 2), null, "citizen001@example.com", "Citizen 001", false, "Mailing block 1, Singapore", "S0000001I", "+6590000001", "Residential block 1, Singapore", "Enrolled", "singpass-subject-001", null, null },
                    { 2, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1992, 2, 3), null, "citizen002@example.com", "Citizen 002", false, "Mailing block 2, Singapore", "S0000002G", "+6590000002", "Residential block 2, Singapore", "Not Enrolled", "singpass-subject-002", null, null },
                    { 3, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1993, 3, 4), null, "citizen003@example.com", "Citizen 003", false, "Mailing block 3, Singapore", "S0000003E", "+6590000003", "Residential block 3, Singapore", "Graduated", "singpass-subject-003", null, null },
                    { 4, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1994, 4, 5), null, "citizen004@example.com", "Citizen 004", false, "Mailing block 4, Singapore", "S0000004C", "+6590000004", "Residential block 4, Singapore", "Suspended", "singpass-subject-004", null, null },
                    { 5, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1995, 5, 6), null, "citizen005@example.com", "Citizen 005", false, "Mailing block 5, Singapore", "S0000005A", "+6590000005", "Residential block 5, Singapore", "Withdrawn", "singpass-subject-005", null, null },
                    { 6, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1996, 6, 7), null, "citizen006@example.com", "Citizen 006", false, "Mailing block 6, Singapore", "S0000006Z", "+6590000006", "Residential block 6, Singapore", "Enrolled", "singpass-subject-006", null, null },
                    { 7, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1997, 7, 8), null, "citizen007@example.com", "Citizen 007", false, "Mailing block 7, Singapore", "S0000007H", "+6590000007", "Residential block 7, Singapore", "Not Enrolled", "singpass-subject-007", null, null },
                    { 8, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1998, 8, 9), null, "citizen008@example.com", "Citizen 008", false, "Mailing block 8, Singapore", "S0000008F", "+6590000008", "Residential block 8, Singapore", "Graduated", "singpass-subject-008", null, null },
                    { 9, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1999, 9, 10), null, "citizen009@example.com", "Citizen 009", false, "Mailing block 9, Singapore", "S0000009D", "+6590000009", "Residential block 9, Singapore", "Suspended", "singpass-subject-009", null, null },
                    { 10, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 10, 11), null, "citizen010@example.com", "Citizen 010", false, "Mailing block 10, Singapore", "S0000010H", "+6590000010", "Residential block 10, Singapore", "Withdrawn", "singpass-subject-010", null, null },
                    { 11, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 11, 12), null, "citizen011@example.com", "Citizen 011", false, "Mailing block 11, Singapore", "S0000011F", "+6590000011", "Residential block 11, Singapore", "Enrolled", "singpass-subject-011", null, null },
                    { 12, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 12, 13), null, "citizen012@example.com", "Citizen 012", false, "Mailing block 12, Singapore", "S0000012D", "+6590000012", "Residential block 12, Singapore", "Not Enrolled", "singpass-subject-012", null, null },
                    { 13, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 1, 14), null, "citizen013@example.com", "Citizen 013", false, "Mailing block 13, Singapore", "S0000013B", "+6590000013", "Residential block 13, Singapore", "Graduated", "singpass-subject-013", null, null },
                    { 14, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 2, 15), null, "citizen014@example.com", "Citizen 014", false, "Mailing block 14, Singapore", "S0000014J", "+6590000014", "Residential block 14, Singapore", "Suspended", "singpass-subject-014", null, null },
                    { 15, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2005, 3, 16), null, "citizen015@example.com", "Citizen 015", false, "Mailing block 15, Singapore", "S0000015I", "+6590000015", "Residential block 15, Singapore", "Withdrawn", "singpass-subject-015", null, null }
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
                table: "TopupRule",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "RuleName", "Status", "TopupAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 001", 2, 70m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 002", 1, 770m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 003", 1, 790m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 004", 1, 170m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 005", 1, 260m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 006", 1, 230m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 007", 2, 220m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 008", 1, 140m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 009", 1, 200m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 010", 1, 860m, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 011", 1, 110m, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 012", 1, 360m, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 013", 1, 580m, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 014", 1, 730m, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 015", 1, 400m, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 016", 1, 680m, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 017", 1, 520m, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 018", 1, 260m, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 019", 1, 10m, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 020", 1, 450m, null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 021", 1, 710m, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 022", 1, 270m, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 023", 1, 630m, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 024", 1, 370m, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 025", 1, 180m, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 026", 1, 640m, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 027", 1, 440m, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 028", 1, 30m, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 029", 2, 710m, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 030", 1, 930m, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 031", 1, 100m, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 032", 2, 340m, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 033", 2, 820m, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 034", 1, 870m, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 035", 1, 770m, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 036", 1, 740m, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 037", 1, 440m, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 038", 1, 650m, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 039", 2, 260m, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 040", 1, 80m, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 041", 1, 250m, null, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 042", 1, 570m, null, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 043", 2, 600m, null, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 044", 2, 770m, null, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 045", 1, 460m, null, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 046", 1, 60m, null, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 047", 1, 550m, null, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 048", 1, 390m, null, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 049", 1, 120m, null, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Random Top-up Rule 050", 1, 460m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupSchedule",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "ExecuteAtDay", "ExecuteAtMonth", "IsActive", "IsDeleted", "LastExecutedAt", "NextExecutionAt", "OneTimeExecutionDate", "ScheduleName", "ScheduleType", "SpecificExecutionTime", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Random Schedule 001", 0, new TimeSpan(0, 18, 30, 0, 0), null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, null, true, false, null, new DateTime(2027, 7, 8, 3, 44, 0, 0, DateTimeKind.Utc), null, "Random Schedule 002", 2, new TimeSpan(0, 3, 44, 0, 0), null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Random Schedule 003", 0, new TimeSpan(0, 5, 1, 0, 0), null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Random Schedule 004", 0, new TimeSpan(0, 4, 53, 0, 0), null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, null, true, false, null, new DateTime(2027, 9, 11, 2, 31, 0, 0, DateTimeKind.Utc), null, "Random Schedule 005", 2, new TimeSpan(0, 2, 31, 0, 0), null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(2027, 5, 7, 17, 44, 0, 0, DateTimeKind.Utc), new DateTime(2027, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Random Schedule 006", 1, new TimeSpan(0, 17, 44, 0, 0), null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, null, true, false, null, new DateTime(2027, 10, 8, 12, 29, 0, 0, DateTimeKind.Utc), null, "Random Schedule 007", 2, new TimeSpan(0, 12, 29, 0, 0), null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Random Schedule 008", 0, new TimeSpan(0, 10, 29, 0, 0), null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, null, true, false, null, new DateTime(2027, 4, 18, 6, 16, 0, 0, DateTimeKind.Utc), null, "Random Schedule 009", 2, new TimeSpan(0, 6, 16, 0, 0), null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(2027, 8, 17, 4, 51, 0, 0, DateTimeKind.Utc), new DateTime(2027, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Random Schedule 010", 1, new TimeSpan(0, 4, 51, 0, 0), null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(2027, 9, 2, 0, 32, 0, 0, DateTimeKind.Utc), new DateTime(2027, 9, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Random Schedule 011", 1, new TimeSpan(0, 0, 32, 0, 0), null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, null, false, false, null, new DateTime(2027, 1, 10, 2, 25, 0, 0, DateTimeKind.Utc), null, "Random Schedule 012", 2, new TimeSpan(0, 2, 25, 0, 0), null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, null, false, false, null, new DateTime(2027, 5, 22, 20, 43, 0, 0, DateTimeKind.Utc), null, "Random Schedule 013", 2, new TimeSpan(0, 20, 43, 0, 0), null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, null, true, false, null, new DateTime(2027, 8, 19, 10, 22, 0, 0, DateTimeKind.Utc), null, "Random Schedule 014", 2, new TimeSpan(0, 10, 22, 0, 0), null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Random Schedule 015", 0, new TimeSpan(0, 1, 17, 0, 0), null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Random Schedule 016", 0, new TimeSpan(0, 13, 32, 0, 0), null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, false, null, new DateTime(2027, 6, 20, 18, 1, 0, 0, DateTimeKind.Utc), new DateTime(2027, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Random Schedule 017", 1, new TimeSpan(0, 18, 1, 0, 0), null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Random Schedule 018", 0, new TimeSpan(0, 13, 16, 0, 0), null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(2027, 6, 17, 2, 15, 0, 0, DateTimeKind.Utc), new DateTime(2027, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Random Schedule 019", 1, new TimeSpan(0, 2, 15, 0, 0), null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, true, false, null, new DateTime(2027, 2, 26, 2, 48, 0, 0, DateTimeKind.Utc), new DateTime(2027, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Random Schedule 020", 1, new TimeSpan(0, 2, 48, 0, 0), null, null }
                });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "CourseFeeAmount", "CourseName", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "GstAmount", "IsDeleted", "MiscFeeAmount", "SchoolId", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 100m, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Foundation course in applied mathematics.", 10m, false, 10m, 1, 1, null, null },
                    { 2, 115m, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Introduction to programming and computing.", 13m, false, 12m, 2, 1, null, null },
                    { 3, 130m, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Professional written and verbal communication.", 15m, false, 15m, 3, 1, null, null },
                    { 4, 145m, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Environmental systems and sustainability.", 18m, false, 17m, 4, 1, null, null },
                    { 5, 160m, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Digital design principles and production.", 20m, false, 20m, 5, 1, null, null },
                    { 6, 175m, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Core hospitality service operations.", 23m, false, 22m, 6, 1, null, null },
                    { 7, 190m, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Fundamentals of electrical systems.", 25m, false, 25m, 7, 1, null, null },
                    { 8, 205m, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Writing techniques across common genres.", 28m, false, 27m, 8, 1, null, null },
                    { 9, 220m, "Data Analytics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Data preparation, analysis and reporting.", 30m, false, 30m, 9, 1, null, null },
                    { 10, 235m, "Legacy Office Applications", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Archived office applications programme.", 33m, false, 32m, 10, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "EducationAccount",
                columns: new[] { "Id", "AccountNumber", "CitizenId", "ClosedAt", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditBalance", "IsDeleted", "OpenedAt", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EA000000000000000001", 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1100m, false, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 2, "EA000000000000000002", 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1200m, false, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 3, "EA000000000000000003", 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1300m, false, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 4, "EA000000000000000004", 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1400m, false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 5, "EA000000000000000005", 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1500m, false, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 6, "EA000000000000000006", 6, new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1600m, false, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null },
                    { 7, "EA000000000000000007", 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1700m, false, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 8, "EA000000000000000008", 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1800m, false, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null },
                    { 9, "EA000000000000000009", 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1900m, false, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null },
                    { 10, "EA000000000000000010", 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2000m, false, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "RefreshToken",
                columns: new[] { "Id", "AuthAccountId", "CreatedAt", "CreatedBy", "DeletedAt", "ExpiresAt", "IsDeleted", "RevokedAt", "TokenHash", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-001", null, null },
                    { 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-002", null, null },
                    { 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-003", null, null },
                    { 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "refresh-token-hash-004", null, null },
                    { 5, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-005", null, null },
                    { 6, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-006", null, null },
                    { 7, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-007", null, null },
                    { 8, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), false, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), "refresh-token-hash-008", null, null },
                    { 9, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-009", null, null },
                    { 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), false, null, "refresh-token-hash-010", null, null }
                });

            migrationBuilder.InsertData(
                table: "SsoIdentity",
                columns: new[] { "Id", "AuthAccountId", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Provider", "ProviderUserId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "ae5873a0-acbe-4976-ba46-69aee20cfa48", null, null },
                    { 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "f8cdef31-a31e-4b4a-93e4-5f571e91255a", null, null },
                    { 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "b1e7cdf2-43ef-4a3e-9eb8-4e63b3ae42f4", null, null },
                    { 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "f116e09e-1a6f-4847-aa57-442705d242d0", null, null },
                    { 5, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "db3c66a7-b7f4-47df-bd7a-3f70fcaaa73d", null, null },
                    { 6, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "0446ecca-6483-4129-bd4f-906f970f18d5", null, null },
                    { 7, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "azure-object-007", null, null },
                    { 8, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "azure-object-008", null, null },
                    { 9, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-009", null, null },
                    { 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-010", null, null },
                    { 11, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-011", null, null },
                    { 12, 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-012", null, null },
                    { 13, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-013", null, null },
                    { 14, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-014", null, null },
                    { 15, 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-015", null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupBatch",
                columns: new[] { "Id", "BatchCode", "CreatedAt", "CreatedBy", "DeletedAt", "ExecutedAt", "IsDeleted", "Status", "TopupRuleId", "TotalAmount", "TotalTargetCount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "REG-BATCH-001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1, 1, 110m, 1, null, null },
                    { 2, "REG-BATCH-002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), false, 2, 2, 120m, 1, null, null },
                    { 3, "REG-BATCH-003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1, 3, 130m, 1, null, null },
                    { 4, "REG-BATCH-004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), false, 2, 4, 140m, 1, null, null },
                    { 5, "REG-BATCH-005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1, 5, 150m, 1, null, null },
                    { 6, "REG-BATCH-006", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), false, 2, 6, 160m, 1, null, null },
                    { 7, "REG-BATCH-007", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1, 7, 170m, 1, null, null },
                    { 8, "REG-BATCH-008", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), false, 2, 8, 180m, 1, null, null },
                    { 9, "REG-BATCH-009", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1, 9, 190m, 1, null, null },
                    { 10, "REG-BATCH-010", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), false, 2, 10, 200m, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupRuleCondition",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Field", "IsDeleted", "Operator", "TopupRuleId", "UpdatedAt", "UpdatedBy", "ValueNumber", "ValueText" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 1, null, null, 12m, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 1, null, null, 22m, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 2, null, null, 700m, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 2, null, null, 1500m, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 3, null, null, 16m, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 3, null, null, 19m, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 4, null, null, 200m, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 4, null, null, 600m, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 5, null, null, 12m, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 5, null, null, 14m, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 6, null, null, 100m, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 6, null, null, 900m, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 7, null, null, 300m, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 7, null, null, 400m, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 8, null, null, 300m, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 8, null, null, 1000m, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 9, null, null, 600m, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 9, null, null, 1300m, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 10, null, null, 300m, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 10, null, null, 600m, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 11, null, null, 200m, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 11, null, null, 700m, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 12, null, null, 13m, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 12, null, null, 22m, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 13, null, null, 14m, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 13, null, null, 19m, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 14, null, null, 16m, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 14, null, null, 20m, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 15, null, null, 13m, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 15, null, null, 20m, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 16, null, null, 14m, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 16, null, null, 23m, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 17, null, null, 17m, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 17, null, null, 22m, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 18, null, null, 400m, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 18, null, null, 900m, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 19, null, null, 15m, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 19, null, null, 22m, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 20, null, null, 17m, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 20, null, null, 19m, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 21, null, null, 14m, null },
                    { 42, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 21, null, null, 18m, null },
                    { 43, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 22, null, null, 16m, null },
                    { 44, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 22, null, null, 17m, null },
                    { 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 23, null, null, 700m, null },
                    { 46, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 23, null, null, 1400m, null },
                    { 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 24, null, null, 16m, null },
                    { 48, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 24, null, null, 22m, null },
                    { 49, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 25, null, null, 14m, null },
                    { 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 25, null, null, 21m, null },
                    { 51, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 26, null, null, 200m, null },
                    { 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 26, null, null, 300m, null },
                    { 53, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 27, null, null, 13m, null },
                    { 54, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 27, null, null, 16m, null },
                    { 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 28, null, null, 500m, null },
                    { 56, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 28, null, null, 1000m, null },
                    { 57, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 29, null, null, 0m, null },
                    { 58, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 29, null, null, 700m, null },
                    { 59, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 30, null, null, 14m, null },
                    { 60, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 30, null, null, 22m, null },
                    { 61, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 31, null, null, 17m, null },
                    { 62, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 31, null, null, 21m, null },
                    { 63, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 32, null, null, 14m, null },
                    { 64, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 32, null, null, 17m, null },
                    { 65, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 33, null, null, 13m, null },
                    { 66, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 33, null, null, 19m, null },
                    { 67, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 34, null, null, 400m, null },
                    { 68, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 34, null, null, 1000m, null },
                    { 69, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 35, null, null, 16m, null },
                    { 70, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 35, null, null, 17m, null },
                    { 71, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 36, null, null, 100m, null },
                    { 72, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 36, null, null, 200m, null },
                    { 73, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 37, null, null, 300m, null },
                    { 74, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 37, null, null, 600m, null },
                    { 75, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 38, null, null, 900m, null },
                    { 76, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 38, null, null, 1500m, null },
                    { 77, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 39, null, null, 200m, null },
                    { 78, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 39, null, null, 1000m, null },
                    { 79, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 40, null, null, 400m, null },
                    { 80, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 40, null, null, 1100m, null },
                    { 81, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 41, null, null, 17m, null },
                    { 82, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 41, null, null, 19m, null },
                    { 83, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 42, null, null, 13m, null },
                    { 84, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 42, null, null, 22m, null },
                    { 85, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 43, null, null, 13m, null },
                    { 86, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 43, null, null, 24m, null },
                    { 87, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 44, null, null, 15m, null },
                    { 88, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 44, null, null, 16m, null },
                    { 89, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 45, null, null, 400m, null },
                    { 90, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 45, null, null, 1200m, null },
                    { 91, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 46, null, null, 700m, null },
                    { 92, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 46, null, null, 1400m, null },
                    { 93, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 47, null, null, 16m, null },
                    { 94, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 47, null, null, 24m, null },
                    { 95, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 48, null, null, 100m, null },
                    { 96, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 48, null, null, 200m, null },
                    { 97, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 4, 49, null, null, 14m, null },
                    { 98, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 6, 49, null, null, 24m, null },
                    { 99, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 50, null, null, 300m, null },
                    { 100, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 6, 50, null, null, 500m, null }
                });

            migrationBuilder.InsertData(
                table: "TopupScheduleRule",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "TopupRuleId", "TopupScheduleId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 26, 2, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 40, 2, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 42, 2, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 9, 2, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 14, 3, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 26, 3, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 12, 3, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 11, 4, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, 4, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 38, 5, null, null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 45, 6, null, null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 18, 7, null, null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 6, 7, null, null },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 27, 7, null, null },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 36, 7, null, null },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 32, 8, null, null },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 37, 8, null, null },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 38, 8, null, null },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 13, 9, null, null },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 35, 9, null, null },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 26, 10, null, null },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 13, 11, null, null },
                    { 24, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 39, 11, null, null },
                    { 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 25, 12, null, null },
                    { 26, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 25, 13, null, null },
                    { 27, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 36, 13, null, null },
                    { 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 14, 14, null, null },
                    { 29, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 15, 14, null, null },
                    { 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 15, 15, null, null },
                    { 31, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 19, 15, null, null },
                    { 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 43, 15, null, null },
                    { 33, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 43, 16, null, null },
                    { 34, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 31, 17, null, null },
                    { 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 23, 17, null, null },
                    { 36, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 25, 17, null, null },
                    { 37, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 27, 18, null, null },
                    { 38, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, 19, null, null },
                    { 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 47, 19, null, null },
                    { 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 9, 19, null, null },
                    { 41, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 21, 20, null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AuthAccountId", "CitizenId", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Role", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, null, null },
                    { 2, 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, null, null },
                    { 3, 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, null, null },
                    { 4, 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, null, null },
                    { 5, 5, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, null, null },
                    { 6, 6, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, null, null },
                    { 7, 7, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, null, null },
                    { 8, 8, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 3, null, null },
                    { 9, 9, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 10, 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 11, 11, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 12, 12, 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 13, 13, 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 14, 14, 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 15, 15, 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null }
                });

            migrationBuilder.InsertData(
                table: "AdhocTopupBatchTarget",
                columns: new[] { "Id", "AdhocTopupBatchId", "Amount", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "FailureReason", "IsDeleted", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, 55m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, null, false, 1, null, null },
                    { 2, 2, 60m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, null, false, 2, null, null },
                    { 3, 3, 65m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, null, false, 1, null, null },
                    { 4, 4, 70m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, null, false, 2, null, null },
                    { 5, 5, 75m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, "Manual review rejected", false, 3, null, null },
                    { 6, 6, 80m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, null, false, 2, null, null },
                    { 7, 7, 85m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, null, false, 1, null, null },
                    { 8, 8, 90m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, null, false, 2, null, null },
                    { 9, 9, 95m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, null, false, 1, null, null },
                    { 10, 10, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, "Manual review rejected", false, 3, null, null }
                });

            migrationBuilder.InsertData(
                table: "AdminProfile",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "FullName", "IsDeleted", "Nric", "PhoneNumber", "SchoolId", "StaffCode", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin001@example.com", "System Administrator 001", false, "T0000001E", "+6591000001", null, "STAFF-001", null, null, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin002@example.com", "System Administrator 002", false, "T0000002C", "+6591000002", null, "STAFF-002", null, null, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin003@example.com", "System Administrator 003", false, "T0000003A", "+6591000003", null, "STAFF-003", null, null, 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin004@example.com", "System Administrator 004", false, "T0000004Z", "+6591000004", null, "STAFF-004", null, null, 4 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin005@example.com", "System Administrator 005", false, "T0000005H", "+6591000005", null, "STAFF-005", null, null, 5 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin006@example.com", "System Administrator 006", false, "T0000006F", "+6591000006", null, "STAFF-006", null, null, 6 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin007@example.com", "Finance Administrator", false, "T0000007D", "+6591000007", null, "STAFF-007", null, null, 7 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin008@example.com", "School Administrator", false, "T0000008B", "+6591000008", 1, "STAFF-008", null, null, 8 }
                });

            migrationBuilder.InsertData(
                table: "AuditLog",
                columns: new[] { "Id", "Action", "ActorUserId", "Category", "IpAddress", "Nric", "OccurredAt", "PayloadJson" },
                values: new object[,]
                {
                    { 1, "AdminLoginSucceeded", 1, 4, "127.0.0.1", null, new DateTime(2026, 1, 1, 0, 1, 0, 0, DateTimeKind.Utc), "{\"seedId\":1}" },
                    { 2, "CreditTransactionCreated", 2, 5, "127.0.0.2", null, new DateTime(2026, 1, 1, 0, 2, 0, 0, DateTimeKind.Utc), "{\"seedId\":2}" },
                    { 3, "AdminLoginSucceeded", 3, 3, "127.0.0.3", null, new DateTime(2026, 1, 1, 0, 3, 0, 0, DateTimeKind.Utc), "{\"seedId\":3}" },
                    { 4, "CreditTransactionCreated", 4, 5, "127.0.0.4", null, new DateTime(2026, 1, 1, 0, 4, 0, 0, DateTimeKind.Utc), "{\"seedId\":4}" },
                    { 5, "AdminLoginSucceeded", 5, 4, "127.0.0.5", null, new DateTime(2026, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc), "{\"seedId\":5}" },
                    { 6, "CreditTransactionCreated", 6, 3, "127.0.0.6", null, new DateTime(2026, 1, 1, 0, 6, 0, 0, DateTimeKind.Utc), "{\"seedId\":6}" },
                    { 7, "AdminLoginSucceeded", 7, 4, "127.0.0.7", null, new DateTime(2026, 1, 1, 0, 7, 0, 0, DateTimeKind.Utc), "{\"seedId\":7}" },
                    { 8, "CreditTransactionCreated", 8, 5, "127.0.0.8", null, new DateTime(2026, 1, 1, 0, 8, 0, 0, DateTimeKind.Utc), "{\"seedId\":8}" },
                    { 9, "AdminLoginSucceeded", 9, 3, "127.0.0.9", null, new DateTime(2026, 1, 1, 0, 9, 0, 0, DateTimeKind.Utc), "{\"seedId\":9}" },
                    { 10, "CreditTransactionCreated", 10, 5, "127.0.0.10", null, new DateTime(2026, 1, 1, 0, 10, 0, 0, DateTimeKind.Utc), "{\"seedId\":10}" },
                    { 11, "Auto Provisioning Sweep Completed", 1, 1, "127.0.0.1", null, new DateTime(2026, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc), "{\"BatchDate\":\"2026-01-01\",\"StartedAt\":\"2026-01-01T00:00:03\\u002B08:00\",\"CompletedAt\":\"2026-01-01T00:04:15\\u002B08:00\",\"ExecutionTime\":\"00:04:12\",\"CreationMetrics\":{\"SuccessCount\":7,\"DuplicateSkippedCount\":2,\"FailedCount\":1},\"FailedCases\":[{\"Nric\":\"S0000015I\",\"FullName\":\"Citizen 015\",\"Reason\":\"Manual review is required for the seeded batch record.\"}],\"FailedCsvBase64\":\"\",\"NotificationSummary\":{\"EmailSentCount\":7},\"ClosureBatchSummary\":{\"ClosedAccountsCount\":2,\"TotalAmountTransferredToCpf\":450.00,\"NotificationsSent\":{\"t90\":3,\"t30\":2,\"t7\":1}}}" }
                });

            migrationBuilder.InsertData(
                table: "EducationCreditTransaction",
                columns: new[] { "Id", "Amount", "BalanceAfter", "BalanceBefore", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "Direction", "EducationAccountId", "IsDeleted", "TransactionCode", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 110m, 1210m, 1100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 001", 1, 1, false, new Guid("00000000-0000-0000-0000-000000000001"), 1, null, null },
                    { 2, 120m, 1320m, 1200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 002", 1, 2, false, new Guid("00000000-0000-0000-0000-000000000002"), 1, null, null },
                    { 3, 130m, 1430m, 1300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 003", 1, 3, false, new Guid("00000000-0000-0000-0000-000000000003"), 1, null, null },
                    { 4, 140m, 1540m, 1400m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 004", 1, 4, false, new Guid("00000000-0000-0000-0000-000000000004"), 4, null, null },
                    { 5, 150m, 1650m, 1500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 005", 1, 5, false, new Guid("00000000-0000-0000-0000-000000000005"), 1, null, null },
                    { 6, 160m, 1760m, 1600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 006", 1, 6, false, new Guid("00000000-0000-0000-0000-000000000006"), 1, null, null },
                    { 7, 170m, 1870m, 1700m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 007", 1, 7, false, new Guid("00000000-0000-0000-0000-000000000007"), 1, null, null },
                    { 8, 180m, 1980m, 1800m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 008", 1, 8, false, new Guid("00000000-0000-0000-0000-000000000008"), 4, null, null },
                    { 9, 190m, 2090m, 1900m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 009", 1, 9, false, new Guid("00000000-0000-0000-0000-000000000009"), 1, null, null },
                    { 10, 200m, 2200m, 2000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed transaction 010", 1, 10, false, new Guid("00000000-0000-0000-0000-000000000010"), 1, null, null },
                    { 11, 55m, 1805m, 1750m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 011", 1, 1, false, new Guid("00000000-0000-0000-0000-000000000011"), 4, null, null },
                    { 12, 60m, 1860m, 1800m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 012", 1, 2, false, new Guid("00000000-0000-0000-0000-000000000012"), 4, null, null },
                    { 13, 65m, 1915m, 1850m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 013", 1, 3, false, new Guid("00000000-0000-0000-0000-000000000013"), 4, null, null },
                    { 14, 70m, 1970m, 1900m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 014", 1, 4, false, new Guid("00000000-0000-0000-0000-000000000014"), 4, null, null },
                    { 15, 75m, 2025m, 1950m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 015", 1, 5, false, new Guid("00000000-0000-0000-0000-000000000015"), 4, null, null },
                    { 16, 80m, 2080m, 2000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 016", 1, 6, false, new Guid("00000000-0000-0000-0000-000000000016"), 4, null, null },
                    { 17, 85m, 2135m, 2050m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 017", 1, 7, false, new Guid("00000000-0000-0000-0000-000000000017"), 4, null, null },
                    { 18, 90m, 2190m, 2100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 018", 1, 8, false, new Guid("00000000-0000-0000-0000-000000000018"), 4, null, null },
                    { 19, 95m, 2245m, 2150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 019", 1, 9, false, new Guid("00000000-0000-0000-0000-000000000019"), 4, null, null },
                    { 20, 100m, 2300m, 2200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 020", 1, 10, false, new Guid("00000000-0000-0000-0000-000000000020"), 4, null, null },
                    { 21, 120m, 980m, 1100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 001", 2, 1, false, new Guid("00000000-0000-0000-0000-000000000021"), 2, null, null },
                    { 22, 70m, 1130m, 1200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 002", 2, 2, false, new Guid("00000000-0000-0000-0000-000000000022"), 2, null, null },
                    { 23, 140m, 1160m, 1300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 003", 2, 3, false, new Guid("00000000-0000-0000-0000-000000000023"), 2, null, null },
                    { 24, 180m, 1220m, 1400m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 004", 2, 4, false, new Guid("00000000-0000-0000-0000-000000000024"), 2, null, null },
                    { 25, 180m, 1320m, 1500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 005", 2, 5, false, new Guid("00000000-0000-0000-0000-000000000025"), 2, null, null },
                    { 26, 100m, 1500m, 1600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 006", 2, 6, false, new Guid("00000000-0000-0000-0000-000000000026"), 2, null, null },
                    { 27, 200m, 1500m, 1700m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 007", 2, 7, false, new Guid("00000000-0000-0000-0000-000000000027"), 2, null, null },
                    { 28, 130m, 1670m, 1800m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 008", 2, 8, false, new Guid("00000000-0000-0000-0000-000000000028"), 2, null, null },
                    { 29, 250m, 1650m, 1900m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 009", 2, 9, false, new Guid("00000000-0000-0000-0000-000000000029"), 2, null, null },
                    { 30, 300m, 1700m, 2000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment transaction 010", 2, 10, false, new Guid("00000000-0000-0000-0000-000000000030"), 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Enrollment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenEmailSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CitizenPhoneNumberSnapshot", "CompletedAt", "CourseDescriptionSnapshot", "CourseId", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "EnrolledAt", "IsDeleted", "SchoolNameSnapshot", "UpdatedAt", "UpdatedBy", "WithdrawnAt" },
                values: new object[,]
                {
                    { 1, "EA000000000000000001", "citizen001@example.com", "Citizen 001", "S0000001I", "+6590000001", null, "Foundation course in applied mathematics.", 1, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), false, "Northview Secondary School", null, null, null },
                    { 2, "EA000000000000000002", "citizen002@example.com", "Citizen 002", "S0000002G", "+6590000002", null, "Introduction to programming and computing.", 2, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), false, "Eastbridge Secondary School", null, null, null },
                    { 3, "EA000000000000000003", "citizen003@example.com", "Citizen 003", "S0000003E", "+6590000003", new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Professional written and verbal communication.", 3, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), false, "Westhaven Secondary School", null, null, null },
                    { 4, "EA000000000000000004", "citizen004@example.com", "Citizen 004", "S0000004C", "+6590000004", null, "Environmental systems and sustainability.", 4, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, "Southpoint Secondary School", null, null, null },
                    { 5, "EA000000000000000005", "citizen005@example.com", "Citizen 005", "S0000005A", "+6590000005", null, "Digital design principles and production.", 5, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), false, "Central Heights School", null, null, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "EA000000000000000006", "citizen006@example.com", "Citizen 006", "S0000006Z", "+6590000006", null, "Core hospitality service operations.", 6, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), false, "Riverside Learning Institute", null, null, null },
                    { 7, "EA000000000000000007", "citizen007@example.com", "Citizen 007", "S0000007H", "+6590000007", new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Fundamentals of electrical systems.", 7, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), false, "Lakeside Technical School", null, null, null },
                    { 8, "EA000000000000000008", "citizen008@example.com", "Citizen 008", "S0000008F", "+6590000008", null, "Writing techniques across common genres.", 8, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Greenfield Academy", null, null, null },
                    { 9, "EA000000000000000009", "citizen009@example.com", "Citizen 009", "S0000009D", "+6590000009", null, "Data preparation, analysis and reporting.", 9, "Data Analytics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), false, "Harbourfront School", null, null, null },
                    { 10, "EA000000000000000010", "citizen010@example.com", "Citizen 010", "S0000010H", "+6590000010", null, "Archived office applications programme.", 10, "Legacy Office Applications", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), false, "Hillcrest Education Centre", null, null, new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "TopupBatchTarget",
                columns: new[] { "Id", "Amount", "CreatedAt", "CreatedBy", "DeletedAt", "EducationAccountId", "FailureReason", "IsDeleted", "Status", "TopupBatchId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 110m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, null, false, 1, 1, null, null },
                    { 2, 120m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, null, false, 2, 2, null, null },
                    { 3, 130m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, null, false, 1, 3, null, null },
                    { 4, 140m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, null, false, 2, 4, null, null },
                    { 5, 150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, "Eligibility check failed", false, 3, 5, null, null },
                    { 6, 160m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, null, false, 2, 6, null, null },
                    { 7, 170m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, null, false, 1, 7, null, null },
                    { 8, 180m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, null, false, 2, 8, null, null },
                    { 9, 190m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, null, false, 1, 9, null, null },
                    { 10, 200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, "Eligibility check failed", false, 3, 10, null, null }
                });

            migrationBuilder.InsertData(
                table: "AdhocTopupBatchTargetTransaction",
                columns: new[] { "Id", "AdhocTopupBatchTargetId", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditTransactionId", "IsDeleted", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, false, null, null },
                    { 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, false, null, null },
                    { 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, false, null, null },
                    { 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, false, null, null },
                    { 5, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 15, false, null, null },
                    { 6, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 16, false, null, null },
                    { 7, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 17, false, null, null },
                    { 8, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 18, false, null, null },
                    { 9, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 19, false, null, null },
                    { 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 20, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "Charge",
                columns: new[] { "Id", "CourseFeeAmountSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EnrollmentId", "GrossAmount", "GstAmountSnapshot", "IsDeleted", "MiscFeeAmountSnapshot", "NetAmount", "PaidAmount", "RemainingAmount", "Status", "SubsidyAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 120m, 10m, false, 10m, 120m, 120m, 0m, 3, 0m, null, null },
                    { 2, 115m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, 140m, 13m, false, 12m, 140m, 70m, 70m, 2, 0m, null, null },
                    { 3, 130m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 160m, 15m, false, 15m, 140m, 140m, 0m, 3, 20m, null, null },
                    { 4, 145m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, 180m, 18m, false, 17m, 180m, 180m, 0m, 3, 0m, null, null },
                    { 5, 160m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, 200m, 20m, false, 20m, 180m, 180m, 0m, 3, 20m, null, null },
                    { 6, 175m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 220m, 23m, false, 22m, 220m, 100m, 120m, 4, 0m, null, null },
                    { 7, 190m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, 240m, 25m, false, 25m, 200m, 200m, 0m, 3, 40m, null, null },
                    { 8, 205m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, 260m, 28m, false, 27m, 260m, 130m, 130m, 2, 0m, null, null },
                    { 9, 220m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, 280m, 30m, false, 30m, 250m, 250m, 0m, 3, 30m, null, null },
                    { 10, 235m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, 300m, 33m, false, 32m, 300m, 300m, 0m, 3, 0m, null, null }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "AccountNumberSnapshot", "CitizenFullNameSnapshot", "CitizenNricSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditTransactionId", "ExternalReference", "IsDeleted", "PaidAt", "PaymentMethod", "Status", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "EA000000000000000001", "Citizen 001", "S0000001I", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 21, null, false, new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 120m, null, null },
                    { 2, "EA000000000000000002", "Citizen 002", "S0000002G", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 22, null, false, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 70m, null, null },
                    { 3, "EA000000000000000003", "Citizen 003", "S0000003E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 23, null, false, new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 140m, null, null },
                    { 4, "EA000000000000000004", "Citizen 004", "S0000004C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 24, null, false, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 180m, null, null },
                    { 5, "EA000000000000000005", "Citizen 005", "S0000005A", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 25, null, false, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 180m, null, null },
                    { 6, "EA000000000000000006", "Citizen 006", "S0000006Z", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 26, "PAY-ONLINE-0006", false, new DateTime(2026, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 100m, null, null },
                    { 7, "EA000000000000000007", "Citizen 007", "S0000007H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 27, "PAY-ONLINE-0007", false, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 200m, null, null },
                    { 8, "EA000000000000000008", "Citizen 008", "S0000008F", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 28, "PAY-ONLINE-0008", false, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 130m, null, null },
                    { 9, "EA000000000000000009", "Citizen 009", "S0000009D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 29, "PAY-ONLINE-0009", false, new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 250m, null, null },
                    { 10, "EA000000000000000010", "Citizen 010", "S0000010H", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 30, "PAY-ONLINE-0010", false, new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 300m, null, null }
                });

            migrationBuilder.InsertData(
                table: "TopupBatchTargetTransaction",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "EducationCreditTransactionId", "IsDeleted", "TopupBatchTargetId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 2, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, 3, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, false, 4, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, false, 5, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, false, 6, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, false, 7, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, false, 8, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, false, 9, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, false, 10, null, null }
                });

            migrationBuilder.InsertData(
                table: "PaymentAllocation",
                columns: new[] { "Id", "Amount", "ChargeGrossAmountSnapshot", "ChargeId", "ChargeNetAmountSnapshot", "ChargeRemainingAmountSnapshot", "CourseNameSnapshot", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "PaymentId", "SchoolNameSnapshot", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 120m, 120m, 1, 120m, 0m, "Applied Mathematics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "Northview Secondary School", null, null },
                    { 2, 70m, 140m, 2, 140m, 70m, "Computer Science Fundamentals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "Eastbridge Secondary School", null, null },
                    { 3, 140m, 160m, 3, 140m, 0m, "Business Communication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 3, "Westhaven Secondary School", null, null },
                    { 4, 180m, 180m, 4, 180m, 0m, "Environmental Science", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, "Southpoint Secondary School", null, null },
                    { 5, 180m, 200m, 5, 180m, 0m, "Digital Media Design", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 5, "Central Heights School", null, null },
                    { 6, 100m, 220m, 6, 220m, 120m, "Hospitality Operations", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 6, "Riverside Learning Institute", null, null },
                    { 7, 200m, 240m, 7, 200m, 0m, "Electrical Engineering Basics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 7, "Lakeside Technical School", null, null },
                    { 8, 130m, 260m, 8, 260m, 130m, "Creative Writing", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 8, "Greenfield Academy", null, null },
                    { 9, 250m, 280m, 9, 250m, 0m, "Data Analytics", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 9, "Harbourfront School", null, null },
                    { 10, 300m, 300m, 10, 300m, 0m, "Legacy Office Applications", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 10, "Hillcrest Education Centre", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdhocTopupBatch_Status",
                table: "AdhocTopupBatch",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AdhocTopupBatchTarget_AdhocTopupBatchId",
                table: "AdhocTopupBatchTarget",
                column: "AdhocTopupBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_AdhocTopupBatchTarget_AdhocTopupBatchId_EducationAccountId",
                table: "AdhocTopupBatchTarget",
                columns: new[] { "AdhocTopupBatchId", "EducationAccountId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"AdhocTopupBatchId\" IS NOT NULL AND \"EducationAccountId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdhocTopupBatchTarget_EducationAccountId",
                table: "AdhocTopupBatchTarget",
                column: "EducationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AdhocTopupBatchTarget_Status",
                table: "AdhocTopupBatchTarget",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AdhocTopupBatchTargetTransaction_AdhocTopupBatchTargetId",
                table: "AdhocTopupBatchTargetTransaction",
                column: "AdhocTopupBatchTargetId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"AdhocTopupBatchTargetId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdhocTopupBatchTargetTransaction_EducationCreditTransactionId",
                table: "AdhocTopupBatchTargetTransaction",
                column: "EducationCreditTransactionId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"EducationCreditTransactionId\" IS NOT NULL");

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
                name: "IX_AuthAccount_Status",
                table: "AuthAccount",
                column: "Status");

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
                name: "IX_Citizen_SingpassSubjectId",
                table: "Citizen",
                column: "SingpassSubjectId",
                unique: true,
                filter: "\"SingpassSubjectId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Course_CourseName",
                table: "Course",
                column: "CourseName");

            migrationBuilder.CreateIndex(
                name: "IX_Course_SchoolId",
                table: "Course",
                column: "SchoolId");

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
                name: "IX_Enrollment_CourseId",
                table: "Enrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseId_EducationAccountId",
                table: "Enrollment",
                columns: new[] { "CourseId", "EducationAccountId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"CourseId\" IS NOT NULL AND \"EducationAccountId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_EducationAccountId",
                table: "Enrollment",
                column: "EducationAccountId");

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
                name: "IX_Payment_EducationCreditTransactionId",
                table: "Payment",
                column: "EducationCreditTransactionId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"EducationCreditTransactionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ExternalReference",
                table: "Payment",
                column: "ExternalReference");

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
                name: "IX_RefreshToken_AuthAccountId",
                table: "RefreshToken",
                column: "AuthAccountId");

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
                name: "IX_School_Email",
                table: "School",
                column: "Email",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_School_Status",
                table: "School",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SsoIdentity_AuthAccountId",
                table: "SsoIdentity",
                column: "AuthAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SsoIdentity_AuthAccountId_Provider",
                table: "SsoIdentity",
                columns: new[] { "AuthAccountId", "Provider" },
                unique: true,
                filter: "\"AuthAccountId\" IS NOT NULL AND \"Provider\" IS NOT NULL");

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
                name: "IX_TopupBatch_BatchCode",
                table: "TopupBatch",
                column: "BatchCode",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"BatchCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupBatch_Status",
                table: "TopupBatch",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TopupBatch_TopupRuleId",
                table: "TopupBatch",
                column: "TopupRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupBatchTarget_EducationAccountId",
                table: "TopupBatchTarget",
                column: "EducationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupBatchTarget_Status",
                table: "TopupBatchTarget",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TopupBatchTarget_TopupBatchId",
                table: "TopupBatchTarget",
                column: "TopupBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupBatchTarget_TopupBatchId_EducationAccountId",
                table: "TopupBatchTarget",
                columns: new[] { "TopupBatchId", "EducationAccountId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"TopupBatchId\" IS NOT NULL AND \"EducationAccountId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupBatchTargetTransaction_EducationCreditTransactionId",
                table: "TopupBatchTargetTransaction",
                column: "EducationCreditTransactionId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"EducationCreditTransactionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupBatchTargetTransaction_TopupBatchTargetId",
                table: "TopupBatchTargetTransaction",
                column: "TopupBatchTargetId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"TopupBatchTargetId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TopupRule_RuleName",
                table: "TopupRule",
                column: "RuleName");

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
                name: "IX_TopupScheduleRule_TopupRuleId",
                table: "TopupScheduleRule",
                column: "TopupRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TopupScheduleRule_TopupScheduleId",
                table: "TopupScheduleRule",
                column: "TopupScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AuthAccountId",
                table: "User",
                column: "AuthAccountId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"AuthAccountId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_CitizenId",
                table: "User",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role",
                table: "User",
                column: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdhocTopupBatchTargetTransaction");

            migrationBuilder.DropTable(
                name: "AdminProfile");

            migrationBuilder.DropTable(
                name: "AiAssistantSetting");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "PaymentAllocation");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "SsoIdentity");

            migrationBuilder.DropTable(
                name: "TopupBatchTargetTransaction");

            migrationBuilder.DropTable(
                name: "TopupRuleCondition");

            migrationBuilder.DropTable(
                name: "TopupScheduleRule");

            migrationBuilder.DropTable(
                name: "AdhocTopupBatchTarget");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Charge");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "TopupBatchTarget");

            migrationBuilder.DropTable(
                name: "TopupSchedule");

            migrationBuilder.DropTable(
                name: "AdhocTopupBatch");

            migrationBuilder.DropTable(
                name: "AuthAccount");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "EducationCreditTransaction");

            migrationBuilder.DropTable(
                name: "TopupBatch");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "EducationAccount");

            migrationBuilder.DropTable(
                name: "TopupRule");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropTable(
                name: "Citizen");
        }
    }
}
