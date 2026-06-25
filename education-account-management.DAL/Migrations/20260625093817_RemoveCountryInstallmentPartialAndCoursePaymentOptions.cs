using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCountryInstallmentPartialAndCoursePaymentOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FasApplication_Country_ParentNationalityId",
                table: "FasApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_FasApplication_Country_StudentNationalityId",
                table: "FasApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_FasSchemeCondition_Country_CountryId",
                table: "FasSchemeCondition");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropIndex(
                name: "IX_FasSchemeCondition_CountryId",
                table: "FasSchemeCondition");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FasSchemeCondition_Value_By_Field",
                table: "FasSchemeCondition");

            migrationBuilder.DropIndex(
                name: "IX_FasApplication_ParentNationalityId",
                table: "FasApplication");

            migrationBuilder.DropIndex(
                name: "IX_FasApplication_StudentNationalityId",
                table: "FasApplication");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ChargeInstallment_Amounts",
                table: "ChargeInstallment");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "FasSchemeCondition");

            migrationBuilder.DropColumn(
                name: "ParentNationalityId",
                table: "FasApplication");

            migrationBuilder.DropColumn(
                name: "StudentNationalityId",
                table: "FasApplication");

            migrationBuilder.DropColumn(
                name: "AllowFullPayment",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "AllowInstallment12Months",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "AllowInstallment3Months",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "AllowInstallment6Months",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "ChargeInstallment");

            migrationBuilder.DropColumn(
                name: "RemainingAmount",
                table: "ChargeInstallment");

            migrationBuilder.AddColumn<string>(
                name: "ValueText",
                table: "FasSchemeCondition",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherNationalitySnapshot",
                table: "FasApplication",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherNationalitySnapshot",
                table: "FasApplication",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentNationalitySnapshot",
                table: "FasApplication",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Singapore", "Singapore", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Malaysia", "Malaysia", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Singapore", "Singapore", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Indonesia", "Indonesia", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Vietnam", "Vietnam", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Singapore", "Singapore", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Singapore", "Singapore", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Thailand", "Thailand", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Philippines", "Philippines", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "FatherNationalitySnapshot", "MotherNationalitySnapshot", "StudentNationalitySnapshot" },
                values: new object[] { "Singapore", "Singapore", "Singapore" });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Field", "ValueText" },
                values: new object[] { 6, null });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Field", "ValueText" },
                values: new object[] { 5, null });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 3,
                column: "ValueText",
                value: "Singapore");

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 4,
                column: "ValueText",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 5,
                column: "ValueText",
                value: "Singapore");

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Field", "ValueText" },
                values: new object[] { 6, null });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Field", "ValueText" },
                values: new object[] { 5, null });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 8,
                column: "ValueText",
                value: "Singapore");

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Field", "ValueText" },
                values: new object[] { 6, null });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 10,
                column: "ValueText",
                value: null);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FasSchemeCondition_Value_By_Field",
                table: "FasSchemeCondition",
                sql: "([Field] IN (1, 5, 6) AND [ValueNumber] IS NOT NULL AND [ValueText] IS NULL AND (([Operator] = 7 AND [ValueNumberTo] IS NOT NULL AND [ValueNumberTo] >= [ValueNumber]) OR ([Operator] <> 7 AND [ValueNumberTo] IS NULL))) OR ([Field] IN (2, 3, 4) AND [ValueText] IS NOT NULL AND [Operator] IN (1, 2) AND [ValueNumber] IS NULL AND [ValueNumberTo] IS NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ChargeInstallment_Amounts",
                table: "ChargeInstallment",
                sql: "[Amount] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_Nric",
                table: "AdminProfile",
                column: "Nric",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Nric\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_FasSchemeCondition_Value_By_Field",
                table: "FasSchemeCondition");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ChargeInstallment_Amounts",
                table: "ChargeInstallment");

            migrationBuilder.DropIndex(
                name: "IX_AdminProfile_Nric",
                table: "AdminProfile");

            migrationBuilder.DropColumn(
                name: "ValueText",
                table: "FasSchemeCondition");

            migrationBuilder.DropColumn(
                name: "FatherNationalitySnapshot",
                table: "FasApplication");

            migrationBuilder.DropColumn(
                name: "MotherNationalitySnapshot",
                table: "FasApplication");

            migrationBuilder.DropColumn(
                name: "StudentNationalitySnapshot",
                table: "FasApplication");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "FasSchemeCondition",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentNationalityId",
                table: "FasApplication",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentNationalityId",
                table: "FasApplication",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "AllowFullPayment",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowInstallment12Months",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowInstallment3Months",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowInstallment6Months",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "ChargeInstallment",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingAmount",
                table: "ChargeInstallment",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 120m, 0m });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 0m, 70m });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 0m, 80m });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 180m, 0m });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 0m, 100m });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 0m, 110m });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 0m, 120m });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 0m, 130m });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 0m, 140m });

            migrationBuilder.UpdateData(
                table: "ChargeInstallment",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "PaidAmount", "RemainingAmount" },
                values: new object[] { 0m, 150m });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "DeletedAt", "IsActive", "IsDeleted", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "SG", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "Singapore", null, null },
                    { 2, "MY", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "Malaysia", null, null },
                    { 3, "ID", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "Indonesia", null, null },
                    { 4, "VN", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "Vietnam", null, null },
                    { 5, "TH", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "Thailand", null, null },
                    { 6, "PH", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "Philippines", null, null },
                    { 7, "CN", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "China", null, null },
                    { 8, "IN", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "India", null, null },
                    { 9, "AU", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "Australia", null, null },
                    { 10, "GB", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "United Kingdom", null, null }
                });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AllowFullPayment", "AllowInstallment12Months", "AllowInstallment3Months", "AllowInstallment6Months" },
                values: new object[] { true, false, true, true });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 3, 1 });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 4, 1 });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 5, 1 });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 6, 1 });

            migrationBuilder.UpdateData(
                table: "FasApplication",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ParentNationalityId", "StudentNationalityId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CountryId", "Field" },
                values: new object[] { null, 5 });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CountryId", "Field" },
                values: new object[] { null, 4 });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 3,
                column: "CountryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 4,
                column: "CountryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 5,
                column: "CountryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CountryId", "Field" },
                values: new object[] { null, 5 });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CountryId", "Field" },
                values: new object[] { null, 4 });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 8,
                column: "CountryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CountryId", "Field" },
                values: new object[] { null, 5 });

            migrationBuilder.UpdateData(
                table: "FasSchemeCondition",
                keyColumn: "Id",
                keyValue: 10,
                column: "CountryId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_FasSchemeCondition_CountryId",
                table: "FasSchemeCondition",
                column: "CountryId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FasSchemeCondition_Value_By_Field",
                table: "FasSchemeCondition",
                sql: "([Field] IN (1, 4, 5) AND [ValueNumber] IS NOT NULL AND [CountryId] IS NULL AND (([Operator] = 7 AND [ValueNumberTo] IS NOT NULL AND [ValueNumberTo] >= [ValueNumber]) OR ([Operator] <> 7 AND [ValueNumberTo] IS NULL))) OR ([Field] IN (2, 3) AND [CountryId] IS NOT NULL AND [Operator] IN (1, 2) AND [ValueNumber] IS NULL AND [ValueNumberTo] IS NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplication_ParentNationalityId",
                table: "FasApplication",
                column: "ParentNationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_FasApplication_StudentNationalityId",
                table: "FasApplication",
                column: "StudentNationalityId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ChargeInstallment_Amounts",
                table: "ChargeInstallment",
                sql: "[Amount] > 0 AND [PaidAmount] >= 0 AND [RemainingAmount] >= 0 AND [PaidAmount] <= [Amount] AND [RemainingAmount] = [Amount] - [PaidAmount]");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Code",
                table: "Country",
                column: "Code",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Code\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Country_IsActive",
                table: "Country",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Name",
                table: "Country",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FasApplication_Country_ParentNationalityId",
                table: "FasApplication",
                column: "ParentNationalityId",
                principalTable: "Country",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FasApplication_Country_StudentNationalityId",
                table: "FasApplication",
                column: "StudentNationalityId",
                principalTable: "Country",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FasSchemeCondition_Country_CountryId",
                table: "FasSchemeCondition",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
