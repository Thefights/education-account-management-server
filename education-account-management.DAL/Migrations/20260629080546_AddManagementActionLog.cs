using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educationaccountmanagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddManagementActionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManagementActionLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    PreviousStatus = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    NewStatus = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ActorUserId = table.Column<int>(type: "int", nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    MetadataJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementActionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagementActionLog_User_ActorUserId",
                        column: x => x.ActorUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_Action",
                table: "ManagementActionLog",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_ActorUserId",
                table: "ManagementActionLog",
                column: "ActorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_BatchId",
                table: "ManagementActionLog",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_EntityId",
                table: "ManagementActionLog",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_EntityType",
                table: "ManagementActionLog",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementActionLog_OccurredAt",
                table: "ManagementActionLog",
                column: "OccurredAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagementActionLog");
        }
    }
}
