using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class addeddomainstatetoabsence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_AbsenceType_TypeId",
                table: "Absence");

            migrationBuilder.AddColumn<int>(
                name: "DomainStateId",
                table: "Absence",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Absence_DomainStateId",
                table: "Absence",
                column: "DomainStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_DomainState_DomainStateId",
                table: "Absence",
                column: "DomainStateId",
                principalTable: "DomainState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_AbsenceType_TypeId",
                table: "Absence",
                column: "TypeId",
                principalTable: "AbsenceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_DomainState_DomainStateId",
                table: "Absence");

            migrationBuilder.DropForeignKey(
                name: "FK_Absence_AbsenceType_TypeId",
                table: "Absence");

            migrationBuilder.DropIndex(
                name: "IX_Absence_DomainStateId",
                table: "Absence");

            migrationBuilder.DropColumn(
                name: "DomainStateId",
                table: "Absence");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_AbsenceType_TypeId",
                table: "Absence",
                column: "TypeId",
                principalTable: "AbsenceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
