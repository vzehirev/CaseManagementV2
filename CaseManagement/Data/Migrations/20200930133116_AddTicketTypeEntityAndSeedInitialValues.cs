using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class AddTicketTypeEntityAndSeedInitialValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TicketTypes",
                column: "Name",
                values: new object[]
                {
                    "TTES 1&2",
                    "TTES 3&4",
                    "RTPU 1&2",
                    "RTPU 3&4",
                    "Execute 1&2",
                    "Execute 3&4"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "Name",
                keyValues: new object[]
                {
                    "TTES 1&2",
                    "TTES 3&4",
                    "RTPU 1&2",
                    "RTPU 3&4",
                    "Execute 1&2",
                    "Execute 3&4"
                });

            migrationBuilder.DropTable(
                name: "TicketTypes");
        }
    }
}
