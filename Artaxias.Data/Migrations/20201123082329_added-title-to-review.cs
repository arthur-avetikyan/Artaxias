using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class addedtitletoreview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Review",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Review");
        }
    }
}
