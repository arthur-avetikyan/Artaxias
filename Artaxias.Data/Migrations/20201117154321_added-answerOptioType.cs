using Microsoft.EntityFrameworkCore.Migrations;

namespace Artaxias.Data.Migrations
{
    public partial class addedanswerOptioType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionType",
                table: "AnswerOption");

            migrationBuilder.AddColumn<int>(
                name: "AnswerOptionTypeId",
                table: "AnswerOption",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AnswerOptionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptionType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOption_AnswerOptionTypeId",
                table: "AnswerOption",
                column: "AnswerOptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOption_AnswerOptionType_AnswerOptionTypeId",
                table: "AnswerOption",
                column: "AnswerOptionTypeId",
                principalTable: "AnswerOptionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOption_AnswerOptionType_AnswerOptionTypeId",
                table: "AnswerOption");

            migrationBuilder.DropTable(
                name: "AnswerOptionType");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOption_AnswerOptionTypeId",
                table: "AnswerOption");

            migrationBuilder.DropColumn(
                name: "AnswerOptionTypeId",
                table: "AnswerOption");

            migrationBuilder.AddColumn<int>(
                name: "OptionType",
                table: "AnswerOption",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
