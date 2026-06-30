using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addNewPaymentConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId",
                table: "PaymentAllocation");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EducationCreditTransaction_BalanceEquation",
                table: "EducationCreditTransaction");

            migrationBuilder.InsertData(
                table: "SsoIdentity",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Provider", "ProviderUserId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[] { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, "singpass-subject-009", null, null, 9 });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CitizenId", "Role" },
                values: new object[] { 9, 4 });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId_ChargeInstallmentId",
                table: "PaymentAllocation",
                columns: new[] { "PaymentId", "ChargeId", "ChargeInstallmentId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PaymentId\" IS NOT NULL AND \"ChargeId\" IS NOT NULL AND \"ChargeInstallmentId\" IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_EducationCreditTransaction_BalanceEquation",
                table: "EducationCreditTransaction",
                sql: "([Direction] = 1 AND [BalanceAfter] = [BalanceBefore] + [Amount]) OR ([Direction] = 2 AND [BalanceAfter] = [BalanceBefore] - [Amount]) OR ([Direction] = 3 AND [BalanceAfter] = [BalanceBefore])");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId_ChargeInstallmentId",
                table: "PaymentAllocation");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EducationCreditTransaction_BalanceEquation",
                table: "EducationCreditTransaction");

            migrationBuilder.DeleteData(
                table: "SsoIdentity",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CitizenId", "Role" },
                values: new object[] { null, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId",
                table: "PaymentAllocation",
                columns: new[] { "PaymentId", "ChargeId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PaymentId\" IS NOT NULL AND \"ChargeId\" IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_EducationCreditTransaction_BalanceEquation",
                table: "EducationCreditTransaction",
                sql: "([Direction] = 1 AND [BalanceAfter] = [BalanceBefore] + [Amount]) OR ([Direction] = 2 AND [BalanceAfter] = [BalanceBefore] - [Amount])");
        }
    }
}
