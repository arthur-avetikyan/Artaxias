using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Artaxias.Data.Migrations
{
    public partial class addedcontracttemplatetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Template_TemplateId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Template_TemplateId",
                table: "Review");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropIndex(
                name: "IX_Review_TemplateId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Question_TemplateId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackTemplateId",
                table: "Review",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FeedbackTemplateId",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContractTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    CreatedDatetimeUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDatetimeUTC = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractTemplate_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackTemplate",
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
                    table.PrimaryKey("PK_FeedbackTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackTemplate_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedbackTemplate_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Review_FeedbackTemplateId",
                table: "Review",
                column: "FeedbackTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_FeedbackTemplateId",
                table: "Question",
                column: "FeedbackTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractTemplate_CreatedByUserId",
                table: "ContractTemplate",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackTemplate_CreatedByUserId",
                table: "FeedbackTemplate",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackTemplate_DepartmentId",
                table: "FeedbackTemplate",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_FeedbackTemplate_FeedbackTemplateId",
                table: "Question",
                column: "FeedbackTemplateId",
                principalTable: "FeedbackTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_FeedbackTemplate_FeedbackTemplateId",
                table: "Review",
                column: "FeedbackTemplateId",
                principalTable: "FeedbackTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_FeedbackTemplate_FeedbackTemplateId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_FeedbackTemplate_FeedbackTemplateId",
                table: "Review");

            migrationBuilder.DropTable(
                name: "ContractTemplate");

            migrationBuilder.DropTable(
                name: "FeedbackTemplate");

            migrationBuilder.DropIndex(
                name: "IX_Review_FeedbackTemplateId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Question_FeedbackTemplateId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "FeedbackTemplateId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "FeedbackTemplateId",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Template",
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

            migrationBuilder.CreateIndex(
                name: "IX_Review_TemplateId",
                table: "Review",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_TemplateId",
                table: "Question",
                column: "TemplateId");

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
        }
    }
}
