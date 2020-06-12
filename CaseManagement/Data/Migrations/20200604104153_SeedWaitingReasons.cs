using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class SeedWaitingReasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WaitingReasons",
                column: "WaitingReasonName",
                values: new object[]
                {
                    "Requester action",
                    "Approval requested",
                    "Not waiting",
                    "Waiting for IT direct",
                    "On hold",
                    "Bundled",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WaitingReasons",
                keyColumn: "WaitingReasonName",
                keyValues: new object[]
                {
                    "Requester action",
                    "Approval requested",
                    "Not waiting",
                    "Waiting for IT direct",
                    "On hold",
                    "Bundled",
                });
        }
    }
}
