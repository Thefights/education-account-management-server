using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Migrations
{
    /// <inheritdoc />
    public partial class A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdText = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false),
                    LockedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailWhitelist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailWhitelist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailWhitelistSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailWhitelistSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MfaSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    EmailEnabled = table.Column<bool>(type: "bit", nullable: false),
                    SmsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MfaSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttemptCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextAttemptAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastError = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtpVerification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthAccountId = table.Column<int>(type: "int", nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    DeliveryMethod = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    OtpHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FailedAttemptCount = table.Column<int>(type: "int", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvalidatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpVerification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtpVerification_AuthAccount_AuthAccountId",
                        column: x => x.AuthAccountId,
                        principalTable: "AuthAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthAccountId = table.Column<int>(type: "int", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RequestedByIp = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetToken_AuthAccount_AuthAccountId",
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
                    AuthAccountId = table.Column<int>(type: "int", nullable: false),
                    ReplacedByRefreshTokenId = table.Column<int>(type: "int", nullable: true),
                    TokenHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StaySignedIn = table.Column<bool>(type: "bit", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_RefreshToken_RefreshToken_ReplacedByRefreshTokenId",
                        column: x => x.ReplacedByRefreshTokenId,
                        principalTable: "RefreshToken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SocialLogin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthAccountId = table.Column<int>(type: "int", nullable: false),
                    Provider = table.Column<int>(type: "int", nullable: false),
                    ProviderUserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProviderEmail = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    LinkedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialLogin_AuthAccount_AuthAccountId",
                        column: x => x.AuthAccountId,
                        principalTable: "AuthAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthAccountId = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActorUserId = table.Column<int>(type: "int", nullable: true),
                    ActorFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActorUserIdText = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Object = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "UserFavoriteProduct",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteProduct", x => new { x.UserId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteProduct_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProductAssignment",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RoleInProduct = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProductAssignment", x => new { x.UserId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_UserProductAssignment_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProductAssignment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuthAccount",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "Email", "FailedLoginCount", "IsDeleted", "LastLoginAt", "LockedUntil", "ModificationDate", "ModifiedBy", "PasswordHash", "Status", "UserIdText" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "phuckhang1088@gmail.com", 0, false, null, null, null, null, "$2a$12$S8UUGws0L8l3mowPuKUsiOMNSgSQR4k9Jzic1gRdvXVKDTUvpxYty", 1, "Sterling" },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "binh.tran@example.com", 0, false, null, null, null, null, null, 1, "user-002" },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "chau.le@example.com", 1, false, null, null, null, null, null, 1, "user-003" },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "dung.pham@example.com", 0, false, null, null, null, null, null, 1, "user-004" },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "giang.hoang@example.com", 0, false, null, null, null, null, null, 1, "user-005" },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "hai.dang@example.com", 0, false, null, null, null, null, null, 2, "user-006" },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "lan.bui@example.com", 2, false, null, null, null, null, null, 1, "user-007" },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "minh.vo@example.com", 0, false, null, null, null, null, null, 1, "user-008" },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "ngan.do@example.com", 0, false, null, null, null, null, null, 1, "user-009" },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "phuc.ngo@example.com", 0, false, null, null, null, null, null, 2, "user-010" }
                });

            migrationBuilder.InsertData(
                table: "EmailWhitelist",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "Value" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "avepoint.com" },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "example.com" },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "admin@example.com" },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "manager@example.com" },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "support@example.com" },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "partner.example.com" },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "security@example.com" },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "ops@example.com" },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "contoso.com" },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "phuckhang1088@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "EmailWhitelistSetting",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "IsEnabled", "ModificationDate", "ModifiedBy" },
                values: new object[] { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, null, null });

            migrationBuilder.InsertData(
                table: "MfaSetting",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "EmailEnabled", "IsDeleted", "IsEnabled", "ModificationDate", "ModifiedBy", "SmsEnabled" },
                values: new object[] { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, false, null, null, false });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "Description", "ImageUrl", "IsDeleted", "ModificationDate", "ModifiedBy", "Name", "Status" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Customer access portal", null, false, null, null, "MOS Portal", 1 },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Administrative management console", null, false, null, null, "MOS Admin", 1 },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Analytics and reporting workspace", null, false, null, null, "MOS Analytics", 1 },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Billing and subscription management", null, false, null, null, "MOS Billing", 1 },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Support ticket management", null, false, null, null, "MOS Support", 1 },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Identity and access management", null, false, null, null, "MOS Identity", 1 },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Document management workspace", null, false, null, null, "MOS Docs", 1 },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Workflow automation module", null, false, null, null, "MOS Workflow", 1 },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Scheduled reporting module", null, false, null, null, "MOS Reports", 2 },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Audit trail review module", null, false, null, null, "MOS Audit", 1 }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "Admin" },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "TenantUser" }
                });

            migrationBuilder.InsertData(
                table: "SocialLogin",
                columns: new[] { "Id", "AuthAccountId", "EmailVerified", "LinkedAt", "Provider", "ProviderEmail", "ProviderUserId" },
                values: new object[,]
                {
                    { 1, 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "an.nguyen@example.com", "google-user-001" },
                    { 2, 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "binh.tran@example.com", "google-user-002" },
                    { 3, 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "chau.le@example.com", "ms-user-003" },
                    { 4, 4, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "dung.pham@example.com", "fb-user-004" },
                    { 5, 5, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "giang.hoang@example.com", "google-user-005" },
                    { 6, 6, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "hai.dang@example.com", "ms-user-006" },
                    { 7, 7, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "lan.bui@example.com", "google-user-007" },
                    { 8, 8, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "minh.vo@example.com", "fb-user-008" },
                    { 9, 9, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "ngan.do@example.com", "ms-user-009" },
                    { 10, 10, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "phuc.ngo@example.com", "google-user-010" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AuthAccountId", "CreatedBy", "CreationDate", "DeletionDate", "FullName", "Gender", "ImageUrl", "IsDeleted", "ModificationDate", "ModifiedBy", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nguyen Van An", 2, null, false, null, null, "+84901234567" },
                    { 2, 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tran Thi Binh", 3, null, false, null, null, "+84901234568" },
                    { 3, 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Le Minh Chau", 4, null, false, null, null, "+84901234569" },
                    { 4, 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Pham Quoc Dung", 2, null, false, null, null, "+84901234570" },
                    { 5, 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hoang Mai Giang", 3, null, false, null, null, "+84901234571" },
                    { 6, 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dang Thanh Hai", 2, null, false, null, null, "+84901234572" },
                    { 7, 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bui Ngoc Lan", 3, null, false, null, null, "+84901234573" },
                    { 8, 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vo Anh Minh", 2, null, false, null, null, "+84901234574" },
                    { 9, 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Do Thuy Ngan", 3, null, false, null, null, "+84901234575" },
                    { 10, 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ngo Duc Phuc", 2, null, false, null, null, "+84901234576" }
                });

            migrationBuilder.InsertData(
                table: "AuditLog",
                columns: new[] { "Id", "Action", "ActorFullName", "ActorUserId", "ActorUserIdText", "Category", "CreatedAt", "IpAddress", "Object" },
                values: new object[,]
                {
                    { 1, 4, "An Nguyen", 1, "admin", 1, new DateTime(2026, 1, 1, 0, 1, 0, 0, DateTimeKind.Utc), "127.0.0.1", "AuthAccount:1:admin" },
                    { 2, 100, "An Nguyen", 1, "admin", 2, new DateTime(2026, 1, 1, 0, 2, 0, 0, DateTimeKind.Utc), "127.0.0.1", "AuthAccount:2:tenant.user" },
                    { 3, 200, "An Nguyen", 1, "admin", 3, new DateTime(2026, 1, 1, 0, 3, 0, 0, DateTimeKind.Utc), "127.0.0.1", "MfaSetting:1" },
                    { 4, 202, "An Nguyen", 1, "admin", 4, new DateTime(2026, 1, 1, 0, 4, 0, 0, DateTimeKind.Utc), "127.0.0.1", "EmailWhitelist:10" },
                    { 5, 300, "An Nguyen", 1, "admin", 5, new DateTime(2026, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc), "127.0.0.1", "Product:10:MOS Audit" },
                    { 6, 400, "Binh Tran", 2, "tenant.user", 6, new DateTime(2026, 1, 1, 0, 6, 0, 0, DateTimeKind.Utc), "127.0.0.1", "Product:2:MOS Admin" },
                    { 7, 500, "An Nguyen", 1, "admin", 7, new DateTime(2026, 1, 1, 0, 7, 0, 0, DateTimeKind.Utc), "127.0.0.1", "AuditLog:Pagination" }
                });

            migrationBuilder.InsertData(
                table: "UserFavoriteProduct",
                columns: new[] { "ProductId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 8, 8 },
                    { 10, 9 },
                    { 1, 10 }
                });

            migrationBuilder.InsertData(
                table: "UserProductAssignment",
                columns: new[] { "ProductId", "UserId", "RoleInProduct" },
                values: new object[,]
                {
                    { 1, 3, 1 },
                    { 2, 4, 2 },
                    { 3, 5, 3 },
                    { 4, 6, 1 },
                    { 5, 8, 2 },
                    { 10, 9, 3 },
                    { 1, 10, 1 }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 6 },
                    { 2, 7 },
                    { 2, 8 },
                    { 2, 9 },
                    { 2, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Action",
                table: "AuditLog",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ActorUserId",
                table: "AuditLog",
                column: "ActorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ActorUserIdText",
                table: "AuditLog",
                column: "ActorUserIdText");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Category",
                table: "AuditLog",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Category_Action_CreatedAt",
                table: "AuditLog",
                columns: new[] { "Category", "Action", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_CreatedAt",
                table: "AuditLog",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Object",
                table: "AuditLog",
                column: "Object");

            migrationBuilder.CreateIndex(
                name: "IX_AuthAccount_Email",
                table: "AuthAccount",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_AuthAccount_Status",
                table: "AuthAccount",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AuthAccount_UserIdText",
                table: "AuthAccount",
                column: "UserIdText",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"UserIdText\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmailWhitelist_Value",
                table: "EmailWhitelist",
                column: "Value",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Value\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_AuthAccountId",
                table: "OtpVerification",
                column: "AuthAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_DeliveryMethod",
                table: "OtpVerification",
                column: "DeliveryMethod");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_ExpiresAt",
                table: "OtpVerification",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_InvalidatedAt",
                table: "OtpVerification",
                column: "InvalidatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_Purpose",
                table: "OtpVerification",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_SessionId",
                table: "OtpVerification",
                column: "SessionId",
                unique: true,
                filter: "\"SessionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_Target",
                table: "OtpVerification",
                column: "Target");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerification_UsedAt",
                table: "OtpVerification",
                column: "UsedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_AuthAccountId",
                table: "PasswordResetToken",
                column: "AuthAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_ExpiresAt",
                table: "PasswordResetToken",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_TokenHash",
                table: "PasswordResetToken",
                column: "TokenHash",
                unique: true,
                filter: "\"TokenHash\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_UsedAt",
                table: "PasswordResetToken",
                column: "UsedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Name",
                table: "Product",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Status",
                table: "Product",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_AuthAccountId",
                table: "RefreshToken",
                column: "AuthAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ExpiresAt",
                table: "RefreshToken",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ReplacedByRefreshTokenId",
                table: "RefreshToken",
                column: "ReplacedByRefreshTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_RevokedAt",
                table: "RefreshToken",
                column: "RevokedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_TokenHash",
                table: "RefreshToken",
                column: "TokenHash",
                unique: true,
                filter: "\"TokenHash\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SocialLogin_AuthAccountId_Provider",
                table: "SocialLogin",
                columns: new[] { "AuthAccountId", "Provider" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialLogin_Provider_ProviderUserId",
                table: "SocialLogin",
                columns: new[] { "Provider", "ProviderUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialLogin_ProviderEmail",
                table: "SocialLogin",
                column: "ProviderEmail");

            migrationBuilder.CreateIndex(
                name: "IX_User_AuthAccountId",
                table: "User",
                column: "AuthAccountId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"AuthAccountId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_PhoneNumber",
                table: "User",
                column: "PhoneNumber",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PhoneNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteProduct_ProductId",
                table: "UserFavoriteProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProductAssignment_ProductId",
                table: "UserProductAssignment",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "EmailWhitelist");

            migrationBuilder.DropTable(
                name: "EmailWhitelistSetting");

            migrationBuilder.DropTable(
                name: "MfaSetting");

            migrationBuilder.DropTable(
                name: "OtpVerification");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "PasswordResetToken");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "SocialLogin");

            migrationBuilder.DropTable(
                name: "UserFavoriteProduct");

            migrationBuilder.DropTable(
                name: "UserProductAssignment");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AuthAccount");
        }
    }
}
