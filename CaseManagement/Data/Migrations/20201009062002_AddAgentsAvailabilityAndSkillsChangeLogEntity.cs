using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class AddAgentsAvailabilityAndSkillsChangeLogEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentsAvailabilityAndSkillsChangeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ChangedByUserId = table.Column<string>(nullable: false),
                    ChangedSkill = table.Column<string>(nullable: false),
                    OldValue = table.Column<bool>(nullable: false),
                    NewValue = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentsAvailabilityAndSkillsChangeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentsAvailabilityAndSkillsChangeLogs_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AgentsAvailabilityAndSkillsChangeLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentsAvailabilityAndSkillsChangeLogs_ChangedByUserId",
                table: "AgentsAvailabilityAndSkillsChangeLogs",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentsAvailabilityAndSkillsChangeLogs_UserId",
                table: "AgentsAvailabilityAndSkillsChangeLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentsAvailabilityAndSkillsChangeLogs");
        }
    }
}
