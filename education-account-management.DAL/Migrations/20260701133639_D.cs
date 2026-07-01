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
            migrationBuilder.DropColumn(
                name: "IsPerComponent",
                table: "FasScheme");

            migrationBuilder.DropColumn(
                name: "SubsidyType",
                table: "FasScheme");

            migrationBuilder.AddColumn<int>(
                name: "SubsidyType",
                table: "FasSchemeTier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 2,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 3,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 4,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 5,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 6,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 7,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 8,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 9,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 10,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 11,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 12,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 13,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 14,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 15,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 16,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 17,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 18,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 19,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 20,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 21,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 22,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 23,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 24,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 25,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 26,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 27,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 28,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 29,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 30,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 31,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 32,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 33,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 34,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 35,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 36,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 37,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 38,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 39,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 40,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 41,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 42,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 43,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 44,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 45,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 46,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 47,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 48,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 49,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 50,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 51,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 52,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 53,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 54,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 55,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 56,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 57,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 58,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 59,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 60,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 61,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 62,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 63,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 64,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 65,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 66,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 67,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 68,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 69,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 70,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 71,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 72,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 73,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 74,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 75,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 76,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 77,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 78,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 79,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 80,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 81,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 82,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 83,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 84,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 85,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 86,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 87,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 88,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 89,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 90,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 91,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 92,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 93,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 94,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 95,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 96,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 97,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 98,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 99,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 100,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 101,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 102,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 103,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 104,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 105,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 106,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 107,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 108,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 109,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 110,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 111,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 112,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 113,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 114,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 115,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 116,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 117,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 118,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 119,
                column: "SubsidyType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FasSchemeTier",
                keyColumn: "Id",
                keyValue: 120,
                column: "SubsidyType",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubsidyType",
                table: "FasSchemeTier");

            migrationBuilder.AddColumn<bool>(
                name: "IsPerComponent",
                table: "FasScheme",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SubsidyType",
                table: "FasScheme",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });

            migrationBuilder.UpdateData(
                table: "FasScheme",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "IsPerComponent", "SubsidyType" },
                values: new object[] { false, 2 });
        }
    }
}
