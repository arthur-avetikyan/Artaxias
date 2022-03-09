using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class userreviewrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Review",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Review_CreatedByUserId",
                table: "Review",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_User_CreatedByUserId",
                table: "Review",
                column: "CreatedByUserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_User_CreatedByUserId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_CreatedByUserId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Review");
        }
    }
}
