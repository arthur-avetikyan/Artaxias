using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class deletedemailconfirmationfromuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                schema: "dbo",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ConfirmationCode",
                schema: "dbo",
                table: "User",
                type: "binary(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: new byte[] { });

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                schema: "dbo",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
