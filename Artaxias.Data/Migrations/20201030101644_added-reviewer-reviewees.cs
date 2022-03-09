using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class addedreviewerreviewees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewerReviewee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewerId = table.Column<int>(nullable: false),
                    RevieweeId = table.Column<int>(nullable: false),
                    ReviewId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewerReviewee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewerReviewee_Review_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Review",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewerReviewee_Employee_RevieweeId",
                        column: x => x.RevieweeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewerReviewee_Employee_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewerReviewee_ReviewId",
                table: "ReviewerReviewee",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewerReviewee_RevieweeId",
                table: "ReviewerReviewee",
                column: "RevieweeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewerReviewee_ReviewerId",
                table: "ReviewerReviewee",
                column: "ReviewerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewerReviewee");
        }
    }
}
