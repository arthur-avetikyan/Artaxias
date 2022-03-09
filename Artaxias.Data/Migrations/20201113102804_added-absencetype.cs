using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class addedabsencetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Absence");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Absence",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AbsenceType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absence_TypeId",
                table: "Absence",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_AbsenceType_TypeId",
                table: "Absence",
                column: "TypeId",
                principalTable: "AbsenceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_AbsenceType_TypeId",
                table: "Absence");

            migrationBuilder.DropTable(
                name: "AbsenceType");

            migrationBuilder.DropIndex(
                name: "IX_Absence_TypeId",
                table: "Absence");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Absence");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Absence",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
