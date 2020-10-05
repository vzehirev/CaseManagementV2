using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class InvertUsersAndAgentsAvailabilityAndSkillsForeignKeyLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentsAvailabilityAndSkills_AspNetUsers_UserId",
                table: "AgentsAvailabilityAndSkills");

            migrationBuilder.DropIndex(
                name: "IX_AgentsAvailabilityAndSkills_UserId",
                table: "AgentsAvailabilityAndSkills");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AgentsAvailabilityAndSkills");

            migrationBuilder.AddColumn<int>(
                name: "AgentAvailabilityAndSkillsId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AgentAvailabilityAndSkillsId",
                table: "AspNetUsers",
                column: "AgentAvailabilityAndSkillsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AgentsAvailabilityAndSkills_AgentAvailabilityAndSkillsId",
                table: "AspNetUsers",
                column: "AgentAvailabilityAndSkillsId",
                principalTable: "AgentsAvailabilityAndSkills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AgentsAvailabilityAndSkills_AgentAvailabilityAndSkillsId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AgentAvailabilityAndSkillsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AgentAvailabilityAndSkillsId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AgentsAvailabilityAndSkills",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AgentsAvailabilityAndSkills_UserId",
                table: "AgentsAvailabilityAndSkills",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentsAvailabilityAndSkills_AspNetUsers_UserId",
                table: "AgentsAvailabilityAndSkills",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
