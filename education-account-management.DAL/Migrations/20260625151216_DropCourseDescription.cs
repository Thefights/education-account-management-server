using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DropCourseDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Course");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Course",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
