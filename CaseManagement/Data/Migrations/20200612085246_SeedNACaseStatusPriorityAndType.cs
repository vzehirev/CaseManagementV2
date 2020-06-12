using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class SeedNACaseStatusPriorityAndType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "DCMOpsMonitoring",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "CasePriorities",
                column: "CasePriorityName",
                values: new object[]
                {
                    "N/A",
                });

            migrationBuilder.InsertData(
                table: "CaseStatuses",
                column: "CaseStatusName",
                values: new object[]
                {
                    "N/A",
                });

            migrationBuilder.InsertData(
                table: "CaseTypes",
                column: "CaseTypeName",
                values: new object[]
                {
                    "N/A",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "DCMOpsMonitoring",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.DeleteData(
                table: "CasePriorities",
                keyColumn: "CasePriorityName",
                keyValues: new object[]
                {
                    "N/A",
                });

            migrationBuilder.DeleteData(
                table: "CaseStatuses",
                keyColumn: "CaseStatusName",
                keyValues: new object[]
                {
                    "N/A",
                });

            migrationBuilder.DeleteData(
                table: "CaseTypes",
                keyColumn: "CaseTypeName",
                keyValues: new object[]
                {
                    "N/A",
                });
        }
    }
}
