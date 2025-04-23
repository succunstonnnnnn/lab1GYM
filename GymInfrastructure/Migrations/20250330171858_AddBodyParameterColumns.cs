using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymInfrastructure.Migrations
{
    public partial class AddDetailedBodyMeasurements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TargetWeight",
                table: "BodyParameters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Waist",
                table: "BodyParameters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Chest",
                table: "BodyParameters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Thigh",
                table: "BodyParameters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Biceps",
                table: "BodyParameters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Calf",
                table: "BodyParameters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Glutes",
                table: "BodyParameters",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetWeight",
                table: "BodyParameters");

            migrationBuilder.DropColumn(
                name: "Waist",
                table: "BodyParameters");

            migrationBuilder.DropColumn(
                name: "Chest",
                table: "BodyParameters");

            migrationBuilder.DropColumn(
                name: "Thigh",
                table: "BodyParameters");

            migrationBuilder.DropColumn(
                name: "Biceps",
                table: "BodyParameters");

            migrationBuilder.DropColumn(
                name: "Calf",
                table: "BodyParameters");

            migrationBuilder.DropColumn(
                name: "Glutes",
                table: "BodyParameters");
        }
    }
}
