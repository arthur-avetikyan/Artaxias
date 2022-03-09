using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Artaxias.Data.Migrations
{
    public partial class addedcreatedupdteddatetietoabsence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDatetimeUTC",
                table: "Absence",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDatetimeUTC",
                table: "Absence",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDatetimeUTC",
                table: "Absence");

            migrationBuilder.DropColumn(
                name: "UpdatedDatetimeUTC",
                table: "Absence");
        }
    }
}
