using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class changedasnswerIdtoplural : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackAnswer_AnswerValue_AnswerValueId",
                table: "FeedbackAnswer");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackAnswer_AnswerValueId",
                table: "FeedbackAnswer");

            migrationBuilder.DropColumn(
                name: "AnswerValueId",
                table: "FeedbackAnswer");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackAnswerId",
                table: "AnswerValue",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "AnswerValueId",
                table: "FeedbackAnswer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswer_AnswerValueId",
                table: "FeedbackAnswer",
                column: "AnswerValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackAnswer_AnswerValue_AnswerValueId",
                table: "FeedbackAnswer",
                column: "AnswerValueId",
                principalTable: "AnswerValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
