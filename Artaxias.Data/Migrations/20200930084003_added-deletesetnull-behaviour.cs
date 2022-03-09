using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class addeddeletesetnullbehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_User_HeadId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Departments_DepartmentId",
                schema: "dbo",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_User_HeadId",
                table: "Departments",
                column: "HeadId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Departments_DepartmentId",
                schema: "dbo",
                table: "User",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_User_HeadId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Departments_DepartmentId",
                schema: "dbo",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_User_HeadId",
                table: "Departments",
                column: "HeadId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Departments_DepartmentId",
                schema: "dbo",
                table: "User",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
