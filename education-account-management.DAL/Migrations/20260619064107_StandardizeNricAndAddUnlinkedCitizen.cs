using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class StandardizeNricAndAddUnlinkedCitizen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nric",
                value: "S0000001I");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nric",
                value: "S0000002G");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nric",
                value: "S0000003E");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 4,
                column: "Nric",
                value: "S0000004C");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 5,
                column: "Nric",
                value: "S0000005A");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 6,
                column: "Nric",
                value: "S0000006Z");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 7,
                column: "Nric",
                value: "S0000007H");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 8,
                column: "Nric",
                value: "S0000008F");

            migrationBuilder.InsertData(
                table: "Citizen",
                columns: new[] { "Id", "CitizenshipStatus", "CreatedAt", "CreatedBy", "DateOfBirth", "DeletedAt", "Email", "FullName", "IsDeleted", "MailingAddress", "Nric", "PhoneNumber", "ResidentialAddress", "SchoolingStatus", "SingpassSubjectId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 16, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 1, 16), null, "unlinked.citizen@example.com", "Unlinked Test Citizen", false, "16 Test Avenue, Singapore", "S0000016G", "+6590000016", "16 Test Avenue, Singapore", "Not Enrolled", "singpass-subject-016-unlinked", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nric",
                value: "T0000001E");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nric",
                value: "T0000002C");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nric",
                value: "T0000003A");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 4,
                column: "Nric",
                value: "T0000004Z");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 5,
                column: "Nric",
                value: "T0000005H");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 6,
                column: "Nric",
                value: "T0000006F");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 7,
                column: "Nric",
                value: "T0000007D");

            migrationBuilder.UpdateData(
                table: "AdminProfile",
                keyColumn: "Id",
                keyValue: 8,
                column: "Nric",
                value: "T0000008B");
        }
    }
}
