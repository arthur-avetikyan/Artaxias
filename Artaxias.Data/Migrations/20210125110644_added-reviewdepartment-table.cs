using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class addedreviewdepartmenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_Employee_EmployeeId",
                table: "Absence");

            migrationBuilder.DropForeignKey(
                name: "FK_Bonus_Employee_EmployeeId",
                table: "Bonus");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackTemplate_Department_DepartmentId",
                table: "FeedbackTemplate");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackTemplate_DepartmentId",
                table: "FeedbackTemplate");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "FeedbackTemplate");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Review",
                newName: "CreatedDateTimeUTC");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Bonus",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Bonus_EmployeeId",
                table: "Bonus",
                newName: "IX_Bonus_ReceiverId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Absence",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Absence_EmployeeId",
                table: "Absence",
                newName: "IX_Absence_ReceiverId");

            migrationBuilder.CreateTable(
                name: "ReviewDepartment",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewDepartment", x => new { x.ReviewId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_ReviewDepartment_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewDepartment_Review_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Review",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDepartment_DepartmentId",
                table: "ReviewDepartment",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_Employee_ReceiverId",
                table: "Absence",
                column: "ReceiverId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bonus_Employee_ReceiverId",
                table: "Bonus",
                column: "ReceiverId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_Employee_ReceiverId",
                table: "Absence");

            migrationBuilder.DropForeignKey(
                name: "FK_Bonus_Employee_ReceiverId",
                table: "Bonus");

            migrationBuilder.DropTable(
                name: "ReviewDepartment");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTimeUTC",
                table: "Review",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Bonus",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Bonus_ReceiverId",
                table: "Bonus",
                newName: "IX_Bonus_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Absence",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Absence_ReceiverId",
                table: "Absence",
                newName: "IX_Absence_EmployeeId");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "FeedbackTemplate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackTemplate_DepartmentId",
                table: "FeedbackTemplate",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_Employee_EmployeeId",
                table: "Absence",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bonus_Employee_EmployeeId",
                table: "Bonus",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackTemplate_Department_DepartmentId",
                table: "FeedbackTemplate",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
