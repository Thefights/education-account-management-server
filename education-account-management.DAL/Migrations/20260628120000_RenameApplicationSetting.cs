using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameApplicationSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_AiAssistantSetting_Singleton",
                table: "AiAssistantSetting");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AiAssistantSetting_TaxRate",
                table: "AiAssistantSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AiAssistantSetting",
                table: "AiAssistantSetting");

            migrationBuilder.RenameTable(
                name: "AiAssistantSetting",
                newName: "ApplicationSetting");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "ApplicationSetting",
                newName: "IsAiFeatureEnabled");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationSetting",
                table: "ApplicationSetting",
                column: "Id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ApplicationSetting_Singleton",
                table: "ApplicationSetting",
                sql: "[Id] = 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ApplicationSetting_TaxRate",
                table: "ApplicationSetting",
                sql: "[TaxRate] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ApplicationSetting_Singleton",
                table: "ApplicationSetting");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ApplicationSetting_TaxRate",
                table: "ApplicationSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationSetting",
                table: "ApplicationSetting");

            migrationBuilder.RenameColumn(
                name: "IsAiFeatureEnabled",
                table: "ApplicationSetting",
                newName: "IsEnabled");

            migrationBuilder.RenameTable(
                name: "ApplicationSetting",
                newName: "AiAssistantSetting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AiAssistantSetting",
                table: "AiAssistantSetting",
                column: "Id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AiAssistantSetting_Singleton",
                table: "AiAssistantSetting",
                sql: "[Id] = 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AiAssistantSetting_TaxRate",
                table: "AiAssistantSetting",
                sql: "[TaxRate] >= 0");
        }
    }
}
