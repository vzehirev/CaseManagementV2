using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class SeedNewCaseTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM CaseTypes");

            migrationBuilder.InsertData(
                table: "CaseTypes",
                column: "CaseTypeName",
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
                keyColumn: "CaseTypeName",
                keyValues: new object[]
                {
                    "Incident",
                    "Service request",
                });
        }
    }
}
