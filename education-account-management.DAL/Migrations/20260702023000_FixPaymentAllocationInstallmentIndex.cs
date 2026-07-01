using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Persistence.SqlServer;

#nullable disable

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260702023000_FixPaymentAllocationInstallmentIndex")]
    public partial class FixPaymentAllocationInstallmentIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId",
                table: "PaymentAllocation");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId",
                table: "PaymentAllocation",
                columns: new[] { "PaymentId", "ChargeId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"ChargeInstallmentId\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId",
                table: "PaymentAllocation");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId",
                table: "PaymentAllocation",
                columns: new[] { "PaymentId", "ChargeId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PaymentId\" IS NOT NULL AND \"ChargeId\" IS NOT NULL");
        }
    }
}
