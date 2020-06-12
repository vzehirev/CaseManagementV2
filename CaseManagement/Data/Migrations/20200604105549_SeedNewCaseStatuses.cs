using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class SeedNewCaseStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM CaseStatuses");

            migrationBuilder.InsertData(
                table: "CaseStatuses",
                column: "CaseStatusName",
                values: new object[]
                {
                    "New",
                    "Waiting",
                    "In process",
                    "Resolved",
                    "Closed",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CaseStatuses",
                keyColumn: "CaseStatusName",
                keyValues: new object[]
                {
                    "New",
                    "Waiting",
                    "In process",
                    "Resolved",
                    "Closed",
                });
        }
    }
}
