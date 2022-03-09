using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class addedinverseprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FeadbackTemplate_CreatedByUserId",
                table: "FeadbackTemplate",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeadbackTemplate_DepartmentId",
                table: "FeadbackTemplate",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeadbackTemplate_User_CreatedByUserId",
                table: "FeadbackTemplate",
                column: "CreatedByUserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeadbackTemplate_Department_DepartmentId",
                table: "FeadbackTemplate",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeadbackTemplate_User_CreatedByUserId",
                table: "FeadbackTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FeadbackTemplate_Department_DepartmentId",
                table: "FeadbackTemplate");

            migrationBuilder.DropIndex(
                name: "IX_FeadbackTemplate_CreatedByUserId",
                table: "FeadbackTemplate");

            migrationBuilder.DropIndex(
                name: "IX_FeadbackTemplate_DepartmentId",
                table: "FeadbackTemplate");
        }
    }
}
