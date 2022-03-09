using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class addedstateentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Review");

            migrationBuilder.AddColumn<int>(
                name: "ReviewStateId",
                table: "ReviewerReviewee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DomainState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainState", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewerReviewee_ReviewStateId",
                table: "ReviewerReviewee",
                column: "ReviewStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewerReviewee_DomainState_ReviewStateId",
                table: "ReviewerReviewee",
                column: "ReviewStateId",
                principalTable: "DomainState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewerReviewee_DomainState_ReviewStateId",
                table: "ReviewerReviewee");

            migrationBuilder.DropTable(
                name: "DomainState");

            migrationBuilder.DropIndex(
                name: "IX_ReviewerReviewee_ReviewStateId",
                table: "ReviewerReviewee");

            migrationBuilder.DropColumn(
                name: "ReviewStateId",
                table: "ReviewerReviewee");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
