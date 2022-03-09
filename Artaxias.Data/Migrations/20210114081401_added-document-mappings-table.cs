using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Artaxias.Data.Migrations
{
    public partial class addeddocumentmappingstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractTemplate_User_CreatedByUserId",
                table: "ContractTemplate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDatetimeUTC",
                table: "ContractTemplate",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ContractTemplate",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDatetimeUTC",
                table: "ContractTemplate",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "ContractTemplateMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateField = table.Column<string>(nullable: false),
                    EntityField = table.Column<string>(nullable: false),
                    ContractTemplateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTemplateMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractTemplateMapping_ContractTemplate_ContractTemplateId",
                        column: x => x.ContractTemplateId,
                        principalTable: "ContractTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractTemplateMapping_ContractTemplateId",
                table: "ContractTemplateMapping",
                column: "ContractTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractTemplate_User_CreatedByUserId",
                table: "ContractTemplate",
                column: "CreatedByUserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractTemplate_User_CreatedByUserId",
                table: "ContractTemplate");

            migrationBuilder.DropTable(
                name: "ContractTemplateMapping");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDatetimeUTC",
                table: "ContractTemplate",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ContractTemplate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDatetimeUTC",
                table: "ContractTemplate",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractTemplate_User_CreatedByUserId",
                table: "ContractTemplate",
                column: "CreatedByUserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
