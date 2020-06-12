using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class SeedNewCasePriorities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM CasePriorities");

            migrationBuilder.InsertData(
                table: "CasePriorities",
                column: "CasePriorityName",
                values: new object[]
                {
                    "Low",
                    "Normal",
                    "Urgent",
                    "Immediate",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CasePriorities",
                keyColumn: "CasePriorityName",
                keyValues: new object[]
                {
                    "Low",
                    "Normal",
                    "Urgent",
                    "Immediate",
                });
        }
    }
}
