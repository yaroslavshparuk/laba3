using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Invoices.DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWorks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkItemId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoryDetails",
                columns: table => new
                {
                    WorkItemId = table.Column<int>(type: "int", nullable: false),
                    RevisionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    RemainingWorkOldValue = table.Column<int>(type: "int", nullable: true),
                    RemainingWorkNewValue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryDetails", x => new { x.WorkItemId, x.RevisionDateTime });
                    table.ForeignKey(
                        name: "FK_HistoryDetails_WorkItems_WorkItemId",
                        column: x => x.WorkItemId,
                        principalTable: "WorkItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserWorks");

            migrationBuilder.DropTable(
                name: "WorkItems");
        }
    }
}
