using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class useremployeecascadedelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Employee_EmployeeId",
                schema: "dbo",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Employee_EmployeeId",
                schema: "dbo",
                table: "User",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Employee_EmployeeId",
                schema: "dbo",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Employee_EmployeeId",
                schema: "dbo",
                table: "User",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
