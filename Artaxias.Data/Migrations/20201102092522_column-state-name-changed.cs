using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class columnstatenamechanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "States",
                table: "Review");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Review",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Review");

            migrationBuilder.AddColumn<int>(
                name: "States",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
