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
                    PayloadJson = table.Column<string>(type: "json", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
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
                });

            migrationBuilder.CreateTable(
                name: "OtpVerification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OtpHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FailedAttemptCount = table.Column<int>(type: "int", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_OtpVerification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtpVerification_AuthAccount_AuthAccountId",
                        column: x => x.AuthAccountId,
                        principalTable: "AuthAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_EducationAccount_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "Id");
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
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_EducationCreditTransaction_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_AdminProfile", x => x.Id);
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
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    PayloadJson = table.Column<string>(type: "json", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_TopupBatchTarget_EducationAccount_EducationAccountId",
                        column: x => x.EducationAccountId,
                        principalTable: "EducationAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopupBatchTargetTransaction_TopupBatchTarget_TopupBatchTargetId",
                        column: x => x.TopupBatchTargetId,
                        principalTable: "TopupBatchTarget",
                        principalColumn: "Id");
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
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Citizen",
                columns: new[] { "Id", "CitizenshipStatus", "CreatedAt", "CreatedBy", "DateOfBirth", "DeletedAt", "Email", "FullName", "IsDeleted", "MailingAddress", "Nric", "PhoneNumber", "ResidentialAddress", "SingpassSubjectId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1991, 1, 2), null, "citizen001@example.com", "Citizen 001", false, "Mailing block 1, Singapore", "S0000001A", "+6590000001", "Residential block 1, Singapore", "singpass-subject-001", null, null },
                    { 2, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1992, 2, 3), null, "citizen002@example.com", "Citizen 002", false, "Mailing block 2, Singapore", "S0000002A", "+6590000002", "Residential block 2, Singapore", "singpass-subject-002", null, null },
                    { 3, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1993, 3, 4), null, "citizen003@example.com", "Citizen 003", false, "Mailing block 3, Singapore", "S0000003A", "+6590000003", "Residential block 3, Singapore", "singpass-subject-003", null, null },
                    { 4, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1994, 4, 5), null, "citizen004@example.com", "Citizen 004", false, "Mailing block 4, Singapore", "S0000004A", "+6590000004", "Residential block 4, Singapore", "singpass-subject-004", null, null },
                    { 5, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1995, 5, 6), null, "citizen005@example.com", "Citizen 005", false, "Mailing block 5, Singapore", "S0000005A", "+6590000005", "Residential block 5, Singapore", "singpass-subject-005", null, null },
                    { 6, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1996, 6, 7), null, "citizen006@example.com", "Citizen 006", false, "Mailing block 6, Singapore", "S0000006A", "+6590000006", "Residential block 6, Singapore", "singpass-subject-006", null, null },
                    { 7, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1997, 7, 8), null, "citizen007@example.com", "Citizen 007", false, "Mailing block 7, Singapore", "S0000007A", "+6590000007", "Residential block 7, Singapore", "singpass-subject-007", null, null },
                    { 8, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1998, 8, 9), null, "citizen008@example.com", "Citizen 008", false, "Mailing block 8, Singapore", "S0000008A", "+6590000008", "Residential block 8, Singapore", "singpass-subject-008", null, null },
                    { 9, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(1999, 9, 10), null, "citizen009@example.com", "Citizen 009", false, "Mailing block 9, Singapore", "S0000009A", "+6590000009", "Residential block 9, Singapore", "singpass-subject-009", null, null },
                    { 10, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 10, 11), null, "citizen010@example.com", "Citizen 010", false, "Mailing block 10, Singapore", "S0000010A", "+6590000010", "Residential block 10, Singapore", "singpass-subject-010", null, null }
                });

            migrationBuilder.InsertData(
                table: "OutboxMessage",
                columns: new[] { "Id", "OccurredAt", "PayloadJson", "RetryCount", "Status", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 1, 0, 0, DateTimeKind.Utc), "{\"messageId\":1}", 1, 1, "AuditProjection" },
                    { 2, new DateTime(2026, 1, 1, 0, 2, 0, 0, DateTimeKind.Utc), "{\"messageId\":2}", 2, 2, "EmailNotification" },
                    { 3, new DateTime(2026, 1, 1, 0, 3, 0, 0, DateTimeKind.Utc), "{\"messageId\":3}", 0, 1, "AuditProjection" },
                    { 4, new DateTime(2026, 1, 1, 0, 4, 0, 0, DateTimeKind.Utc), "{\"messageId\":4}", 1, 2, "EmailNotification" },
                    { 5, new DateTime(2026, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc), "{\"messageId\":5}", 2, 3, "AuditProjection" },
                    { 6, new DateTime(2026, 1, 1, 0, 6, 0, 0, DateTimeKind.Utc), "{\"messageId\":6}", 0, 2, "EmailNotification" },
                    { 7, new DateTime(2026, 1, 1, 0, 7, 0, 0, DateTimeKind.Utc), "{\"messageId\":7}", 1, 1, "AuditProjection" },
                    { 8, new DateTime(2026, 1, 1, 0, 8, 0, 0, DateTimeKind.Utc), "{\"messageId\":8}", 2, 2, "EmailNotification" },
                    { 9, new DateTime(2026, 1, 1, 0, 9, 0, 0, DateTimeKind.Utc), "{\"messageId\":9}", 0, 1, "AuditProjection" },
                    { 10, new DateTime(2026, 1, 1, 0, 10, 0, 0, DateTimeKind.Utc), "{\"messageId\":10}", 1, 3, "EmailNotification" }
                });

            migrationBuilder.InsertData(
                table: "TopupRule",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "RuleName", "Status", "TopupAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 001", 1, 110m, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 002", 1, 120m, null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 003", 1, 130m, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 004", 1, 140m, null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 005", 2, 150m, null, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 006", 1, 160m, null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 007", 1, 170m, null, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 008", 1, 180m, null, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 009", 1, 190m, null, null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Top-up Rule 010", 2, 200m, null, null }
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
                table: "OtpVerification",
                columns: new[] { "Id", "AuthAccountId", "CreatedAt", "CreatedBy", "DeletedAt", "ExpiresAt", "FailedAttemptCount", "IsDeleted", "OtpHash", "SessionId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 11, 0, 0, DateTimeKind.Utc), 1, false, "otp-hash-001", "otp-session-001", null, null },
                    { 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 12, 0, 0, DateTimeKind.Utc), 2, false, "otp-hash-002", "otp-session-002", null, null },
                    { 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 13, 0, 0, DateTimeKind.Utc), 0, false, "otp-hash-003", "otp-session-003", null, null },
                    { 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 14, 0, 0, DateTimeKind.Utc), 1, false, "otp-hash-004", "otp-session-004", null, null },
                    { 5, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 15, 0, 0, DateTimeKind.Utc), 2, false, "otp-hash-005", "otp-session-005", null, null },
                    { 6, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 16, 0, 0, DateTimeKind.Utc), 0, false, "otp-hash-006", "otp-session-006", null, null },
                    { 7, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 17, 0, 0, DateTimeKind.Utc), 1, false, "otp-hash-007", "otp-session-007", null, null },
                    { 8, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 18, 0, 0, DateTimeKind.Utc), 2, false, "otp-hash-008", "otp-session-008", null, null },
                    { 9, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 19, 0, 0, DateTimeKind.Utc), 0, false, "otp-hash-009", "otp-session-009", null, null },
                    { 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2026, 1, 1, 0, 20, 0, 0, DateTimeKind.Utc), 1, false, "otp-hash-010", "otp-session-010", null, null }
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
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "00000000-0000-0000-1ece-baa24fa8003c", null, null },
                    { 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "azure-object-002", null, null },
                    { 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, "azure-object-003", null, null },
                    { 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-004", null, null },
                    { 5, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-005", null, null },
                    { 6, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-006", null, null },
                    { 7, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-007", null, null },
                    { 8, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-008", null, null },
                    { 9, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-009", null, null },
                    { 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-010", null, null }
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
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 1, 1, null, null, 19m, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 2, null, null, 20m, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, 1, 3, null, null, null, "Enrolled" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 4, null, null, 22m, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 1, 5, null, null, 23m, null },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, 4, 6, null, null, null, "Enrolled" },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, false, 1, 7, null, null, 25m, null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 8, null, null, 26m, null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, false, 1, 9, null, null, null, "Enrolled" },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, false, 4, 10, null, null, 28m, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AuthAccountId", "CitizenId", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Role", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, null, null },
                    { 2, 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, null, null },
                    { 3, 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 3, null, null },
                    { 4, 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 5, 5, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 6, 6, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 7, 7, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 8, 8, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 9, 9, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null },
                    { 10, 10, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, null, null }
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
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "FullName", "IsDeleted", "StaffCode", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin001@example.com", "Admin Profile 001", false, "STAFF-001", null, null, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin002@example.com", "Admin Profile 002", false, "STAFF-002", null, null, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin003@example.com", "Admin Profile 003", false, "STAFF-003", null, null, 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin004@example.com", "Admin Profile 004", false, "STAFF-004", null, null, 4 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin005@example.com", "Admin Profile 005", false, "STAFF-005", null, null, 5 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin006@example.com", "Admin Profile 006", false, "STAFF-006", null, null, 6 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin007@example.com", "Admin Profile 007", false, "STAFF-007", null, null, 7 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin008@example.com", "Admin Profile 008", false, "STAFF-008", null, null, 8 },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin009@example.com", "Admin Profile 009", false, "STAFF-009", null, null, 9 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin010@example.com", "Admin Profile 010", false, "STAFF-010", null, null, 10 }
                });

            migrationBuilder.InsertData(
                table: "AuditLog",
                columns: new[] { "Id", "Action", "ActorUserId", "Category", "IpAddress", "OccurredAt", "PayloadJson" },
                values: new object[,]
                {
                    { 1, "AdminLoginSucceeded", 1, 4, "127.0.0.1", new DateTime(2026, 1, 1, 0, 1, 0, 0, DateTimeKind.Utc), "{\"seedId\":1}" },
                    { 2, "CreditTransactionCreated", 2, 5, "127.0.0.2", new DateTime(2026, 1, 1, 0, 2, 0, 0, DateTimeKind.Utc), "{\"seedId\":2}" },
                    { 3, "AdminLoginSucceeded", 3, 3, "127.0.0.3", new DateTime(2026, 1, 1, 0, 3, 0, 0, DateTimeKind.Utc), "{\"seedId\":3}" },
                    { 4, "CreditTransactionCreated", 4, 5, "127.0.0.4", new DateTime(2026, 1, 1, 0, 4, 0, 0, DateTimeKind.Utc), "{\"seedId\":4}" },
                    { 5, "AdminLoginSucceeded", 5, 4, "127.0.0.5", new DateTime(2026, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc), "{\"seedId\":5}" },
                    { 6, "CreditTransactionCreated", 6, 3, "127.0.0.6", new DateTime(2026, 1, 1, 0, 6, 0, 0, DateTimeKind.Utc), "{\"seedId\":6}" },
                    { 7, "AdminLoginSucceeded", 7, 4, "127.0.0.7", new DateTime(2026, 1, 1, 0, 7, 0, 0, DateTimeKind.Utc), "{\"seedId\":7}" },
                    { 8, "CreditTransactionCreated", 8, 5, "127.0.0.8", new DateTime(2026, 1, 1, 0, 8, 0, 0, DateTimeKind.Utc), "{\"seedId\":8}" },
                    { 9, "AdminLoginSucceeded", 9, 3, "127.0.0.9", new DateTime(2026, 1, 1, 0, 9, 0, 0, DateTimeKind.Utc), "{\"seedId\":9}" },
                    { 10, "CreditTransactionCreated", 10, 5, "127.0.0.10", new DateTime(2026, 1, 1, 0, 10, 0, 0, DateTimeKind.Utc), "{\"seedId\":10}" }
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
                    { 11, 55m, 1855m, 1750m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 011", 1, 1, false, new Guid("00000000-0000-0000-0000-000000000011"), 4, null, null },
                    { 12, 60m, 1910m, 1800m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 012", 1, 2, false, new Guid("00000000-0000-0000-0000-000000000012"), 4, null, null },
                    { 13, 65m, 1965m, 1850m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 013", 1, 3, false, new Guid("00000000-0000-0000-0000-000000000013"), 4, null, null },
                    { 14, 70m, 2020m, 1900m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 014", 1, 4, false, new Guid("00000000-0000-0000-0000-000000000014"), 4, null, null },
                    { 15, 75m, 2075m, 1950m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 015", 1, 5, false, new Guid("00000000-0000-0000-0000-000000000015"), 4, null, null },
                    { 16, 80m, 2130m, 2000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 016", 1, 6, false, new Guid("00000000-0000-0000-0000-000000000016"), 4, null, null },
                    { 17, 85m, 2185m, 2050m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 017", 1, 7, false, new Guid("00000000-0000-0000-0000-000000000017"), 4, null, null },
                    { 18, 90m, 2240m, 2100m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 018", 1, 8, false, new Guid("00000000-0000-0000-0000-000000000018"), 4, null, null },
                    { 19, 95m, 2295m, 2150m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 019", 1, 9, false, new Guid("00000000-0000-0000-0000-000000000019"), 4, null, null },
                    { 20, 100m, 2350m, 2200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seed adhoc transaction 020", 1, 10, false, new Guid("00000000-0000-0000-0000-000000000020"), 4, null, null }
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
                filter: "\"IsDeleted\" = 0 AND \"Nric\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_SingpassSubjectId",
                table: "Citizen",
                column: "SingpassSubjectId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"SingpassSubjectId\" IS NOT NULL");

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
                filter: "\"IsDeleted\" = 0 AND \"TransactionCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EducationCreditTransaction_Type",
                table: "EducationCreditTransaction",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_AuthAccountId",
                table: "OtpVerification",
                column: "AuthAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_ExpiresAt",
                table: "OtpVerification",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_SessionId",
                table: "OtpVerification",
                column: "SessionId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"SessionId\" IS NOT NULL");

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
                name: "IX_SsoIdentity_AuthAccountId",
                table: "SsoIdentity",
                column: "AuthAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SsoIdentity_Provider",
                table: "SsoIdentity",
                column: "Provider");

            migrationBuilder.CreateIndex(
                name: "IX_SsoIdentity_Provider_ProviderUserId",
                table: "SsoIdentity",
                columns: new[] { "Provider", "ProviderUserId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Provider\" IS NOT NULL AND \"ProviderUserId\" IS NOT NULL");

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
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "OtpVerification");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "SsoIdentity");

            migrationBuilder.DropTable(
                name: "TopupBatchTargetTransaction");

            migrationBuilder.DropTable(
                name: "TopupRuleCondition");

            migrationBuilder.DropTable(
                name: "AdhocTopupBatchTarget");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "EducationCreditTransaction");

            migrationBuilder.DropTable(
                name: "TopupBatchTarget");

            migrationBuilder.DropTable(
                name: "AdhocTopupBatch");

            migrationBuilder.DropTable(
                name: "AuthAccount");

            migrationBuilder.DropTable(
                name: "EducationAccount");

            migrationBuilder.DropTable(
                name: "TopupBatch");

            migrationBuilder.DropTable(
                name: "Citizen");

            migrationBuilder.DropTable(
                name: "TopupRule");
        }
    }
}
