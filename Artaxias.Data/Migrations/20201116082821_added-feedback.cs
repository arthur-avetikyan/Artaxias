using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Artaxias.Data.Migrations
{
    public partial class addedfeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_FeadbackTemplate_TemplateId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_FeadbackTemplate_TemplateId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewerReviewee_Review_ReviewId",
                table: "ReviewerReviewee");

            migrationBuilder.DropTable(
                name: "FeadbackTemplate");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "ReviewerReviewee",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeedbackId",
                table: "ReviewerReviewee",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvidedAt = table.Column<DateTime>(nullable: false),
                    ReviewerRevieweeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedback_ReviewerReviewee_ReviewerRevieweeId",
                        column: x => x.ReviewerRevieweeId,
                        principalTable: "ReviewerReviewee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Template_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Template_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedbackId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    AnswerValueId = table.Column<int>(nullable: true),
                    OpenTextValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackAnswer_AnswerValue_AnswerValueId",
                        column: x => x.AnswerValueId,
                        principalTable: "AnswerValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedbackAnswer_Feedback_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedbackAnswer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ReviewerRevieweeId",
                table: "Feedback",
                column: "ReviewerRevieweeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswer_AnswerValueId",
                table: "FeedbackAnswer",
                column: "AnswerValueId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswer_FeedbackId",
                table: "FeedbackAnswer",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswer_QuestionId",
                table: "FeedbackAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_CreatedByUserId",
                table: "Template",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_DepartmentId",
                table: "Template",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Template_TemplateId",
                table: "Question",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Template_TemplateId",
                table: "Review",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewerReviewee_Review_ReviewId",
                table: "ReviewerReviewee",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Template_TemplateId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Template_TemplateId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewerReviewee_Review_ReviewId",
                table: "ReviewerReviewee");

            migrationBuilder.DropTable(
                name: "FeedbackAnswer");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                table: "ReviewerReviewee");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "ReviewerReviewee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "FeadbackTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeadbackTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeadbackTemplate_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeadbackTemplate_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeadbackTemplate_CreatedByUserId",
                table: "FeadbackTemplate",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeadbackTemplate_DepartmentId",
                table: "FeadbackTemplate",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_FeadbackTemplate_TemplateId",
                table: "Question",
                column: "TemplateId",
                principalTable: "FeadbackTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_FeadbackTemplate_TemplateId",
                table: "Review",
                column: "TemplateId",
                principalTable: "FeadbackTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewerReviewee_Review_ReviewId",
                table: "ReviewerReviewee",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
