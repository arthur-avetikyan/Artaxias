using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class renameddomainstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewerReviewee_DomainState_ReviewStateId",
                table: "ReviewerReviewee");

            migrationBuilder.DropIndex(
                name: "IX_ReviewerReviewee_ReviewStateId",
                table: "ReviewerReviewee");

            migrationBuilder.DropColumn(
                name: "ReviewStateId",
                table: "ReviewerReviewee");

            migrationBuilder.AddColumn<int>(
                name: "DomainStateId",
                table: "ReviewerReviewee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewerReviewee_DomainStateId",
                table: "ReviewerReviewee",
                column: "DomainStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewerReviewee_DomainState_DomainStateId",
                table: "ReviewerReviewee",
                column: "DomainStateId",
                principalTable: "DomainState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewerReviewee_DomainState_DomainStateId",
                table: "ReviewerReviewee");

            migrationBuilder.DropIndex(
                name: "IX_ReviewerReviewee_DomainStateId",
                table: "ReviewerReviewee");

            migrationBuilder.DropColumn(
                name: "DomainStateId",
                table: "ReviewerReviewee");

            migrationBuilder.AddColumn<int>(
                name: "ReviewStateId",
                table: "ReviewerReviewee",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
