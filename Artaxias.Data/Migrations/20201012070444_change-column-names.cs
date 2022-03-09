using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Artaxias.Data.Migrations
{
    public partial class changecolumnnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentDate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ResignmentDate",
                table: "Employee");

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractEnd",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractStart",
                table: "Employee",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractEnd",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ContractStart",
                table: "Employee");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignmentDate",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ResignmentDate",
                table: "Employee",
                type: "datetime2",
                nullable: true);
        }
    }
}
