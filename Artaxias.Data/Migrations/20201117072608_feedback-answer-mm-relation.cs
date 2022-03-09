using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class feedbackanswermmrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerValue_FeedbackAnswer_FeedbackAnswerId",
                table: "AnswerValue");

            migrationBuilder.DropIndex(
                name: "IX_AnswerValue_FeedbackAnswerId",
                table: "AnswerValue");

            migrationBuilder.DropColumn(
                name: "FeedbackAnswerId",
                table: "AnswerValue");

            migrationBuilder.CreateTable(
                name: "FeedbackAnswerValue",
                columns: table => new
                {
                    FeedbackAnswerId = table.Column<int>(nullable: false),
                    AnswerValueId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackAnswerValue", x => new { x.AnswerValueId, x.FeedbackAnswerId });
                    table.ForeignKey(
                        name: "FK_FeedbackAnswerValue_AnswerValue_AnswerValueId",
                        column: x => x.AnswerValueId,
                        principalTable: "AnswerValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedbackAnswerValue_FeedbackAnswer_FeedbackAnswerId",
                        column: x => x.FeedbackAnswerId,
                        principalTable: "FeedbackAnswer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswerValue_FeedbackAnswerId",
                table: "FeedbackAnswerValue",
                column: "FeedbackAnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackAnswerValue");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackAnswerId",
                table: "AnswerValue",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerValue_FeedbackAnswerId",
                table: "AnswerValue",
                column: "FeedbackAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerValue_FeedbackAnswer_FeedbackAnswerId",
                table: "AnswerValue",
                column: "FeedbackAnswerId",
                principalTable: "FeedbackAnswer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
