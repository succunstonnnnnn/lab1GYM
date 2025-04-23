using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymInfrastructure.Migrations
{
    public partial class AddProgressTrackingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BodyParameters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Weight = table.Column<float>(nullable: false),
                    Circumferences = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyParameters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    PhotoPath = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgressHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgressTrackings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Circumferences = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressTrackings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_BodyParameters_UserId", table: "BodyParameters", column: "UserId");
            migrationBuilder.CreateIndex(name: "IX_PhotoEntries_UserId", table: "PhotoEntries", column: "UserId");
            migrationBuilder.CreateIndex(name: "IX_ProgressHistories_UserId", table: "ProgressHistories", column: "UserId");
            migrationBuilder.CreateIndex(name: "IX_ProgressTrackings_UserId", table: "ProgressTrackings", column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BodyParameters");
            migrationBuilder.DropTable(name: "PhotoEntries");
            migrationBuilder.DropTable(name: "ProgressHistories");
            migrationBuilder.DropTable(name: "ProgressTrackings");
        }
    }
}
