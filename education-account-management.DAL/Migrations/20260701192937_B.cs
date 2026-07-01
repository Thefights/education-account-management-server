using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class B : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId",
                table: "PaymentAllocation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocation_PaymentId_ChargeId",
                table: "PaymentAllocation",
                columns: new[] { "PaymentId", "ChargeId" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"PaymentId\" IS NOT NULL AND \"ChargeId\" IS NOT NULL");
        }
    }
}
