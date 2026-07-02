using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class D : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Enrollment",
                keyColumn: "Id",
                keyValue: 1145,
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Enrollment",
                keyColumn: "Id",
                keyValue: 1203,
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Enrollment",
                keyColumn: "Id",
                keyValue: 1261,
                column: "Status",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Enrollment",
                keyColumn: "Id",
                keyValue: 1145,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Enrollment",
                keyColumn: "Id",
                keyValue: 1203,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Enrollment",
                keyColumn: "Id",
                keyValue: 1261,
                column: "Status",
                value: 1);
        }
    }
}
