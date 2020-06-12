using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class SeedCaseTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CaseTypes",
                column: "Type",
                values: new object[]
                {
                    "Incident",
                    "Service request",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CaseTypes",
                keyColumn: "Type",
                keyValues: new object[]
                {
                    "Incident",
                    "Service request",
                });
        }
    }
}
