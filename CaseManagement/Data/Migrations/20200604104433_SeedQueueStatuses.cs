using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class SeedQueueStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "QueueStatuses",
                column: "QueueStatusName",
                values: new object[]
                {
                    "New",
                    "Waiting",
                    "In process",
                    "Resolved",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QueueStatuses",
                keyColumn: "QueueStatusName",
                keyValues: new object[]
                {
                    "New",
                    "Waiting",
                    "In process",
                    "Resolved",
                });
        }
    }
}
