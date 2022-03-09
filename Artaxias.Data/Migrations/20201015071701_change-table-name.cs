using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class changetablename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Questions_QuestionId",
                table: "AnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerValues_AnswerOptions_AnswerOptionId",
                table: "AnswerValues");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_FeadbackTemplates_TemplateId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeadbackTemplates",
                table: "FeadbackTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerValues",
                table: "AnswerValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerOptions",
                table: "AnswerOptions");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "FeadbackTemplates",
                newName: "FeadbackTemplate");

            migrationBuilder.RenameTable(
                name: "AnswerValues",
                newName: "AnswerValue");

            migrationBuilder.RenameTable(
                name: "AnswerOptions",
                newName: "AnswerOption");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_TemplateId",
                table: "Question",
                newName: "IX_Question_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerValues_AnswerOptionId",
                table: "AnswerValue",
                newName: "IX_AnswerValue_AnswerOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerOptions_QuestionId",
                table: "AnswerOption",
                newName: "IX_AnswerOption_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeadbackTemplate",
                table: "FeadbackTemplate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerValue",
                table: "AnswerValue",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerOption",
                table: "AnswerOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOption_Question_QuestionId",
                table: "AnswerOption",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerValue_AnswerOption_AnswerOptionId",
                table: "AnswerValue",
                column: "AnswerOptionId",
                principalTable: "AnswerOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_FeadbackTemplate_TemplateId",
                table: "Question",
                column: "TemplateId",
                principalTable: "FeadbackTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOption_Question_QuestionId",
                table: "AnswerOption");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerValue_AnswerOption_AnswerOptionId",
                table: "AnswerValue");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_FeadbackTemplate_TemplateId",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeadbackTemplate",
                table: "FeadbackTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerValue",
                table: "AnswerValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerOption",
                table: "AnswerOption");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "FeadbackTemplate",
                newName: "FeadbackTemplates");

            migrationBuilder.RenameTable(
                name: "AnswerValue",
                newName: "AnswerValues");

            migrationBuilder.RenameTable(
                name: "AnswerOption",
                newName: "AnswerOptions");

            migrationBuilder.RenameIndex(
                name: "IX_Question_TemplateId",
                table: "Questions",
                newName: "IX_Questions_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerValue_AnswerOptionId",
                table: "AnswerValues",
                newName: "IX_AnswerValues_AnswerOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerOption_QuestionId",
                table: "AnswerOptions",
                newName: "IX_AnswerOptions_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeadbackTemplates",
                table: "FeadbackTemplates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerValues",
                table: "AnswerValues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerOptions",
                table: "AnswerOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Questions_QuestionId",
                table: "AnswerOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerValues_AnswerOptions_AnswerOptionId",
                table: "AnswerValues",
                column: "AnswerOptionId",
                principalTable: "AnswerOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_FeadbackTemplates_TemplateId",
                table: "Questions",
                column: "TemplateId",
                principalTable: "FeadbackTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
