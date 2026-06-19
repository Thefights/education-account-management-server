using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUnlinkedCitizenTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Citizen",
                columns: new[] { "Id", "CitizenshipStatus", "CreatedAt", "CreatedBy", "DateOfBirth", "DeletedAt", "Email", "FullName", "IsDeleted", "MailingAddress", "Nric", "PhoneNumber", "ResidentialAddress", "SchoolingStatus", "SingpassSubjectId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 17, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 1, 15), null, "unlinked.citizen017@example.com", "Unlinked Test Citizen 017", false, "17 Test Avenue, Singapore", "S0000017E", "+6590000017", "17 Test Avenue, Singapore", "Not Enrolled", "singpass-subject-017-unlinked", null, null },
                    { 18, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 2, 15), null, "unlinked.citizen018@example.com", "Unlinked Test Citizen 018", false, "18 Test Avenue, Singapore", "S0000018C", "+6590000018", "18 Test Avenue, Singapore", "Enrolled", "singpass-subject-018-unlinked", null, null },
                    { 19, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 3, 15), null, "unlinked.citizen019@example.com", "Unlinked Test Citizen 019", false, "19 Test Avenue, Singapore", "S0000019A", "+6590000019", "19 Test Avenue, Singapore", "Not Enrolled", "singpass-subject-019-unlinked", null, null },
                    { 20, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 4, 15), null, "unlinked.citizen020@example.com", "Unlinked Test Citizen 020", false, "20 Test Avenue, Singapore", "S0000020E", "+6590000020", "20 Test Avenue, Singapore", "Enrolled", "singpass-subject-020-unlinked", null, null },
                    { 21, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 5, 15), null, "unlinked.citizen021@example.com", "Unlinked Test Citizen 021", false, "21 Test Avenue, Singapore", "S0000021C", "+6590000021", "21 Test Avenue, Singapore", "Not Enrolled", "singpass-subject-021-unlinked", null, null },
                    { 22, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2000, 6, 15), null, "unlinked.citizen022@example.com", "Unlinked Test Citizen 022", false, "22 Test Avenue, Singapore", "S0000022A", "+6590000022", "22 Test Avenue, Singapore", "Enrolled", "singpass-subject-022-unlinked", null, null },
                    { 23, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2001, 7, 15), null, "unlinked.citizen023@example.com", "Unlinked Test Citizen 023", false, "23 Test Avenue, Singapore", "S0000023Z", "+6590000023", "23 Test Avenue, Singapore", "Not Enrolled", "singpass-subject-023-unlinked", null, null },
                    { 24, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2002, 8, 15), null, "unlinked.citizen024@example.com", "Unlinked Test Citizen 024", false, "24 Test Avenue, Singapore", "S0000024H", "+6590000024", "24 Test Avenue, Singapore", "Enrolled", "singpass-subject-024-unlinked", null, null },
                    { 25, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2003, 9, 15), null, "unlinked.citizen025@example.com", "Unlinked Test Citizen 025", false, "25 Test Avenue, Singapore", "S0000025F", "+6590000025", "25 Test Avenue, Singapore", "Not Enrolled", "singpass-subject-025-unlinked", null, null },
                    { 26, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2004, 10, 15), null, "unlinked.citizen026@example.com", "Unlinked Test Citizen 026", false, "26 Test Avenue, Singapore", "S0000026D", "+6590000026", "26 Test Avenue, Singapore", "Enrolled", "singpass-subject-026-unlinked", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Citizen",
                keyColumn: "Id",
                keyValue: 26);
        }
    }
}
