using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class AddAgentsAvailabilityAndSkillsTableAndSeedMoreTicketTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentsAvailabilityAndSkills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false),
                    TTES12 = table.Column<bool>(nullable: false),
                    TTES34 = table.Column<bool>(nullable: false),
                    RTPU12 = table.Column<bool>(nullable: false),
                    RTPU34 = table.Column<bool>(nullable: false),
                    EX12 = table.Column<bool>(nullable: false),
                    EX34 = table.Column<bool>(nullable: false),
                    Other12 = table.Column<bool>(nullable: false),
                    Other34 = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentsAvailabilityAndSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentsAvailabilityAndSkills_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentsAvailabilityAndSkills_UserId",
                table: "AgentsAvailabilityAndSkills",
                column: "UserId");

            migrationBuilder.InsertData(
                table: "TicketTypes",
                column: "Name",
                values: new object[]
                {
                    "Other 1&2",
                    "Other 3&4"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "Name",
                keyValues: new object[]
                {
                    "Other 1&2",
                    "Other 3&4"
                });

            migrationBuilder.DropTable(
                name: "AgentsAvailabilityAndSkills");
        }
    }
}
