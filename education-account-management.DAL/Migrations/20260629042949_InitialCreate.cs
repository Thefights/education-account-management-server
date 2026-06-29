using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxGrossHouseholdIncome",
                table: "FasSchemeTier",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 3,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 4,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 5,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 6,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 7,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 8,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 9,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 10,
                column: "MaxGrossHouseholdIncome",
                value: null);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FasSchemeTier_Amounts_NonNegative",
                table: "FasSchemeTier",
                sql: "([MaxPerCapitaIncome] IS NULL OR [MaxPerCapitaIncome] >= 0) AND ([MaxGrossHouseholdIncome] IS NULL OR [MaxGrossHouseholdIncome] >= 0) AND ([SubsidyValue] IS NULL OR [SubsidyValue] >= 0) AND ([CourseFeeSubsidyValue] IS NULL OR [CourseFeeSubsidyValue] >= 0) AND ([MiscFeeSubsidyValue] IS NULL OR [MiscFeeSubsidyValue] >= 0)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FasSchemeTier_DisplayOrder_NonNegative",
                table: "FasSchemeTier",
                sql: "[DisplayOrder] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_FasSchemeTier_Amounts_NonNegative",
                table: "FasSchemeTier");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FasSchemeTier_DisplayOrder_NonNegative",
                table: "FasSchemeTier");

            migrationBuilder.DropColumn(
                name: "MaxGrossHouseholdIncome",
                table: "FasSchemeTier");
        }
    }
}
