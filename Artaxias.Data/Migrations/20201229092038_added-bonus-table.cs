using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Artaxias.Data.Migrations
{
    public partial class addedbonustable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bonus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    CreatedDatetimeUTC = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDatetimeUTC = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    EmployeeId = table.Column<int>(nullable: false),
                    ApproverId = table.Column<int>(nullable: false),
                    RequesterId = table.Column<int>(nullable: false),
                    DomainStateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bonus_Employee_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bonus_DomainState_DomainStateId",
                        column: x => x.DomainStateId,
                        principalTable: "DomainState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bonus_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bonus_Employee_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bonus_ApproverId",
                table: "Bonus",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Bonus_DomainStateId",
                table: "Bonus",
                column: "DomainStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Bonus_EmployeeId",
                table: "Bonus",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bonus_RequesterId",
                table: "Bonus",
                column: "RequesterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonus");
        }
    }
}
